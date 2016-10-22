using Assets;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GuardNavVisibility : MonoBehaviour
{

    public GuardNavigator GuardNavigator;

    void OnTriggerEnter(Collider other)
    {
        var livingEntity = other.gameObject.GetComponentInChildren<LivingEntity>();
        if (GuardNavigator == null || livingEntity == null ||
            GuardNavigator.GuardState >= GuardState.Seek) return;
        GuardNavigator.GuardState = GuardState.Seek;
        GuardNavigator.Target = livingEntity;
    }

    void OnTriggerExit(Collider other)
    {
        var livingEntity = other.gameObject.GetComponentInChildren<LivingEntity>();
        if (GuardNavigator == null || livingEntity == null || livingEntity != GuardNavigator.Target) return;
        GuardNavigator.GuardState = GuardState.Idle;
        GuardNavigator.Target = null;
    }
}
