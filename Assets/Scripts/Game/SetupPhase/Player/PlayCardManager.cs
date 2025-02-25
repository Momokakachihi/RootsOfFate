using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayCardManager : MonoBehaviour
{
    public static PlayCardManager Instance;

    public Text[] playCardTexts; // UI Text for each PlayCard slot (1-10)
    public GameObject sacrificePopup;
    public GameObject sacrificePopupAlt;
    public GameObject blockPopup;

    public int currentSlotID; // Slot being sacrificed or blocked
    public int energy = 0; // Player energy
    private bool[] slotOccupied = new bool[10]; // Tracks slot usage

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ✅ Set Card in a Slot
    public void SetPlayCard(string cardName, int cardValue, int slotID, bool isPlayer1)
    {
        if (!TurnManager.Instance.CanPlayerInteract(slotID, isPlayer1))
        {
            Debug.Log("Invalid action! Not your turn or wrong slot.");
            return;
        }

        if (slotID < 1 || slotID > 10)
        {
            Debug.LogError("Invalid slot ID!");
            return;
        }

        int index = slotID - 1;

        if (slotOccupied[index])
        {
            Debug.Log($"Slot {slotID} is already occupied!");
            return;
        }

        playCardTexts[index].text = $"Card: {cardName}\nValue: {cardValue}";
        slotOccupied[index] = true;
        Debug.Log($"Placed {cardName} (Value: {cardValue}) in Slot {slotID}");
    }

    public void ClearSlotInstantly(int slotID)
    {
        if (slotID < 1 || slotID > 10)
        {
            Debug.LogError("Invalid slot ID for clearing!");
            return;
        }

        int index = slotID - 1;

        // ✅ Clear slot (same as ConfirmSacrifice)
        playCardTexts[index].text = "";
        slotOccupied[index] = false;

        Debug.Log($"Cleared slot {slotID} after card was destroyed.");
    }

    // ✅ Confirm Sacrifice
    public void ConfirmSacrifice()
    {
        if (currentSlotID < 1 || currentSlotID > 10)
        {
            Debug.LogError("Invalid slot ID for sacrifice!");
            return;
        }

        int index = currentSlotID - 1;
        bool isPlayer1Turn = TurnManager.Instance.isPlayer1Turn;
        bool isPlayer1Slot = currentSlotID <= 5; // P1 = Slots 1-5, P2 = Slots 6-10

        // ❌ Prevent sacrificing opponent's cards
        if ((isPlayer1Turn && !isPlayer1Slot) || (!isPlayer1Turn && isPlayer1Slot))
        {
            Debug.LogError("You cannot sacrifice your opponent's card!");
            return;
        }

        // ❌ Prevent sacrificing an empty slot
        if (!slotOccupied[index])
        {
            Debug.Log($"Slot {currentSlotID} is already empty!");
            return;
        }

        // ✅ Perform the sacrifice
        Debug.Log($"Sacrificed card in slot {currentSlotID}! +1 Energy");
        energy += 1;

        // ✅ Clear slot
        playCardTexts[index].text = "";
        slotOccupied[index] = false;

        // ✅ Close the correct popup
        if (currentSlotID <= 5) sacrificePopup.SetActive(false);
        else sacrificePopupAlt.SetActive(false);

        currentSlotID = 0; // Reset slot tracking
    }

    // ✅ Confirm Block
    public void ConfirmBlock()
    {
        if (currentSlotID < 1 || currentSlotID > 10)
        {
            Debug.LogError("Invalid blocking slot!");
            return;
        }

        int index = currentSlotID - 1;

        // ❌ Check if a card exists to block
        if (!slotOccupied[index])
        {
            Debug.LogError("No card to block with!");
            return;
        }

        Debug.Log($"Blocking attack with card in slot {currentSlotID}");

        // ✅ Resolve blocking in AttackManager
        AttackManager.Instance.ResolveAttack(true);

        // Close block popup
        blockPopup.SetActive(false);
    }

    // ✅ Calculate Damage from Active Cards
    public int CalculateTotalDamage(bool isPlayer1)
    {
        int totalDamage = 0;

        for (int i = 0; i < 10; i++)
        {
            if (slotOccupied[i])
            {
                bool isPlayer1Slot = (i < 5); // Slots 1-5 for Player 1, 6-10 for Player 2

                if ((isPlayer1 && isPlayer1Slot) || (!isPlayer1 && !isPlayer1Slot))
                {
                    totalDamage += GetSlotDamage(i + 1);
                }
            }
        }

        Debug.Log($"Total attack damage: {totalDamage}");
        return totalDamage;
    }

    // ✅ Get Damage for a Specific Slot
    public int GetSlotDamage(int slotID)
    {
        return ((slotID - 1) % 5) + 1; // Slot 1 = 1 DMG, Slot 2 = 2 DMG, ..., Slot 5 = 5 DMG
    }

    // ✅ Get Card Health (For Blocking)
    public int GetCardHealth(int slotID)
    {
        return ((slotID - 1) % 5) + 1;
    }

    // ✅ Update Card Health (After Blocking)
    public void UpdateCardHealth(int slotID, int newHealth)
    {
        if (newHealth <= 0)
        {
            RemoveCard(slotID);
        }
        else
        {
            Debug.Log($"Card in slot {slotID} now has {newHealth} HP.");
        }
    }

    // ✅ Remove Card (Destroyed After Blocking)
    public void RemoveCard(int slotID)
    {
        int index = slotID - 1;
        playCardTexts[index].text = "";
        slotOccupied[index] = false;
        Debug.Log($"Card in slot {slotID} has been removed.");
    }

    // ✅ Close Popup (Sacrifice or Block)
    public void ClosePopup()
    {
        sacrificePopup.SetActive(false);
        sacrificePopupAlt.SetActive(false);
        blockPopup.SetActive(false);
    }

    public int GetSelectedBlockSlot()
    {
        return currentSlotID; // Returns the slot selected for blocking
    }

    public bool IsSlotOccupied(int slotID)
    {
        int index = slotID - 1;
        return index >= 0 && index < slotOccupied.Length && slotOccupied[index];
    }

    public int[] GetOccupiedSlots(bool isPlayer1)
    {
        List<int> occupiedSlots = new List<int>();

        for (int i = 0; i < slotOccupied.Length; i++)
        {
            if (slotOccupied[i])
            {
                int slotID = i + 1;

                // Ensure it's only getting the correct player's slots
                if ((isPlayer1 && slotID <= 5) || (!isPlayer1 && slotID > 5))
                {
                    occupiedSlots.Add(slotID);
                }
            }
        }

        return occupiedSlots.ToArray();
    }
}
