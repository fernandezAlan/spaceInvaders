using UnityEngine;

public class SmallEnemy : EnemyBase
{
    private float enemySpeed = 0.2f;          // Velocidad de avance en Z
    public float frequency = 1f;      // Velocidad del zigzag
    private float amplitude = 5f;      // Amplitud del zigzag en X

    private Vector3 basePosition;
    private float timeOffset;

    protected override void Start()
    {
        base.Start();
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        basePosition = transform.position;
        timeOffset = Random.Range(0f, 10f);
    }

    protected override void Update()
    {
        base.Update(); // Call the base Update method to handle firing bullets
        Debug.LogWarning("speed: " + enemySpeed); // Log when the Update method is called
        // Movimiento hacia adelante en Z
        basePosition += Vector3.back * enemySpeed * Time.deltaTime; // Z negativo = hacia el jugador

        // Zigzag lateral en X
        float wave = Mathf.Sin((Time.time + timeOffset) * frequency) * amplitude;
        //Debug.LogWarning("wave: " + wave); // Log the calculated wave value
        // Nueva posición combinada
        transform.position = new Vector3(basePosition.x + wave, basePosition.y, basePosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(other.GetComponent<Bullet>().damageAmount);
        }
    }
}