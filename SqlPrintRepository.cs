using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintProfit.Worker;
using Microsoft.Data.SqlClient;

public sealed class SqlPrintRepository
{
    private readonly string _connectionString;

    public SqlPrintRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("PrintProfit")
            ?? throw new InvalidOperationException("Connection string 'PrintProfit' no configurada.");
    }

    public async Task InsertPrintJobAsync(PrintJobInsertDto job, CancellationToken ct = default)
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync(ct);

        await using var cmd = new SqlCommand("dbo.usp_InsertPrintJobWithCharge", conn)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@SourceEventRecordId", (object?)job.SourceEventRecordId ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@SourceJobId", (object?)job.SourceJobId ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@QueueName", job.QueueName);
        cmd.Parameters.AddWithValue("@DocumentName", (object?)job.DocumentName ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@SubmittedBy", (object?)job.SubmittedBy ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@TotalPages", (object?)job.TotalPages ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@TotalBytes", (object?)job.TotalBytes ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@JobStatus", job.JobStatus);
        cmd.Parameters.AddWithValue("@SubmittedAt", job.SubmittedAt);
        cmd.Parameters.AddWithValue("@CompletedAt", (object?)job.CompletedAt ?? DBNull.Value);

        await cmd.ExecuteNonQueryAsync(ct);
    }
}