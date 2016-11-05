using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Inventory {
    [RequireComponent(typeof(Rigidbody))]
    public class ProjectilePickup : Pickup {
        public int Damage;
        public float LifeTime;

        [UsedImplicitly]
        private void Update() {
            LifeTime += Time.deltaTime;
            if (transform.position.y < -1000)
                Destroy(gameObject);
        }

        [UsedImplicitly]
        private void OnCollisionEnter(Collision collision) {
            if (!(LifeTime < 5)) return;
            var lifeScript = collision.gameObject.GetComponent<LivingEntity.LivingEntity>();
            if (lifeScript == null) return;
            lifeScript.Health -= Damage;
            Destroy(gameObject);
        }
    }
}