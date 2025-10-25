using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace PatchServe
{
    public static class Updater
    {
        internal static Uri BaseUri;
        public static string AppXmlUri;
        public static string AppTitle;
        public static string UserAgent;
        public static IAuthentication auth;
        public delegate void PatchInfoParser(PatchXmlParser args);
        public static event PatchInfoParser patchInfoParser;

        public static void StartCheck(string xmluri, Assembly assembly = null)
        {
            AppXmlUri = xmluri;
        }

        private static object CheckUpdate(Assembly assembly)
        {
            var companyAttribute =
            (AssemblyCompanyAttribute)GetAttribute(assembly, typeof(AssemblyCompanyAttribute));
            string appCompany = companyAttribute != null ? companyAttribute.Company : "";

            if (string.IsNullOrEmpty(AppTitle))
            {
                var titleAttribute =
                    (AssemblyTitleAttribute)GetAttribute(assembly, typeof(AssemblyTitleAttribute));
                AppTitle = titleAttribute != null ? titleAttribute.Title : assembly.GetName().Name;
            }

            string registryLocation = !string.IsNullOrEmpty(appCompany)
                ? $@"Software\{appCompany}\{AppTitle}\PatchInstaller"
                : $@"Software\{AppTitle}\PatchInstaller";

            PatchInfoEventArgs args;
            string xml = null;

            if (AppXmlUri != null) 
            { 
                BaseUri = new Uri(AppXmlUri);
                using WebClient Client = GetWebClient(BaseUri, auth);
                xml = Client.DownloadString(BaseUri);
            }

            var parseArgs = new PatchXmlParser(xml);
            patchInfoParser(parseArgs);
            args = parseArgs.UpdateInfo;

            return args;
        }

        private static Attribute GetAttribute(Assembly assembly, Type attributeType)
        {
            object[] attributes = assembly.GetCustomAttributes(attributeType, false);
            if (attributes.Length == 0)
            {
                return null;
            }

            return (Attribute)attributes[0];
        }

        internal static WebClientExt GetWebClient(Uri uri, IAuthentication basicAuthentication)
        {
            var webClient = new WebClientExt
            {
                CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore)
            };
                basicAuthentication?.Apply(ref webClient);

                webClient.Headers[HttpRequestHeader.UserAgent] = UserAgent;

            return webClient;
        }

        public static void ShowUpdateDialog()
        {
            PatchDownload updateWindow = new PatchDownload();
            updateWindow.Activate();
        }
    }
}