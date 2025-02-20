using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    // List of cards in this slot
    public List<PickCard> cards = new List<PickCard>();

    // Slot ID (1-5)
    public int slotID;

    // Assign card values from the makeshift database
    public void InitializeCards(List<CardData> cardDataList)
    {

        

        Debug.Log($"Initializing Slot {slotID}...");

        List<CardData> slotCardData = cardDataList.FindAll(data => data.slotID == slotID);

        Debug.Log($"Slot {slotID} found {slotCardData.Count} cards.");


        ShuffleList(slotCardData);
        for (int i = 0; i < cards.Count; i++)
        {
            if (i < slotCardData.Count)
            {
                cards[i].CardValue = slotCardData[i].cardValue;
                cards[i].CardName = slotCardData[i].CardName;
                Debug.Log($"Assigned {cards[i].CardName} (Value: {cards[i].CardValue}) to Slot {slotID}");
            }
            else
            {
                Debug.LogError($"Slot {slotID} does not have enough cards! Expected {cards.Count}, got {slotCardData.Count}");
            }
        }

    }


    public void ChooseRandomCard()
    {
        if (cards.Count == 0)
        {
            Debug.LogError("No cards in this slot!");
            return;
        }

        // Randomly pick a card from the list
        int randomIndex = Random.Range(0, cards.Count);
        PickCard chosenCard = cards[randomIndex];

        // Reveal the chosen card
        chosenCard.Reveal();
    }
    private void ShuffleList(List<CardData> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]); // Swap values
        }
    }
}