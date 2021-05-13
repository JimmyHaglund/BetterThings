using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JobAdReader {
    [Serializable]
    internal class ApplicationStatus {
        private static ApplicationStatus _instance;
        private static JobAd[] _ads;
        public ApplicationDayStatus[] Days;

        public static void SetDays(IEnumerable<SearchDay> days) {
            
        }

        public static void Load(string filePath) {
            if (!File.Exists(filePath)) {
                File.Create(filePath);
            }
            var fileString = File.ReadAllText(filePath);
            _instance = JsonUtility.FromJson<ApplicationStatus>(filePath);
        }
    }

    [Serializable] public class ApplicationDayStatus {
        public string Date;
        public string Applied = "N";
    }
}