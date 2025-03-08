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
        optionToggle.isOn = false;
        checkBox.sprite =  defaultSprite;
        gameObject.SetActive(true);

        optionToggle.onValueChanged.RemoveAllListeners();
        optionToggle.onValueChanged.AddListener(OnOptionSelected);
    }

    private void OnOptionSelected(bool flag)
    {
        checkBox.sprite = flag ? selectedSprite : defaultSprite;

        if (flag)
        {
            Debug.Log("Selected Option :: " + optionText.text);
            QAPanelView.ResultAction?.Invoke(optionText.text);
        }
    }
}
