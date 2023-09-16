using System;
using UnityEngine;

public class EnemyController : BaseStateMachine<EnemyState>
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float minAttackRange = 7.5f;

    [SerializeField] private float attackDelay = 3f;
    [SerializeField] private float damageAmount = 25f;
    private TowerGrid _towerGrid;
    private Tower _caravan;
    private Vector3 _destination;
    private Tower _currentTarget;
    private bool _isDead = false;
    private float lastAttack;
    
    public static event Action<EnemyController> OnEnemyDeath;
    
    void Start()
    {
        PlayPhaseManager.OnBeforePlayPhaseChanged += HandlePlayPhaseChange;
        Tower.OnTowerDeath += HandleTowerDeath;
        
        _towerGrid = FindObjectOfType<TowerGrid>();
        if(!_towerGrid) {
            Destroy(gameObject);
        }

        Initialize();
    }

    void Initialize() {
        _caravan = _towerGrid.GetTowerByName("Caravan", out int x, out int z)?.GetTower();
        if(_caravan) {
            _destination = _towerGrid.GetGridPositionInWorld(x, z);
            _currentTarget = _caravan;
        }
        ChangeState(EnemyState.Targeting);
    }

    void Update()
    {
        Think();
        Act();
    }

    protected override void Think() {
        switch(State) {
            case EnemyState.Targeting:
                if(isInAttackingRange()) {
                    ChangeState(EnemyState.Attacking);
                }
            break;
            case EnemyState.Attacking:
                // If tower is dead, return to targeting?
            break;
            case EnemyState.Dead:
            case EnemyState.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(State), State, null);
        }
    }

    protected override void Act() {
        switch(State) {
            case EnemyState.Targeting:
                transform.Translate((_destination - this.transform.position).normalized * moveSpeed * Time.deltaTime);
            break;
            case EnemyState.Attacking:
                if(CanAttack()) {
                    Attack();
                }
            break;
            case EnemyState.Dead:
            case EnemyState.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(State), State, null);
        }
    }

    public override void ChangeState(EnemyState newState) {
        if (newState == State) return;
        
        EnemyState prevState = State;

        State = newState;
        switch (newState) {
            case EnemyState.Targeting:
                break;
            case EnemyState.Attacking:
                break;
            case EnemyState.Dead:
                Die();
                break;
            case EnemyState.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Tower") && State != EnemyState.Attacking) {
            Debug.DrawLine(transform.position, other.transform.position);
            Tower potentialTarget = other.GetComponentInParent<Tower>();

            if(potentialTarget.State != TowerState.Dead) {
                _currentTarget = potentialTarget;
                _destination = GetRandomPointAroundTarget(other.transform.position, minAttackRange);
            }
        }
    }

    private void HandlePlayPhaseChange(PlayPhase phase)
    {
        if(phase == PlayPhase.Victory) {
            ChangeState(EnemyState.Dead);
        }
    }

    private void HandleTowerDeath(Tower tower)
    {
        if(tower == _currentTarget) {
            ChangeState(EnemyState.Targeting);
            _currentTarget = _caravan;
            _destination = GetRandomPointAroundTarget(_caravan.gameObject.transform.position, minAttackRange);
        }
    }

    bool isInAttackingRange() {
        return Vector3.Distance(this.transform.position, _destination) <= minAttackRange;
    }

    bool CanAttack() {
        return (Time.time - lastAttack) >= attackDelay;
    }

    private void Attack() {
        lastAttack = Time.time;
        Debug.Log("Enemy Attack");
        _currentTarget.TakeDamage(damageAmount);
    }

    private void Die() {
        OnEnemyDeath?.Invoke(this);
        if(_isDead) return;
        _isDead = true;
        Destroy(gameObject);
    }

    private Vector3 GetRandomPointAroundTarget(Vector3 center, float radius) {
        float angle = UnityEngine.Random.Range(0, 2f * Mathf.PI);
        return center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
    }
}
