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



    private int MaxTotalPoints;
    private int MinTotalPoints;
    private int TotalQusetionsCount;

    private List<QADataModel> qADataModels;
    public void RenderView()
    {
        var instance = GameManager.Instance;
        if (instance != null)
        {
            MaxTotalPoints = instance.MaxTotalPoints;
            MinTotalPoints = instance.MinPointsToPass;
            qADataModels = instance.GetAllQuestionAns();

            TotalQusetionsCount = qADataModels.Count;
        }

        AddListeners(); 

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

    private void OnNextButtonClicked()
    {
        throw new NotImplementedException();
    }

    private void OnStartButtonClicked()
    {
        throw new NotImplementedException();
    }

    private void OnNoButtonClicked()
    {
        throw new NotImplementedException();
    }

    private void OnYesButtonClicked()
    {
        throw new NotImplementedException();
    }

    private void OnCloseClicked()
    {
        Disable();
    }

    private void SetDefaultData()
    {

    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
