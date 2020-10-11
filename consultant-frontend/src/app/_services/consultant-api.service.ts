import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConsultantApiService {

  private endpointOrigin = `https://localhost:5001`;
  private fileShareEndpointOrigin = `https://localhost:44379`;

  constructor(private client: HttpClient) {

  }

  /* getUserRoles$(userId: string): Observable<Auth0Role[]> {
    return this.client.get<Auth0Role[]>(`${this.endpointOrigin}/api/v2/users/${userId}/roles`);
  } */

  // User endpoints
  // i. GET    - /user - Get's the user's information, currently just the role, based on the auth token
  getUser$(): Observable<any> {
    return this.client.get<any>(`${this.endpointOrigin}/user`);
  }

  // Case endpoints

  //GET    - /case/all - Get all cases for this client
  getAllClientCases$(): Observable<any[]> {
    return this.client.get<any[]>(`${this.endpointOrigin}/case/all`);
  }

  //getAllCases()
     /*
  ### Case 
     ii. GET    - /case/{caseId} - Get the case with this ID.
    iii. PUT    - /case/{caseId}/assignTo/{consultantId} - Reassign this case to the given consultant (auth, consultant)
     vi. PUT    - /case/{caseId}/status?status={newStatusText} - Update the status to a new status. A status that doesnt exist will get 404 (auth, consultant)
      v. PUT    - /case/{caseId} - Update all aspects of a case with the given ID.
     vi. POST   - /case/new - Create a new case for this client. Give it to the given consultant (auth)
    vii. DELETE - /case/{caseId} - Delete the case with this ID.

  ### Appointment
      i. GET    - /appointment/{caseId}/all - Get all the appointments for this case
     ii. GET    - /appointment/{caseId}/{appointmentId} - Get the appointment with this ID.
    iii. POST   - /appointment/{caseId}/new - Create a new appointment for a given case. If a client is logged in, automatically include them as a related party (auth)
     iv. PUT    - /appointment/{caseId}/{appointmentId} - Update this appointment with given datetime or name (auth, either consultant or related party)
      v. DELETE - /appointment/{caseId}/{appointmentId}/cancel - Cancels an appointment, deleting it from the server (auth)

  ### Note
      i. GET    - /note/{caseId}/all - Get all notes for this case
     ii. GET    - /note/{caseId}/{noteId} - Get the note with this ID.
    iii. POST   - /note/{caseId}/new - Create a new note for this case with the enclosed data
     iv. PUT    - /note/{caseId}/{noteId} - Update a note with the given noteId with the enclosed data
      v. DELETE - /note/{caseId}/{noteId} - Delete the note with this ID. */

  uploadFileList$(files : FileList): Observable<any> {

    let formData : FormData = new FormData();
    for (let i = 0; i < files.length; ++i) {
      formData.append(`file${i}`, files[i]);
    }

    return this.client.post(`${this.fileShareEndpointOrigin}/api/FileShare`, formData);
  }
}
