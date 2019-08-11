using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using setrunner.Models;
using setrunner.Attributes;

namespace setrunner.Controllers
{
    [Route("api/v1/sets")]
    [ApiController]
    public class SetsController : DBController
    {
        private const int DEFAULT_SET_TRACK_LIMIT = 50;

        public SetsController()
        {
        }

        // GET api/v1/sets/ID/tracks
        [Route("{id}/tracks", Name="GetSetTracks")]
        [ProducesResponseType(typeof(List<TrackModel>),200)]
        [ProducesResponseType(typeof(List<TrackModel>),404)] //TODO figure out why single argument not supported
        [HttpGet]
        public IActionResult GetSetlistTracksById(string id, int limit = DEFAULT_SET_TRACK_LIMIT, int skip = 0)
        {
            var command = $"CALL GetSetlistTracksById('{id}', {limit}, {skip})";
            return GetItemsViaStoredProc(command, ReadTrack);
        }
    }
}
