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
            GuardNavigator.Action >= GuardTargetAction.Seek) return;
        GuardNavigator.Action = GuardTargetAction.Seek;
        GuardNavigator.Target = livingEntity;
    }

    void OnTriggerExit(Collider other)
    {
        var livingEntity = other.gameObject.GetComponentInChildren<LivingEntity>();
        if (GuardNavigator == null || livingEntity == null || livingEntity != GuardNavigator.Target) return;
        GuardNavigator.Action = GuardTargetAction.Idle;
        GuardNavigator.Target = null;
    }
}
