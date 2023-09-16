using System.Collections.Generic;
using UnityEngine;

public class TowerCastingController : MonoBehaviour
{
    private List<EnemyController> _enemiesInRange = new List<EnemyController>();
    public SpellScriptableObject spellToCast; // THIS NEEDS TO BE REPLACED WITH A TOWER SPELL SO
    [SerializeField] private Transform castPoint;
    private float lastCast;
    public Tower Tower {get; set;}

    public void Update() {
        if(Tower.State == TowerState.Dead) return;

        if(_enemiesInRange.Count > 0) {
            Tower.ChangeState(TowerState.Attacking);
        } else {
            Tower.ChangeState(TowerState.Idle);
        }
    }

    public void Attack() {
        if (spellToCast == null) return;
        
        if(CanCast()) {
            CastSpellAt(_enemiesInRange[0].transform.position);
        }
    }

    bool CanCast() {
        return (Time.time - lastCast) >= spellToCast.CastDelay;
    }

    void CastSpellAt(Vector3 target) {
        // Handle different casting points Self, Target, Point

        lastCast = Time.time;

        Spell spellInstance = Instantiate(
            spellToCast.Projectile,
            castPoint.position,
            Quaternion.FromToRotation(Vector3.forward, target - castPoint.position));
        spellInstance.Initialize(spellToCast, this.gameObject);
    }

    void OnTriggerEnter(Collider other) {
        EnemyController enemy = other.gameObject.GetComponent<EnemyController>();

        if(enemy != null) {
            EnemyController.OnEnemyDeath += HandleEnemyDeath;
            _enemiesInRange.Add(enemy);
        }
    }

    private void HandleEnemyDeath(EnemyController controller)
    {
        if(_enemiesInRange.Contains(controller)) {
            _enemiesInRange.Remove(controller);
        }
    }

    void OnTriggerExit(Collider other) {
        EnemyController enemy = other.gameObject.GetComponent<EnemyController>();

        if(enemy != null) {
            _enemiesInRange.Remove(enemy);
        }
    }
}
