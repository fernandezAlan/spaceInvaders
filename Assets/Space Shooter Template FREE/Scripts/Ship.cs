using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Ship : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip shootClip;
    protected SpriteRenderer spriteRender;
    protected Color originalColor;
    public GameObject bullet;
    public float fireDelay = 0.2f; // Retraso en segundos
    public HealthBarUI healthBar; // Referencia a la barra de vida
    public Coroutine healthBarCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRender = GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRender.color;
    }
    public IEnumerator ShowHealthBarTemporarily()
    {
        healthBar.activeHealthBar(); // Mostrar barra
        yield return new WaitForSeconds(1f);
        healthBar.desactiveHealthBar(); // Ocultar después de 1 segundo
    }

    public IEnumerator DelayedFire()
    {
        // Reproducir el sonido de disparo
        audioSource.PlayOneShot(shootClip);

        // Esperar un momento antes de lanzar la bala
        yield return new WaitForSeconds(fireDelay);

        // Instanciar la bala
        Instantiate(bullet, transform.position, Quaternion.identity);
    }
    public IEnumerator FlashRed()
    {
        spriteRender.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRender.color = originalColor;
    }
    public void FireBullet()
    {
        StartCoroutine(DelayedFire());
    }
    protected virtual void Respawn()
    {
        if (healthBar != null)
        {
            healthBar.restoreHealth(); // Restore health bar to full
        }
        spriteRender.color = originalColor;
        gameObject.SetActive(true);
    }
    public float TakeDamage(float damage,float health,float healthTotal)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0f, 100f);

        if (healthBar != null)
        {
            healthBar.SetHealth(health, healthTotal); // Actualizá la barra
        }
        if (gameObject.activeSelf) { 
        StartCoroutine(FlashRed());
        }
       return health; // Return the updated health value
    }


}
