using UnityEngine;

[CreateAssetMenu(menuName = "TD/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Prefab")]
    public GameObject prefab;

    [Header("Stats")]
    public float health = 10f;
    public float movementSpeed = 2f;
    public float damage = 1f;
    public float damageCooldown = 1f;

    [Header("Reward")]
    public float worth = 5f;
}