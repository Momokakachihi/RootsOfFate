using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    public bool isPlayer1Turn = true;
    public Text turnText;
    public Button endTurnButton;
    public Camera player1Camera;
    public Camera player2Camera;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateTurnUI();
        endTurnButton.onClick.AddListener(EndTurn);
    }

    public void EndTurn()
    {
        isPlayer1Turn = !isPlayer1Turn;
        UpdateTurnUI();
        SwitchCamera();
    }

    public void InitiateAttack()
    {
        SwitchCamera();
    }

    public bool CanPlayerInteract(int slotID, bool isPlayer1)
    {
        if (isPlayer1Turn != isPlayer1) return false;
        if (isPlayer1 && slotID > 5) return false;
        if (!isPlayer1 && slotID <= 5) return false;

        return true;
    }

    private void SwitchCamera()
    {
        player1Camera.gameObject.SetActive(isPlayer1Turn);
        player2Camera.gameObject.SetActive(!isPlayer1Turn);
    }

    private void UpdateTurnUI()
    {
        if (turnText != null)
            turnText.text = isPlayer1Turn ? "Player 1's Turn" : "Player 2's Turn";
    }
}
