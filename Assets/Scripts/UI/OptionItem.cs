using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using static UnityEngine.Rendering.GPUSort;

public class OptionItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI optionText;
    [SerializeField] private TextMeshProUGUI contextText;
    [SerializeField] private Button optionButton;
    [SerializeField] private Image checkBox;
    public int index;

    public Sprite defaultSprite;
    public Sprite selectedSprite;

    public void RenderView(string option , string context , ToggleGroup tGroup)
    {
        optionText.text = option;
        contextText.text = context;
       
        checkBox.sprite =  defaultSprite;
        gameObject.SetActive(true);

        optionButton.onClick.RemoveAllListeners();
        optionButton.onClick.AddListener(OnOptionSelected);
    }

    public void OnOptionSelected()
    {
        checkBox.sprite = selectedSprite;

        //if (flag)
        {
            Debug.Log("Selected Option :: " + optionText.text);
            // 
            SpecialQuestionPanelView.OptionSelectedAction?.Invoke(index, optionText.text);
        }
    }

    public void OnOptionDeselected()
    {
        checkBox.sprite = defaultSprite;
    }
}
