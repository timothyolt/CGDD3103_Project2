//Timothy Oltjenbruns

using Assets;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{

    public Team Team;
    public int HealthRegen;
    public int HealthTimeToHeal;
    public int HealthMax;

    private int _health;
    public float TimeSinceDamage { get; private set; }
    public float TimeSinceHeal { get; private set; }
    public int Health {
        get { return _health; }
        set {
            if (value < _health)
                TimeSinceDamage = 0;
            else
                TimeSinceHeal = 0;
            _health = value;
            if (_health <= 0)
                Destroy(gameObject);
            else if (_health > HealthMax)
                _health = HealthMax;
        }
    }
    private void TimeHealth()
    {
        TimeSinceDamage += Time.deltaTime;
        TimeSinceHeal += Time.deltaTime;
        if (TimeSinceDamage > HealthTimeToHeal && TimeSinceHeal > HealthTimeToHeal && Health < HealthMax)
            Health += HealthRegen;
    }
    
	void Start () {
        Health = HealthMax;
	}
	
	void Update () {
        TimeHealth();
	}
}
