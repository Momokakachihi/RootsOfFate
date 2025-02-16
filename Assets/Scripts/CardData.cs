[System.Serializable]
public class CardData
{
    public int cardValue; // The value of the card (1-5)
    public int slotID;    // The slot where the card belongs (1-5)
    public string CardName; // The name of the card

    public CardData(int value, int slot, string name)
    {
        cardValue = value;
        slotID = slot;
        CardName = name;
    }
}