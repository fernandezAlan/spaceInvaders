using UnityEngine;

public class EnemyBullet : Bullet
{
    public float moveSpeed = 5f; // Speed of the bullet
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bullet collided with: " + other.name); // Log the name of the object collided with
        if (other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        transform.Translate(0f, 0f, -moveSpeed * Time.deltaTime);
    }
}
