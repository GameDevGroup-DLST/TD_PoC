using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// One repository for all scriptable objects. Create your query methods here to keep your business logic clean.
/// I make this a MonoBehaviour as sometimes I add some debug/development references in the editor.
/// If you don't feel free to make this a standard class
/// </summary>
public class ResourceSystem : StaticInstance<ResourceSystem> {
    public List<SpellScriptableObject> Spells { get; private set; }
    private Dictionary<string, SpellScriptableObject> _SpellDict;
    
    protected override void Awake() {
        base.Awake();
        AssembleResources();
    }
    
    private void AssembleResources() {
        Spells = Resources.LoadAll<SpellScriptableObject>("Spells").ToList();
        _SpellDict = Spells.ToDictionary(r => r.SpellName, r => r);
    }
}   