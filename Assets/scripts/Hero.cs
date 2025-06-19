using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour 
{
    public Rigidbody heroRigidBody; // Reference to the hero's rigid body object
    public float accelerationRate = 500f;
    public GameObject bullet;
    public float maxVelocity = 1500f;
    private float fireRate = 1f; // Time in seconds between shots
    private float defaultFireRate = 1f; // Default fire rate
    private void Awake()
    {
        heroRigidBody = GetComponent<Rigidbody>();
    }
    private void getPlayerInput() 
    {
        Debug.Log("getPlayerInput called"); // Log when the method is called
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (heroRigidBody.linearVelocity.z <= maxVelocity) 
            {
                heroRigidBody.AddForce(0f, 0f, accelerationRate * Time.deltaTime);
            }
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if (heroRigidBody.linearVelocity.z >= -maxVelocity)
            {
                heroRigidBody.AddForce(0f, 0f, -accelerationRate * Time.deltaTime);
            }
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (heroRigidBody.linearVelocity.x <= maxVelocity)
            {
                heroRigidBody.AddForce(accelerationRate * Time.deltaTime, 0f, 0f);
                transform.rotation = Quaternion.Euler(0, 0, -20);
            }
        }
        else {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (heroRigidBody.linearVelocity.x >= -maxVelocity)
            {
                heroRigidBody.AddForce(-accelerationRate * Time.deltaTime, 0f, 0f);
                // Rota el objeto 45 grados en el eje X
                transform.rotation = Quaternion.Euler(0, 0, 20);
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (fireRate > 0) fireRate -= Time.deltaTime; // Decrease fire rate timer
        
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButtonDown(0)) && fireRate <=0)
        {
            FireBullet();
            fireRate = defaultFireRate; // Reset fire rate timer
        }
    }

    private void FireBullet()
    {
        Instantiate(bullet, transform.position, Quaternion.identity);
    }
    private void Update()
    {
        getPlayerInput();
       
    }
}