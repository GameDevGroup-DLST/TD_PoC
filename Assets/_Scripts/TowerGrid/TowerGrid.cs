using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGrid : Singleton<TowerGrid>
{
    public TowerTypeScriptableObject[] availableTowers = new TowerTypeScriptableObject[1];
    [SerializeField] TowerTypeScriptableObject caravanTowerSO;
    [SerializeField] TowerTypeScriptableObject defaultTileSO;
    private TowerTypeScriptableObject currentlySelectedTowerTypeSO;
    private int activeIndex = 0;

    private GridXZ<TowerGridObject> grid;
    private TowerTypeScriptableObject.Dir _dir = TowerTypeScriptableObject.Dir.South;

    [SerializeField] private bool debug = true;
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;
    [SerializeField] private float cellSize = 10f;
    [SerializeField] private LayerMask mouseColliderLayerMask;

    public delegate void TowerChangeAction();
    public static event TowerChangeAction OnTowerChanged;

    override protected void Awake() {
        base.Awake();
        
        currentlySelectedTowerTypeSO = availableTowers[activeIndex];
        
        grid = new GridXZ<TowerGridObject>(
            gridWidth,
            gridHeight,
            cellSize,
            this.transform.position,
            (GridXZ<TowerGridObject> g, int x, int z) => new TowerGridObject(g, x, z),
            debug
        );
    }

    private void Start() {
        InitializeGrid();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            CreateTower(currentlySelectedTowerTypeSO);
        }
        if (Input.GetMouseButtonDown(1)) {
            DestroyTower();
        }
        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            activeIndex = (activeIndex + 1) % availableTowers.Length;
            currentlySelectedTowerTypeSO = availableTowers[activeIndex];

            OnTowerChanged?.Invoke();
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            activeIndex = activeIndex - 1;

            activeIndex = activeIndex >= 0 ? activeIndex : (availableTowers.Length - 1);
            currentlySelectedTowerTypeSO = availableTowers[activeIndex];

            OnTowerChanged?.Invoke();
        }
    }

    private void InitializeGrid() {
        for(int x = 0; x < gridWidth; x++) {
            for(int z = 0; z < gridHeight; z++) {
                CreateTower(defaultTileSO, x, z);
            }
        }

        CreateTower(caravanTowerSO, gridHeight/2, gridWidth/2);
        Vector3 caravanWorldPosition = grid.GetWorldPosition(gridHeight/2, gridWidth/2);
        
        Camera.main.transform.position = Camera.main.transform.position + new Vector3(caravanWorldPosition.x - 60f, 0, caravanWorldPosition.z - 60f);
    }

    private void CreateTower(TowerTypeScriptableObject towerType) {
        Vector3 mousePosition = GetMouseWorldPosition();
        grid.GetXZ(mousePosition, out int x, out int z);

        List<Vector2Int> gridPositionList = GetGridPositionListAtMousePosition();

        if(CanBuildInArea(gridPositionList)) {
            Vector2Int rotationOffset = towerType.GetRotationOffset(_dir);
            Vector3 towerObjectWorldPosition = grid.GetWorldPosition(x, z) +
                new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

            Tower tower = Tower.Create(towerObjectWorldPosition, new Vector2Int(x,z), _dir, towerType);

            foreach(Vector2Int gridPosition in gridPositionList) {
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetTower(tower);
            }
        } else {
            CodeMonkey.Utils.UtilsClass.CreateWorldTextPopup(
                null,
                "Cannot Build Here!",
                mousePosition,
                40,
                Color.red,
                mousePosition + new Vector3(0, 20),
                1f
            );
        }
    }

    private void CreateTower(TowerTypeScriptableObject towerType, int x, int z) {
        List<Vector2Int> gridPositionList = currentlySelectedTowerTypeSO.GetGridPositionList(new Vector2Int(x,z), _dir);

        if(CanBuildInArea(gridPositionList)) {
            Vector2Int rotationOffset = towerType.GetRotationOffset(_dir);
            Vector3 towerObjectWorldPosition = grid.GetWorldPosition(x, z) +
                new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

            Tower tower = Tower.Create(towerObjectWorldPosition, new Vector2Int(x,z), _dir, towerType);

            foreach(Vector2Int gridPosition in gridPositionList) {
                grid.GetGridObject(gridPosition.x, gridPosition.y).GetTower()?.DestroySelf(); // for sanity reasons
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetTower(tower);
            }
        } else {
            CodeMonkey.Utils.UtilsClass.CreateWorldTextPopup(
                null,
                "Cannot Build Here!",
                grid.GetWorldPosition(x, z),
                40,
                Color.red,
                grid.GetWorldPosition(x, z) + new Vector3(0, 20),
                1f
            );
        }
    }

    private void DestroyTower() {
        TowerGridObject gridObject = grid.GetGridObject(GetMouseWorldPosition());
        Tower tower = gridObject.GetTower();
        if (tower != null && tower.CanBeDestroyed()) {
            List<Vector2Int> gridPositionList = tower.GetGridPositionList();
            
            tower.DestroySelf();
            
            foreach(Vector2Int gridPosition in gridPositionList) {
                Vector2Int rotationOffset = defaultTileSO.GetRotationOffset(_dir);

                Vector3 towerObjectWorldPosition = grid.GetWorldPosition(gridPosition.x, gridPosition.y) +
                new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

                Tower empty = Tower.Create(towerObjectWorldPosition, new Vector2Int(gridPosition.x, gridPosition.y), _dir, defaultTileSO);
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetTower(empty);
            }
        } else {
            grid.GetXZ(GetMouseWorldPosition(), out int x, out int z);
            CodeMonkey.Utils.UtilsClass.CreateWorldTextPopup(
                null,
                "Cannot Destroy Tile!",
                grid.GetWorldPosition(x, z),
                40,
                Color.red,
                grid.GetWorldPosition(x, z) + new Vector3(0, 20),
                1f
            );
        }
    }

    private void RotateTower(bool counterClockwise = false) {
        if(counterClockwise) {
            _dir = TowerTypeScriptableObject.GetPrevDir(_dir);
        } else {
            _dir = TowerTypeScriptableObject.GetNextDir(_dir);
        }

        CodeMonkey.Utils.UtilsClass.CreateWorldTextPopup(
            null,
            "" + _dir,
            GetMouseWorldPosition(),
            40,
            Color.green,
            GetMouseWorldPosition() + new Vector3(0, 20),
            1f
        );
    }

    private bool CanBuildInArea(List<Vector2Int> gridPositionList) {
        foreach(Vector2Int gridPosition in gridPositionList) {
            if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                return false;
            }
        }

        return true;
    }

    private List<Vector2Int> GetGridPositionListAtMousePosition() {
        grid.GetXZ(GetMouseWorldPosition(), out int x, out int z);
        return currentlySelectedTowerTypeSO.GetGridPositionList(new Vector2Int(x,z), _dir);
    }

    public Vector3 GetMouseWorldPosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, mouseColliderLayerMask)) {
            return hit.point;
        } else {
            return Vector3.zero;
        }
    }

    public Vector3 GetMouseWorldSnappedPosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, mouseColliderLayerMask)) {
            grid.GetXZ(hit.point, out int x, out int z);
            Vector2Int rotationOffset = currentlySelectedTowerTypeSO.GetRotationOffset(_dir);
            return grid.GetWorldPosition(x, z) +
                new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
        } else {
            return Vector3.zero;
        }
    }

    public Quaternion GetTowerRotation() {
        return Quaternion.Euler(0, currentlySelectedTowerTypeSO.GetRotationAngle(_dir), 0);
    }

    public TowerTypeScriptableObject GetSelectedTowerSO() => currentlySelectedTowerTypeSO;

    public abstract class GridObject<T> {
        protected GridXZ<T> _grid;
        protected int _x;
        protected int _z;

        public GridObject() {
        }

        public GridObject(GridXZ<T> grid, int x, int z) {
            _grid = grid;
            _x = x;
            _z = z;
        }

        public override string ToString()
        {
            return _x + "," + _z;
        }
    }

    public class TowerGridObject : GridObject<TowerGridObject> {
        private Tower _tower;

        public TowerGridObject(GridXZ<TowerGridObject> grid, int x, int z) {
            _grid = grid;
            _x = x;
            _z = z;
        }

        public bool CanBuild() => _tower == null || _tower.CanBeOverridden();

        public Tower GetTower() => _tower;

        public void ClearTower() {
            _tower = null;
            _grid.TriggerGridObjectChanged(_x,_z);
        }

        public TowerGridObject SetTower(Tower tower) {
            _tower = tower;
            _grid.TriggerGridObjectChanged(_x,_z);
            return this;
        }

        public override string ToString()
        {
            return _x + "," + _z + "\n" + _tower?.GetTowerName();
        }
    }
}
