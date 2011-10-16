CREATE TABLE [error_documents] (
[code] INTEGER  NOT NULL PRIMARY KEY,
[content] TEXT  NULL,
[mimetype] VARCHAR(32) DEFAULT 'text/plain' NULL,
);
INSERT INTO error_documents VALUES('400', 'Error: 400 Bad Request
The request sent by your browser was incorrectly formed or contained invalid data.
This may indicate an error with your browser software.', NULL);
INSERT INTO error_documents VALUES('401', 'Error: 401 Unauthorized
You are not authorized to view this resource.', NULL);