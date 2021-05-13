using System.Diagnostics;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System;

namespace JobAdReader {
    internal class AdPage : MonoBehaviour {
        [Header("Dependencies")]
        [SerializeField] private TMP_Text _headlineText = null;
        [SerializeField] private TMP_Text _descriptionText = null;
        [SerializeField] private TMP_Text _coverLetterText = null;
        [SerializeField] private GameObject _appliedNotifierObject = null;
        [SerializeField] private Toggle _applyButton = null;
        [Header("Info box")]
        [SerializeField] private TMP_Text _occupationText = null;
        [SerializeField] private TMP_Text _occupationGroupText = null;
        [SerializeField] private TMP_Text _occupationFieldText = null;
        [SerializeField] private TMP_Text _municipalityText = null;
        [SerializeField] private TMP_Text _townText = null;
        [SerializeField] private TMP_Text _deadlineText = null;
        // [SerializeField] private TMP_Text _afUrlText = null;
        [SerializeField] private TMP_Text _recruiterText = null;
        [SerializeField] private TMP_Text _employerText = null;
        [SerializeField] private TMP_Text _emailText = null;
        // [SerializeField] private TMP_Text _applyUrlText = null;
        [SerializeField] private GameObject _applicationUrlButton = null;
        [Header("Settings")]
        [SerializeField] private string _mailFromAdress = "JimmyHaglund@gmail.com";

        private string CsvFolderAddress => AppSettings.AppliedReportsFolder;
        private string CvPath => Path.Combine(AppSettings.CvBaseFolder, AppSettings.CvBaseName + AdStatus.FileAppendix + ".pdf");
        private string ApplicationsPath => Path.Combine(
            AppSettings.ApplicationsFolder, DateTime.Today.ToString("yyyy-MM"), ApplicationFolderName);
        private string ApplicationFolderName => _ad.OccupationFiltered.Trim() + "_" + _ad.Recruiter.Trim();

        private JobAd _ad;

        public JobAd Ad => _ad;

        public void SetAd(JobAd ad) {
            if (ad == null) return;
            _ad = ad;
            gameObject.name = ad.Headline;
            _headlineText.text = ad.Headline;
            _descriptionText.text = ad.Description;
            _appliedNotifierObject.gameObject.SetActive(ad.Applied);

            _occupationText.text = "Yrke: " + ad.OccupationFiltered;
            _occupationGroupText.text = "Yrkeskategori: " + ad.OccupationGroupFiltered;
            _occupationFieldText.text = "Yrkesfält: " + ad.OccupationField;
            _municipalityText.text = "Region: " + ad.Municipality;
            _townText.text = "Stad: " + ad.Town;
            _deadlineText.text = "Deadline: " + ad.ApplicationDeadline;
            // _afUrlText.text = ad.AfUrl;
            _recruiterText.text = "Rekryterare: " + ad.Recruiter;
            _employerText.text = "Anställare: " + ad.Workplace;
            if (ad.ApplicationEmail == "") _emailText.gameObject.SetActive(false);
            _emailText.text = "Mail: " + ad.ApplicationEmail;
            if (ad.ApplicationUrl == "") _applicationUrlButton.SetActive(false);
            // _applyUrlText.text = ad.ApplicationUrl;

            if (ad.Applied) {
                _applyButton.isOn = false;
                _applyButton.gameObject.SetActive(false);
            }

        }

        public void Display() {
            gameObject.SetActive(true);
        }

        public void Expand(bool expanded) {
            if (!expanded) {
                _applyButton.gameObject.SetActive(false);

                return;
            }
            if (!_ad.Applied) {
                _applyButton.gameObject.SetActive(true);
            }
        }

        public void OpenAfUrl() {
            Process.Start(_ad.AfUrl);
        }

        public void OpenApplicationUrl() {
            if (_ad.ApplicationUrl == default) {
                OpenApplicationEmail();
                return;
            }
            try {
                Process.Start(_ad.ApplicationUrl);
            } catch (Exception e) { }
        }

        public void RegisterApplied() {
            ApplicationSaver.SaveCSV(CsvFolderAddress, _ad);
            AdStatus.SaveStatus(_ad.Id, true);
            ApplicationSaver.CopyCV(CvPath, ApplicationsPath, "JimmyHaglund_CV.pdf");
            ApplicationSaver.GenerateCoverLetter(_coverLetterText.text, ApplicationsPath);
            OpenApplicationUrl();
            _ad.Applied = true;
            _applyButton.gameObject.SetActive(false);
            _appliedNotifierObject.gameObject.SetActive(true);
        }

        private void OpenApplicationEmail() {
            try {
                Process.Start("mailto:" + _ad.ApplicationEmail);
            } catch (Exception e) { }
        }
    }
}