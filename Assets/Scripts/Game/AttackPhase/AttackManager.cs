using System.Collections;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public static AttackManager Instance;

    public PlayCardManager playCardManager;
    public TurnManager turnManager;
    public PlayerHealth player1Health;
    public PlayerHealth player2Health;
    public GameObject blockPopup; // Assign in Inspector

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartAttack()
    {
        Debug.Log("Starting attack phase...");
        turnManager.InitiateAttack(); // Switch camera using TurnManager
        blockPopup.SetActive(true); // Show block popup
    }

    public void SkipBlocking()
    {
        blockPopup.SetActive(false);
        ResolveAttack(false);
    }

    public void ResolveAttack(bool opponentBlocks)
    {
        int totalDamage = playCardManager.CalculateTotalDamage(turnManager.isPlayer1Turn);

        if (opponentBlocks)
        {
            Debug.Log("Opponent chose to block. Resolving blocking phase...");
            HandleBlocking();
        }
        else
        {
            Debug.Log("Opponent did not block! Applying direct damage.");
            ApplyDirectDamage(totalDamage);
        }
    }

    private void HandleBlocking()
    {
        bool isOpponentPlayer1 = !turnManager.isPlayer1Turn;
        int[] blockingSlots = playCardManager.GetOccupiedSlots(isOpponentPlayer1);
        int[] attackingSlots = playCardManager.GetOccupiedSlots(turnManager.isPlayer1Turn);

        if (blockingSlots.Length == 0)
        {
            Debug.Log("No available cards to block, applying direct damage.");
            ApplyDirectDamage(playCardManager.CalculateTotalDamage(turnManager.isPlayer1Turn));
            return;
        }

        int totalExcessDamage = 0; // Store any leftover damage to apply to the opponent

        for (int i = 0; i < blockingSlots.Length; i++)
        {
            int blockSlotID = blockingSlots[i];
            int blockHP = playCardManager.GetCardHealth(blockSlotID);
            int attackSlotID = (i < attackingSlots.Length) ? attackingSlots[i] : -1; // Get matching attacker slot
            int attackDMG = (attackSlotID != -1) ? playCardManager.GetSlotDamage(attackSlotID) : 0;

            if (attackSlotID != -1)
            {
                // Attacking card exists, check for destruction
                if (blockHP == attackDMG)
                {
                    // Both attacker and blocker are destroyed
                    Debug.Log($"Attacker in slot {attackSlotID} and blocker in slot {blockSlotID} both destroyed!");
                    playCardManager.ClearSlotInstantly(blockSlotID);
                    playCardManager.ClearSlotInstantly(attackSlotID);
                }
                else if (blockHP > attackDMG)
                {
                    // Blocker survives with reduced HP
                    int remainingHP = blockHP - attackDMG;
                    playCardManager.UpdateCardHealth(blockSlotID, remainingHP);
                    Debug.Log($"Blocking card in slot {blockSlotID} survived with {remainingHP} HP.");
                }
                else
                {
                    // Blocker is destroyed, calculate spillover damage
                    int excessDamage = attackDMG - blockHP;
                    totalExcessDamage += Mathf.Max(0, excessDamage);
                    Debug.Log($"Blocking card in slot {blockSlotID} was destroyed! Excess damage: {excessDamage}");

                    playCardManager.ClearSlotInstantly(blockSlotID);
                }
            }
        }

        // Apply any leftover damage to the opponent
        if (totalExcessDamage > 0)
        {
            ApplyDirectDamage(totalExcessDamage);
            Debug.Log($"Excess damage {totalExcessDamage} applied to opponent.");
        }

        StartCoroutine(EndTurnAfterAttack());
    }

    private void ApplyDirectDamage(int damage)
    {
        if (turnManager.isPlayer1Turn)
            player2Health.TakeDamage(damage);
        else
            player1Health.TakeDamage(damage);

        Debug.Log($"Applied {damage} direct damage to opponent.");
        StartCoroutine(EndTurnAfterAttack());
    }

    private IEnumerator EndTurnAfterAttack()
    {
        yield return new WaitForSeconds(1f);
        blockPopup.SetActive(false);
        turnManager.EndTurn();
    }

    public void ConfirmBlock()
    {
        Debug.Log("Opponent confirmed blocking with all available cards.");
        blockPopup.SetActive(false);
        ResolveAttack(true);
    }
}
