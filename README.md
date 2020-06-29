# consultant-website

## Current Status
 Database scaffolding finished.
 Db models implemented.
 Interfaces, Mappers, and Repositories implemented.

## Planned controllers and endpoints
  
  ### Consultant
      i. GET  - /consultant/{id}/cases - Get the current cases assigned to this consultant (auth)

  ### Client
      i. GET    - /client/{id} - Get this client's id if an id/email pair exists or create a new one if it doesnt and return the id (no auth guard but protected)
     ii. GET    - /client/{id}/cases - Get the current cases for this client (auth)

  ### Case 
      i. PUT    - /case/{id}/assignTo/{consultantId} - Reassign this case to the given consultant (auth, consultant)
     ii. PUT    - /case/{id}/status?status={newStatusText} - Update the status to a new status. A status that doesnt exist will get 404 (auth, consultant)
    iii. POST   - /case/new - Create a new case for this client. Give it to the given consultant or the consultant withe fewest cases if none specified (auth)

  ### Appointment
      i. POST   - /appointment/new - Create a new appointment for a given case. If a client is logged in, automatically include them as a related party (auth)
     ii. PUT    - /appointment/{id} - Update this appointment with given datetime or name (auth, either consultant or related party)
    iii. DELETE - /client/{id}/cases/{caseId}/appointments/{appointmentId}/cancel - Cancels an appointment, deleting it from the server (auth)
 

## Roadmap
 ### Implement Basic User Stories
 Two user stories to implement:
   1. I, as a consultant, want to log in and review my cases.
    a. Relevant endpoints:
        i. 
      
   2. I, as a client, want to log in and review my current cases or start a new case.
    a. Relevant endpoints:
        

 ### Future User Stories
   1. I, as a client, want to log in and upload files relevant to my case
   2. I, as a client, want to schedule an appointment
   3. I, as a consultant, want to review my appointments
   4. I, as a consultant, want to reschedule my appointments
   5. I, as a client, want to reschedule or cancel my appointment