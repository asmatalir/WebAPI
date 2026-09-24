CREATE OR ALTER PROCEDURE sp_GetAllAttendance
AS
BEGIN
    SELECT AttendanceID, StudentID, SubjectID, AttendanceDate, Status, IsActive,
           CreatedBy, CreatedOn, ModifiedBy, ModifiedOn
    FROM Attendance;
END
GO

CREATE OR ALTER PROCEDURE sp_GetAttendanceByID
    @AttendanceID INT
AS
BEGIN
    SELECT AttendanceID, StudentID, SubjectID, AttendanceDate, Status, IsActive,
           CreatedBy, CreatedOn, ModifiedBy, ModifiedOn
    FROM Attendance
    WHERE AttendanceID = @AttendanceID AND IsActive=1;
END
GO