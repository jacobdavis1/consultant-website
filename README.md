# consultant-website

## Current Status
 Database scaffolding finished.
 Db models implemented.
 Interfaces, Mappers, and Repositories implemented.

## Planned controllers and endpoints
  
  ### Consultant
      i. GET    - /consultant/{userId}/case/all - Get the current cases assigned to this consultant (auth)

  ### Client
      i. GET    - /client - Get this client's id if an id/email pair exists or create a new one if it doesnt and return the id (no auth guard but protected)
     ii. GET    - /client/{userId}/case/all - Get the current cases for this client (auth)

  ### Case 
      i. PUT    - /case/{id}/assignTo/{consultantId} - Reassign this case to the given consultant (auth, consultant)
     ii. PUT    - /case/{id}/status?status={newStatusText} - Update the status to a new status. A status that doesnt exist will get 404 (auth, consultant)
    iii. POST   - /case/new - Create a new case for this client. Give it to the given consultant or the consultant with the fewest cases if none specified (auth)

  ### Appointment
      i. GET    - /case/{caseId}/appointment/all - Get all the appointments for this case
     ii. GET    - /case/{caseId}/appointment/{appointmentId} - Get the appointment with this ID.
    iii. POST   - /case/{caseId}/appointment/new - Create a new appointment for a given case. If a client is logged in, automatically include them as a related party (auth)
     iv. PUT    - /case/{caseId}/appointment/{appointmentId} - Update this appointment with given datetime or name (auth, either consultant or related party)
      v. DELETE - /case/{caseId}/appointment/{appointmentId}/cancel - Cancels an appointment, deleting it from the server (auth)

  ### Note
      i. GET    - /case/{caseId}/note/all - Get all notes for this case
     ii. GET    - /case/{caseId}/note/{noteId} - Get the note with this id
    iii. POST   - /case/{caseId}/note/new - Create a new note for this case with the enclosed data
     iv. PUT    - /case/{caseId}/note/{noteId} - Update a note with the given noteId with the enclosed data
      v. DELETE - /case/{caseId}/note/{noteId} - Delete the note with this id
 

## Roadmap
 ### Implement Basic User Stories
 Two user stories to implement:
   1. I, as a consultant, want to log in and review my cases.
   2. I, as a client, want to log in and review my current cases or start a new case.

 ### Future User Stories
   1. I, as a client, want to log in and upload files relevant to my case
   2. I, as a client, want to schedule an appointment
   3. I, as a consultant, want to review my appointments
   4. I, as a consultant, want to reschedule my appointments
   5. I, as a client, want to reschedule or cancel my appointment