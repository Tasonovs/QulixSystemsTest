USE QulixSystemsDB

CREATE TABLE Companies(
	Id int PRIMARY KEY IDENTITY,
	Name nvarchar(50) NOT NULL,
	OrganizationalForm nvarchar(10) NOT NULL
)
GO

CREATE TABLE Employees(
	Id int PRIMARY KEY IDENTITY,
	LastName nvarchar(50) NOT NULL,
	FirstName nvarchar(50) NOT NULL,
	MiddleName nvarchar(50) NOT NULL,
	EmploymentDate date NOT NULL,
	Position nvarchar(50) NOT NULL,
	CompanyId int NOT NULL,

	FOREIGN KEY (CompanyId)  REFERENCES Companies (Id)
)
GO