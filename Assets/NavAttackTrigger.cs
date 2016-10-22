using Assets;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NavAttackTrigger : MonoBehaviour
{

    public LivingEntityNavigator Navigator;

    void OnTriggerEnter(Collider other)
    {
        int work;
        var livingEntity = other.gameObject.GetComponent<LivingEntity>();
        if (Navigator is ProtectNavigator && livingEntity != null)
            work = 0;
        if (Navigator == null || livingEntity == null ||
            livingEntity.Team == Navigator.GetComponent<LivingEntity>().Team || Navigator.Action >= TargetAction.Attack) return;
        Navigator.Action = TargetAction.Attack;
        Navigator.Target = livingEntity;
    }

    void OnTriggerExit(Collider other)
    {
        var livingEntity = other.gameObject.GetComponentInChildren<LivingEntity>();
        if (Navigator == null || livingEntity == null || livingEntity != Navigator.Target) return;
        Navigator.Action = TargetAction.Seek;
    }
}
