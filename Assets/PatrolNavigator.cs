using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
public class PatrolNavigator : GuardNavigator
{
    public GameObject[] Waypoints = new GameObject[5];
    private int _waypointTargetIndex;

    protected override void NoTarget()
    {
        var waypoint = Waypoints[_waypointTargetIndex];
        if (waypoint == null)
        {
            _waypointTargetIndex = (_waypointTargetIndex+1)%Waypoints.Length;
            return;
        }
        var sqrDistance2D = (new Vector2(waypoint.transform.position.x, waypoint.transform.position.z) - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude;
        if (sqrDistance2D < 4)
            _waypointTargetIndex = (_waypointTargetIndex + 1) % Waypoints.Length;
        else
            NavMeshAgent.destination = Waypoints[_waypointTargetIndex].transform.position;
    }

}
