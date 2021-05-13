using System;
using System.IO;

namespace JobAdReader {
    internal static class ApplicationSaver {
        public static void SaveCSV(string folderPath, JobAd ad) {
            var fileName = DateTime.Today.ToString("yyyy-MM") + ".csv";
            var filePath = Path.Combine(folderPath, fileName);
            if (!File.Exists(filePath)) {
                File.WriteAllText(filePath, GetHeadline());
            }
            using (var stream = File.AppendText(filePath)) {
                stream.WriteLine(ExtractAdData(ad));
            }
        }

        public static void CopyCV(string fromPath, string toDir, string fileName) {
            Directory.CreateDirectory(toDir);
            File.Copy(fromPath, Path.Combine(toDir, fileName));
        }

        public static void GenerateCoverLetter(string letterText, string toPath) {
            var mdFileName = "PersonligtBrev.md";
            var rawFilePath = Path.Combine(toPath, mdFileName);
            File.WriteAllText(rawFilePath, letterText);
            var command = "/C Docker run --rm -v \"" + toPath + 
                "\":/work -w /work pandoc/latex " + mdFileName + " -o PersonligtBrev.pdf";
            System.Diagnostics.Process.Start("CMD.exe", command);
            OpenFolder(toPath);

        }

        private static void OpenFolder(string path) {
            System.Diagnostics.Process.Start(@path);
        }

        private static string ExtractAdData(JobAd ad) {
            var place = ad.Town;
            if (place == "") place = ad.Municipality;

            return string.Concat(ad.Recruiter, ",", place, ",", ad.OccupationFiltered, ",", DateTime.Today.ToString("yyyy-MM-dd"));
        }

        private static string GetHeadline() {
            return "Företag, Ort, Beskrivning, Sökdatum\n";
        }
    }
}