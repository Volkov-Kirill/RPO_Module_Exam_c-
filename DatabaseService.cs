using Npgsql;
using System;
using System.Data;
using Serilog;
using DotNetEnv;

namespace MediTrack
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            Env.Load();
            // Чтение из .env (если пакет подключен)
            var host = DotNetEnv.Env.GetString("DB_HOST");
            var port = DotNetEnv.Env.GetString("DB_PORT");
            var db = DotNetEnv.Env.GetString("DB_NAME");
            var user = DotNetEnv.Env.GetString("DB_USER");
            var pass = DotNetEnv.Env.GetString("DB_PASSWORD");

            _connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={pass};";
        }

        public DataTable GetMedications()
        {
            var table = new DataTable();
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                Log.Information("Успешное подключение к базе данных PostgreSQL.");
                
                using var cmd = new NpgsqlCommand("SELECT * FROM medications", conn);
                using var adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                
                Log.Error("Произошла ошибка при загрузке данных.");
            }
            return table;
        }
    }
}