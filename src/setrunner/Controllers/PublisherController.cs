using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using setrunner.Models;
using setrunner.Attributes;

namespace setrunner.Controllers
{
    [Route("api/v1/publishers")]
    [ApiController]
    public class PublishersController : DBController
    {
        public PublishersController()
        {
        }

        // GET api/v1/publishers/ID/tracks
        [Route("{id}/tracks/popular", Name="GetPublisherTracks")]
        [ProducesResponseType(typeof(List<TrackModel>),200)]
        [ProducesResponseType(typeof(List<TrackModel>),404)]
        [HttpGet]
        public IActionResult GetPublisherTracksById(string id, int limit = DEFAULT_LIMIT, int skip = 0)
        {
            var command = $"CALL GetPublisherPopularTracks('{id}', {limit}, {skip})";
            return GetItemsViaStoredProc(command, ReadTrackWithCounts);
        }
    }
}
