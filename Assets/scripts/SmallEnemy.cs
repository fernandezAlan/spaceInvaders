using System.Buffers.Text;
using UnityEngine;
using System.Collections;


public class SmallEnemy : EnemyBase
{
    private float enemySpeed = 0.2f;          // Velocidad de avance en Z
    private float frequency = 0.5f;      // Velocidad del zigzag
    private float amplitude = 5f;      // Amplitud del zigzag en X
    public float Totalhealth = 100f; // Salud del enemigo    
    public float currentHealth = 100f; // Salud actual del enemigo
    private float timeOffset;
    private float baseZ;         // Para controlar movimiento en Z
    private float startX;        // Para mantener la posición inicial en X
    private bool isDescending = false;
    private bool isZigzagging = false;
    private bool isSlidingToStart = false;

    protected override void Start()
    {
        base.Start();
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        timeOffset = Random.Range(1f, 10f);
    }
    public override void ActiveShip()
    {
        base.ActiveShip(); // Activa isActive


        isDescending = true;
        timeOffset = Random.Range(1f, 10f);

        StartCoroutine(DescendAndStartZigzag());
    }
    IEnumerator DescendAndStartZigzag()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(0, 0, -2f); // Z negativo: acercarse al jugador
        float duration = 1f;
        float elapsed = 0f;

        // DESCENSO
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;

        // Guardar la posición de partida para zigzag
        startX = transform.position.x;
        baseZ = transform.position.z;

        isDescending = false;

        // PREPARAR PARA ENTRAR EN EL ZIGZAG
        Vector3 currentPos = transform.position;
        float firstWave = Mathf.Sin((Time.time + timeOffset) * frequency) * amplitude;
        Vector3 zigzagStartPos = new Vector3(startX + firstWave, currentPos.y, baseZ);

        isSlidingToStart = true;
        float slideDuration = 0.5f;
        float slideElapsed = 0f;

        // DESPLAZAMIENTO SUAVE HACIA EL PUNTO DE ENTRADA AL ZIGZAG
        while (slideElapsed < slideDuration)
        {
            float t = slideElapsed / slideDuration;
            // Podés aplicar easing suave si querés:
             t = t * t * (3f - 2f * t); // SmoothStep
            transform.position = Vector3.Lerp(currentPos, zigzagStartPos, t);

            slideElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = zigzagStartPos;

        isSlidingToStart = false;
        isZigzagging = true;
    }
    protected override void Update()
    {
        base.Update();

     
    if (!isZigzagging || isSlidingToStart) return;

    // Movimiento continuo en Z
    baseZ -= enemySpeed * Time.deltaTime;

    // Zigzag lateral en X
    float wave = Mathf.Sin((Time.time + timeOffset) * frequency) * amplitude;

    transform.position = new Vector3(startX + wave, transform.position.y, baseZ);
}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(other.GetComponent<Bullet>().damageAmount);
        }
        
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Si el enemigo colisiona con el jugador, se destruye a sí mismo
            TakeDamage(Totalhealth);
        }
    }
    public void TakeDamage(float damageAmount)
    {
        StartCoroutine(FlashRed());
        currentHealth = TakeDamage(damageAmount, currentHealth, Totalhealth);
        if (healthBarCoroutine != null)
        {
            StopCoroutine(healthBarCoroutine); // Reiniciar si ya estaba activa
        }
        healthBarCoroutine = StartCoroutine(ShowHealthBarTemporarily());
        if (currentHealth <= 0f)
        {
            DestroyShip();
        }
    }
}