using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private PlanningUI planningUI;


    void Start()
    {
        GameManager.OnAfterStateChanged += HandleGameStateChange;
        PlayPhaseManager.OnAfterPlayPhaseChanged += HandlePlayPhaseChange;
    }

    private void HandlePlayPhaseChange(PlayPhase phase)
    {
        if(phase == PlayPhase.Planning) {
            planningUI.gameObject.SetActive(true);
            // defenceUI.gameObject.SetActive(false);
        } else if(phase == PlayPhase.Defence) {
            planningUI.gameObject.SetActive(false);
            // defenceUI.gameObject.SetActive(true);
        }
    }

    private void HandleGameStateChange(GameState state)
    {
        if(state == GameState.Gameplay) {
            // something
        } else {
            planningUI.gameObject.SetActive(false);
            // defenceUI.gameObject.SetActive(false);
        }
    }
}
