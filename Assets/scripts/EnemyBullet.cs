using UnityEngine;

public class EnemyBullet : Bullet
{
    public float moveSpeed = 5f; // Speed of the bullet

    protected override void Start()
    {
        base.Start(); // Call the base class Start method to handle lifetime
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        EnemyStatsSO stats = gm.GetCurrentEnemyStats();
        damageAmount = stats.damage; // Set the damage amount from the GameManager's current enemy stats
    }
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
