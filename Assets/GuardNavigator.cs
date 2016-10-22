using System;
using Assets;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
public class GuardNavigator : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private float _timeSinceAttack;
    public float AttackCooldown;
    public int PunchDamage;
    public GuardTargetAction Action;
    public LivingEntity Target;
	// Use this for initialization
	void Start ()
	{
	    _navMeshAgent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _timeSinceAttack += Time.deltaTime;
	    switch (Action)
        {
            case GuardTargetAction.Idle:
                _navMeshAgent.Stop();
                break;
            case GuardTargetAction.Seek:
                _navMeshAgent.Resume();
                if (Target != null)
                    _navMeshAgent.destination = Target.transform.position;
                break;
            case GuardTargetAction.Attack:
                _navMeshAgent.Resume();
                if (_timeSinceAttack > AttackCooldown)
                    Attack();
                else if (Target != null)
                    _navMeshAgent.destination = Target.transform.position;
                break;
	        default:
	            throw new ArgumentOutOfRangeException();
        }
    }

    private void Attack()
    {
        if (Target == null) return;
        Target.Health -= PunchDamage;
        _timeSinceAttack = 0;
    }
}
