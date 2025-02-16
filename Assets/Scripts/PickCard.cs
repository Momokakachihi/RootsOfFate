using UnityEngine;

public class PickCard : MonoBehaviour
{
    private Vector3 targetScale = new Vector3(1f, 1f, 1f);
    private Renderer cardRenderer;
    private Color originalColor;
    public Color highlightColor = Color.yellow;
    public int CardValue;
    public string CardName;

    public float colorTransitionSpeed = 1f;
    public float scaleSpeed = 5f;
    public float scaleX = 0.2f;
    public float scaleY = 0.3f;
    private bool isSelected = false;

    private Slot parentSlot;

    void Start()
    {
        cardRenderer = GetComponent<Renderer>();
        originalColor = cardRenderer.material.color;
        parentSlot = GetComponentInParent<Slot>(); // Get parent slot reference
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
        cardRenderer.material.color = Color.Lerp(cardRenderer.material.color, originalColor, Time.deltaTime * colorTransitionSpeed);
    }

    void OnMouseEnter()
    {
        if (!isSelected && IsTopCard())
        {
            cardRenderer.material.color = highlightColor;

            float newScaleX = targetScale.x * (1 + scaleX);
            float newScaleY = targetScale.y * (1 + scaleY);
            transform.localScale = new Vector3(newScaleX, newScaleY, targetScale.z);
        }
    }

    void OnMouseExit()
    {
        if (!isSelected)
        {
            cardRenderer.material.color = originalColor;
            transform.localScale = targetScale;
        }
    }

    void OnMouseDown()
    {
        if (!IsTopCard())
        {
            Debug.Log("Cannot pick this card! Only the top card can be selected.");
            return;
        }

        int slotID = parentSlot.slotID;
        if (PlayCardManager.Instance.IsSlotOccupied(slotID))
        {
            Debug.Log($"Slot {slotID} is already occupied! Choose another.");
            return;
        }

        isSelected = true;
        cardRenderer.material.color = Color.green;

        Debug.Log("Card Selected: " + CardName);
        PlayCardManager.Instance.SetPlayCard(CardName, CardValue, slotID);

        parentSlot.cards.Remove(this);
        Destroy(gameObject);
    }

    public void Reveal()
    {
        Debug.Log("Revealed Card: " + CardName + ", Value: " + CardValue);
    }

    private bool IsTopCard()
    {
        if (parentSlot == null || parentSlot.cards.Count == 0) return false;

        return parentSlot.cards[parentSlot.cards.Count - 1] == this; // Check if this card is last in the list
    }
}
