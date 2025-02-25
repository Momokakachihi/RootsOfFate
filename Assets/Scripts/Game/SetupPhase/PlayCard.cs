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
            Debug.Log($"No card to sacrifice in slot {slotID}");
            return;
        }

        // Check if it's the player's turn and they can interact with this slot
        bool isPlayer1 = TurnManager.Instance.isPlayer1Turn;
        if (!TurnManager.Instance.CanPlayerInteract(slotID, isPlayer1))
        {
            Debug.Log("You cannot sacrifice this card! It's not your turn.");
            return;
        }

        Debug.Log($"Play Card Clicked in Slot {slotID}! Opening Sacrifice Popup.");

        if (slotID <= 5)
        {
            PlayCardManager.Instance.sacrificePopup.SetActive(true);
        }
        else
        {
            PlayCardManager.Instance.sacrificePopupAlt.SetActive(true);
        }

        // Tell the manager which slot is being sacrificed
        PlayCardManager.Instance.currentSlotID = slotID;
    }

}
