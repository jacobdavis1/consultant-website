CREATE TABLE Consultants (
    consultantId varchar(40) PRIMARY KEY
    , firstName varchar(100)
    , middleName varchar(100)
    , lastName varchar(100)
);

CREATE TABLE CaseStatuses (
    statusId varchar(40) PRIMARY KEY
    , statusText varchar(100)
);

CREATE TABLE Cases (
    caseId varchar(40) PRIMARY KEY
    , caseTitle varchar(100)
    , activeConsultantId varchar(40) REFERENCES Consultants(consultantId)
    , currentStatusId varchar(40) REFERENCES CaseStatuses(statusId)
);

CREATE TABLE CaseNotes (
    noteId varchar(40) 
    , caseId varchar(40) REFERENCES Cases(caseId)
    , content varchar(255)
);

CREATE TABLE Appointments (
    appointmentId varchar(40) PRIMARY KEY
    , caseId varchar(40) REFERENCES Cases(caseId)
    , appointmentDateTime timestamptz
    , appointmentTitle varchar(100)
);

CREATE TABLE Clients (
    clientId varchar(40) PRIMARY KEY
    , firstName varchar(100)
    , middleName varchar(100)
    , lastName varchar(100)
    , email varchar(100)
);

CREATE TABLE CaseClient (
    caseId varchar(40) REFERENCES Cases(caseId)
    , clientId varchar(40) REFERENCES Clients(clientId)
);