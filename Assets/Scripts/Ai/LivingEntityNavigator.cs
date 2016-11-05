using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Ai {
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(LivingEntity.LivingEntity))]
    public abstract class LivingEntityNavigator : MonoBehaviour {
        public TargetAction Action;
        public float AttackCooldown;
        public int PunchDamage;
        public LivingEntity.LivingEntity Target;
        public float TimeSinceAttack;

        [UsedImplicitly]
        private void Update() {
            TimeSinceAttack += Time.deltaTime;
            switch (Action) {
                case TargetAction.NoTarget:
                    NoTarget();
                    break;
                case TargetAction.Seek:
                    Seek();
                    break;
                case TargetAction.Attack:
                    if (TimeSinceAttack > AttackCooldown)
                        Attack();
                    else
                        Seek();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected abstract void NoTarget();
        protected abstract void Attack();
        protected abstract void Seek();
    }
}