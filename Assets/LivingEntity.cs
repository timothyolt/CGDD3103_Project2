//Timothy Oltjenbruns
//CGDD3101_Project1
//Updated 9/14/2016
using UnityEngine;
using System.Collections;

public class LivingEntity : MonoBehaviour
{

    public int healthRegen;
    public int healthTimeToHeal;
    public int healthMax;

    private int _health;
    public float timeSinceDamage { get; private set; }
    public float timeSinceHeal { get; private set; }
    public int health {
        get { return _health; }
        set {
            if (value < _health)
                timeSinceDamage = 0;
            else
                timeSinceHeal = 0;
            _health = value;
            if (_health <= 0)
                Destroy(gameObject);
            else if (_health > healthMax)
                _health = healthMax;
        }
    }
    private void timeHealth()
    {
        timeSinceDamage += Time.deltaTime;
        timeSinceHeal += Time.deltaTime;
        if (timeSinceDamage > healthTimeToHeal && timeSinceHeal > healthTimeToHeal && health < healthMax)
            health += healthRegen;
    }


	// Use this for initialization
	void Start () {
        health = healthMax;
	}
	
	// Update is called once per frame
	void Update () {
        timeHealth();
	}
}
