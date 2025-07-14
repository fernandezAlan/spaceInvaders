using System.Collections;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         StartCoroutine(ActivateWave());
    }
    IEnumerator ActivateWave()
    {
        foreach (Transform row in transform) // Recorre cada fila
        {
            // Inicia activación de naves dentro de la fila
            StartCoroutine(ActivateRow(row));

            // Espera 15 segundos antes de pasar a la próxima fila
            yield return new WaitForSeconds(10f);
        }
    }
    IEnumerator SlideForward(Transform target, float distance, float duration)
    {
        Vector3 startPos = target.position;
        Vector3 endPos = startPos + new Vector3(0, 0, distance);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            target.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Asegura que termine exactamente en la posición final
        target.position = endPos;
    }
    IEnumerator ActivateRow(Transform row)
    {
        foreach (Transform enemy in row)
        {
            SmallEnemy ship = enemy.GetComponent<SmallEnemy>();
            if (ship != null)
            {
                ship.ActiveShip(); // Solo esto
                yield return new WaitForSeconds(2f); // Delay entre naves
            }
        }

        // Delay entre filas si querés:
        yield return new WaitForSeconds(15f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
