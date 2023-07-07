using System;
using UnityEngine;

public class GridXZ<TGridObject>
{
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs {
        public int x;
        public int z;
    }

    private int _width;
    private int _height;
    private float _cellSize;
    private Vector3 _originPosition;
    private TGridObject[,] _gridArray;

    public GridXZ(int width, int height, float cellSize, Vector3 originPosition, Func<GridXZ<TGridObject>, int, int, TGridObject> createGridObject, bool showDebug = false) {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _originPosition = originPosition;

        _gridArray = new TGridObject[width, height];
        
        for(int x = 0; x < this._gridArray.GetLength(0); x++) {
            for(int z = 0; z < _gridArray.GetLength(1); z++) {
                _gridArray[x, z] = createGridObject(this, x, z);
            }
        }

        if(showDebug) {
            TextMesh[,] debugTextArray = new TextMesh[_width, _height];

            for(int x = 0; x < this._gridArray.GetLength(0); x++) {
                for(int z = 0; z < _gridArray.GetLength(1); z++) {
                    debugTextArray[x, z] = CodeMonkey.Utils.UtilsClass.CreateWorldText(
                        _gridArray[x,z]?.ToString(),
                        null,
                        GetWorldPosition(x, z) + new Vector3(_cellSize, 0, _cellSize) * .5f,
                        30,
                        Color.blue,
                        TextAnchor.MiddleCenter,
                        TextAlignment.Center
                    );
                    
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, _height), GetWorldPosition(_width, _height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(_width, 0), GetWorldPosition(_width, _height), Color.white, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) => {
                debugTextArray[eventArgs.x, eventArgs.z].text = _gridArray[eventArgs.x, eventArgs.z]?.ToString();
            };
        }
    }

    public int GetWidth() => _width;
    public int GetHeight() => _width;
    public int GetCellSize() => _width;
    public Vector3 GetWorldPosition(int x, int z) => new Vector3(x, 0, z) * _cellSize + _originPosition;
    public void GetXZ(Vector3 worldPosition, out int x, out int z) {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
        z = Mathf.FloorToInt((worldPosition - _originPosition).z / _cellSize);
    }

    public void TriggerGridObjectChanged(int x, int z) {
        if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, z = z});
    }

    public void SetGridObject(int x, int z, TGridObject value) {
        if (x >= 0 && z >= 0 && x < _width && z < _height) {
            _gridArray[x,z] = value;
            TriggerGridObjectChanged(x, z);
        }
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value) {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        SetGridObject(x, z, value);
    }

    public TGridObject GetGridObject(int x, int z) {
        if (x >= 0 && z >= 0 && x < _width && z < _height) {
            return _gridArray[x, z];
        }
        return default(TGridObject);
    }

    public TGridObject GetGridObject(Vector3 worldPosition) {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        return GetGridObject(x, z);
    }
}
