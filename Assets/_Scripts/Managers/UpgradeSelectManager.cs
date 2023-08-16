using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSelectManager : StaticInstance<UpgradeSelectManager>
{
    [SerializeField] private UpgradePicker upgradeSelectUIPrefab;
    private UpgradePicker upgradeSelectInstance = null;

    private Transform mainUIContainer = null;

    protected override void Awake()
    {
        base.Awake();

        PlayPhaseManager.OnAfterPlayPhaseChanged += HandlePhaseChange;
    }

    private void HandlePhaseChange(PlayPhase phase)
    {
        if(phase == PlayPhase.Victory) {
            if(!upgradeSelectInstance) {
                if(!mainUIContainer) {
                    mainUIContainer = GameObject.FindGameObjectWithTag("UIContainer").transform;
                    if(!mainUIContainer) throw new System.Exception("Could Not Find UI Container");
                }
                upgradeSelectInstance = Instantiate(upgradeSelectUIPrefab, mainUIContainer);
            }
        } else {
            if(upgradeSelectInstance) {
                Object.Destroy(upgradeSelectInstance.gameObject);
                upgradeSelectInstance = null;
            }
        }
    }
}
