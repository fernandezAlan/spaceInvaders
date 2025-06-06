using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour 
{
    public Rigidbody heroRigidBody; // Reference to the hero's rigid body object
    public float accelerationRate = 500f;
    public float maxVelocity = 1500f;

    private void Awake()
    {
        heroRigidBody = GetComponent<Rigidbody>();
    }
    private void getPlayerInput() 
    {
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
                heroRigidBody.AddForce(accelerationRate * Time.deltaTime,0f,0f);
            }
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (heroRigidBody.linearVelocity.x >= -maxVelocity)
            {
                heroRigidBody.AddForce(-accelerationRate * Time.deltaTime, 0f, 0f);
            }
        }
    }

    private void Update()
    {
        getPlayerInput();
    }
}