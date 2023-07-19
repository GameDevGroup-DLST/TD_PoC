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
    void Start() => ChangeState(GameState.LevelSelect);

    public void ChangeState(GameState newState) {
        if (newState == State) return;
        
        GameState prevState = State;
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState) {
            case GameState.LevelSelect:
                HandleLevelSelect();
                break;
            case GameState.Gameplay:
                HandleChangeToGameplay();
                break;
            case GameState.Cutscene: // May or may not be used
                HandleCutscene();
                break;
            case GameState.Pause:
                PauseGame();
                break;
            case GameState.GameOver:
                break;
            case GameState.Win:
                break;
            case GameState.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);
        
        Debug.Log($"New State: {newState} from {prevState}");
    }

    private void HandleChangeToGameplay() {
        if(PlayPhaseManager.Instance.Phase == PlayPhase.None) {
            PlayPhaseManager.Instance.ChangePhase(PlayPhase.Planning);
        }
    }
    
    private void HandleCutscene() {
        throw new NotImplementedException();
    }

    private void HandleLevelSelect() {
        // Display Level Select Screen

        ChangeState(GameState.Gameplay);
    }

    private void PauseGame() {
        // Display Pause Screen
    }
}

[Serializable]
public enum GameState {
    None,
    LevelSelect,
    Gameplay,
    Cutscene,
    Pause,
    GameOver,
    Win,
}