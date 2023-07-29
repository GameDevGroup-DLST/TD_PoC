using UnityEngine;
using UnityEngine.UI;

abstract public class Card : MonoBehaviour
{
    [SerializeField] protected Image cardImage;
    protected CardScriptableObject cardData;
    
    abstract public void Ininitialize();
    abstract public void HandleSelect();

    public void SetCardData(CardScriptableObject so) {
        cardData = so;
    }
}
