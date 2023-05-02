using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedGameObjectVisual;

    private BaseAction baseAction;

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        textMeshPro.text = baseAction.GetActionName().ToUpper();
        button.onClick.AddListener(() => UnitActionSystem.Instance.SetSelectedAction(baseAction));
    }

    public void UpdateSelectedVisual(){
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        selectedGameObjectVisual.SetActive(selectedAction == baseAction);
    }

}
