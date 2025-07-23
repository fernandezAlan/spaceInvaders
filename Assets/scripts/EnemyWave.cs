using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    public float delayBetweenRows = 3f;
    public float delayBetweenShips = 1f;

    private int currentRowIndex = 0;
    private int currentShipIndex = 0;

    private float timer = 0f;
    private bool activating = true;
    private bool waitingBetweenRows = false;
    private float maxDescend = -11f; // Altura máxima de descenso para las naves

    private Transform[] rows;

    void Start()
    {
        ApplyStatsFromGameManager();

        int rowCount = transform.childCount;
        rows = new Transform[rowCount];
        for (int i = 0; i < rowCount; i++)
        {
            rows[i] = transform.GetChild(i);
        }

        ActivateFirstShip(); // 👉 activa inmediatamente la primera nave
    }

    void Update()
    {
        if (!activating || currentRowIndex >= rows.Length) return;

        timer += Time.deltaTime;

        if (waitingBetweenRows)
        {
            if (timer >= delayBetweenRows)
            {
                timer = 0f;
                waitingBetweenRows = false;
                currentShipIndex = 0;
                currentRowIndex++;
            }
            return;
        }

        if (currentRowIndex < rows.Length)
        {
            Transform currentRow = rows[currentRowIndex];

            // Espera para la segunda nave en adelante
            if (currentShipIndex < currentRow.childCount && timer >= delayBetweenShips)
            {
                Transform enemy = currentRow.GetChild(currentShipIndex);
                if (enemy != null)
                {
                    SmallEnemy ship = enemy.GetComponent<SmallEnemy>();
                    if (ship != null)
                    {
                        ship.maxDescend = maxDescend;
                        ship.descend = true;
                        ship.ActiveShip();
                    }
                }

                currentShipIndex++;
                timer = 0f;
            }
            else if (currentShipIndex >= currentRow.childCount)
            {
                waitingBetweenRows = true;
                timer = 0f;
                maxDescend -= 2f; // aumenta la altura máxima de descenso para las siguientes filas
            }
        }
    }

    private void ActivateFirstShip()
    {
        if (rows.Length > 0 && rows[0].childCount > 0)
        {
            Transform enemy = rows[0].GetChild(0);
            if (enemy != null)
            {
                SmallEnemy ship = enemy.GetComponent<SmallEnemy>();
                if (ship != null)
                {
                    ship.maxDescend = maxDescend;
                    ship.descend = true;
                    ship.ActiveShip();
                }
            }

            currentShipIndex = 1; // empezamos desde la segunda nave
        }
    }

    private void ApplyStatsFromGameManager()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        EnemyStatsSO stats = gm.GetCurrentEnemyStats();
        delayBetweenRows = stats.delayBetweenRows;
        delayBetweenShips = stats.delayBetweenShips;
    }
}
