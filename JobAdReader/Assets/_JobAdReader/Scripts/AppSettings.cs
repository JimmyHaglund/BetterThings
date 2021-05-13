using System.IO;
using UnityEngine;

namespace JobAdReader {
    class AppSettings {
        public static string CvBaseFolder => _instance.cvBaseFolder;
        public static string AppliedReportsFolder => _instance.appliedReportsFolder;
        public static string ApplicationsFolder => _instance.applicationsFolder;
        public static string StatusesFolder => _instance.statusesFolder;
        public static string SearchResultsFolder => _instance.searchResultsFolder;
        public static string CvBaseName => _instance.cvBaseName;

        public static void Load() {
            var settingsPath = Path.Combine(Application.streamingAssetsPath, "Settings.json");
            string contents = File.ReadAllText(settingsPath);
            _instance = JsonUtility.FromJson<AppSettings>(contents);
        }

        private static AppSettings _instance;
        public string cvBaseFolder = null;
        public string appliedReportsFolder = null;
        public string applicationsFolder = null;
        public string statusesFolder = null;
        public string searchResultsFolder = null;
        public string cvBaseName = null;
    }
}
