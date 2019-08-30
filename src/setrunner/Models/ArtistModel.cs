using System;
using System.Collections.Generic;

namespace setrunner.Models
{
    public class ArtistModel
    {
        public string Name {get;set;}
        public List<TrackModel> Tracks {get;set;}
        public List<SetlistModel> Sets {get;set;}
    }
}