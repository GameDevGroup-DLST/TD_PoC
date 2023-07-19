using System;
using UnityEngine;

public class GameStateManager : StaticInstance<GameStateManager>
{
    public static event Action<GameplayState> OnBeforeGameplayStateChanged;
    public static event Action<GameplayState> OnAfterGameplayStateChanged;

    public GameplayState State { get; private set; }

    protected override void Awake() {
        base.Awake();
        GameManager.OnBeforeStateChanged += OnGameStateChange;
    }

    // Kick the game off with the first state
    void Start() => ChangeState(GameplayState.None);

    public void ChangeState(GameplayState newState) {
        if (newState == State || GameManager.Instance.State != GameState.Gameplay) return;
        
        OnBeforeGameplayStateChanged?.Invoke(newState);

        State = newState;
        switch (newState) {
            case GameplayState.Planning:
                HandlePlanning();
                break;
            case GameplayState.Defence:
                HandleDefence();
                break;
            case GameplayState.Victory:
                HandleVictory();
                break;
            case GameplayState.Defeat:
                HandleDefeat();
                break;
            case GameplayState.None:
                // Maybe we can just clean up everything here
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterGameplayStateChanged?.Invoke(newState);
        
        Debug.Log($"New state: {newState}");
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
        GameManager.Instance.ChangeState(GameState.LevelSelect);
    }

    private void HandleDefeat() {
        // Show Results
        GameManager.Instance.ChangeState(GameState.GameOver);
    }

    private void OnGameStateChange(GameState state)
    {
        if(state == GameState.Gameplay) { // Prepare to go into Planning
            ChangeState(GameplayState.Planning);
        }
    }
}

[Serializable]
public enum GameplayState {
    Planning,
    Defence,
    Victory,
    Defeat,
    None
}