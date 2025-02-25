using System.Collections.Generic;
using UnityEngine;

public class CardSlotManager : MonoBehaviour
{
    // List of slots (assign in the Inspector)
    public List<Slot> slots;

    private List<CardData> cardDataList = new List<CardData>();
    void Start()
    {
        InitializeCardData();
        AssignCardValues();
    }

    void AssignCardValues()
    {
        Debug.Log($"Card Data List Count: {cardDataList.Count}");
        foreach (Slot slot in slots)
        {
            slot.InitializeCards(cardDataList);
        }
    }


    // Example: Call this method when a slot is clicked
    public void OnSlotChosen(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count)
        {
            Debug.LogError("Invalid slot index!");
            return;
        }

        // Choose a random card from the selected slot
        slots[slotIndex].ChooseRandomCard();
    }

    void InitializeCardData()
    {
        for (int slot = 1; slot <= 10; slot++) // Slot 1 to 10
        {
            for (int value = 1; value <= 5; value++) // 5 cards per slot
            {
                char cardName = (char)('A' + (slot - 1) * 5 + (value - 1)); // Generates 'A' to 'Y'
                cardDataList.Add(new CardData(value, slot, $"Card {cardName}"));
            }
        }
    }

}