using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptionsManager : StaticInstance<GameOptionsManager>
{
    [SerializeField] private Transform mainCanvas;
    
    [SerializeField] private GameObject optionsPanelPrefab;
    
    private GameObject _optionsPanelInstance;
    
    public void ToggleOptionsPanel()
    {
        if(_optionsPanelInstance != null)
        {
            Destroy(_optionsPanelInstance);
        }
        else
        {
            _optionsPanelInstance = Instantiate(optionsPanelPrefab, mainCanvas);
        }
        
    }
}
