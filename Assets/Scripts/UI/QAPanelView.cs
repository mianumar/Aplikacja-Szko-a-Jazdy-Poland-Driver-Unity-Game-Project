using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Firebase.Extensions;
using System;

public class QAPanelView : MonoBehaviour
{
    [SerializeField] private Button buttonClose;
    [SerializeField] private Button buttonYes;
    [SerializeField] private Button buttonNo;
    [SerializeField] private Button buttonStart;
    [SerializeField] private Button buttonNext;


    [SerializeField] private TextMeshProUGUI textTimer;
    [SerializeField] private TextMeshProUGUI textCurrentQuestion;
    [SerializeField] private TextMeshProUGUI textDescription;
    [SerializeField] private TextMeshProUGUI textQuestion;

    [SerializeField] private TextMeshProUGUI textReamingTimeToRead;
    [SerializeField] private TextMeshProUGUI textReamingTimeToAnswer;
    [SerializeField] private Image remainQuestionSlider;

    [SerializeField] private Slider sliderRemainingQuestionsToAns;

    [SerializeField] private GameObject timeToReadQuestionPanel;
    [SerializeField] private GameObject timeToAnsPanel;

    // Ans Panel referance
    [SerializeField] private Image remainTimeToAnsSlider;
    [SerializeField] private TextMeshProUGUI textTimeReamingAns;


    private int currentQesIndex = 0;


    private int BASIC_READ_TIME = 15;
    private int BASIC_ANSWER_TIME = 10;
    private int SPECIAL_ANSWER_TIME = 50;
    public void RenderView()
    {
        ResetData();
       
        AddListeners();

        SimpleQuestionDataModel _dataModel = GameManager.Instance.GetSimpleQuestionFromList(currentQesIndex);


        gameObject.SetActive(true);
        RenderQuestionPanelView(_dataModel);

        StartGameTimer();
    }

    public void StartGameTimer()
    {
        GameUtils.GameTimer.GameClockTimer(this, 0f, (currentTime) =>
        {
           // Debug.Log("Time Remain " + currentTime);
            int sec = (int)currentTime % 60;
            int mint = (int)currentTime / 60;
            textTimer.text = mint + ":" + sec.ToString("00");
        });

    }

    private void AddListeners()
    {
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnCloseClicked);

        buttonYes.onClick.RemoveAllListeners();
        buttonYes.onClick.AddListener(OnYesButtonClicked);

        buttonNo.onClick.RemoveAllListeners();
        buttonNo.onClick.AddListener(OnNoButtonClicked);

        buttonStart.onClick.RemoveAllListeners();
        buttonStart.onClick.AddListener(OnStartButtonClicked);

        buttonNext.onClick.RemoveAllListeners();
        buttonNext.onClick.AddListener(OnNextButtonClicked);


    }

    private void RenderQuestionPanelView(SimpleQuestionDataModel simpleQuestionData)
    {
        if (simpleQuestionData != null)
        {
            currentQesIndex += 1;
            textCurrentQuestion.text = currentQesIndex.ToString();
            sliderRemainingQuestionsToAns.value = (currentQesIndex / 32) * 100;

            textDescription.text = simpleQuestionData.question;

            GameUtils.GameTimer.CoutDownTimer(this, BASIC_READ_TIME, (timeRemain) =>
            {
                Debug.Log("Time Remain " + timeRemain);
                textReamingTimeToRead.text = timeRemain.ToString();
                remainQuestionSlider.fillAmount = timeRemain / (BASIC_READ_TIME*1.0f);
            });

        }
    }

    private void RenderSpecialQuestionPanelView(SpecializedQuestionModel specializedQuestion)
    {

    }


    private void RenderAnsPanelView()
    {
        timeToAnsPanel.SetActive(true);
    }

    /// <summary>
    /// Display Next Question
    /// </summary>
    private void OnNextButtonClicked()
    {
        if (currentQesIndex < 20)
        {
            SimpleQuestionDataModel _dataModel = GameManager.Instance.GetSimpleQuestionFromList(currentQesIndex);
            RenderQuestionPanelView(_dataModel);
        }else if(currentQesIndex < 32)
        {
            SpecializedQuestionModel _dataModel = GameManager.Instance.GetSpecialQuestionFromList(currentQesIndex-20);
            RenderSpecialQuestionPanelView(_dataModel);
        }

    }

    /// <summary>
    /// Time to answer panel render
    /// </summary>
    private void OnStartButtonClicked()
    {
        timeToReadQuestionPanel.SetActive(false);
        RenderAnsPanelView();
    }

    private void OnNoButtonClicked()
    {

    }

    private void OnYesButtonClicked()
    {

    }

    private void OnCloseClicked()
    {
        UIHandler.Instance.exitPopup.RenderView();
        //Disable();
    }

    private void ResetData()
    {
        currentQesIndex = 0;
        textTimer.text = "0:00";
        remainQuestionSlider.fillAmount = 1;
        textReamingTimeToRead.text = BASIC_READ_TIME.ToString();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
