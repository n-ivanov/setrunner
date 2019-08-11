using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using setrunner.Models;
using setrunner.Attributes;

namespace setrunner.Controllers
{
    [Route("api/v1/artists")]
    [ApiController]
    public class ArtistsController : DBController
    {
        private const int DEFAULT_LIMIT = 10;

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

        // GET api/v1/artists/ID/popular
        [Route("{id}/tracks/popular")]
        [ProducesResponseType(typeof(List<TrackModel>),200)]
        [ProducesResponseType(typeof(List<TrackModel>),404)] //TODO figure out why single argument not supported
        [HttpGet]
        public IActionResult GetArtistPopularTracks(string id, int limit = DEFAULT_LIMIT, int skip = 0)
        {
            var command = $"CALL GetArtistPopularTracks('{id}', {limit}, {skip})";
            return GetItemsViaStoredProc(command, ReadTrack);
        }

        // GET api/v1/artists/ID/sets/tracks
        [Route("{id}/sets/tracks")]
        [ProducesResponseType(typeof(List<TrackModel>),200)]
        [ProducesResponseType(typeof(List<TrackModel>),404)]
        [HttpGet]
        [RequireParameter(MatchMode.All,"n")]
        public IActionResult GetArtistRecentSetTracks(string id, int n=5, int limit = DEFAULT_LIMIT, int skip = 0)
        {
            var command = $"CALL GetArtistTracksInLastNSets('{id}', {n}, {limit}, {skip})";
            return GetItemsViaStoredProc(command, ReadTrack);
        }

        // GET api/v1/artists/ID/sets/tracks
        [Route("{id}/sets/tracks")]
        [ProducesResponseType(typeof(List<TrackModel>),200)]
        [ProducesResponseType(typeof(List<TrackModel>),404)]
        [HttpGet]
        [RequireParameter(MatchMode.Any,"startDate","endDate")]
        public IActionResult GetArtistRecentSetTracks(string id, string startDate = null, string endDate = null, int limit = DEFAULT_LIMIT, int skip = 0)
        {
            FixDateParameters(ref startDate, ref endDate);
            var command = $"CALL GetArtistTracksInSetsInDateRange('{id}', '{startDate}', '{endDate}', {limit}, {skip})";
            return GetItemsViaStoredProc(command, ReadTrack);
        }

        // GET api/v1/artists/ID/sets
        [Route("{id}/sets")]
        [ProducesResponseType(typeof(List<TrackModel>),200)]
        [ProducesResponseType(typeof(List<TrackModel>),404)]
        [HttpGet]
        public IActionResult GetArtistSets(string id, int limit = DEFAULT_LIMIT, int skip = 0)
        {
            var command = $"CALL GetArtistSets('{id}',  {limit}, {skip})";
            return GetItemsViaStoredProc(command, ReadSetlist);
        }

        // GET api/v1/artists/ID/sets
        [Route("{id}/sets")]
        [ProducesResponseType(typeof(List<TrackModel>),200)]
        [ProducesResponseType(typeof(List<TrackModel>),404)]
        [HttpGet]
        [RequireParameter(MatchMode.Any,"startDate","endDate")]
        public IActionResult GetArtistSetsInDateRange(string id, string startDate = null, string endDate = null, int limit = DEFAULT_LIMIT, int skip = 0)
        {
            FixDateParameters(ref startDate, ref endDate);
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

        private void FixDateParameters(ref string startDate, ref string endDate)
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
