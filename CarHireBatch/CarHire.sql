USE [master]
GO

IF DB_ID('CarHire') IS NOT NULL
	DROP DATABASE [CarHire]
GO

--Create CarHire database. A booking database for a car rental firm.


CREATE DATABASE [CarHire]
ON  PRIMARY 
(NAME = N'CR_Data', FILENAME = N'C:\Users\Ross\Desktop\OO2FinalAssignment\CarHireDB\CarHire.mdf' , MAXSIZE = UNLIMITED, FILEGROWTH = 20%)

--Commented out the Log File creation section because the existence of a log file was causing errors when connecting the .mdf 
--file to the Visual Studio project. Username permission also needed to be set to full control to attach the database.

--LOG ON 
--(NAME = N'CR_Log', FILENAME = N'C:\Cars\Car_Rental_log.ldf' , MAXSIZE = UNLIMITED, FILEGROWTH = 20%)

GO

USE [CarHire]
GO

--Create Table [dbo].[Car]   
CREATE TABLE [dbo].[Car](
	[ID] [int] NOT NULL,
	[Make] [varchar](30) NOT NULL,
	[Model] [varchar](30) NOT NULL,
	[Size] [varchar](30),
	CONSTRAINT [PK_Car] PRIMARY KEY CLUSTERED ([ID] ASC)) ON [PRIMARY]
Go

--Populate Table [dbo].[Car]
INSERT INTO Car
VALUES (1, 'Toyota', 'Aygo', 'Small'),
(2, 'Toyota', 'Avensis', 'Large'),
(3, 'Ford', 'Focus', 'Medium'),
(4, 'Ford', 'Mondeo', 'Large'),
(5, 'Ford', 'Fiesta', 'Small'),
(6, 'Honda', 'Civic', 'Medium'),
(7, 'Honda', 'Accord', 'Large'),
(8, 'Volkswagen', 'Polo', 'Small'),
(9, 'Volkswagen', 'Passat', 'Large'),
(10, 'Volkswagen', 'Golf', 'Medium');


--Create Table [dbo].[Booking]    

CREATE TABLE [dbo].[Booking](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CarID] [int] NOT NULL FOREIGN KEY REFERENCES Car(ID),
	[StartDate] [smalldatetime] NOT NULL,
	[EndDate] [smalldatetime] NOT NULL,	

CONSTRAINT [PK_Booking] PRIMARY KEY CLUSTERED ([ID] ASC))ON [PRIMARY]
GO

SET IDENTITY_INSERT dbo.Booking ON
INSERT INTO Booking ([ID], [CarID], [StartDate], [EndDate])
Values 
(1, 10, '06/04/2014', '06/11/2014'),
(2, 9, '06/05/2014', '06/12/2014')




