using UnityEngine;

public class SmallEnemy : EnemyBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    //collision
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("SmallEnemy collided with: " + other.name); // Log the name of the object collided with
        if (other.CompareTag("Bullet")) {
            TakeDamage(other.GetComponent<Bullet>().damageAmount);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
