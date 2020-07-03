# consultant-website

## Current Status
 Basic functionalities live-tested via console runner.
 Authentication implemented via Auth0.
 Db rework in progress to take advantage of Auth0 identity values and remove all PII from consultant-website db.
 Design will be identity-service-agnostic, in case a different auth service is used in the future.

## Controllers and endpoints
  ### User
      i. GET    - /user - Get's the user's information, currently just the role, based on the auth token
  
  ### Case 
      i. GET    - /case/all - Get all cases for this client or counsultant, depending on role
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
      v. DELETE - /note/{caseId}/{noteId} - Delete the note with this ID.
 

## Roadmap
 ### Current User Stories
 Stories supported by the API currently:
   1. I, as a consultant, want to log in and review my cases.
   2. I, as a client, want to log in and review my current cases or start a new case.
   3. I, as a client, want to schedule an appointment
   4. I, as a consultant, want to review my appointments
   5. I, as a consultant, want to reschedule my appointments
   6. I, as a client, want to reschedule or cancel my appointment

 ### Future User Stories
   1. I, as a client, want to log in and upload files relevant to my case (this will be done on the front end via Azure Containers)
   