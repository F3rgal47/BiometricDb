CREATE TABLE [dbo].[EmployeeDetails] (
    [Id]              INT          IDENTITY (1, 1) NOT NULL,
    [Forename]        VARCHAR (50) NULL,
    [Surname]         VARCHAR (60) NULL,
    [Email]           VARCHAR (50) NULL,
    [TelephoneNo]     INT          NULL,
    [AccessLevel]     VARCHAR (50) NULL,
    [BiometricMarker] INT          NULL,
    [AddressLine1]    VARCHAR (50) NULL,
    [AddressLine2]    VARCHAR (50) NULL,
    [AddressLine3]    VARCHAR (50) NULL,
    [City]            VARCHAR (50) NULL,
    [Postcode]        VARCHAR (50) NULL,
    [Photo]           VARBINARY(MAX)        NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

