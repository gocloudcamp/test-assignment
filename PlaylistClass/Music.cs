using System;

namespace PlaylistClass
{
    public class Music
    {
        public string path { get; private set; }
        public string name { get; private set; }
        public DateTime duration { get; private set; }

        public Music(string name, string singer)
        {
            this.name = name;
            this.duration = Convert.ToDateTime(duration);
        }

        public Music(string url, string name, string duration)
        {
            this.path = url;
            this.name = name;
            this.duration = Convert.ToDateTime(duration);
        }
        public Music(string name)
        {
            this.name = name;
        }

        public string GetDate()
        {
            return "";
        }
    }
}
