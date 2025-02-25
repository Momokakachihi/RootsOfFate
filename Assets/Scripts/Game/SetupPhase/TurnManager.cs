using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isTurnManager : MonoBehaviour
{
    public static isTurnManager Instance;

    public bool isPlayer1Turn = true; // True = Player 1's turn, False = Player 2's turn

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void EndTurn()
    {
        isPlayer1Turn = !isPlayer1Turn; // Switch turn
        Debug.Log(isPlayer1Turn ? "Player 1's turn!" : "Player 2's turn!");
    }

    public bool CanPlayerInteract(int slotID, bool isPlayer1)
    {
        if (isPlayer1Turn != isPlayer1) return false; // Not their turn
        if (isPlayer1 && slotID > 5) return false;   // Player 1 can't use 6-10
        if (!isPlayer1 && slotID <= 5) return false; // Player 2 can't use 1-5

        return true;
    }
}
