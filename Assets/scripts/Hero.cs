using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Ship
{
    public Rigidbody heroRigidBody; // Reference to the hero's rigid body object
    public float accelerationRate = 500f;
    public float maxVelocity = 1500f;
    public float fireCooldown = 0.5f;
    private float cooldownTimer = 0f;
    public float speed = 5f;
    public float totalHealth = 100f; // Health of the hero
    public float currentHealth = 100f; // Current health of the hero
    private GameManager gameManager; // Reference to the GameManager
    private void Awake()
    {
        heroRigidBody = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }
 
    // Called when the hero collides with an object
    
    private void TakeDamage(float damage)
    {
        currentHealth = TakeDamage(damage, currentHealth, totalHealth); // Call the base class TakeDamage method
        if (currentHealth <= 0)
        {
            int lives = gameManager.GetLives();
            int newLives = lives - 1;
            gameManager.SetLives(newLives);
            gameObject.SetActive(false);
            Invoke("Respawn", 1f);
        }
    }

    protected override void Respawn()
    {
        base.Respawn(); // Call the base class Respawn method to restore health bar and sprite color
        currentHealth = totalHealth;

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hero collided with: " + other.name); // Log the name of the object collided with
        if (other.CompareTag("EnemyBullet"))
        {
            TakeDamage(other.GetComponent<EnemyBullet>().damageAmount);
        }
    }
 
    private void Update()
    {
        // Countdown for shooting cooldown
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && cooldownTimer <= 0f)
        {
          
            FireBullet();
            cooldownTimer = fireCooldown;
        }
        float moveX = Input.GetAxis("Horizontal"); // A/D o flechas izq/der
        float moveZ = Input.GetAxis("Vertical");   // W/S o flechas arriba/abajo

        Vector3 move = new Vector3(moveX, 0f, moveZ); // Movimiento en plano XZ

        transform.position += move * speed * Time.deltaTime;

    }
}