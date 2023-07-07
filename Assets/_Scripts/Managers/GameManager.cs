using System;
using UnityEngine;

/// <summary>
/// Nice, easy to understand enum-based game manager. For larger and more complex games, look into
/// state machines. But this will serve just fine for most games.
/// </summary>
public class GameManager : StaticInstance<GameManager> {
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    public GameState State { get; private set; }

    // Kick the game off with the first state
    void Start() => ChangeState(GameState.Gameplay);

    public void ChangeState(GameState newState) {
        if (newState == State) return;
        
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState) {
            case GameState.Gameplay:
                HandleChangeToGameplay();
                break;
            case GameState.Cutscene:
                HandleCutscene();
                break;
            case GameState.Pause:
                PauseGame();
                break;
            case GameState.GameOver:
                break;
            case GameState.Win:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);
        
        Debug.Log($"New state: {newState}");
    }

    private void HandleChangeToGameplay() {

    }
    
    private void HandleCutscene() {
        
    }

    private void PauseGame() {
        
    }
    
    
}

[Serializable]
public enum GameState {
    Gameplay,
    Cutscene,
    Pause,
    GameOver,
    Win,
}