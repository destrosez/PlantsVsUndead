using UnityEngine;
using UnityEngine.Serialization;

public class TowerController : MonoBehaviour
{
    [SerializeField] LayerMask _enemyLayerMask;
    
    public GameObject ProjectilePrefab;
    public float ShootCooldown;
    public float ShootRange;
    public bool IsIncomeGenerator;
    public float IncomeCooldown;
    public float IncomeAmount;
    
    private float _shootTimer;
    private float _incomeTimer;

    private void Start()
    {
        _incomeTimer = IncomeCooldown;
        gameObject.tag = "Tower";
    }

    private void Update()
    {
        _shootTimer -= Time.deltaTime;
        _incomeTimer -= Time.deltaTime;

        if (ProjectilePrefab != null && _shootTimer <= 0f)
        {
            Vector3 dir = transform.forward;
            Vector3 origin = transform.position + Vector3.up * 0.5f;
            
            if (Physics.Raycast(origin, dir,  out RaycastHit hit, ShootRange, _enemyLayerMask))
            {
                Instantiate(ProjectilePrefab, origin, Quaternion.LookRotation(-dir));
                _shootTimer = ShootCooldown;
            }
        }

        _incomeTimer -= Time.deltaTime;
        if (IsIncomeGenerator && _incomeTimer <= 0f)
        {
            GameManager.Instance.AddMoney(IncomeAmount);
            _incomeTimer = IncomeCooldown;
        }
    }
}