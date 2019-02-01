import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  userDTO: any = {};
  @Output() canceFromRegister = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  register() {
    console.log(this.userDTO);
  }

  cancel() {
    this.canceFromRegister.emit(false);
    console.log('Cancelled...');
  }

}
