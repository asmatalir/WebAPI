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
}
