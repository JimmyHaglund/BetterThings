using UnityEngine;
using UnityEngine.Events;

namespace JobAdReader {
    public class ToggleOnlyOffEvent : MonoBehaviour {
        [SerializeField] private UnityEvent _onToggleOff = new UnityEvent();
        public void OnToggle(bool on) {
            if (on) return;
            _onToggleOff.Invoke();
        }
    }
}