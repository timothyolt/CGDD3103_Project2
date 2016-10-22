using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
public class GuardNavigator : LivingEntityNavigator
{
    protected NavMeshAgent NavMeshAgent;
	void Start ()
	{
	    NavMeshAgent = GetComponent<NavMeshAgent>();
	}

    protected override void NoTarget()
    {
        NavMeshAgent.Stop();
    }

    protected override void Attack()
    {
        if (Target == null) return;
        NavMeshAgent.Resume();
        Target.Health -= PunchDamage;
        TimeSinceAttack = 0;
    }

    protected override void Seek()
    {
        NavMeshAgent.Resume();
        if (Target != null)
            NavMeshAgent.destination = Target.transform.position;
    }
}
