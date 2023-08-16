using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask mouseColliderLayerMask;

    // Update is called once per frame
    void Update()
    {
        HandleLookAt();
    }

    private void HandleLookAt() {
        Vector3 mousePosition = GetMouseWorldPosition();
        this.transform.LookAt(new Vector3(mousePosition.x, this.transform.position.y, mousePosition.z));
    }

    public Vector3 GetMouseWorldPosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, mouseColliderLayerMask)) {
            return hit.point;
        } else {
            return Vector3.zero;
        }
    }
}