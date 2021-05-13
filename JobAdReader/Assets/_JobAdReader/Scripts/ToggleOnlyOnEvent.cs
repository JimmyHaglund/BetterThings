using UnityEngine;
using UnityEngine.Events;

namespace JobAdReader {
    public class ToggleOnlyOnEvent : MonoBehaviour {
        [SerializeField] private UnityEvent _onToggleOn = new UnityEvent();
        public void OnToggle(bool on) {
            if (!on) return;
            _onToggleOn.Invoke();
        }
    }
}