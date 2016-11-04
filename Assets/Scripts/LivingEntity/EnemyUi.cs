//Timothy Oltjenbruns
//CGDD3101_Project1
//Updated 9/12/2016

using UnityEngine;

namespace Assets.Scripts.LivingEntity
{
    public class EnemyUi : MonoBehaviour
    {
        public LivingEntity enemyLifeScript;
        private TextMesh textMesh;
        // Use this for initialization
        void Start()
        {
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
}