using System;
using System.Xml.Serialization;

namespace PatchServe
{
    [XmlRoot("item")]
    public class PatchInfoEventArgs : EventArgs
    {
        private string _changelogUri;
        private string _downloadUri;

        public PatchInfoEventArgs()
        {

        }

        public bool IsUpdateAvailable { get; set; }

        [XmlElement("url")]
        public string DownloadUri
        {
            get => GetUri(PatchServe.BaseUri, _downloadUri);
            set => _downloadUri = value;
        }

        [XmlElement("changelogurl")]
        public string ChangelogUri
        {
            get => GetUri(PatchServe.BaseUri, _changelogUri);
            set => _changelogUri = value;
        }

        [XmlElement("version")]
        public string LatestVersion { get; set; }
        public Version InstalledVersion { get; set; }

        internal static string GetUri(Uri baseUri, string url)
        {
            if (!string.IsNullOrEmpty(url) && Uri.IsWellFormedUriString(url, UriKind.Relative))
            {
                var uri = new Uri(baseUri, url);

                if (uri.IsAbsoluteUri)
                {
                    url = uri.AbsoluteUri;
                }
            }

            return url;
        }
    }
}
