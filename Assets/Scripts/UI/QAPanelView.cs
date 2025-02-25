using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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

    private int totalSimpleQuestionCount;
    private int totalSpecialQuestionCount;

    private List<int> randomSimpleQstnIndexList = new List<int>();
    private List<int> randomSpecialQstnIndexList = new List<int>();

    public async void RenderView()
    {
        var instance = GameManager.Instance;
        if (instance != null)
        {
            // qADataModels = instance.GetAllQuestionAns();
        }
        randomSimpleQstnIndexList.Clear();
        randomSpecialQstnIndexList.Clear();
        AddListeners();
        await ServerHandler.instance.GetSimpleQuestionsCount().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                totalSimpleQuestionCount = task.Result;

                randomSimpleQstnIndexList = GetRandomIndex(1, totalSimpleQuestionCount);


            }
        });
        await ServerHandler.instance.GetSpecilizedQuestionsCount().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {

                totalSpecialQuestionCount = task.Result;
                randomSpecialQstnIndexList = GetRandomIndex(1, totalSpecialQuestionCount);
                gameObject.SetActive(true);
            }
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

    private List<int> GetRandomIndex(int min, int max)
    {
        System.Random random = new System.Random();
        return Enumerable.Range(min, max-1).OrderBy(x => random.Next()).Take(32).ToList();

        //foreach (int num in uniqueNumbers)
        //{
        //    Debug.Log(num);
        //}

    }

    private async void RenderQuestionPanelView(int index)
    {
        ServerHandler.SimpleQuestionDataModel simpleQuestionData = null;
        await ServerHandler.instance.GetRandomSimpleQuestion(index).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                simpleQuestionData = task.Result as ServerHandler.SimpleQuestionDataModel;
            }
        });
        textCurrentQuestion.text = (currentQesIndex + 1).ToString();
        if (simpleQuestionData != null)
        {

            //GameUtils.GameTimer.CoutDownTimer(this, qaData.ReadingTime, (timeRemain) =>
            //{
            //    Debug.Log("Time Remain " + timeRemain);
            //});

        }
    }


    private void RenderAnsPanelView()
    {

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
