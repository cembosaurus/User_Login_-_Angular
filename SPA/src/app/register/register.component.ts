import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  userDTO: any = {};

  constructor() { }

  ngOnInit() {
  }

  register() {
    console.log(this.userDTO);
  }

  cancel() {
    console.log('Cancelled...');
  }

}
