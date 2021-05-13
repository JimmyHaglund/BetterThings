using UnityEngine;
using System.Linq;

namespace JobAdReader.Scripts {
    internal class AdDisplayer : MonoBehaviour {
        [SerializeField] private DayBadge _dayBadgeTemplate = null;
        private string SearchResultFolderPath => AppSettings.SearchResultsFolder;
        private string SearchStatusFolderPath => AppSettings.SearchResultsFolder;

        private void Awake() {
            BaseCoverLetter.Load();
            AppSettings.Load();
            GenerateBadges();
        }

        private void GenerateBadges() {
            var days = AdLoader.LoadAds(SearchResultFolderPath, SearchStatusFolderPath);
            foreach(SearchDay day in days) {
                InstantiateBadge(day);
            }
        }

        private void InstantiateBadge(SearchDay day) {
            var badge = Instantiate(_dayBadgeTemplate, _dayBadgeTemplate.transform.parent);
            badge.transform.SetAsFirstSibling();
            badge.SetDay(day);
            badge.gameObject.SetActive(true);
            // badge.DisplayProgrammer();
        }
    }
}