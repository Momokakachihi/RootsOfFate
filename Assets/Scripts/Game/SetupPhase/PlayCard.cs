using UnityEngine;

public class PlayCard : MonoBehaviour
{
    public int slotID; // Track which slot this card belongs to

    void OnMouseDown()
    {
        if (PlayCardManager.Instance == null)
        {
            Debug.LogError("PlayCardManager instance is missing!");
            return;
        }

        // Check if this slot actually has a card
        if (!PlayCardManager.Instance.IsSlotOccupied(slotID))
        {
            Debug.Log("No card to sacrifice in this slot!");
            return;
        }

        Debug.Log($"Play Card Clicked in Slot {slotID}! Opening Sacrifice Popup.");
        PlayCardManager.Instance.sacrificePopup.SetActive(true);

        // Tell the manager which slot is being sacrificed
        PlayCardManager.Instance.currentSlotID = slotID;
    }
}
