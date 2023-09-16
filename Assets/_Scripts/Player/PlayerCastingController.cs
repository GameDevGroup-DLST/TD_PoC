using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCastingController : MonoBehaviour
{
    public int activeSpellIndex = 0;

    [SerializeField] private List<SpellScriptableObject> spells = new List<SpellScriptableObject>(3);
    [SerializeField] private Transform castPoint;
    [SerializeField] private LayerMask mouseColliderLayerMask;
    private float lastCast;

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.State == GameState.Gameplay && PlayPhaseManager.Instance.Phase == PlayPhase.Defence) {
            if(Input.GetMouseButtonDown(0) && CanCast()) {
                CastSpell();
            }
        }
    }

    bool CanCast() {
        return (Time.time - lastCast) >= spells[activeSpellIndex].CastDelay;
    }

    void CastSpell() {
        // Handle different casting points Self, Target, Point

        lastCast = Time.time;

        // Handle different projectile spawns
        Spell spellInstance = Instantiate(spells[activeSpellIndex].Projectile, castPoint.position, Quaternion.FromToRotation(Vector3.forward, (GetMouseWorldPosition() - castPoint.position)));
        spellInstance.Initialize(spells[activeSpellIndex], this.gameObject);
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
