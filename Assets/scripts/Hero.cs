using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour 
{
    public Rigidbody heroRigidBody; // Reference to the hero's rigid body object
    public float accelerationRate = 500f;
    public GameObject bullet;
    public float maxVelocity = 1500f;
    public float fireCooldown = 0.5f;
    private float cooldownTimer = 0f;
    public float speed = 5f;
    public float health = 100f; // Health of the hero
    public HealthBarUI healthBar; // Referencia a la barra de vida
    private GameManager gameManager; // Reference to the GameManager
    private void Awake()
    {
        heroRigidBody = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0f, 100f);

        if (healthBar != null)
        {
            healthBar.SetHealth(health, 100f); // Actualizá la barra
        }

        if (health <= 0)
        {
            int lives = gameManager.GetLives();
            int newLives = lives - 1;
            gameManager.SetLives(newLives);
            gameObject.SetActive(false);
            Invoke("Respawn", 1f);
            Debug.LogWarning("Hero is already dead!");
        }
    }

    private void Respawn()
    {
        health = 100f;

        if (healthBar != null)
        {
            healthBar.SetHealth(health, 100f); // Restaurá la barra a full
        }

        gameObject.SetActive(true);
        Debug.Log("Hero respawned!");
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hero collided with: " + other.name); // Log the name of the object collided with
        if (other.CompareTag("EnemyBullet"))
        {
            TakeDamage(other.GetComponent<EnemyBullet>().damageAmount);
        }
    }

    private void FireBullet()
    {
      
        Instantiate(bullet, transform.position, Quaternion.identity);
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