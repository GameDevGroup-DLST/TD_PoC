using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            GameState currentState = GameManager.Instance.State;
            GameState newGameState = currentState == GameState.Gameplay ? GameState.Pause : GameState.Gameplay;
            
            GameManager.Instance.ChangeState(newGameState);
        }
    }
}
