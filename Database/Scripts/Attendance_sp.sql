CREATE OR ALTER PROCEDURE sp_GetAllAttendance
AS
BEGIN
    SELECT AttendanceID, StudentID, SubjectID, AttendanceDate, Status, IsActive,
           CreatedBy, CreatedOn, ModifiedBy, ModifiedOn
    FROM Attendance;
END
GO