import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConsultantApiService {

  private endpointOrigin = `https://localhost:5001`;

  constructor(private client: HttpClient) {

  }

  /* getUserRoles$(userId: string): Observable<Auth0Role[]> {
    return this.client.get<Auth0Role[]>(`${this.endpointOrigin}/api/v2/users/${userId}/roles`);
  } */

  // Case endpoints

  //GET    - /case/all - Get all cases for this client
  getAllClientCases$(): Observable<any[]> {
    return this.client.get<any[]>(`${this.endpointOrigin}/case/all`);
  }

  //getAllCases()
     /*ii. GET    - /case/consultant/all - Get all cases for this consultant
    iii. GET    - /case/{caseId} - Get the case with this ID.
     iv. PUT    - /case/{caseId}/assignTo/{consultantId} - Reassign this case to the given consultant (auth, consultant)
      v. PUT    - /case/{caseId}/status?status={newStatusText} - Update the status to a new status. A status that doesnt exist will get 404 (auth, consultant)
     vi. POST   - /case/new - Create a new case for this client. Give it to the given consultant or the consultant with the fewest cases if none specified (auth)
    vii. DELETE - /case/{caseId} - Delete the case with this ID.

  ### Appointment
      i. GET    - /appointment/all - Get all the appointments for this case
     ii. GET    - /appointment/{appointmentId} - Get the appointment with this ID.
    iii. POST   - /appointment/new - Create a new appointment for a given case. If a client is logged in, automatically include them as a related party (auth)
     iv. PUT    - /appointment/{appointmentId} - Update this appointment with given datetime or name (auth, either consultant or related party)
      v. DELETE - /appointment/{appointmentId}/cancel - Cancels an appointment, deleting it from the server (auth)

  ### Note
      i. GET    - /note/all - Get all notes for this case
     ii. GET    - /note/{noteId} - Get the note with this ID.
    iii. POST   - /note/new - Create a new note for this case with the enclosed data
     iv. PUT    - /note/{noteId} - Update a note with the given noteId with the enclosed data
      v. DELETE - /note/{noteId} - Delete the note with this ID.*/
}
