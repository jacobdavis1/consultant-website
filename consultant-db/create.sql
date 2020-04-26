CREATE TABLE Cases (
    caseId varchar(40) PRIMARY KEY
    , caseTitle varchar(100)
);

CREATE TABLE CaseNotes (
    noteId varchar(40) 
    , caseId varchar(40) REFERENCES Cases(caseId)
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