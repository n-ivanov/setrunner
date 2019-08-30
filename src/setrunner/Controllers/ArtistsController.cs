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
        public ArtistsController()
        {
        }

        // GET api/v1/artists/ID/popular
        [Route("{id}/tracks/popular", Name="GetArtistPopularTracks")]
        [ProducesResponseType(typeof(List<TrackModel>),200)]
        [ProducesResponseType(typeof(List<TrackModel>),404)] //TODO figure out why single argument not supported
        [HttpGet]
        public IActionResult GetArtistPopularTracks(string id, int limit = DEFAULT_LIMIT, int skip = 0)
        {
            var command = $"CALL GetArtistPopularTracks('{id}', {limit}, {skip})";
            return GetItemsViaStoredProc(command, ReadTrackWithCounts);
        }

        // GET api/v1/artists/ID/sets/tracks
        [Route("{id}/sets/tracks", Name="GetArtistRecentSetTracks")]
        [ProducesResponseType(typeof(List<TrackModel>),200)]
        [ProducesResponseType(typeof(List<TrackModel>),404)]
        [HttpGet]
        [RequireParameter(MatchMode.All,"n")]
        public IActionResult GetArtistRecentSetTracks(string id, int n=5, int limit = DEFAULT_LIMIT, int skip = 0)
        {
            var command = $"CALL GetArtistTracksInLastNSets('{id}', {n}, {limit}, {skip})";
            return GetItemsViaStoredProc(command, ReadTrackWithCounts);
        }

        // GET api/v1/artists/ID/sets/tracks
        [Route("{id}/sets/tracks", Name="GetArtistRecentSetTracksByDate")]
        [ProducesResponseType(typeof(List<TrackModel>),200)]
        [ProducesResponseType(typeof(List<TrackModel>),404)]
        [HttpGet]
        [RequireParameter(MatchMode.Any,"startDate","endDate")]
        public IActionResult GetArtistRecentSetTracks(string id, string startDate = null, string endDate = null, int limit = DEFAULT_LIMIT, int skip = 0)
        {
            FixDateParameters(ref startDate, ref endDate);
            var command = $"CALL GetArtistTracksInSetsInDateRange('{id}', '{startDate}', '{endDate}', {limit}, {skip})";
            return GetItemsViaStoredProc(command, ReadTrackWithCounts);
        }

        // GET api/v1/artists/ID/sets
        [Route("{id}/sets", Name="GetArtistRecentSets")]
        [ProducesResponseType(typeof(List<SetlistModel>),200)]
        [ProducesResponseType(typeof(List<SetlistModel>),404)]
        [HttpGet]
        public IActionResult GetArtistSets(string id, bool withTracks=false, int limit = DEFAULT_LIMIT, int skip = 0)
        {
            var command = $"CALL GetArtistSets('{id}',  {limit}, {skip})";
            return GetItemsViaStoredProc(command, ReadSetlist);
        }

        // GET api/v1/artists/ID/sets
        [Route("{id}/sets", Name="GetArtistRecentSetsByDate")]
        [ProducesResponseType(typeof(List<SetlistModel>),200)]
        [ProducesResponseType(typeof(List<SetlistModel>),404)]
        [HttpGet]
        [RequireParameter(MatchMode.Any,"startDate","endDate")]
        public IActionResult GetArtistSetsInDateRange(string id, bool withTracks=false, string startDate = null, string endDate = null, int limit = DEFAULT_LIMIT, int skip = 0)
        {
            FixDateParameters(ref startDate, ref endDate);
            var command = $"CALL GetArtistSetsInDateRange('{id}', '{startDate}', '{endDate}', {limit}, {skip})";
            return GetItemsViaStoredProc(command, ReadSetlist);
        }

    }
}
