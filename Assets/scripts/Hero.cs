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
    public int totalHealth = 100; // Health of the hero
    public int currentHealth = 100; // Current health of the hero
    private GameManager gameManager; // Reference to the GameManager
    protected override void Awake()
    {
        base.Awake(); // Call the base class Awake method to initialize audio source, sprite renderer, etc.
        heroRigidBody = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }
 
    // Called when the hero collides with an object
    
    private void TakeDamage(int damage)
    {
        gameManager.AddDamage(damage); // Add damage to the total damage received
        currentHealth = TakeDamage(damage, currentHealth, totalHealth); // Call the base class TakeDamage method
        if (currentHealth <= 0)
        {
            int lives = gameManager.GetLives();
            int newLives = lives - 1;
            gameManager.SetLives(newLives);
            //gameObject.SetActive(false);
            StartSplosion(); // Call the method to start the explosion effect
            gameManager.Invoke("RespawnPlayer", 1f); // Espera 1 segundo antes de reaparecer
            Destroy(gameObject);
        }
    }

    protected override void Respawn()
    {
        base.Respawn(); // Call the base class Respawn method to restore health bar and sprite color
        Instantiate(gameObject); // Instantiate a new hero object
        currentHealth = totalHealth;
        transform.localPosition = initPos; // Reset position to initial position
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            TakeDamage(other.GetComponent<EnemyBullet>().damageAmount);
        }
        if (other.CompareTag("Enemy"))
        {
            // Si el enemigo colisiona con el jugador, se destruye a sí mismo
            TakeDamage(totalHealth);
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Si el enemigo colisiona con el jugador, se destruye a sí mismo
            TakeDamage(totalHealth);
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