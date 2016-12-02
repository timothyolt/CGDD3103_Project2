using Assets.Scripts.Inventory;
using Assets.Scripts.Inventory.Items;
using UnityEngine;

namespace Assets.Scripts.Ai {
    [RequireComponent(typeof(NavMeshAgent))]
    public class PatrolNavigator : GuardNavigator {
        private int _waypointTargetIndex;
        public GameObject[] Waypoints = new GameObject[5];

        protected override void NoTarget() {
            var waypoint = Waypoints[_waypointTargetIndex];
            if (waypoint == null) {
                _waypointTargetIndex = (_waypointTargetIndex + 1)%Waypoints.Length;
                return;
            }
            var sqrDistance2D =
            (new Vector2(waypoint.transform.position.x, waypoint.transform.position.z) -
             new Vector2(transform.position.x, transform.position.z)).sqrMagnitude;
            if (sqrDistance2D < 4)
                _waypointTargetIndex = (_waypointTargetIndex + 1)%Waypoints.Length;
            else
                NavMeshAgent.destination = Waypoints[_waypointTargetIndex].transform.position;
        }

        protected override void Attack()
        {
            if (Target == null)
            {
                Action = TargetAction.NoTarget;
                return;
            }
            NavMeshAgent.Resume();
            var forward = transform.forward;
            forward.y += 1;
            var force = transform.forward;
            force.Scale(new Vector3(500, 1, 500));
            var itemDrop =
                Instantiate(Resources.Load<GameObject>(Item.FromId(Item.ItemId.Shot1).PrefabString),
                    transform.position + forward + new Vector3(0, 1, 0), transform.rotation) as GameObject;
            itemDrop?.GetComponent<ProjectilePickup>().Activate(transform.forward);
            TimeSinceAttack = 0;
        }
    }
}