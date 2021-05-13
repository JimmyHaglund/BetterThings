using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Text;

namespace JobAdReader {
    static class AdLoader {
        [Serializable]
        public class JobAdCollection {
            public JobAd[] Ads;
        }
        public static List<SearchDay> LoadAds(string adFolderPathRoot, string statusFolderPathRoot) {
            var directories = Directory.GetDirectories(adFolderPathRoot);
            var ads = new List<SearchDay>(directories.Length);
            AdStatus.FolderPath = statusFolderPathRoot;
            foreach (var directory in directories) {
                var programmerResults = LoadAdsInDirectory(directory, "*output*programmer*");
                var tattarjobbResults = LoadAdsInDirectory(directory, "*output*tattarjobb*");

                var dateString = Regex.Match(directory, @".*\\(.*)").Result("$1");
                var date = GetDateFromString(dateString);
                var programmerStatuses = LoadDayStatuses(date, "_P", programmerResults.Length);
                var tattarjobbStatuses = LoadDayStatuses(date, "_T", tattarjobbResults.Length);

                var day = new SearchDay(date, programmerResults, tattarjobbResults);
                for (int n = 0; n < programmerStatuses.Length; n++) {
                    programmerResults[n].Applied = programmerStatuses[n];
                    programmerResults[n].Id = n;
                }
                for (int n = 0; n < tattarjobbStatuses.Length; n++) {
                    tattarjobbResults[n].Applied = tattarjobbStatuses[n];
                    tattarjobbResults[n].Id = n;
                }

                ads.Add(day);
            }
            return ads;
        }

        private static JobAd[] LoadAdsInDirectory(string directoryPath, string searchPattern) {
            var filePaths = Directory.GetFiles(directoryPath, searchPattern);
            if (filePaths.Length == 0) return null;
            try {
                var fileContents = File.ReadAllText(filePaths[0]);
                fileContents = string.Concat("{ \"Ads\":", fileContents, "}");
                return JsonUtility.FromJson<JobAdCollection>(fileContents).Ads;
            } catch (Exception e) {
                Debug.LogError(e.Message);
                return null;
            }
        }

        private static DateTime GetDateFromString(string dateString) {
            var split = dateString.Split('-');
            var year = int.Parse(split[0]);
            var month = int.Parse(split[1]);
            var day = int.Parse(split[2]);
            return new DateTime(year, month, day);
        }

        private static bool[] LoadDayStatuses(DateTime date, string appendix, int numberOfAds) {
            AdStatus.SetDate(date);
            AdStatus.FileAppendix = appendix;
            AdStatus.ActiveNumberOfAds = numberOfAds;
            return AdStatus.LoadStatuses();
        }
    }
}