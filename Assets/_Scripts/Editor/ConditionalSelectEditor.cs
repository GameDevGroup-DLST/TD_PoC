using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

[CustomEditor(typeof(SpellScriptableObject))]
public class ConditionalSelectEditor : Editor
{
    bool showMovementOptions = true;
	private SpellScriptableObject spellSO;

	public override void OnInspectorGUI () {
		// Unity provides a target when overriding the inspector look, and it has always the same type we declare on CustmoEditor(typeof()) on top
		spellSO = (SpellScriptableObject)target;

        spellSO.SpellName = EditorGUILayout.TextField("Spell Name", spellSO.SpellName);
        spellSO.Projectile = (Spell)EditorGUILayout.ObjectField("Projectile Prefab", spellSO.Projectile, typeof(Spell));

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Spell Details", EditorStyles.boldLabel);

        spellSO.DamageAmount = EditorGUILayout.FloatField("Damage Amount", spellSO.DamageAmount);
        spellSO.ManaCost = EditorGUILayout.FloatField("Mana Cost", spellSO.ManaCost);
        spellSO.SpellRadius = EditorGUILayout.FloatField("Spell Radius", spellSO.SpellRadius);

        EditorGUILayout.Space();
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
        EditorGUILayout.LabelField("Lifetime", EditorStyles.boldLabel);

        spellSO.DestroyOnHit = EditorGUILayout.Toggle("Destroy On Hit", spellSO.DestroyOnHit);
        spellSO.Lifetime = EditorGUILayout.FloatField("Lifetime", spellSO.Lifetime);
	
		//IF something has changed, we let unity know, so it makes us save the scene, and therefore, the smart button values will be saved
		if(GUI.changed)
			EditorUtility.SetDirty(target);
	}

}
