using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TowerCastingController))]
public class Tower : BaseStateMachine<TowerState>, IDamageable
{
    [SerializeField] bool _canBeOverridden;
    [SerializeField] bool _canBeDestroyed = true;
    [SerializeField] GameObject visual;
    [SerializeField] GameObject destroyedVisual;
    private float _currentHealth = 0f;
    private bool _isDead = false;
    private TowerTypeScriptableObject _towerTypeSO;
    private Vector2Int _origin;
    private TowerTypeScriptableObject.Dir _dir;
    private TowerCastingController _towerCaster;

    public static event Action<Tower> OnTowerDeath;

    public static Tower Create (Vector3 worldPosition, Vector2Int origin, TowerTypeScriptableObject.Dir dir, TowerTypeScriptableObject towerTypeSO) {
        Transform towerTransform = Instantiate(towerTypeSO.prefab, worldPosition, Quaternion.Euler(0, towerTypeSO.GetRotationAngle(dir), 0));

        Tower tower = towerTransform.GetComponent<Tower>();
        TowerCastingController towerCaster = towerTransform.GetComponent<TowerCastingController>();

        tower._towerTypeSO = towerTypeSO;
        tower._origin = origin;
        tower._dir = dir;
        towerCaster.spellToCast = towerTypeSO.spellToCast;
        tower._currentHealth = towerTypeSO.maxHealth;
        tower._towerCaster = towerCaster;

        towerCaster.Tower = tower;

        Vector2Int pos = towerTypeSO.GetGridPositionList(origin, dir).First();
        tower.name = $"{tower.GetTowerName()} ({pos.x},{pos.y})";

        return tower;
    }

    private void Awake() {
        PlayPhaseManager.OnBeforePlayPhaseChanged += HandlePlayPhaseChange;
    }

    private void HandlePlayPhaseChange(PlayPhase phase)
    {
        switch(phase) {
            case PlayPhase.Planning:
                Debug.Log("REVIVE TOWER");
                ChangeState(TowerState.None);
            break;
            case PlayPhase.Defence:
                ChangeState(TowerState.Idle);
            break;
            default:
            break;
        }
    }

    private void Update() {
        Think();
        Act();
    }

    protected override void Think()
    {
        switch(State) {
            case TowerState.Idle:
            case TowerState.Attacking:
            case TowerState.Dead:
            case TowerState.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(State), State, null);
        }
    }

    protected override void Act()
    {
        switch(State) {
            case TowerState.Attacking:
                _towerCaster.Attack();
                break;
            case TowerState.Idle:
            case TowerState.Dead:
            case TowerState.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(State), State, null);
        }
    }

    public override void ChangeState(TowerState newState)
    {
        if (newState == State) return;
        
        TowerState prevState = State;

        State = newState;
        switch (newState) {
            case TowerState.Idle:
                break;
            case TowerState.Attacking:
                break;
            case TowerState.Dead:
                OnTowerDeath?.Invoke(this);
                // switch to dead prefab
                destroyedVisual.SetActive(true);
                visual.SetActive(false);
                break;
            case TowerState.None:
                // switch to alive prefab
                destroyedVisual.SetActive(false);
                visual.SetActive(true);
                Heal(_towerTypeSO.maxHealth);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    public void TakeDamage(float value)
    {
        _currentHealth -= value;

        if (_currentHealth < 0) {
            ChangeState(TowerState.Dead);
        }
    }

    public void Heal(float value)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + value, 0, _towerTypeSO.maxHealth);
    }

    public string GetTowerName() => _towerTypeSO.name;
    public bool CanBeOverridden() => _canBeOverridden;
    public bool CanBeDestroyed() => _canBeDestroyed;
    public List<Vector2Int> GetGridPositionList() =>  _towerTypeSO.GetGridPositionList(_origin, _dir);
    public void DestroySelf() => Destroy(gameObject);
}
