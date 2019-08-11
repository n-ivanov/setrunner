using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using setrunner.Models;
using setrunner.Attributes;

namespace setrunner.Controllers
{
    [Route("api/v1/venues")]
    [ApiController]
    public class VenuesController : DBController
    {
        public VenuesController()
        {
        }

        // GET api/v1/venues/ID/tracks
        [Route("{id}/tracks/popular", Name="GetVenueTracks")]
        [ProducesResponseType(typeof(List<TrackModel>),200)]
        [ProducesResponseType(typeof(List<TrackModel>),404)]
        [HttpGet]
        public IActionResult GetVenueTracksById(string id, int limit = DEFAULT_LIMIT, int skip = 0)
        {
            var command = $"CALL GetVenueEventPopularTracks('{id}', {limit}, {skip})";
            return GetItemsViaStoredProc(command, ReadTrackWithCounts);
        }

         // GET api/v1/venues/ID/tracks
        [Route("{id}/tracks/popular", Name="GetVenueTracksByDate")]
        [ProducesResponseType(typeof(List<TrackModel>),200)]
        [ProducesResponseType(typeof(List<TrackModel>),404)]
        [HttpGet]
        [RequireParameter(MatchMode.Any,"startDate","endDate")]
        public IActionResult GetVenueTracksByIdInDateRange(string id, string startDate = null, string endDate = null, int limit = DEFAULT_LIMIT, int skip = 0)
        {
            FixDateParameters(ref startDate, ref endDate);
            var command = $"CALL GetVenueEventPopularTracksInDateRange('{id}', '{startDate}', '{endDate}', {limit}, {skip})";
            return GetItemsViaStoredProc(command, ReadTrackWithCounts);
        }
    }
}
