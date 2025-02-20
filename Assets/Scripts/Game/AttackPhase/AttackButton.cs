using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour
{
    public Button attackButton;

    void Start()
    {
        attackButton.onClick.AddListener(() => AttackManager.Instance.Attack());
    }
}
