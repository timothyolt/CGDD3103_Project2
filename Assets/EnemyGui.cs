//Timothy Oltjenbruns
//CGDD3101_Project1
//Updated 9/12/2016
using UnityEngine;
using System.Collections;

public class EnemyGui : MonoBehaviour
{
    private LivingEntity enemyLifeScript;
    private TextMesh textMesh;
    // Use this for initialization
    void Start()
    {
        enemyLifeScript = gameObject.GetComponentInParent<LivingEntity>();
        textMesh = gameObject.GetComponent<TextMesh>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(new Vector3(0, 180f, 0));
        if (enemyLifeScript != null && textMesh != null)
            textMesh.text = enemyLifeScript.Health.ToString();
	}
}
