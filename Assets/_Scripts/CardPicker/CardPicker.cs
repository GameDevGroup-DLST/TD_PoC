using System.Collections.Generic;
using UnityEngine;

public class CardPicker : MonoBehaviour
{
    [SerializeField] protected Card cardPrefab;
    [SerializeField] protected Transform cardContainer;
    [SerializeField] protected CardScriptableObject dummySO;

    protected List<Card> cards = new List<Card>();

    private void OnEnable() {
        PopulateLevelPicker();
    }

    private void OnDisable() {
        foreach (var instance in cards) {
            GameObject.Destroy(instance);
        }
        cards.Clear();
    }

    public void PopulateLevelPicker() { // Need some sort of decision maker to grab levels
        for (int i = 0; i < 3; i++) {
            Card card = Instantiate(cardPrefab, cardContainer);
            card.SetCardData(dummySO);
            card.Ininitialize();
            cards.Add(card);
        }
    }
}
