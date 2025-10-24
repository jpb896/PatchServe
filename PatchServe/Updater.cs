using System;
using System.Reflection;

namespace PatchServe
{
    public static class Updater
    {
        internal static Uri BaseUri;
        public static string AppXmlUri;

        public static void StartCheck(string xmluri, Assembly assembly = null)
        {
            AppXmlUri = xmluri;
        }

        private static object CheckUpdate(Assembly assembly)
        {
            if (AppXmlUri != null) 
            { 
                BaseUri = new Uri(AppXmlUri);
            }
        }
    }
}
