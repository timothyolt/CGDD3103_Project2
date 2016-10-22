﻿using System;
using Assets;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(LivingEntity))]
public abstract class LivingEntityNavigator : MonoBehaviour
{
    public float TimeSinceAttack;
    public float AttackCooldown;
    public int PunchDamage;
    public TargetAction Action;
    public LivingEntity Target;
	
	void Update ()
	{
	    TimeSinceAttack += Time.deltaTime;
	    switch (Action)
        {
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