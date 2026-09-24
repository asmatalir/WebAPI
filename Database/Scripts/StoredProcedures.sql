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

CREATE OR ALTER PROCEDURE sp_AddStudent
    @StudentName VARCHAR(100),
    @Age INT,
    @Gender VARCHAR(10),
    @City VARCHAR(100),
    @DepartmentID INT = NULL,
    @CreatedBy VARCHAR(100),
    @NewStudentID INT OUTPUT
AS
BEGIN
    INSERT INTO Student (StudentName, Age, Gender, City, DepartmentID, IsActive, CreatedBy, CreatedOn)
    VALUES (@StudentName, @Age, @Gender, @City, @DepartmentID, 1, @CreatedBy, GETDATE());

    SET @NewStudentID = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_UpdateStudent
    @StudentID INT,
    @StudentName VARCHAR(100),
    @Age INT,
    @Gender VARCHAR(10),
    @City VARCHAR(100),
    @DepartmentID INT = NULL,
    @IsActive BIT,
    @ModifiedBy VARCHAR(100)
AS
BEGIN
    UPDATE Student
    SET StudentName = @StudentName,
        Age = @Age,
        Gender = @Gender,
        City = @City,
        DepartmentID = @DepartmentID,
        IsActive = @IsActive,
        ModifiedBy = @ModifiedBy,
        ModifiedOn = GETDATE()
    WHERE StudentID = @StudentID;
END
GO

CREATE OR ALTER PROCEDURE sp_DeleteStudent
    @StudentID INT,
    @ModifiedBy VARCHAR(100)
AS
BEGIN
    UPDATE Student
    SET IsActive = 0,
        ModifiedBy = @ModifiedBy,
        ModifiedOn = GETDATE()
    WHERE StudentID = @StudentID;
END
GO
