using Assets;

using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GuardNavAttack : MonoBehaviour
{

    public GuardNavigator GuardNavigator;

    void OnTriggerEnter(Collider other)
    {
        var livingEntity = other.gameObject.GetComponentInChildren<LivingEntity>();
        if (GuardNavigator == null || livingEntity == null ||
            GuardNavigator.GuardState >= GuardState.Attack) return;
        GuardNavigator.GuardState = GuardState.Attack;
        GuardNavigator.Target = livingEntity;
    }
}
