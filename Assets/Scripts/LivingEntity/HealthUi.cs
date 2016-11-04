using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.LivingEntity
{
    public class HealthUi : MonoBehaviour {
        public LivingEntity LivingEntity;
        public RectTransform HealthBar;
        public Vector2 HealthBarMaxSize;
        public Text HealthText;

        [UsedImplicitly]
        private void Update () {
            if (LivingEntity == null) return;
            if (HealthBar != null)
                HealthBar.sizeDelta = new Vector2((float) LivingEntity.Health / LivingEntity.HealthMax * HealthBarMaxSize.x, HealthBarMaxSize.y);
            if (HealthText != null)
                HealthText.text = LivingEntity.Health.ToString();
        }
    }
}
