using UnityEngine;
using UnityEngine.UI; // Import TextMeshPro namespace

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 20;
    private int currentHealth;

    public Text healthText; // UI reference

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        Debug.Log($"{gameObject.name} took {damage} damage! Remaining HP: {currentHealth}");
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Debug.Log($"{gameObject.name} has been defeated!");
            // Add game-over logic here
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        Debug.Log($"{gameObject.name} healed! Current HP: {currentHealth}");
        UpdateHealthUI();
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = $"{currentHealth}";
    }
}
