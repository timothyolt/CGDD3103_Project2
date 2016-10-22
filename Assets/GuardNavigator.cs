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
    public GuardState GuardState;
    public LivingEntity Target;
    public SphereCollider VisibilityTrigger;
    public SphereCollider AttackTrigger;
	// Use this for initialization
	void Start ()
	{
	    _navMeshAgent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _timeSinceAttack += Time.deltaTime;
	    switch (GuardState)
        {
            case GuardState.Idle:
                _navMeshAgent.Stop();
                break;
            case GuardState.Seek:
                _navMeshAgent.Resume();
                if (Target != null)
                    _navMeshAgent.destination = Target.transform.position;
                break;
            case GuardState.Attack:
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
