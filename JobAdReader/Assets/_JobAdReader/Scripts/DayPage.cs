using System;
using System.Collections.Generic;
using UnityEngine;

namespace JobAdReader {
    internal class DayPage : MonoBehaviour {
        [SerializeField] private AdPage _adPageTemplate;
        private DateTime _displayedDate;
        private DateTime _desiredDisplayedDate;
        private Dictionary<DateTime, DayPage> _adPages = new Dictionary<DateTime, DayPage>();
        private Vector2 _defaultPosition;

        public void SetAds(DateTime date, IReadOnlyCollection<JobAd> ads) {
            if (!_adPages.ContainsKey(date)) {
                var newPage = Instantiate(this, transform.parent);
                newPage.gameObject.name = gameObject.name + "_" + date.ToString("yyyy-MM-dd");
                newPage.gameObject.SetActive(true);
                newPage.Hide();
                newPage.GenerateAdpages(ads);
                _adPages.Add(date, newPage);
            }
            _desiredDisplayedDate = date;
        }

        public void Refresh() {
            HideDisplayedPages();
            if (_adPages.TryGetValue(_desiredDisplayedDate, out var newDisplay)) {
                newDisplay.Show();
            }
            AdStatus.SetDate(_desiredDisplayedDate);
            _displayedDate = _desiredDisplayedDate;
        }

        public void HideDisplayedPages() {
            if (_adPages.TryGetValue(_displayedDate, out var displayed)) {
                displayed.Hide();
            }
        }

        private void Hide() {
            var rectTransform = transform as RectTransform;
            rectTransform.anchoredPosition = new Vector3(3000, 0, 0);
        }

        private List<AdPage> GenerateAdpages(IReadOnlyCollection<JobAd> ads) {
            var newPages = new List<AdPage>(ads.Count);
            foreach(var ad in ads) {
                var adPage = Instantiate(_adPageTemplate, _adPageTemplate.transform.parent);
                adPage.SetAd(ad);
                newPages.Add(adPage);
                adPage.gameObject.SetActive(true);
            }
            return newPages;
        }

        private void Show() {
            var rectTransform = transform as RectTransform;
            rectTransform.anchoredPosition = _defaultPosition;
        }

        private void Awake() {
            var rectTransform = transform as RectTransform;
            _defaultPosition = rectTransform.anchoredPosition;
        }
    }
}