-- Create tables
CREATE TABLE Roles (
    roleId int GENERATED ALWAYS AS IDENTITY
    , roleText varchar(100) NOT NULL
    , PRIMARY KEY (roleId)
);

CREATE TABLE Users (
    rowId int GENERATED ALWAYS AS IDENTITY
    , userId varchar(40) NOT NULL
    , userRole int REFERENCES Roles(roleId) NOT NULL
    , PRIMARY KEY (rowId)
);

CREATE TABLE CaseStatuses (
    statusId int GENERATED ALWAYS AS IDENTITY
    , statusText varchar(100) NOT NULL
    , PRIMARY KEY (statusId)
);

CREATE TABLE Cases (
    caseId int GENERATED ALWAYS AS IDENTITY
    , caseTitle varchar(100)
    , activeConsultantId int REFERENCES Users(rowId) 
    , currentStatusId int REFERENCES CaseStatuses(statusId)
    , PRIMARY KEY (caseId)
);

CREATE TABLE CaseNotes (
    noteId int GENERATED ALWAYS AS IDENTITY
    , caseId int REFERENCES Cases(caseId)
    , content varchar(255)
    , PRIMARY KEY (noteId)
);

CREATE TABLE Appointments (
    appointmentId int GENERATED ALWAYS AS IDENTITY
    , caseId int REFERENCES Cases(caseId)
    , appointmentDateTime timestamptz NOT NULL
    , appointmentTitle varchar(100) NOT NULL
    , PRIMARY KEY (appointmentId)
);

CREATE TABLE CaseClient (
    rowId int GENERATED ALWAYS AS IDENTITY
    , caseId int REFERENCES Cases(caseId)
    , clientId int REFERENCES Users(rowId)
    , PRIMARY KEY (rowId)
);

-- Insert base roles and statuses
INSERT INTO Roles (roleText) VALUES ('Consultant');
INSERT INTO Roles (roleText) VALUES ('Client');

INSERT INTO CaseStatuses (statusText) VALUES ('Unassigned');
INSERT INTO CaseStatuses (statusText) VALUES ('Assigned');
INSERT INTO CaseStatuses (statusText) VALUES ('Awaiting Documentation');
INSERT INTO CaseStatuses (statusText) VALUES ('Complete');