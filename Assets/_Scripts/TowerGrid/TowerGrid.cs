using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGrid : Singleton<TowerGrid>
{
    public TowerTypeScriptableObject[] availableTowers = new TowerTypeScriptableObject[1];
    [SerializeField] TowerTypeScriptableObject caravanTowerSO;
    private TowerTypeScriptableObject towerTypeSO;
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
        
        towerTypeSO = availableTowers[activeIndex];
        
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
        CreateTower(caravanTowerSO, gridHeight/2, gridWidth/2);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            CreateTower(towerTypeSO);
        }
        if (Input.GetMouseButtonDown(1)) {
            DestroyTower();
        }
        // if (Input.GetKeyDown(KeyCode.E)) { // Need to figure how to rotate at object center, not at pivot point
        //     RotateTower();
        // }
        // if (Input.GetKeyDown(KeyCode.Q)) {
        //     RotateTower(true);
        // }
        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            activeIndex = (activeIndex + 1) % availableTowers.Length;
            towerTypeSO = availableTowers[activeIndex];

            OnTowerChanged?.Invoke();
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            activeIndex = activeIndex - 1;

            activeIndex = activeIndex >= 0 ? activeIndex : (availableTowers.Length - 1);
            towerTypeSO = availableTowers[activeIndex];

            OnTowerChanged?.Invoke();
        }
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
        List<Vector2Int> gridPositionList = towerTypeSO.GetGridPositionList(new Vector2Int(x,z), _dir);

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
        if (tower != null) {
            tower.DestroySelf();
            
            List<Vector2Int> gridPositionList = tower.GetGridPositionList();

            foreach(Vector2Int gridPosition in gridPositionList) {
                grid.GetGridObject(gridPosition.x, gridPosition.y).ClearTower();
            }
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
        return towerTypeSO.GetGridPositionList(new Vector2Int(x,z), _dir);
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
            Vector2Int rotationOffset = towerTypeSO.GetRotationOffset(_dir);
            return grid.GetWorldPosition(x, z) +
                new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
        } else {
            return Vector3.zero;
        }
    }

    public Quaternion GetTowerRotation() {
        return Quaternion.Euler(0, towerTypeSO.GetRotationAngle(_dir), 0);
    }

    public TowerTypeScriptableObject GetSelectedTowerSO() => towerTypeSO;

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

        public bool CanBuild() => _tower == null;

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
