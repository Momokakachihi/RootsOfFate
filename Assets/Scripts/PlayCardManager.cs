using UnityEngine;
using UnityEngine.UI;

public class PlayCardManager : MonoBehaviour
{
    public static PlayCardManager Instance;

    public Text[] playCardTexts; // UI Text for each PlayCard slot (1-5)
    public GameObject sacrificePopup; // Assign in Inspector
    public int currentSlotID; // Track which slot is being sacrificed

    private bool[] slotOccupied = new bool[5]; // Tracks each slot
    public int energy = 0; // Player's energy count

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetPlayCard(string cardName, int cardValue, int slotID)
    {
        if (slotID < 1 || slotID > 5)
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
        Debug.Log($"Transferred {cardName} (Value: {cardValue}) to PlayCard Slot {slotID}");
    }

    public bool IsSlotOccupied(int slotID)
    {
        if (slotID < 1 || slotID > 5) return false;
        return slotOccupied[slotID - 1];
    }

    public void ConfirmSacrifice()
    {
        if (currentSlotID < 1 || currentSlotID > 5)
        {
            Debug.LogError("Invalid slot ID for sacrifice!");
            return;
        }

        int index = currentSlotID - 1;

        if (!slotOccupied[index])
        {
            Debug.Log($"Slot {currentSlotID} is already empty!");
            return;
        }

        Debug.Log($"Sacrificed card in slot {currentSlotID}! +1 Energy");
        energy += 1;

        // Clear slot
        playCardTexts[index].text = "";
        slotOccupied[index] = false;

        // Close popup
        sacrificePopup.SetActive(false);
        currentSlotID = 0; // Reset slot tracking
    }

    public void ClosePopup()
    {
        sacrificePopup.SetActive(false);
    }
}
