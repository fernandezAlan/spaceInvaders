using System.Buffers.Text;
using UnityEngine;
using System.Collections;


public class SmallEnemy : EnemyBase
{
    private float enemySpeed = 0.2f;          // Velocidad de avance en Z
    private float frequency = 0.5f;      // Velocidad del zigzag
    private float amplitude = 5f;      // Amplitud del zigzag en X
    private int Totalhealth = 50; // Salud del enemigo    
    private int currentHealth = 50; // Salud actual del enemigo
    private float timeOffset;
    private float baseZ;         // Para controlar movimiento en Z
    private float startX;        // Para mantener la posición inicial en X
    private bool isDescending = false;
    private bool isZigzagging = false;
    private bool isSlidingToStart = false;
    private int pointsValue; // Valor de puntos al destruir el enemigo
    public float maxDescend; // Altura máxima de descenso
    public bool descend = true; // Controla si el enemigo desciende o asciende
    private GameManager gameManager; // Referencia al GameManager
    protected override void Start()
    {
        base.Start();
        timeOffset = Random.Range(1f, 10f);
        ApplyStatsFromGameManager(); // Aplicar estadísticas desde GameManager
    }

    private void ApplyStatsFromGameManager()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        EnemyStatsSO stats = gameManager.GetCurrentEnemyStats();

        frequency = stats.speed;
        Totalhealth = stats.health;
        currentHealth = stats.health;
        pointsValue = stats.pointsValue; // Valor de puntos al destruir el enemigo
        //damageAmount = stats.damage;
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

        // Ignorar si aún no está zigzagueando o se está deslizando hacia el punto inicial
        if (!isZigzagging || isSlidingToStart) return;
        float currentZ = transform.position.z;
        // Cambiar dirección si se pasa de los límites
        if (descend && currentZ <= maxDescend + 0.01f)
        {
            descend = false; // Empezar a subir
        }
        else if (!descend && currentZ >= 0f - 0.01f)
        {
            descend = true; // Empezar a bajar nuevamente
        }

        // Movimiento en Z (arriba o abajo)
        if (descend)
            baseZ -= enemySpeed * Time.deltaTime;
        else
            baseZ += enemySpeed * Time.deltaTime;

        // Movimiento lateral en zigzag
        float wave = Mathf.Sin((Time.time + timeOffset) * frequency) * amplitude;
        transform.position = new Vector3(startX + wave, transform.position.y, baseZ);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(other.GetComponent<Bullet>().damageAmount);
        }
        if (other.CompareTag("Player"))
        {
            // Si el enemigo colisiona con el jugador, se destruye a sí mismo
            TakeDamage(Totalhealth);
        }
        if (other.CompareTag("collisionWall"))
        {
            DestroyShip(false);
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Si el enemigo colisiona con el jugador, se destruye a sí mismo
            TakeDamage(Totalhealth);
        }
        if (collision.gameObject.CompareTag("collisionWall"))
        {
            DestroyShip(false);
        }
    }
    public void TakeDamage(int damageAmount)
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
            gameManager.AddPoints(pointsValue); // Añadir puntos al puntaje del jugador
            DestroyShip(true);
        }
    }
}