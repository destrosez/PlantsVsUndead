using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    [SerializeField] public PlantData[] _plantTypes;

    private bool _shovelMode;
    private int selectedPlant = -1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, 100f))
        {
            return;
        }

        if (_shovelMode && hit.transform.CompareTag("Tower"))
        {
            var tower = hit.transform.gameObject;
            var tile  = tower.transform.parent;
            var collider   = tile.GetComponent<Collider>();
            if (collider != null) collider.enabled = true;
            Destroy(tower);
            _shovelMode = false;
            return;
        }

        if (!_shovelMode && selectedPlant >= 0 && hit.transform.CompareTag("Tile"))
        {
            var data = _plantTypes[selectedPlant];
            if (!GameManager.Instance.TrySpendMoney(data.cost))
            {
                return;
            }
            hit.collider.enabled = false;
            
            Vector3 pos = hit.transform.position + Vector3.up * data.placementYOffset;
            var go = Instantiate(data.prefab, pos, Quaternion.identity);
            go.transform.SetParent(hit.transform, true);

            var unit = go.GetComponent<Unit>();
            if (unit != null)
            {
                unit.Health = data.health;
            }

            var towerController = go.GetComponent<TowerController>();
            if (towerController != null)
            {
                towerController.ProjectilePrefab  = data.projectilePrefab;
                towerController.ShootCooldown     = data.shootCooldown;
                towerController.ShootRange        = data.shootRange;
                towerController.IsIncomeGenerator = data.isIncomeGenerator;
                towerController.IncomeCooldown    = data.incomeCooldown;
                towerController.IncomeAmount      = data.incomeAmount;
            }
        }
    }
    
    public void SelectPlant(int index)
    {
        if (index < 0 || index >= _plantTypes.Length)
        {
            return;
        }
        selectedPlant = index;
    }
    
    public void SelectShovel()
    {
        _shovelMode = true;
        selectedPlant = -1;
    }
}