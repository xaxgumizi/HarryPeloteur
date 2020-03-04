import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';


@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  loginForm: FormGroup;

  constructor(private fb: FormBuilder, private router: Router) { }

  ngOnInit() {
    this.loginForm = this.fb.group({  // Crée une instance de FormGroup
      username: [],                   // Crée une instance de FormControl
      password: [],                   // Crée une instance de FormControl
    });
  }

  login(){
    if(this.loginForm.value.username == "webservice" &&  this.loginForm.value.username == "webservice" )
      this.router.navigate(['/listParties']);
    else  
      alert("faux");
  }

}
