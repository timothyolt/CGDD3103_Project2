using Assets.Scripts.Inventory;
using Assets.Scripts.Inventory.Items;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Ai {
    [RequireComponent(typeof(NavMeshAgent))]
    public class GuardNavigator : LivingEntityNavigator {
        protected NavMeshAgent NavMeshAgent;

        [UsedImplicitly]
        private void Start() {
            NavMeshAgent = GetComponent<NavMeshAgent>();
        }

        protected override void NoTarget() {
            NavMeshAgent.Stop();
        }
        protected override void Attack()
        {
            if (Target == null)
            {
                Action = TargetAction.NoTarget;
                return;
            }
            NavMeshAgent.Resume();
            Target.Health -= PunchDamage;
            TimeSinceAttack = 0;
        }

        protected override void Seek() {
            NavMeshAgent.Resume();
            if (Target != null)
                NavMeshAgent.destination = Target.transform.position;
            else
                Action = TargetAction.NoTarget;
        }
    }
}