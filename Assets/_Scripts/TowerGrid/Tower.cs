using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private TowerTypeScriptableObject _towerTypeSO;
    private Vector2Int _origin;
    private TowerTypeScriptableObject.Dir _dir;

    public static Tower Create (Vector3 worldPosition, Vector2Int origin, TowerTypeScriptableObject.Dir dir, TowerTypeScriptableObject towerTypeSO) {
        Transform towerTransform = Instantiate(towerTypeSO.prefab, worldPosition, Quaternion.Euler(0, towerTypeSO.GetRotationAngle(dir), 0));

        Tower tower = towerTransform.GetComponent<Tower>();

        tower._towerTypeSO = towerTypeSO;
        tower._origin = origin;
        tower._dir = dir;

        return tower;
    }

    public string GetTowerName() {
        return _towerTypeSO.name;
    }

    public List<Vector2Int> GetGridPositionList() {
        return _towerTypeSO.GetGridPositionList(_origin, _dir);
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }
}
