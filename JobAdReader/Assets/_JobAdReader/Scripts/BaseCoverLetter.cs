using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

namespace JobAdReader {
    static class BaseCoverLetter {
        public static string Text;

        public static void Load() {
            var path = Path.Combine(Application.streamingAssetsPath, "CoverLetterBase.md");
            Text = File.ReadAllText(path);
        }
    }
}
