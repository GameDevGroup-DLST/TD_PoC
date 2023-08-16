using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade Card", menuName = "ScriptableObjects/Cards/Upgrade")]
public class UpgradeCardScriptableObject : CardScriptableObject
{
    public string upgradeName;
    public string upgradeDescription;
    public Sprite upgradeThumbnail;
    public int rarity;
}
