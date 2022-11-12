import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Result } from 'src/result';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
@Component({
  selector: 'app-signup-form',
  templateUrl: './signup-form.component.html',
  styleUrls: ['./signup-form.component.scss']
})
export class SignupFormComponent implements OnInit {

  form!: FormGroup
  password?: any;
  email!: any;
  userName!: any;
  result!: Result; 
  
  submitted = false;

  constructor(private router: Router, private http: HttpClient, private formBuilder: FormBuilder) { }
  
  ngOnInit(): void {
    this.form = this.formBuilder.group({
      UserName:['',[ Validators.required]],
      Email:['', [Validators.required, Validators.email]],
      Password:['', [Validators.required, Validators.minLength(6)]]
    });

  }

  onSubmit()
  {
   this.submitted = true;
   
   if (this.form.invalid) {
    return
   }
   this.http.post("https://localhost:7230/api/Auth/SignUp", this.form.value)
   .subscribe(
    response => 
    {
      this.result = response;
      console.log(this.result);
      if (this.result.error?.code == 0) 
      {
        alert("Success");
        this.router.navigate(['login']);
      }
    }, 
    error => 
    {
      if (error.status == 400 ) 
      {
        console.log(error.status),
        console.log(error.error.errors),
        this.password = error.error.errors.Password,
        this.email = error.error.errors.Email,
        this.userName = error.error.errors.UserName
      }
    });
  }
}
