using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Firebase.Extensions;
using System;
using UnityEngine.Video;
using Unity.VisualScripting;

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

    [SerializeField] private TextMeshProUGUI textReamingTimeToRead;
    [SerializeField] private Image remainTimetoReadQuestionSlider;

    [SerializeField] private Slider sliderRemainingQuestionsToAns;

    [SerializeField] private GameObject timeToReadQuestionPanel;
    [Header("Debug")]
    [SerializeField] private GameObject timeToReadTextObject;
    [SerializeField] private Image questionImage;

    // Ans Panel referance
    [Header("Time To Ans ")]
    [SerializeField] private GameObject timeToAnsPanel;
    [SerializeField] private GameObject timeToAnsTextObject;
    [SerializeField] private Image remainTimeToAnsSlider;
    [SerializeField] private TextMeshProUGUI textTimeReamingAns;
    [SerializeField] private Image questionImageTimeToAns;
    [SerializeField] private GameObject videoPlayerPanel;


    public VideoPlayer videoPlayer;



    private int currentQesIndex = 0;


    private int BASIC_READ_TIME = 15;
    private int BASIC_ANSWER_TIME = 10;
    private int SPECIAL_ANSWER_TIME = 50;

    public Color timeEndColor;
    public Color timeStartColor;

    private SimpleQuestionDataModel _dataModel = null;
    private SpecializedQuestionModel _dataModelSpecial = null;
    public void RenderView()
    {
        ResetData();
       
        AddListeners();

        _dataModel = GameManager.Instance.GetSimpleQuestionFromList(currentQesIndex);
       
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
                remainTimetoReadQuestionSlider.fillAmount = timeRemain / (BASIC_READ_TIME*1.0f);
                remainTimetoReadQuestionSlider.color = timeRemain < (BASIC_READ_TIME *0.25f) ? timeEndColor : timeStartColor;
                if(timeRemain <= 0)
                {
                    Debug.LogError("Reading Timer End :: START ANSWER ");
                    OnStartButtonClicked();
                    
                }
            });

        }
    }

    private void DisplayVideoFileForQuestion(string fileName)
    {
        videoPlayerPanel.gameObject.SetActive(true);
        var videoClip = Resources.Load<VideoClip>("/Videos/SIMPLE/" + fileName+".mp4");
        if(videoClip != null)
        {
           videoPlayer.clip = videoClip;    
        }
        if (!videoPlayer.isPrepared)
        {
            videoPlayer.prepareCompleted += OnVideoPrepared; // Subscribe to the event
            videoPlayer.Prepare(); // Prepare the video asynchronously
            Debug.Log("Preparing video to get length...");
        }
        else
        {
            videoPlayerPanel.SetActive(true);
            Debug.Log("Clip length :: " + videoPlayer.clip.length);
            videoPlayer.Play();
        }
    }

    private void OnVideoPrepared(VideoPlayer source)
    {
        videoPlayer.prepareCompleted -= OnVideoPrepared;
        Debug.Log("Clip length :: " + source.length);
        videoPlayerPanel.SetActive(true);
        videoPlayer.Play();
    }

    private void RenderSpecialQuestionPanelView(SpecializedQuestionModel specializedQuestion)
    {

    }


    private void RenderAnsPanelView()
    {
        timeToAnsPanel.SetActive(true);
        GameUtils.GameTimer.CoutDownTimer(this, BASIC_ANSWER_TIME, (timeRemain) =>
        {
            Debug.Log("Time Remain " + timeRemain);
            textTimeReamingAns.text = timeRemain.ToString();
            remainTimeToAnsSlider.fillAmount = timeRemain / (BASIC_ANSWER_TIME * 1.0f);
            remainTimeToAnsSlider.color = timeRemain < (BASIC_ANSWER_TIME * 0.25f) ? timeEndColor : timeStartColor;
            if (timeRemain <= 0)
            {
                Debug.LogError("Reading Timer End :: NEXT QUESTION");
               OnNextButtonClicked();
            }
        });
    }

    /// <summary>
    /// Display Next Question
    /// </summary>
    private void OnNextButtonClicked()
    {
        buttonStart.gameObject.SetActive(true);
        videoPlayerPanel.SetActive(false);
        timeToReadQuestionPanel.SetActive(true);
        if (currentQesIndex < 20)
        {
            _dataModel = GameManager.Instance.GetSimpleQuestionFromList(currentQesIndex);
            RenderQuestionPanelView(_dataModel);
        }else if(currentQesIndex < 32)
        {
            _dataModelSpecial = GameManager.Instance.GetSpecialQuestionFromList(currentQesIndex-20);
            RenderSpecialQuestionPanelView(_dataModelSpecial);
        }

    }

    /// <summary>
    /// Time to answer panel render
    /// </summary>
    private void OnStartButtonClicked()
    {
        buttonStart.gameObject.SetActive(false);
        timeToReadQuestionPanel.SetActive(false);

        if (GameConstants.GetFileExtensionFromUrl(_dataModel.media_link).Equals(".mp4"))
        {
            DisplayVideoFileForQuestion(_dataModel.id.ToString());
        }
        else
        {
            Debug.LogError("Display Images for this Question");
            questionImage.sprite = questionImageTimeToAns.sprite = GameManager.Instance.GetQuestionSprite(_dataModel.id);
            RenderAnsPanelView();

        }

        
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
        remainTimetoReadQuestionSlider.fillAmount = 1;
        textReamingTimeToRead.text = BASIC_READ_TIME.ToString();
        buttonNo.interactable = false;
        buttonYes.interactable = false;
        buttonStart.gameObject.SetActive(true);
        timeToReadQuestionPanel.SetActive(true);
        videoPlayerPanel.SetActive(false);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
