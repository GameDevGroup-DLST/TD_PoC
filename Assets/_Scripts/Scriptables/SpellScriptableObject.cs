using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "ScriptableObjects/Spell", order = 0)]
public class SpellScriptableObject : ScriptableObject {

    public string SpellName;
    public Spell Projectile;
    // Damage Types / Elements
    public float DamageAmount = 10f;
    public float ManaCost = 5f;
    public float SpellRadius = .5f;

    [Header("Movement")]
    public float MoveSpeed = 15f;
    public MovementType MovementType = MovementType.None;

    [Header("Sinusoidal Settings")]
    [Range(1,20)]
    public float Period = 5f;

    [Header("Custom Movement")]
    public AnimationCurve xMovement = new AnimationCurve();
    public AnimationCurve yMovement = new AnimationCurve();


    [Header("Lifetime")]
    public bool DestroyOnHit = true;
    public float Lifetime = 2f;

    // Status Effects
    // Thumbnail
    // Cast Delay
    // Projectile Motion
}