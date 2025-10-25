using System;

namespace PatchServe
{
    public class PatchXmlParser : EventArgs
    {
        public PatchXmlParser(string remoteData)
        {
            RemoteData = remoteData;
        }
        public string RemoteData { get; }

        public PatchInfoEventArgs UpdateInfo { get; set; }
    }
}
