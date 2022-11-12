import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  userName?:string = "salom";
  constructor(private router: Router) { }

  ngOnInit(): void {
  }
  
  authLogOut(){
    localStorage.removeItem('token');
    alert("token is removed");
    this.router.navigate(['login']);
  }

  isLoggedIn(){
    let jwtHelper = new JwtHelperService();
    let token = localStorage.getItem('token');
    if (!token)
       return false;
    let expireDate = jwtHelper.getTokenExpirationDate(token);
    let isExpired = jwtHelper.isTokenExpired(token);
    console.log("expire time => ", expireDate, "isExpired", isExpired);
    
    return !isExpired;   
  }

  currentUser(): any
  {
    this.userName = "klejaskk";
    let token = localStorage.getItem('token');
    if (!token) 
       return null;
    let decodedToken = new JwtHelperService().decodeToken(token);
    this.userName = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
    return decodedToken;   

  }
}
