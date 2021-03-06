using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.LivingEntity {
    public class EnemyUi : MonoBehaviour {
        private TextMesh _textMesh;
        public LivingEntity EnemyLifeScript;


        [UsedImplicitly]
        private void Start() {
            _textMesh = gameObject.GetComponent<TextMesh>();
        }

        [UsedImplicitly]
        private void Update() {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(new Vector3(0, 180f, 0));
            if (EnemyLifeScript != null && _textMesh != null)
                _textMesh.text = EnemyLifeScript.Health.ToString();
        }
    }
}