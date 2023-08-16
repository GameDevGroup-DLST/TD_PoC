using UnityEngine;

public static class SpellMovement { // UPDATE THESE TO ALSO HEAD TOWARDS THEIR DESTINATION (WHERE THE PLAYER CLICKED)
    public static void LinearMove(SpellScriptableObject spellDetails, Spell spell) {
        if (spellDetails == null) return;

        if (spellDetails.MoveSpeed > 0) {
            spell.transform.position = spell.transform.position + (spellDetails.MoveSpeed * Time.deltaTime * spell.transform.forward);
        }
    }

    public static void SineMove(SpellScriptableObject spellDetails, Spell spell) {
        if (spellDetails == null) return;

        if (spellDetails.MoveSpeed > 0) {
            spell.transform.position = spell.transform.position + spellDetails.MoveSpeed * Time.deltaTime * (spell.transform.forward + (spell.transform.right * Mathf.Cos(Time.time * spellDetails.Period))); // Add salt to the Cos Function
        }
    }

    public static void CustomMove(SpellScriptableObject spellDetails, Spell spell) {
        if (spellDetails == null) return;

        if (spellDetails.MoveSpeed > 0) {
            float delta = Time.time - spell.startTime;
            float x = spellDetails.xMovement.Evaluate(delta/spellDetails.Lifetime);
            float y = spellDetails.yMovement.Evaluate(delta/spellDetails.Lifetime);
            spell.transform.position = spell.transform.position + (spellDetails.MoveSpeed * Time.deltaTime * (spell.transform.forward * x + spell.transform.right * y));
        }
    }
}
