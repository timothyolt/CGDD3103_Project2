//Timothy Oltjenbruns
//CGDD3101_Project1
//Updated 9/12/2016
using UnityEngine;
using UnityEngine.UI;

public class HealthUi : MonoBehaviour {
    public LivingEntity LivingEntity;
    public RectTransform HealthBar;
    public Vector2 HealthBarMaxSize;
    public Text HealthText;
	
	void Update () {
	    if (LivingEntity == null) return;
	    if (HealthBar != null)
	        HealthBar.sizeDelta = new Vector2((float) LivingEntity.health / LivingEntity.healthMax * HealthBarMaxSize.x, HealthBarMaxSize.y);
	    if (HealthText != null)
	        HealthText.text = LivingEntity.health.ToString();
	}
}
