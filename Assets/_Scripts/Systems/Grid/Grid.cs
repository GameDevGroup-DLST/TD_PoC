using System;
using UnityEngine;

public class Grid<TGridObject>
{
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs {
        public int x;
        public int y;
    }

    private int _width;
    private int _height;
    private float _cellSize;
    private Vector3 _originPosition;
    private TGridObject[,] _gridArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject) {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _originPosition = originPosition;

        _gridArray = new TGridObject[width, height];
        
        for(int x = 0; x < this._gridArray.GetLength(0); x++) {
            for(int y = 0; y < _gridArray.GetLength(1); y++) {
                _gridArray[x, y] = createGridObject(this, x, y);
            }
        }

        bool showDebug = true;
        if(showDebug) {
            TextMesh[,] debugTextArray = new TextMesh[_width, _height];

            for(int x = 0; x < this._gridArray.GetLength(0); x++) {
                for(int y = 0; y < _gridArray.GetLength(1); y++) {
                    debugTextArray[x, y] = CodeMonkey.Utils.UtilsClass.CreateWorldText(_gridArray[x,y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(_cellSize, _cellSize) * .5f, 30, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, _height), GetWorldPosition(_width, _height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(_width, 0), GetWorldPosition(_width, _height), Color.white, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) => {
                debugTextArray[eventArgs.x, eventArgs.y].text = _gridArray[eventArgs.x, eventArgs.y]?.ToString();
            };
        }
    }

    public int GetWidth() => _width;
    public int GetHeight() => _width;
    public int GetCellSize() => _width;
    public Vector3 GetWorldPosition(int x, int y) => new Vector3(x, y) * _cellSize + _originPosition;
    private void GetXY(Vector3 worldPosition, out int x, out int y) {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
        y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
    }

    public void TriggerGridObjectChanged(int x, int y) {
        if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y});
    }

    public void SetGridObject(int x, int y, TGridObject value) {
        if (x >= 0 && y >= 0 && x < _width && y < _height) {
            _gridArray[x,y] = value;
            TriggerGridObjectChanged(x, y);
        }
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public TGridObject GetGridObject(int x, int y) {
        if (x >= 0 && y >= 0 && x < _width && y < _height) {
            return _gridArray[x, y];
        }
        return default(TGridObject);
    }

    public TGridObject GetGridObject(Vector3 worldPosition) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }
}
