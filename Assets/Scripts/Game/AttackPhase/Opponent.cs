using UnityEngine;
using UnityEngine.UI;

public class Opponent : MonoBehaviour
{
    public int health = 20; // Opponent's starting health
    public Text opponentHPText; // Reference to UI Text element

    private void Start()
    {
        UpdateHPDisplay();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"Opponent takes {damage} damage!");
        health -= damage;

        if (health <= 0)
        {
            Debug.Log("Opponent has been defeated!");
        }
        UpdateHPDisplay();
    }

    public bool TryBlock()
    {
        // Placeholder for blocking logic (e.g., random chance, user input, etc.)
        bool willBlock = Random.value > 0.5f; // 50% chance to block

        if (willBlock)
        {
            Debug.Log("Opponent blocked the attack!");
        }
        else
        {
            Debug.Log("Opponent failed to block the attack!");
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
}
