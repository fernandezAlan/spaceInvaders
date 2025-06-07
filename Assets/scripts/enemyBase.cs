using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private float health = 100f; // Health of the enemy
   
    public void TakeDamage(float damageAmount) 
    {
        Debug.LogWarning("taking damege:" + damageAmount); // Log the damage amount
        health -= damageAmount; // Reduce health by the damage amount
        if (health <= 0f) 
        {
            Destroy(gameObject); // Call Die method if health is zero or less
        }
    }
}
