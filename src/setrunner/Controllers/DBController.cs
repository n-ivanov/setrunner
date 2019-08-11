using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using setrunner.Models;

namespace setrunner.Controllers
{

    public abstract class DBController : Controller
    {
        protected string connectionString_;

        public DBController()
        {
            var server = Environment.GetEnvironmentVariable("DB_SERVER");
            var uid = Environment.GetEnvironmentVariable("DB_USER");
            var password = Environment.GetEnvironmentVariable("DB_PWD");
            var database = Environment.GetEnvironmentVariable("DB_NAME");
            var port = Environment.GetEnvironmentVariable("DB_PORT");
            connectionString_ = 
                $"server={server};port={port};uid={uid};pwd={password};database={database}";
        }

        protected IActionResult GetItemsViaStoredProc<T>(string commandText, Func<MySqlDataReader,T> creationFunc)
        {
            try 
            {
                var items = GetItemsViaStoredProcUnsafe(commandText, creationFunc);
                if(items.Count == 0)
                {
                    return NotFound();
                }
                return Ok(items);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex}");
                return Json(new Dictionary<string,string>()
                {
                    {"message","Exception performing database operation."},
                    {"command", commandText},
                    {"exception",$"{ex}"}
                });
            }
        }

        protected List<T> GetItemsViaStoredProcUnsafe<T>(string commandText, Func<MySqlDataReader,T> creationFunc)
        {
            Console.WriteLine($"Executing {commandText}...");
            using(var connection = new MySqlConnection(connectionString_))
            {
                Console.WriteLine($"Connecting to MySQL with connection string={connectionString_}...");
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = commandText;
                var items = new List<T>();
                using(var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        items.Add(creationFunc(reader));
                    }
                }
                return items;
            } 
        }
    }
}
