using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(SpellScriptableObject))]
public class SpellScriptableObjectEditor : Editor
{
    bool showMovementOptions = true;
	private SpellScriptableObject spellSO;

	public override void OnInspectorGUI () {
		// Unity provides a target when overriding the inspector look, and it has always the same type we declare on CustmoEditor(typeof()) on top
		spellSO = (SpellScriptableObject)target;
        BuildMetaSettings();
        BuildDamageSettings();
        BuildMovementSettings();
        BuildLifetimeSettings();
	}

    private void BuildMetaSettings() {
        EditorGUILayout.LabelField("Spell Metadata", EditorStyles.boldLabel);
        spellSO.SpellName = EditorGUILayout.TextField("Spell Name", spellSO.SpellName);
        spellSO.SpellThumbnail = (Sprite)EditorGUILayout.ObjectField("Spell Thumbnail", spellSO.SpellThumbnail, typeof(Sprite));
        spellSO.Projectile = (Spell)EditorGUILayout.ObjectField("Projectile Prefab", spellSO.Projectile, typeof(Spell));
        spellSO.ManaCost = EditorGUILayout.FloatField("Mana Cost", spellSO.ManaCost);
        spellSO.CastDelay = EditorGUILayout.FloatField("Cast Delay", spellSO.CastDelay);

        EditorGUILayout.Space();
    }

    private void BuildDamageSettings() {
        EditorGUILayout.LabelField("Damage", EditorStyles.boldLabel);

        spellSO.DamageAmount = EditorGUILayout.FloatField("Damage Amount", spellSO.DamageAmount);
        spellSO.SpellRadius = EditorGUILayout.FloatField("Spell Radius", spellSO.SpellRadius);
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("OnHitEffects"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("OnKillEffects"));
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }

        EditorGUILayout.Space();
    }

    private void BuildMovementSettings() {
        EditorGUILayout.LabelField("Movement Options", EditorStyles.boldLabel);

        spellSO.MoveSpeed = EditorGUILayout.Slider("Move Speed", spellSO.MoveSpeed, 0f, 50f);
		spellSO.MovementType = (MovementType)EditorGUILayout.EnumPopup("Movement Type", spellSO.MovementType);
		switch (spellSO.MovementType)
		{
			//Default usage just triggers an event in case someone is listening.
		case MovementType.None:
        case MovementType.Linear:
        	break;

		case MovementType.Sinusoidal:
            showMovementOptions = EditorGUILayout.BeginFoldoutHeaderGroup(showMovementOptions, "Sinusoidal Options");
            if(showMovementOptions) {
                spellSO.Period = EditorGUILayout.Slider("Period", spellSO.Period, 1f, 20f);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
			break;

        case MovementType.Custom:
            showMovementOptions = EditorGUILayout.BeginFoldoutHeaderGroup(showMovementOptions, "Custom Options");
            if(showMovementOptions) {
                spellSO.xMovement = EditorGUILayout.CurveField("xMovement", spellSO.xMovement);
                spellSO.yMovement = EditorGUILayout.CurveField("yMovement", spellSO.yMovement);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
			break;
		}

        EditorGUILayout.Space();
    }

    private void BuildLifetimeSettings() {
        EditorGUILayout.LabelField("Lifetime", EditorStyles.boldLabel);

        spellSO.DestroyOnHit = EditorGUILayout.Toggle("Destroy On Hit", spellSO.DestroyOnHit);
        spellSO.Lifetime = EditorGUILayout.FloatField("Lifetime", spellSO.Lifetime);
	
		if(GUI.changed) {
            EditorUtility.SetDirty(target);
        }

        EditorGUILayout.Space();
    }

}
