using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class Spell : MonoBehaviour
{
    protected SpellScriptableObject spellToCast;
    protected GameObject caster;

    private SphereCollider sphereCollider;
    private Rigidbody rb;

    protected Action<SpellScriptableObject, Spell> handleMovement;

    public float startTime { get; private set; }

    private void Awake() {

        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        startTime = Time.time;
    }

    public Spell Initialize(SpellScriptableObject spell, GameObject caster) {
        spellToCast = spell;

        sphereCollider.radius = spellToCast.SpellRadius;

        switch (spellToCast.MovementType) {
            case MovementType.Linear:
                handleMovement += SpellMovement.LinearMove;
                break;
            case MovementType.Sinusoidal:
                handleMovement += SpellMovement.SineMove;
                break;
            case MovementType.Custom:
                handleMovement += SpellMovement.CustomMove;
                break;
            case MovementType.None:
            default:
                break;
        }

        Invoke(nameof(Die), spell.Lifetime);

        return this;
    }

    private void Update()
    {
        handleMovement?.Invoke(spellToCast, this);     
    }

    private void OnTriggerEnter(Collider other) {
        // Apply Spell Effects
        // Apply Hit Particles
        // Apply Sound Effects

        if(spellToCast.DestroyOnHit) {
            Die();
        }
    }

    private void Die()
    {
        // Do other things, then die

        Destroy(gameObject);
    }
}
