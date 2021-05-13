using UnityEngine;
using JimmyHaglund;
using TMPro;

namespace JobAdReader {
    internal class DayBadge : MonoBehaviour {
        [SerializeField] private DayPage _programmerDayPage = null;
        [SerializeField] private DayPage _tattarjobbDayPage = null;
        [SerializeField] [ChildReferenceButton] private TMP_Text _badgeText = null;
        private SearchDay _day;

        public void SetDay(SearchDay day) {
            var text = day.Date.ToString("yyyy-MM-dd");
            _badgeText.text = text;
            gameObject.name = text;
            _day = day;
        }

        public void DisplayProgrammer() {
            _tattarjobbDayPage.HideDisplayedPages();
            _programmerDayPage.SetAds(_day.Date, _day.ProgrammerAds);
            _programmerDayPage.Refresh();
            AdStatus.FileAppendix = "_P";
        }

        public void DisplayTattarjobb() {
            _programmerDayPage.HideDisplayedPages();
            _tattarjobbDayPage.SetAds(_day.Date, _day.TattarjobbAds);
            _tattarjobbDayPage.Refresh();
            AdStatus.FileAppendix = "_T";
        }
    }
}