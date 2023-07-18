using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerGhost : MonoBehaviour
{
    private Transform _visual;
    [SerializeField] private Material _previewMaterial;

    // Start is called before the first frame update
    void Start()
    {
        RefreshVisual();

        TowerGrid.OnTowerChanged += TowerChangeAction;
    }

    private void TowerChangeAction() {
        RefreshVisual();
    }

    void LateUpdate()
    {
        Vector3 targetPosition = TowerGrid.Instance.GetMouseWorldSnappedPosition();
        targetPosition.y = 1f;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
        transform.rotation = Quaternion.Lerp(transform.rotation, TowerGrid.Instance.GetTowerRotation(), Time.deltaTime * 15f);
    }

    private void RefreshVisual() {
        if (_visual != null) {
            Destroy(_visual.gameObject);
            _visual = null;
        }

        TowerTypeScriptableObject towerTypeSO = TowerGrid.Instance.GetSelectedTowerSO();

        if (towerTypeSO != null) {
            _visual = Instantiate(towerTypeSO.visual, Vector3.zero, Quaternion.identity);
            _visual.parent = transform;
            _visual.localPosition = Vector3.zero;
            _visual.localEulerAngles = Vector3.zero;
            SetLayerRecursive(_visual.gameObject, 11);

            foreach(var renderer in _visual.GetComponentsInChildren<Renderer>()) {
                renderer.materials = (new List<Material>(Enumerable.Repeat(_previewMaterial, renderer.materials.Length))).ToArray();
            }
        }
    }

    private void SetLayerRecursive(GameObject targetGameObject, int layer) {
        targetGameObject.layer = layer;

        foreach(Transform child in targetGameObject.transform) {
            if (child == null) {
                continue;
            }

            SetLayerRecursive(child.gameObject, layer);
        }
    }
}
