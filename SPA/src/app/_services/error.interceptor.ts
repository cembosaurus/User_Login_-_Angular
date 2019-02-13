import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';


@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        return next.handle(req).pipe(
            catchError(err => {

                if (err instanceof HttpErrorResponse) {


                    // ... in case of UNAUTHORIZED ...
                    if (err.status === 401) {

                        return throwError(err.statusText);

                    }


                    // ... in case of MY CUSTOM error
                    const custErrFromAPI = err.headers.get('My-Custom-Error');

                    if (custErrFromAPI) {

                        return throwError(custErrFromAPI);

                    }


                    // ... in case of MODELSTATE error. Modelstate error is type of 'object' ...
                    const serverError = err.error.errors;
                    let modelStateErrors = '';

                    if (serverError && typeof serverError === 'object') {

                        for (const key in serverError) {

                            if (serverError[key]) {

                                modelStateErrors += serverError[key] + '\n';

                            }
                        }
                    }


                    return throwError(modelStateErrors || serverError || 'Server Error');

                }
            })
        );

    }

}

export const ErrorInterceptorProvider = {

    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true

};
