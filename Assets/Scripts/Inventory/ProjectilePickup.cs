using Assets.Scripts.Ai;
using UnityEngine;

namespace Assets.Scripts.Inventory
{
    [RequireComponent(typeof(Rigidbody))]
    public class ProjectilePickup : Pickup {
        public int Damage;
        public float LifeTime;

        void Update ()
        {
            LifeTime += Time.deltaTime;
            if (transform.position.y < -1000)
                Destroy(gameObject);
        }

        void OnCollisionEnter(Collision collision)
        {
            if (LifeTime < 5)
            {
                var lifeScript = collision.gameObject.GetComponent<LivingEntity.LivingEntity>();
                if (lifeScript == null) return;
                lifeScript.Health -= Damage;
                Destroy(gameObject);
            }
        }
    }
}
