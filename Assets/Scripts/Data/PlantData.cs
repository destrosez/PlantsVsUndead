using UnityEngine;

[CreateAssetMenu(menuName = "TD/Plant Data")]
public class PlantData : ScriptableObject
{
    [Header("Prefab + UI")]
    public GameObject prefab;
    public int cost = 50;

    [Header("Placement")]
    public float placementYOffset = 1f;

    [Header("Health")]
    public float health = 10f;

    [Header("Shooting (Peashooter)")]
    public GameObject projectilePrefab;
    public float shootCooldown = 1f;
    public float shootRange = 15f;

    [Header("Income (Sun)")]
    public bool isIncomeGenerator = false;
    public float incomeCooldown = 5f;
    public float incomeAmount = 25f;
}