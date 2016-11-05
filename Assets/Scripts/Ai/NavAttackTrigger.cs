using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Ai {
    [RequireComponent(typeof(Collider))]
    public class NavAttackTrigger : MonoBehaviour {
        public LivingEntityNavigator Navigator;

        [UsedImplicitly]
        private void OnTriggerEnter(Collider other) {
            var livingEntity = other.gameObject.GetComponent<LivingEntity.LivingEntity>();
            if (Navigator == null || livingEntity == null ||
                livingEntity.Team == Navigator.GetComponent<LivingEntity.LivingEntity>().Team ||
                Navigator.Action >= TargetAction.Attack) return;
            Navigator.Action = TargetAction.Attack;
            Navigator.Target = livingEntity;
        }

        [UsedImplicitly]
        private void OnTriggerExit(Collider other) {
            var livingEntity = other.gameObject.GetComponentInChildren<LivingEntity.LivingEntity>();
            if (Navigator == null || livingEntity == null || livingEntity != Navigator.Target) return;
            Navigator.Action = TargetAction.Seek;
        }
    }
}