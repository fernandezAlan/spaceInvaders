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
    private GameManager gameManager; // Reference to the GameManager
    private void Awake()
    {
        heroRigidBody = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void TakeDamage(float damage)
    {
        health -= damage; // Reduce health by the damage amount
        if (health <=0)
        {
            //game manager
            int lives = gameManager.GetLives(); // Get current lives from GameManager
            int newLives = lives - 1; // Decrease lives by 1
            gameManager.SetLives(newLives); // Update lives in GameManager
            gameObject.SetActive(false); // Oculta al héroe
            Invoke("Respawn", 1f);
            Debug.LogWarning("Hero is already dead!"); // Log a warning if the hero is already dead 
        }

    }

    private void Respawn()
    {
        health = 100f; // Reset health to 100
        //transform.position = Vector3.zero; // Reset position to the origin
        gameObject.SetActive(true); // Reactivate the hero
        Debug.Log("Hero respawned!"); // Log when the hero respawns
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