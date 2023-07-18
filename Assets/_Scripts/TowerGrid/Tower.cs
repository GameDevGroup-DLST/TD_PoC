using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] bool _canBeOverridden;
    [SerializeField] bool _canBeDestroyed = true;
    private TowerTypeScriptableObject _towerTypeSO;
    private Vector2Int _origin;
    private TowerTypeScriptableObject.Dir _dir;

    public static Tower Create (Vector3 worldPosition, Vector2Int origin, TowerTypeScriptableObject.Dir dir, TowerTypeScriptableObject towerTypeSO) {
        Transform towerTransform = Instantiate(towerTypeSO.prefab, worldPosition, Quaternion.Euler(0, towerTypeSO.GetRotationAngle(dir), 0));

        Tower tower = towerTransform.GetComponent<Tower>();

        tower._towerTypeSO = towerTypeSO;
        tower._origin = origin;
        tower._dir = dir;

        Vector2Int pos = towerTypeSO.GetGridPositionList(origin, dir).First();
        tower.name = $"{tower.GetTowerName()} ({pos.x},{pos.y})";

        return tower;
    }

    public string GetTowerName() => _towerTypeSO.name;

    public bool CanBeOverridden() => _canBeOverridden;
    public bool CanBeDestroyed() => _canBeDestroyed;

    public List<Vector2Int> GetGridPositionList() =>  _towerTypeSO.GetGridPositionList(_origin, _dir);

    public void DestroySelf() => Destroy(gameObject);
}
