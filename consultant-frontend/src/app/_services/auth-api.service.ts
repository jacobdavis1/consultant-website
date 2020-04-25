import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

export class Auth0Role {
  id: string;
  name: string;
  description: string;
}

export class Auth0User {
  email: string;
  identities: { provider: string; user_id: string; }[];
  given_name: string;
  family_name: string;
  name: string;

  getUserId(identityIndex: number): string {
    return this.identities[identityIndex].provider + "|" + this.identities[identityIndex].user_id;
  }
}

@Injectable({
  providedIn: 'root'
})
export class AuthApiService {

  private endpointOrigin = `https://${environment.auth0_domain}`;

  constructor(private client: HttpClient) {

  }

  getUserRoles$(userId: string): Observable<Auth0Role[]> {
    return this.client.get<Auth0Role[]>(`${this.endpointOrigin}/api/v2/users/${userId}/roles`);
  }

  getUser$(userId: string): Observable<Auth0User> {
    return this.client.get<Auth0User>(`${this.endpointOrigin}/api/v2/users/${userId}`);
  }
}
