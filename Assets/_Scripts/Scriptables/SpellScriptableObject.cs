using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "ScriptableObjects/Spell", order = 0)]
public class SpellScriptableObject : ScriptableObject {

    // METADATA
    public string SpellName;
    public Spell Projectile;
    public Sprite SpellThumbnail; // To be implemented

    public float ManaCost = 5f; // To be implemented
    public float CastDelay = 3f; // To be implemented
    public TargetType CastTarget = TargetType.None; // To be implemented

    // DAMAGE SETINGS -------------------------------------
    public float DamageAmount = 10f;
    public float SpellRadius = .5f;
    public List<OnHitEffect> OnHitEffects = new List<OnHitEffect>(); // To be implemented
    public List<OnKillEffect> OnKillEffects = new List<OnKillEffect>(); // To be implemented

    // MOVEMENT -------------------------------------------
    public float MoveSpeed = 15f;
    public MovementType MovementType = MovementType.None;

    // SINUSOIDAL ------------------
    public float Period = 5f;
    // ---- END SINUSOIDAL ---------

    // CUSTOM ----------------------
    public AnimationCurve xMovement = new AnimationCurve();
    public AnimationCurve yMovement = new AnimationCurve();
    // -------- END CUSTOM ---------
    // ----------------------------- END MOVEMENT ---------

    // LIFETIME -------------------------------------------
    public bool DestroyOnHit = true;
    public float Lifetime = 2f;
}