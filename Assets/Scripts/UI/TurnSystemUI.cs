using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnSystemUI : MonoBehaviour
{

    [SerializeField] private Button endTurnButton;
    [SerializeField] private TextMeshProUGUI turnNumberText;

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystemUI_OnTurnChanged;
        
        endTurnButton.onClick.AddListener(() => TurnSystem.Instance.NextTurn());
        
        UpdateTurnText();
    }

    private void TurnSystemUI_OnTurnChanged(object sender, System.EventArgs e)
    {
        UpdateTurnText();
    }

    private void UpdateTurnText()
    {
        turnNumberText.text = "Turn " + TurnSystem.Instance.GetTurnNumber();
    }

}
