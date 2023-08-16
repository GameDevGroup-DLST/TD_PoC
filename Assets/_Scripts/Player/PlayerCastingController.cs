using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCastingController : MonoBehaviour
{
    [SerializeField] private List<SpellScriptableObject> spells = new List<SpellScriptableObject>(3);
    [SerializeField] private Transform castPoint;

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.State == GameState.Gameplay && PlayPhaseManager.Instance.Phase == PlayPhase.Defence) {
            if(Input.GetMouseButtonDown(0)) {
                CastSpell();
            }
        }
    }

    void CastSpell() {
        // Handle different casting points Self, Target, Point
        Spell spellInstance = Instantiate(spells[0].Projectile, castPoint.position, castPoint.rotation);
        spellInstance.Initialize(spells[0], this.gameObject);
    }
}
