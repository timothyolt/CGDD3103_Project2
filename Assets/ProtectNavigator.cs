using Assets;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
public class ProtectNavigator : LivingEntityNavigator
{
    protected NavMeshAgent NavMeshAgent;
    public GameObject Protect;
    public bool CloseEnoughToProtect;

	void Start ()
	{
	    NavMeshAgent = GetComponent<NavMeshAgent>();
	    if (Protect.GetComponentInChildren<NavProtectTrigger>() != null)
	        Protect.GetComponentInChildren<NavProtectTrigger>().Navigator = this;
        if (Protect.GetComponentInChildren<NavVisibilityTrigger>() != null)
            Protect.GetComponentInChildren<NavVisibilityTrigger>().Navigator = this;
    }

    protected override void NoTarget()
    {
        if (Protect == null) return;
        if (!CloseEnoughToProtect)
        {
            NavMeshAgent.Resume();
            NavMeshAgent.destination = Protect.transform.position;
        }
        else
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

    protected override void Seek()
    {
        NavMeshAgent.Resume();
        if (Target != null)
            NavMeshAgent.destination = Target.transform.position;
        else
            Action = TargetAction.NoTarget;
    }
}
