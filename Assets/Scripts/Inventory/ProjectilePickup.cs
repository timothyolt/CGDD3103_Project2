using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Inventory {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class ProjectilePickup : Pickup
    {
        public int Damage;
        public float MaxVolatileTime;
        public float ActivationForceStrength;

        public void Activate(Vector3 direction)
        {
            direction.Scale(new Vector3(ActivationForceStrength, 0, ActivationForceStrength));
            GetComponent<Rigidbody>()?.AddForce(direction);
            VolatileTime = MaxVolatileTime;
        }

        private float _volatileTime; 
        public float VolatileTime
        {
            get { return _volatileTime; }
            set
            {
                _volatileTime = value;
                const float threshold = 5;
                if (_volatileTime < 0 || _expand == null) return;
                var lerp = _volatileTime/threshold;
                var scale = Mathf.Clamp(1 - lerp, 0, 1);
                _expand.localScale = new Vector3(scale, scale, scale);
                GetComponent<Collider>().material.bounciness = Mathf.Clamp(lerp, 0, 1);
            }
        }

        private Transform _expand;

        public ProjectilePickup(float maxVolatileTime)
        {
            MaxVolatileTime = maxVolatileTime;
        }

        [UsedImplicitly]
        private void Start()
        {
            _expand = transform.FindChild("Circle");
            //Delayed setter call from inspector
            VolatileTime = _volatileTime;
        }

        [UsedImplicitly]
        private void Update() {
            if (VolatileTime > 0)
                VolatileTime -= Time.deltaTime;
            if (transform.position.y < -1000)
                Destroy(gameObject);
        }

        [UsedImplicitly]
        private void OnCollisionEnter(Collision collision) {
            if (VolatileTime <= 0) return;
            var lifeScript = collision.gameObject.GetComponent<LivingEntity.LivingEntity>();
            if (lifeScript == null) return;
            lifeScript.Health -= Damage;
            //Destroy(gameObject);
        }
    }
}