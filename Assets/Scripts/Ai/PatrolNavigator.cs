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

        protected override void Attack() {
            if (Target == null) {
                Action = TargetAction.NoTarget;
                return;
            }
            NavMeshAgent.Resume();
            Target.Health -= PunchDamage;
            TimeSinceAttack = 0;
        }
    }
}