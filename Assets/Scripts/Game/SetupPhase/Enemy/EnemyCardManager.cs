using UnityEngine;
using UnityEngine.UI;

public class EnemyCardManager : MonoBehaviour
{
    public static EnemyCardManager Instance;

    public Text[] enemyPlayCardTexts; // UI Texts for enemy's play slots (1-5)
    public GameObject sacrificePopup; // Assign in Inspector
    public int currentSlotID; // Track which slot is being sacrificed

    private bool[] enemySlotsOccupied = new bool[5]; // Tracks enemy slots
    public int enemyEnergy = 0; // Enemy's energy count

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetEnemyCard(string cardName, int cardValue, int slotID)
    {
        if (slotID < 1 || slotID > 5)
        {
            Debug.LogError("Invalid slot ID!");
            return;
        }

        int index = slotID - 1;

        if (enemySlotsOccupied[index])
        {
            Debug.Log($"Enemy's Slot {slotID} is already occupied!");
            return;
        }

        enemyPlayCardTexts[index].text = $"Card: {cardName}\nValue: {cardValue}";
        enemySlotsOccupied[index] = true;
        Debug.Log($"Transferred {cardName} (Value: {cardValue}) to Enemy's PlayCard Slot {slotID}");
    }

    public bool IsEnemySlotOccupied(int slotID)
    {
        if (slotID < 1 || slotID > 5) return false;
        return enemySlotsOccupied[slotID - 1];
    }

    public void ConfirmEnemySacrifice()
    {
        if (currentSlotID < 1 || currentSlotID > 5)
        {
            Debug.LogError("Invalid slot ID for sacrifice!");
            return;
        }

        int index = currentSlotID - 1;

        if (!enemySlotsOccupied[index])
        {
            Debug.Log($"Enemy's Slot {currentSlotID} is already empty!");
            return;
        }

        Debug.Log($"Enemy sacrificed card in slot {currentSlotID}! +1 Energy");
        enemyEnergy += 1;

        // Clear slot
        enemyPlayCardTexts[index].text = "";
        enemySlotsOccupied[index] = false;

        // Close popup
        sacrificePopup.SetActive(false);
        currentSlotID = 0; // Reset slot tracking
    }

    public void ClosePopup()
    {
        sacrificePopup.SetActive(false);
    }
}
