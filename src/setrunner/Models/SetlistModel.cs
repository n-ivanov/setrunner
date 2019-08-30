using System;
using System.Collections.Generic;

namespace setrunner.Models
{
    public class SetlistModel
    {
        public string Name {get;set;}
        public string Uri {get;set;}
        public string Artist {get;set;}
        public string Venue {get;set;}
        public DateTime Date {get; set;}
        public List<TrackModel> Tracks {get;set;}
    }
}