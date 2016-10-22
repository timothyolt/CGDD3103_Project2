using Assets;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NavProtectTrigger : MonoBehaviour
{

    public ProtectNavigator Navigator;

    void OnTriggerEnter(Collider other)
    {
        if (Navigator == null || other.gameObject != Navigator.gameObject) return;
        Navigator.CloseEnoughToProtect = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (Navigator == null || other.gameObject != Navigator.gameObject) return;
        Navigator.CloseEnoughToProtect = false;
    }
}
