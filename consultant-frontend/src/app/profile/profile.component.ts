import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AuthApiService, Auth0Role } from '../_services/auth-api.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  constructor(public auth: AuthService, public authApi: AuthApiService) { }

  ngOnInit() {

  }

}