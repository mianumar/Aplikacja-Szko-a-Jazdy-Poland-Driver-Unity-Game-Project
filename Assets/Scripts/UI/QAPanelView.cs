using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI testDescription;
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


    private List<QADataModel> qADataModels;
    private int currentQesIndex = 0;
    public void RenderView()
    {
        var instance = GameManager.Instance;
        if (instance != null)
        {
            qADataModels = instance.GetAllQuestionAns();
        }

        AddListeners();
        SetDefaultData();
        gameObject.SetActive(true);
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

    private void SetDefaultData()
    {
        textCurrentQuestion.text = (currentQesIndex+1).ToString();
    }

    private void OnNextButtonClicked()
    {
       
    }

    private void OnStartButtonClicked()
    {
        
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

   

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
