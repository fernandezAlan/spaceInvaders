using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float moveSpeed = 5f; // Speed of the bullet
    private float lifetime = 5f; // Lifetime of the bullet in seconds
    public float damageAmount = 25f; // Damage amount dealt by the bullet
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy the bullet after 5 seconds to prevent memory leaks
    }



    // Update is called once per frame
    void Update()
    {
        transform.Translate(0f, 0f, moveSpeed * Time.deltaTime);
    }
}
