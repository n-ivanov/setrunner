namespace setrunner.Models
{
    public class TrackModel
    {
        public string Name {get; set;}
        public string Artist {get;set;}
        public string Publisher {get;set;}
        public bool OnSpotify {get;set;}
        public bool OnSoundcloud {get;set;}
        public bool OnYoutube {get;set;}
        public bool OnAppleMusic {get;set;}
    }

    public class TrackWithCountModel : TrackModel
    {
        public int CountSets {get;set;}
    }
}

