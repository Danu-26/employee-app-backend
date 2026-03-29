USE EmployeeDB;
GO

CREATE TABLE Departments
(
    DepartmentCode NVARCHAR(10) PRIMARY KEY,
    DepartmentName NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE Employees
(
    EmployeeId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Age INT NOT NULL,
    Salary DECIMAL(18,2) NOT NULL,
    DepartmentCode NVARCHAR(10) NOT NULL,
    FOREIGN KEY (DepartmentCode) REFERENCES Departments(DepartmentCode)
);
GO