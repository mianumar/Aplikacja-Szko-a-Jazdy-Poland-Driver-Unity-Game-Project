using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using static UnityEngine.Rendering.GPUSort;

public class OptionItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI optionText;
    [SerializeField] private TextMeshProUGUI contextText;
    [SerializeField] private Toggle optionToggle;
    [SerializeField] private Image checkBox;

    public Sprite defaultSprite;
    public Sprite selectedSprite;

    public void RenderView(string option , string context , ToggleGroup tGroup)
    {
        optionText.text = option;
        contextText.text = context;
        optionToggle.group = tGroup;
        checkBox.sprite =  defaultSprite;
        gameObject.SetActive(true);

        optionToggle.onValueChanged.RemoveAllListeners();
        optionToggle.onValueChanged.AddListener(OnOptionSelected);
    }

    private void OnOptionSelected(bool arg0)
    {
        checkBox.sprite = arg0 ? selectedSprite : defaultSprite;

        Debug.Log("Selected Option :: "+optionText.text);
        QAPanelView.ResultAction?.Invoke(optionText.text);
    }
}
