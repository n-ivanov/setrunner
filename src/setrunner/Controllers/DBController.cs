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
        protected const int DEFAULT_LIMIT = 10;
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

        protected TrackModel ReadTrackWithCounts(MySqlDataReader reader)
        {
            return new TrackWithCountModel()
            {
                Name = reader.GetString("track_name"),
                Artist = reader.GetString("artist_name"),
                Publisher = reader.GetString("publisher_name"),
                OnSpotify = reader.GetBoolean("on_spotify"),
                OnSoundcloud = reader.GetBoolean("on_soundcloud"),
                OnYoutube = reader.GetBoolean("on_youtube"),
                OnAppleMusic = reader.GetBoolean("on_apple_music"),
                CountSets = reader.GetInt32("count")
            };
        }

        protected TrackModel ReadTrack(MySqlDataReader reader)
        {
            return new TrackModel()
            {
                Name = reader.GetString("track_name"),
                Artist = reader.GetString("artist_name"),
                Publisher = reader.GetString("publisher_name"),
                OnSpotify = reader.GetBoolean("on_spotify"),
                OnSoundcloud = reader.GetBoolean("on_soundcloud"),
                OnYoutube = reader.GetBoolean("on_youtube"),
                OnAppleMusic = reader.GetBoolean("on_apple_music"),
            };
        }

        protected SetlistModel ReadSetlist(MySqlDataReader reader)
        {
            return new SetlistModel()
            {
                Name = reader.GetString("set_name"),
                Artist = reader.GetString("artist_name"),
                Uri = reader.GetString("uri"),
                Venue = reader.GetString("venue_event"),
                Date = reader.GetDateTime("set_date")
            };
        }

        protected void FixDateParameters(ref string startDate, ref string endDate)
        {
            if(String.IsNullOrWhiteSpace(startDate))
            {
                startDate = "0001-01-01";
            }
            else if(string.IsNullOrWhiteSpace(endDate))
            {
                endDate = $"{DateTime.Now:yyyy-MM-dd}";
            }
        }

    }
}
