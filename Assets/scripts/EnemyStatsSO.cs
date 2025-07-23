using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Game Data/Enemy Stats")]
public class EnemyStatsSO : ScriptableObject
{
    public float speed = 1f;
    public int health = 100;
    public int damage = 10;
    public float delayBetweenShips = 4f;
    public float delayBetweenRows = 15f;
    public int pointsValue = 10; // Valor de puntos al destruir el enemigo
}