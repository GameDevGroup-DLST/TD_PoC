using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    private float _currentHealth;
    private EnemyController _enemyController;

    public void Heal(float value)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + value, 0, maxHealth);
    }

    public void TakeDamage(float value)
    {
        _currentHealth -= value;

        if (_currentHealth < 0) {
            _enemyController.ChangeState(EnemyState.Dead);
        }
    }

    void Awake()
    {
        _currentHealth = maxHealth;
        _enemyController = gameObject.GetComponent<EnemyController>();
        if(!_enemyController) {
            Destroy(gameObject);
        }
    }
}
