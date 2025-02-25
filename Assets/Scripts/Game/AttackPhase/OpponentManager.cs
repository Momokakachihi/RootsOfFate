using UnityEngine;
using UnityEngine.UI;

public class OpponentManager : MonoBehaviour
{
    public static OpponentManager Instance;
    public int health = 20;
    public Text opponentHPText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateHPDisplay();
    }

    public void TakeDamage(int damage, int slotID)
    {
        Debug.Log($"Opponent takes {damage} damage from Slot {slotID}!");
        health -= damage;

        if (health <= 0)
        {
            Debug.Log("Opponent has been defeated!");
        }
        UpdateHPDisplay();
    }

    public bool TryBlock(int slotID)
    {
        bool willBlock = Random.value > 0.5f; // 50% chance to block

        if (willBlock)
        {
            Debug.Log($"Opponent blocked attack from Slot {slotID}!");
        }
        else
        {
            Debug.Log($"Opponent failed to block attack from Slot {slotID}!");
        }

        return willBlock;
    }

    private void UpdateHPDisplay()
    {
        if (opponentHPText != null)
        {
            opponentHPText.text = "Opponent HP: " + health;
        }
        else
        {
            Debug.LogWarning("Opponent HP Text is not assigned!");
        }
    }

    public void OpponentTurn()
    {
        Debug.Log("Opponent's turn! (AI logic can be added here)");

        // TODO: Implement AI actions like attacking or playing cards.

        TurnManager.Instance.EndTurn(); // Switch back to Player after opponent's actions
    }
}
