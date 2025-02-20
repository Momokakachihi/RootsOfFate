using System.ComponentModel;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public static AttackManager Instance;
    public PlayCardManager playCardManager;
    public Opponent opponent;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Attack()
    {
        Debug.Log("Attack phase started!");

        for (int i = 0; i < playCardManager.playCardTexts.Length; i++)
        {
            if (!playCardManager.IsSlotOccupied(i + 1)) continue; // Skip empty slots

            string cardInfo = playCardManager.playCardTexts[i].text;

            if (cardInfo.Contains("Used Ability"))
            {
                Debug.Log($"Card in Slot {i + 1} has used its ability and cannot attack.");
                continue;
            }

            Debug.Log($"Card from Slot {i + 1} attacks!");

            bool blocked = opponent.TryBlock();

            if (!blocked)
            {
                opponent.TakeDamage(1); // Default damage is 1 per attack
            }
        }
    }
}
