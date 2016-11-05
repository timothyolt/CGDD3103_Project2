using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.LivingEntity {
    public class LivingEntity : MonoBehaviour {
        private int _health;
        public int HealthMax;
        public int HealthRegen;
        public int HealthTimeToHeal;
        public Team Team;
        public float TimeSinceDamage { get; private set; }
        public float TimeSinceHeal { get; private set; }

        public int Health {
            get { return _health; }
            set {
                if (value < _health)
                    TimeSinceDamage = 0;
                else
                    TimeSinceHeal = 0;
                _health = value;
                if (_health <= 0)
                    Destroy(gameObject);
                else if (_health > HealthMax)
                    _health = HealthMax;
            }
        }

        private void TimeHealth() {
            TimeSinceDamage += Time.deltaTime;
            TimeSinceHeal += Time.deltaTime;
            if (TimeSinceDamage > HealthTimeToHeal && TimeSinceHeal > HealthTimeToHeal && Health < HealthMax)
                Health += HealthRegen;
        }

        [UsedImplicitly]
        private void Start() {
            Health = HealthMax;
        }

        [UsedImplicitly]
        private void Update() {
            TimeHealth();
        }
    }
}