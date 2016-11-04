using UnityEngine;

namespace Assets.Scripts.Ai
{
    [RequireComponent(typeof(Collider))]
    public class NavVisibilityTrigger : MonoBehaviour
    {

        public LivingEntityNavigator Navigator;

        void OnTriggerEnter(Collider other)
        {
            var livingEntity = other.gameObject.GetComponent<LivingEntity.LivingEntity>();
            if (Navigator == null || livingEntity == null || 
                livingEntity.Team == Navigator.GetComponent<LivingEntity.LivingEntity>().Team || Navigator.Action >= TargetAction.Seek) return;
            Navigator.Action = TargetAction.Seek;
            Navigator.Target = livingEntity;
        }

        void OnTriggerExit(Collider other)
        {
            var livingEntity = other.gameObject.GetComponentInChildren<LivingEntity.LivingEntity>();
            if (Navigator == null || livingEntity == null || livingEntity != Navigator.Target) return;
            Navigator.Action = TargetAction.NoTarget;
            Navigator.Target = null;
        }
    }
}
