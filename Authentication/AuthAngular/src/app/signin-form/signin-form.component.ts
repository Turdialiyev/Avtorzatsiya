import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Result } from 'src/result';

@Component({
  selector: 'app-signin-form',
  templateUrl: './signin-form.component.html',
  styleUrls: ['./signin-form.component.scss']
})
export class SigninFormComponent implements OnInit {
  
  form!: FormGroup
  password?: any;
  userName!: any;
  result!: Result; 
  
  submitted = false;

  constructor(private http: HttpClient, private formBuilder: FormBuilder,  private router: Router) { }
  
  ngOnInit(): void {
    this.form = this.formBuilder.group({
      UserName:['',[ Validators.required]],
      Password:['', [Validators.required, Validators.minLength(6)]]
    });

  }

  onSubmit()
  {
   this.submitted = true;
   
   if (this.form.invalid) {
    return
   }
   this.http.post("https://localhost:7230/api/Auth/SignIn", this.form.value)
   .subscribe(
    response => 
    {
      this.result = response;
      console.log(this.result);
      if (this.result.error?.code == 0 && this.result.data != null) 
      {
        alert("Success");
        this.router.navigate(['/']);
        localStorage.setItem("token", this.result.data);
      }
    }, 
    error => 
    {
      if (error.status == 400 ) 
      {
        console.log(error.status),
        console.log(error.error.errors),
        this.password = error.error.errors.Password,
        this.userName = error.error.errors.UserName
      }
    });
  }
}
