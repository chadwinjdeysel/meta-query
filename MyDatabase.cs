using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text;
using System.IO;

public class MyDatabase
{
    private readonly string _connectionString;

    public string Server { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Database { get; set; }

    public MyDatabase(string server, string username, string password, string database)
    {
        Server = server;
        UserName = username;
        Password = password;
        Database = database;

        _connectionString = $"Server={Server};Database={Database};User Id={UserName};Password={Password};TrustServerCertificate=true;";
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public void GetConnection() {
        using (var conn = CreateConnection())
        {
            conn.Open();

            var result = conn.Query(@$"USE {Database} 
                SELECT @@VERSION");
        }
    }

    public void GetSchema()
    {
        using (var conn = CreateConnection())
        {
            conn.Open();
            
            var result = conn.Query<Schema>($@"USE {Database} 
                SELECT 
                    t.name AS TableName, 
                    STRING_AGG(c.name + '\' + UPPER(TYPE_NAME(c.user_type_id)), ',') AS ColumnsAndTypes,
                    STRING_AGG(fk.name + '\' + c.name, ',') AS Relationships
                FROM 
                    sys.tables AS t
                INNER JOIN 
                    sys.columns AS c ON t.object_id = c.object_id
                LEFT JOIN 
                    sys.foreign_key_columns AS fkc ON t.object_id = fkc.parent_object_id AND c.column_id = fkc.parent_column_id
                LEFT JOIN 
                    sys.objects AS fk ON fkc.constraint_object_id = fk.object_id
                LEFT JOIN 
                    sys.tables AS rt ON fkc.referenced_object_id = rt.object_id
                LEFT JOIN 
                    sys.columns AS rc ON fkc.referenced_object_id = rc.object_id AND fkc.referenced_column_id = rc.column_id
                GROUP BY 
                    t.name;
            ");

            FileService.WriteSchema(result);
        }
    }
}
