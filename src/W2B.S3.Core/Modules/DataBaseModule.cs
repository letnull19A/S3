using Npgsql;

namespace W2B.S3.Core.Modules;

public sealed class DataBaseModule
{
  private Dictionary<string, string> _args;
  
  public DataBaseModule(Dictionary<string, string> args)
  {
    _args = args;
  }

  public void Init()
  { 
    Console.WriteLine($"initialization {nameof(DataBaseModule)}");
  }

  public string BuildConnectionString()
  {
    string host = _args["--pgHost"];
    string database = _args["--pgDatabase"];
    string username = _args["--pgUser"];
    string password = _args["--pgPassword"];
    string port = _args["--pgPort"];

    return $"Host={host};Database={database};Username={username};Password={password};Port={port};";
  }

  public void CheckConnection()
  {
    Console.WriteLine("Checking connection...");

    using(var connection = new NpgsqlConnection(BuildConnectionString()))
    {
      connection.Open();

      Console.WriteLine("Postgres successfull conected!");

      connection.Close();

      Console.WriteLine("Closing connection");
    }
  }
}
