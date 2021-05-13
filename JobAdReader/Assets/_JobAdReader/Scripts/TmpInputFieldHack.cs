using UnityEngine;
using TMPro;

namespace JobAdReader {
    public class TmpInputFieldHack : MonoBehaviour {
        [Header("When setting the text of a TMP input field it uses getcomponent internally to get the text. " +
            "This means it won't work if not currently enabled, so we have to do it when it's enabled. " +
            "That' why this steaming-hot garbage script exists.")]
        [SerializeField] private AdPage _adPage;
        private void Start() {
            var inputField = GetComponent<TMP_InputField>();
            if (inputField == null) return;
            var ad = _adPage.Ad;
            inputField.text = string.Format(BaseCoverLetter.Text, ad.OccupationFiltered, ad.Recruiter);
            Destroy(this);
        }
    }
}