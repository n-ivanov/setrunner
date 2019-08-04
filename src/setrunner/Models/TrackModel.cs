namespace setrunner.Models
{
    public class TrackModel
    {
        public string Name {get; set;}
        public string Artist {get;set;}
        public string Publisher {get;set;}
        public bool AvailableOnSpotify {get;set;}
        public bool AvailableOnSoundcloud {get;set;}
        public bool AvailableOnYoutube {get;set;}
        public bool AvailableOnAppleMusic {get;set;}
        public int CountSets {get;set;}
    }
}

