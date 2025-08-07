using UnityEngine;

[RequireComponent(typeof(Unit))]
public class EnemyController : MonoBehaviour
{
    public float MovementSpeed;
    public float Damage;
    public float DamageCooldown;
    public float Worth;

    private float _damageTimer;
    private Unit _unit;

    private void Start()
    {
        _unit = GetComponent<Unit>();
        _damageTimer = 0f;
        gameObject.tag = "Enemy";
    }

    private void Update()
    {
        _damageTimer -= Time.deltaTime;
        if (Physics.Raycast(transform.position, Vector3.back, out var hit, 0.6f))
        {
            if (hit.transform.CompareTag("TileGoal"))
            {
                GameManager.Instance.LoseLife();
                Destroy(gameObject);
                return;
            }

            if (hit.transform.CompareTag("Tower"))
            {
                if (_damageTimer <= 0f)
                {
                    var towerUnit = hit.transform.GetComponent<Unit>();
                    towerUnit?.TakeDamage(Damage);
                    _damageTimer = DamageCooldown;
                }
                return;
            }
        }
        transform.Translate(Vector3.back * (MovementSpeed * Time.deltaTime), Space.World);
    }


    private void OnDestroy()
    {
        if (_unit != null && _unit.Health <= 0f)
        {
            GameManager.Instance.AddMoney(Worth);
        }
    }
}