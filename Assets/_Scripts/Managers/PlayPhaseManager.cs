using System;
using UnityEngine;

public class PlayPhaseManager : StaticInstance<PlayPhaseManager>
{
    public static event Action<PlayPhase> OnBeforePlayPhaseChanged;
    public static event Action<PlayPhase> OnAfterPlayPhaseChanged;

    public PlayPhase Phase { get; private set; }

    protected override void Awake() {
        base.Awake();
        GameManager.OnBeforeStateChanged += OnGameStateChange;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            ChangePhase(PlayPhase.Planning);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            ChangePhase(PlayPhase.Defence);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)) {
            ChangePhase(PlayPhase.Victory);
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)) {
            ChangePhase(PlayPhase.Defeat);
        }
        if(Input.GetKeyDown(KeyCode.Alpha0)) {
            ChangePhase(PlayPhase.None);
        }
    }

    public void ChangePhase(PlayPhase newPhase) {
        if (newPhase == Phase) return;
        
        PlayPhase prevPhase = Phase;
        OnBeforePlayPhaseChanged?.Invoke(newPhase);

        Phase = newPhase;
        switch (newPhase) {
            case PlayPhase.Planning:
                HandlePlanning();
                break;
            case PlayPhase.Defence:
                HandleDefence();
                break;
            case PlayPhase.Victory:
                HandleVictory();
                break;
            case PlayPhase.Defeat:
                HandleDefeat();
                break;
            case PlayPhase.None:
                // Maybe we can just clean up everything here
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newPhase), newPhase, null);
        }

        OnAfterPlayPhaseChanged?.Invoke(newPhase);
        
        Debug.Log($"New Play Phase: {newPhase} from {prevPhase}");
    }

    private void HandlePlanning() {
        // Bring up planning UI
    }
    
    private void HandleDefence() {
        // Spawn Enemies

        // Begin timer to round end
    }

    private void HandleVictory() {
        // Show Results


        ChangePhase(PlayPhase.None);
        GameManager.Instance.ChangeState(GameState.LevelSelect);
    }

    private void HandleDefeat() {
        // Show Results

        
        ChangePhase(PlayPhase.None);
        GameManager.Instance.ChangeState(GameState.GameOver);
    }

    private void OnGameStateChange(GameState Phase)
    {
        if(Phase == GameState.Gameplay) { // Prepare to go into Planning
            ChangePhase(PlayPhase.Planning);
        }
    }
}

[Serializable]
public enum PlayPhase {
    None,
    Planning,
    Defence,
    Victory,
    Defeat,
}