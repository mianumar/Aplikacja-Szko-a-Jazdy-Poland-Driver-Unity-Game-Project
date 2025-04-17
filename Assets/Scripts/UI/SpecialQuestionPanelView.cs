using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;

public class SpecialQuestionPanelView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Image bgImage;
    [SerializeField] private GameObject noImagePanel;

    [SerializeField] private TextMeshProUGUI textReamingTimeToAns;
    [SerializeField] private Image remainTimeToAnsSlider;

    [SerializeField] private ToggleGroup toggleGroup;

    public List<OptionItem> options = new List<OptionItem>();   

    private SpecializedQuestionModel _data;

    private int SPECIAL_ANSWER_TIME = 50;

    public static Action<int,string> OptionSelectedAction;


    [Header("Timer COLOR")]

    public Color timeEndColor;
    public Color timeStartColor;

    public void RenderView(SpecializedQuestionModel _data)
    {
        OptionSelectedAction += OnOptionSelected;
        gameObject.SetActive(true);
        this._data = _data;
        questionText.text = _data.question;
        RenderQuestionImage(_data);
        RenderOptions(_data);


        GameUtils.GameTimer.CoutDownTimer(this, SPECIAL_ANSWER_TIME, (timeRemain) =>
        {
            Debug.Log("Time Remain " + timeRemain);
            textReamingTimeToAns.text = timeRemain + "s";
            remainTimeToAnsSlider.fillAmount = timeRemain / (SPECIAL_ANSWER_TIME * 1.0f);
            remainTimeToAnsSlider.color = timeRemain < (SPECIAL_ANSWER_TIME * 0.25f) ? timeEndColor : timeStartColor;
            if (timeRemain <= 0)
            {
                QAPanelView.QuestionTimeEndAction?.Invoke();
                Debug.LogError("Reading Timer End :: NEXT QUESTION");
               
            }
        });

    }

    private void OnOptionSelected(int selectedIndex , string optionText)
    {
        for (int i = 0; i < options.Count; i++)
        {
            if(selectedIndex - 1 == i)
                continue;
            options[i].OnOptionDeselected();
        }
        // options[selectedIndex-1].OnOptionSelected();
        QAPanelView.ResultAction?.Invoke(optionText);
    }

    private void RenderQuestionImage(SpecializedQuestionModel _data)
    {
        string extention = GameConstants.GetFileExtensionFromUrl(_data.media_link);
        noImagePanel.SetActive(false);
        if (extention.Equals(".jpg"))
        {
            bgImage.sprite = GameManager.Instance.GetSpecializedQuestioSprite(_data.id);
        }
        else
        {
            noImagePanel.SetActive(true);
        }

    }

    private void RenderOptions(SpecializedQuestionModel _data)
    {
        options[0].RenderView("A",_data.option_a,toggleGroup);
        options[1].RenderView("B",_data.option_b,toggleGroup);
        options[2].RenderView("C",_data.option_c,toggleGroup);
    }

    public void DisableView()
    {
        if (gameObject.activeInHierarchy)
        {
            GameUtils.GameTimer.StopCoundownTimer(this);
        }

       gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        OptionSelectedAction -= OnOptionSelected;
        GameUtils.GameTimer.StopCoundownTimer(this);
    }

}
