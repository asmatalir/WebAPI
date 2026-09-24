using System.Data;
using CollegeAPI.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CollegeAPI.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly string _connectionString;

    public StudentRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("CollegeDB")
            ?? throw new InvalidOperationException("Connection string 'CollegeDB' not found.");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<IEnumerable<Student>> GetAllAsync()
    {
        using var connection = CreateConnection();
        return await connection.QueryAsync<Student>(
            "sp_GetAllStudents",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        using var connection = CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Student>(
            "sp_GetStudentById",
            new { StudentID = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> AddAsync(Student student)
    {
        using var connection = CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("StudentName", student.StudentName);
        parameters.Add("Age", student.Age);
        parameters.Add("Gender", student.Gender);
        parameters.Add("City", student.City);
        parameters.Add("DepartmentID", student.DepartmentID);
        parameters.Add("CreatedBy", student.CreatedBy);
        parameters.Add("NewStudentID", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            "sp_AddStudent",
            parameters,
            commandType: CommandType.StoredProcedure);

        return parameters.Get<int>("NewStudentID");
    }

    public async Task<bool> UpdateAsync(Student student)
    {
        using var connection = CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(
            "sp_UpdateStudent",
            new
            {
                student.StudentID,
                student.StudentName,
                student.Age,
                student.Gender,
                student.City,
                student.DepartmentID,
                student.IsActive,
                student.ModifiedBy
            },
            commandType: CommandType.StoredProcedure);

        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id, string modifiedBy)
    {
        using var connection = CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(
            "sp_DeleteStudent",
            new { StudentID = id, ModifiedBy = modifiedBy },
            commandType: CommandType.StoredProcedure);

        return rowsAffected > 0;
    }
}
