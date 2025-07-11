using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image fillImage;

    public void SetHealth(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }
    public void restoreHealth()
    {
        fillImage.fillAmount = 1f; // Restore health bar to full
    }

    public void desactiveHealthBar() { 
    gameObject.SetActive(false); // Hide the health bar
    }
    public void activeHealthBar()
    {
        gameObject.SetActive(true); // Show the health bar
    }
}