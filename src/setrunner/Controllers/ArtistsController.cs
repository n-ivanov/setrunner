using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using setrunner.Models;

namespace setrunner.Controllers
{
    [Route("api/v1/artists")]
    [ApiController]
    public class ArtistsController : Controller
    {
        private string connectionString_;

        public ArtistsController()
        {
            var server = Environment.GetEnvironmentVariable("DB_SERVER");
            var uid = Environment.GetEnvironmentVariable("DB_USER");
            var password = Environment.GetEnvironmentVariable("DB_PWD");
            var database = Environment.GetEnvironmentVariable("DB_NAME");
            var port = Environment.GetEnvironmentVariable("DB_PORT");
            connectionString_ = 
                $"server={server};port={port};uid={uid};pwd={password};database={database}";
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "deadmau5", "value2" };
        }

        // GET api/values/5
        [Route("{id}/tracks/popular")]//{limit:int}&{skip:int}")]
        [ProducesResponseType(typeof(List<TrackModel>),200)]
        [ProducesResponseType(typeof(List<TrackModel>),404)] //TODO figure out why single argument not supported
        [HttpGet]
        public IActionResult GetArtistPopularTracks(string id)//, int limit = 25, int skip = 0)
        {
            int limit = 25;
            int skip = 10;
            Console.WriteLine($"Received request for artist {id} with limit={limit} and skip={skip}.");
            using(var connection = new MySqlConnection(connectionString_))
            {
                Console.WriteLine($"Connecting to MySQL with connection string={connectionString_}...");
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"CALL GetArtistPopularTracks('{id}', {limit}, {skip})";
                var tracks = new List<TrackModel>();
                using(var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        var track = new TrackModel()
                        {
                            Name = reader.GetString("track_name"),
                            Artist = reader.GetString("artist_name"),
                            Publisher = reader.GetString("publisher_name"),
                            AvailableOnSpotify = reader.GetBoolean("on_spotify"),
                            AvailableOnSoundcloud = reader.GetBoolean("on_soundcloud"),
                            AvailableOnYoutube = reader.GetBoolean("on_youtube"),
                            AvailableOnAppleMusic = reader.GetBoolean("on_apple_music"),
                            CountSets = reader.GetInt32("count")
                        };
                        tracks.Add(track);
                    }
                }
                if(tracks.Count == 0)
                {
                    return NotFound();
                }
                return Ok(tracks);
            }         
        }

    }
}
