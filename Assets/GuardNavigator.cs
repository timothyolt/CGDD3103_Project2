using Assets;
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
        var itemDrop = Instantiate(Resources.Load<GameObject>(Item.GetPrefabResource(Item.Id.Shot1)), transform.position + forward, transform.rotation) as GameObject;
        if (itemDrop != null && itemDrop.GetComponent<Rigidbody>() != null)
            itemDrop.GetComponent<Rigidbody>().AddForce(force);
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
