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
    public class ArtistsController : DBController
    {
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

        // GET api/v1/artists/ID/popular
        [Route("{id}/tracks/popular")]//&{skip:int}")]
        [ProducesResponseType(typeof(List<TrackModel>),200)]
        [ProducesResponseType(typeof(List<TrackModel>),404)] //TODO figure out why single argument not supported
        [HttpGet]
        public IActionResult GetArtistPopularTracks(string id, int limit = 25, int skip = 0)
        {
            var command = $"CALL GetArtistPopularTracks('{id}', {limit}, {skip})";
            return GetItemsViaStoredProc(command, ReadTrack);
        }

        // GET api/v1/artists/ID/sets/tracks
        [Route("{id}/sets/tracks")]//&{skip:int}")]
        [ProducesResponseType(typeof(List<TrackModel>),200)]
        [ProducesResponseType(typeof(List<TrackModel>),404)]
        [HttpGet]
        public IActionResult GetArtistRecentSetTracks(string id, int numSets, int limit = 25, int skip = 0)
        {
            var command = $"CALL GetArtistsTracksInLastNSets('{id}', {numSets}, {limit}, {skip})";
            return GetItemsViaStoredProc(command, ReadTrack);
        }

        // GET api/v1/artists/ID/sets/tracks
        //TODO - Add ability to only provide one of the date parameters (i.e. sets before X, sets after Y type queries)
        [Route("{id}/sets/tracks")]//&{skip:int}")]
        [ProducesResponseType(typeof(List<TrackModel>),200)]
        [ProducesResponseType(typeof(List<TrackModel>),404)]
        [HttpGet]
        public IActionResult GetArtistRecentSetTracks(string id, string startDate, string endDate, int limit = 25, int skip = 0)
        {
            var command = $"CALL GetArtistTracksInSetsInDateRange('{id}', '{startDate}', '{endDate}', {limit}, {skip})";
            return GetItemsViaStoredProc(command, ReadTrack);
        }

        // GET api/v1/artists/ID/sets
        [Route("{id}/sets")]//&{skip:int}")]
        [ProducesResponseType(typeof(List<TrackModel>),200)]
        [ProducesResponseType(typeof(List<TrackModel>),404)]
        [HttpGet]
        public IActionResult GetArtistSets(string id, int limit = 25, int skip = 0)
        {
            var command = $"CALL GetArtistSets('{id}',  {limit}, {skip})";
            return GetItemsViaStoredProc(command, ReadSetlist);
        }

        // GET api/v1/artists/ID/sets
        [Route("{id}/sets")]//&{skip:int}")]
        [ProducesResponseType(typeof(List<TrackModel>),200)]
        [ProducesResponseType(typeof(List<TrackModel>),404)]
        [HttpGet]
        public IActionResult GetArtistSetsInDateRange(string id, string startDate, string endDate, int limit = 25, int skip = 0)
        {
            var command = $"CALL GetArtistSetsInDateRange('{id}', '{startDate}', '{endDate}', {limit}, {skip})";
            return GetItemsViaStoredProc(command, ReadSetlist);
        }

        private TrackModel ReadTrack(MySqlDataReader reader)
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
                CountSets = reader.GetInt32("count")
            };
        }

        private SetlistModel ReadSetlist(MySqlDataReader reader)
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
    }
}
