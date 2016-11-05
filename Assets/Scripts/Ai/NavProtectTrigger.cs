using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Ai {
    [RequireComponent(typeof(Collider))]
    public class NavProtectTrigger : MonoBehaviour {
        public ProtectNavigator Navigator;

        [UsedImplicitly]
        private void OnTriggerEnter(Collider other) {
            if (Navigator == null || other.gameObject != Navigator.gameObject) return;
            Navigator.CloseEnoughToProtect = true;
        }

        [UsedImplicitly]
        private void OnTriggerExit(Collider other) {
            if (Navigator == null || other.gameObject != Navigator.gameObject) return;
            Navigator.CloseEnoughToProtect = false;
        }
    }
}