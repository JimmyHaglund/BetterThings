using System.IO;
using System.Text;
using System;

namespace JobAdReader {
    public static class AdStatus {
        public static string ActiveFilePath => Path.Combine(FolderPath, _dateString + FileAppendix);
        public static string FileAppendix;
        public static int ActiveNumberOfAds;
        public static string FolderPath;
        private static string _dateString;

        public static bool[] LoadStatuses() {
            var filePath = ActiveFilePath;
            var result = new bool[ActiveNumberOfAds];
            if (!File.Exists(filePath)) {
                using (var stream = File.Create(filePath)) {
                    var statusString = "";
                    for (int n = 0; n < ActiveNumberOfAds; n++) {
                        statusString += "N ";
                    }
                    byte[] info = new UTF8Encoding(true).GetBytes(statusString);
                    stream.Write(info, 0, info.Length);
                }
                return result;
            }
            var fileString = File.ReadAllText(filePath);
            var split = fileString.Split(' ');
            for (int n = 0; n < result.Length; n++) {
                result[n] = split[n] == "Y";
            }
            return result;
        }

        public static void SaveStatus(int index, bool status) {
            var statusSymbol = status ? "Y" : "N";
            string[] statuses;
            using (var stream = File.OpenText(ActiveFilePath)) {
                var data = stream.ReadLine();
                statuses = data.Split(' ');
                statuses[index] = statusSymbol;
            }
            var writeData = string.Join(" ", statuses);
            File.WriteAllText(ActiveFilePath, writeData);
        }

        public static void SetDate(DateTime date) {
            _dateString = date.ToString("yyyy-MM-dd");
        }
    }
}