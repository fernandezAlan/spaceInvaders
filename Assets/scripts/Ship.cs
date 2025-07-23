using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Ship : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip shootClip;
    public AudioClip explosionClip;
    protected SpriteRenderer spriteRender;
    protected Color originalColor;
    public GameObject bullet;
    public float fireDelay = 0.2f; // Retraso en segundos
    public HealthBarUI healthBar; // Referencia a la barra de vida
    public Coroutine healthBarCoroutine;
    public Vector3 initPos; // Initial position of the hero
    public GameObject explosionPrefab;
    public Rigidbody rb;
    public Collider col;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
       

    }
    protected virtual void Awake() {
        audioSource = GetComponent<AudioSource>();
        spriteRender = GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRender.color;
        initPos = transform.localPosition;
    }
    //GHOST MODE
    public void SetInvulnerable(float duration)
    {
        StartCoroutine(InvulnerabilityCoroutine(duration));
    }
    private IEnumerator InvulnerabilityCoroutine(float duration)
    {
        // Invulnerable: sin colisión y transparente
        rb.detectCollisions = false;
        col.enabled = false;
        SetTransparency(0.4f);

        yield return new WaitForSeconds(duration);

        // Restaurar colisión y opacidad
        rb.detectCollisions = true;
        col.enabled = true;
        SetTransparency(1f);
    }
    public void SetTransparency(float alpha)
    {
        Color color = spriteRender.color;
        color.a = alpha;
        spriteRender.color = color;
    }

    public IEnumerator ShowHealthBarTemporarily()
    {
        healthBar.activeHealthBar(); // Mostrar barra
        yield return new WaitForSeconds(1f);
        healthBar.desactiveHealthBar(); // Ocultar después de 1 segundo
    }


    public void StartSplosion()
    {
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // Forzar el inicio inmediato
            var ps = explosion.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }

            // Destruir manualmente como respaldo
            Destroy(explosion, 2f);
        }
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
    }
    public int TakeDamage(int damage,int health,int healthTotal)
    {
        health -= damage;
        //health = Mathf.Clamp(health, 0f, 100f);

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
