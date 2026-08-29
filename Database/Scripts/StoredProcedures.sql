-- Run this against your existing database (Student table).

CREATE OR ALTER PROCEDURE sp_GetAllStudents
AS
BEGIN
    SELECT StudentID, StudentName, Age, Gender, City, DepartmentID, IsActive,
           CreatedBy, CreatedOn, ModifiedBy, ModifiedOn
    FROM Student;
END
GO

CREATE OR ALTER PROCEDURE sp_GetStudentById
    @StudentID INT
AS
BEGIN
    SELECT StudentID, StudentName, Age, Gender, City, DepartmentID, IsActive,
           CreatedBy, CreatedOn, ModifiedBy, ModifiedOn
    FROM Student
    WHERE StudentID = @StudentID;
END
GO
