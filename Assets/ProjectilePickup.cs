using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectilePickup : Pickup {
    public int Damage;
    public float LifeTime;

	void Update ()
    {
        LifeTime += Time.deltaTime;
        if (transform.position.y < -1000)
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (LifeTime < 5)
        {
            var lifeScript = collision.gameObject.GetComponent<LivingEntity>();
            if (lifeScript == null) return;
            lifeScript.Health -= Damage;
            Destroy(gameObject);
        }
    }
}
