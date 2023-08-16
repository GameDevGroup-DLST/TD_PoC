using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevDebug : MonoBehaviour
{
    public TMPro.TMP_Text gameStateText,
        playPhaseText,
        towerText;

    void Start()
    {
        GameManager.OnBeforeStateChanged += HandleGameStateChanged;
        PlayPhaseManager.OnBeforePlayPhaseChanged += HandlePlayPhaseChanged;        
        TowerGrid.OnTowerChanged += HandleTowerChanged;
    }

    private void HandleTowerChanged(TowerTypeScriptableObject tower) {
        towerText.SetText($"Current Tower: {tower.name}");
    }

    private void HandleGameStateChanged(GameState state)
    {
        playPhaseText.gameObject.SetActive(state == GameState.Gameplay);

        gameStateText.SetText($"Game State: {state}");
    }
    
    private void HandlePlayPhaseChanged(PlayPhase phase)
    {
        towerText.gameObject.SetActive(phase == PlayPhase.Planning);

        playPhaseText.SetText($"Play Phase: {phase}");
    }
}
