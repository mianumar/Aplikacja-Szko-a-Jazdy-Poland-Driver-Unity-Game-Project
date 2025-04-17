using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Firebase.Extensions;
using System;
using UnityEngine.Video;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine.Rendering;

public class QAPanelView : MonoBehaviour
{
    [SerializeField] private SpecialQuestionPanelView specialQuestionPanelView;
    [SerializeField] private Button buttonClose;
    [SerializeField] private Button buttonYes;
    [SerializeField] private Button buttonNo;
    [SerializeField] private Button buttonStart;
    [SerializeField] private Button buttonNext;

    [Header("Time To Read")]
    [SerializeField] private TextMeshProUGUI textTimer;
    [SerializeField] private TextMeshProUGUI textCurrentQuestion;
    [SerializeField] private TextMeshProUGUI textDescription;

    [SerializeField] private TextMeshProUGUI textReamingTimeToRead;
    [SerializeField] private Image remainTimetoReadQuestionSlider;

    [SerializeField] private Slider sliderRemainingQuestionsToAns;

    [SerializeField] private GameObject timeToReadQuestionPanel;
    [SerializeField] private GameObject timeToReadTextObject;


    // Ans Panel referance
    [Header("Time To Ans ")]
    [SerializeField] private GameObject noImagePanel;
    [SerializeField] private GameObject timeToAnsPanel;
    [SerializeField] private GameObject timeToAnsTextObject;
    [SerializeField] private Image remainTimeToAnsSlider;
    [SerializeField] private TextMeshProUGUI textTimeReamingAns;
    [SerializeField] private TextMeshProUGUI textTimeToAns;
    [SerializeField] private Image bgImageTimeToAns;
    [SerializeField] private GameObject videoPlayerPanel;

    public string lightModeButtonDefaultColor = "#E1EDF0";
    public string lightModeButtonSelectedColor = "#7193A0";
    public string darkModeButtonSelectedColor = "#476B7A";
    public string darkModeButtonDefaultColor = "#7193A0";

    [Header("Video Player Panel")]
    public VideoPlayer videoPlayer;
    public Slider videoDurationBar;


    [SerializeField] private GameObject optionSimpelObject;
    [SerializeField] private GameObject optionSpecialObject;

    public int currentQesIndex = 0;


    private int BASIC_READ_TIME = 15;
    private int BASIC_ANSWER_TIME = 10;

    [Header("Timer COLOR")]

    public Color timeEndColor;
    public Color timeStartColor;

    [Header("Time to Ans COLOR")]
    public Color defaultTextColor;
    public Color noImageTextColor;

    public SimpleQuestionDataModel _dataModel = null;
    public SpecializedQuestionModel _dataModelSpecial = null;
    public bool IsMediaDone = false;

    public static Action<string> ResultAction;
    public static Action QuestionTimeEndAction;
    private bool Skipped = true;
    private float totalGameTime;

    public ResultType currectSelectedResult;

    public void RenderView()
    {
        ResultAction += OnResultSelected;
        QuestionTimeEndAction += OnQuestionTimeEnd;
        ResetData();

        AddListeners();

        _dataModel = GameManager.Instance.GetSimpleQuestionFromList(currentQesIndex);

        gameObject.SetActive(true);
        RenderQuestionPanelView(_dataModel);
        StartGameTimer();

    }

    private void OnQuestionTimeEnd()
    {
        Skipped = true;
        OnNextButtonClicked();
    }

    IEnumerator CheckForMediaDownloaded()
    {
        // yield return new WaitUntil(()=>GameManager.Instance.IsAllMediaDownloaded);
        yield return new WaitForSeconds(2f);
        Debug.LogError("CheckForMediaDownloaded :: ALL MEDIA DOWNLOADED SUCCESSFULLY..");
        IsMediaDone = true;
        if (IsMediaDone)
        {
            RenderQuestionPanelView(_dataModel);

            StartGameTimer();
        }
    }



    public void StartGameTimer()
    {
        GameUtils.GameTimer.GameClockTimer(this, 0f, (currentTime) =>
        {
            totalGameTime = currentTime;
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
        buttonNo.GetComponent<Image>().color = GameConstants.GetColorFromHexCode(ThemeSwitcher.instance.isDarkMode ? darkModeButtonDefaultColor : lightModeButtonDefaultColor);
        buttonYes.GetComponent<Image>().color = GameConstants.GetColorFromHexCode(ThemeSwitcher.instance.isDarkMode ? darkModeButtonDefaultColor : lightModeButtonDefaultColor);

        if (simpleQuestionData != null)
        {
            timeToReadQuestionPanel.SetActive(true);
            buttonStart.gameObject.SetActive(true);
            videoPlayerPanel.SetActive(false);
            timeToAnsPanel.SetActive(false);
            buttonNo.interactable = false;
            buttonYes.interactable = false;

            optionSimpelObject.SetActive(true);
            optionSpecialObject.SetActive(false);
            textCurrentQuestion.text = (currentQesIndex + 1).ToString();
            sliderRemainingQuestionsToAns.value = ((currentQesIndex + 1) / (32 * 1.0f)) * 100;
            textDescription.text = simpleQuestionData.question;

            GameUtils.GameTimer.CoutDownTimer(this, BASIC_READ_TIME, (timeRemain) =>
            {
                Debug.Log("Time Remain " + timeRemain);
                textReamingTimeToRead.text = timeRemain + "s";
                remainTimetoReadQuestionSlider.fillAmount = timeRemain / (BASIC_READ_TIME * 1.0f);
                remainTimetoReadQuestionSlider.color = timeRemain < (BASIC_READ_TIME * 0.25f) ? timeEndColor : timeStartColor;
                if (timeRemain <= 0)
                {
                    Debug.LogError("Reading Timer End :: START ANSWER ");
                    OnStartButtonClicked();

                }
            });

        }
    }

    private void DisplayVideoFileForQuestion(string fileName)
    {
        // videoPlayerPanel.gameObject.SetActive(true);
        // var videoClip = Resources.Load<VideoClip>("Videos/SIMPLE/" + fileName) as VideoClip;
        string fileUrl = "file://" + GameManager.Instance.VIDEO_FILE_DIR_SIMPLE + fileName;
        videoPlayer.url = fileUrl;
        videoPlayer.loopPointReached += OnVideoEnd;
        //videoPlayer.clip = videoClip;
        if (!videoPlayer.isPrepared)
        {
            videoPlayer.prepareCompleted += OnVideoPrepared; // Subscribe to the event
            videoPlayer.Prepare(); // Prepare the video asynchronously

            Debug.Log("Preparing video to get length...");
        }
        else
        {
            bgImageTimeToAns.gameObject.SetActive(false);
            videoPlayerPanel.SetActive(true);
            videoDurationBar.gameObject.SetActive(true);
            Debug.Log("Clip length :: " + videoPlayer.clip.length);
            // UpdateVideoplayBar(videoPlayer.clip.length);
            videoPlayer.Play();

        }
    }

    Coroutine videoPlayerRoutine = null;
    void Update()
    {
        if (videoPlayer.isPlaying)
        {
            float progress = (float)(videoPlayer.time / videoPlayer.length);
            videoDurationBar.value = progress;
        }
    }

    //private void UpdateVideoplayBar(double length)
    //{
    //    float len = (float)length;
    //    while (length >= 0)
    //    {
    //        len -= Time.deltaTime;
    //        videoDurationBar.value = len;
    //    }
    //}


    private void OnVideoEnd(VideoPlayer source)
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
        videoDurationBar.value = 1f;
        Debug.Log("Video Ended");
        bgImageTimeToAns.gameObject.SetActive(false);
        RenderAnsPanelView();

    }

    private void OnVideoPrepared(VideoPlayer source)
    {
        videoPlayer.prepareCompleted -= OnVideoPrepared;
        Debug.Log("Clip length :: " + source.length);
        videoPlayerPanel.SetActive(true);
        videoDurationBar.gameObject.SetActive(true);
        videoPlayer.Play();

    }

    private void RenderSpecialQuestionPanelView(SpecializedQuestionModel specializedQuestion)
    {
        buttonStart.gameObject.SetActive(false);

        textCurrentQuestion.text = (currentQesIndex + 1).ToString();
        sliderRemainingQuestionsToAns.value = ((currentQesIndex + 1) / (32 * 1.0f)) * 100;
        optionSimpelObject.SetActive(false);
        optionSpecialObject.SetActive(true);
        timeToReadQuestionPanel.SetActive(false);
        //if (currentQesIndex == (GameConstants.MAX_QUESTION_COUNT - 1))
        //{
        //   // buttonNext.gameObject.SetActive(false);
        //}
        specialQuestionPanelView.RenderView(specializedQuestion);
    }

    private void OnResultSelected(string selectedResult)
    {
        if (_dataModelSpecial.answer.Equals(selectedResult))
        {
            currectSelectedResult = ResultType.CORRECT;
            // int points = GameConstants.GetSpecialQuestionsPoint((currentQesIndex - 20));
            // GameManager.Instance.UpdateUserSummaryData(currentQesIndex, points, ResultType.CORRECT, QuestionType.SPECIAL);
        }
        else
        {
            currectSelectedResult = ResultType.WRONG;
            //GameManager.Instance.UpdateUserSummaryData(currentQesIndex, 0, ResultType.WRONG, QuestionType.SPECIAL);
        }
        Skipped = false;
        ////Render Summary If all questions done
       
        
    }


    private void RenderAnsPanelView()
    {
        buttonNo.interactable = true;
        buttonYes.interactable = true;
        videoDurationBar.gameObject.SetActive(false);
        timeToAnsPanel.SetActive(true);
        GameUtils.GameTimer.CoutDownTimer(this, BASIC_ANSWER_TIME, (timeRemain) =>
        {
            Debug.Log("Time Remain " + timeRemain);
            textTimeReamingAns.text = timeRemain + "s";
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
        int points = 0;
        QuestionType questionType = currentQesIndex < 20 ? QuestionType.BASIC : QuestionType.SPECIAL;
        if (!Skipped)
        {
            Skipped = true;

            if (currectSelectedResult == ResultType.CORRECT)
            {
                if (questionType == QuestionType.BASIC)
                {
                    points = GameConstants.GetSimpleQuestionsPoint(currentQesIndex);
                }
                else
                {
                    points = GameConstants.GetSpecialQuestionsPoint((currentQesIndex - 20));
                }
                GameManager.Instance.UpdateUserSummaryData(currentQesIndex, points, ResultType.CORRECT, questionType);
            }
            else
            {
                GameManager.Instance.UpdateUserSummaryData(currentQesIndex, 0, ResultType.WRONG, questionType);
            }
        }
        else
        {

            GameManager.Instance.UpdateUserSummaryData(currentQesIndex, 0, ResultType.SKIPPED, questionType);

        }
        videoPlayer.Stop();
        currentQesIndex += 1;

        if (currentQesIndex < 20)
        {
            _dataModel = GameManager.Instance.GetSimpleQuestionFromList(currentQesIndex);
            RenderQuestionPanelView(_dataModel);
        }
        else if (currentQesIndex < 32)
        {
            _dataModelSpecial = GameManager.Instance.GetSpecialQuestionFromList(currentQesIndex - 20);
            RenderSpecialQuestionPanelView(_dataModelSpecial);
        }
        else
        {
            buttonNext.gameObject.SetActive(false);
           // if (currentQesIndex == (GameConstants.MAX_QUESTION_COUNT - 1))
            {
                GameUtils.GameTimer.StopClock(this);
                GameManager.Instance.UpdateGameTimerInSummaryData(totalGameTime);
                Disable();
                UIHandler.Instance.summaryScreenView.RenderView();
            }
        }


    }

    /// <summary>
    /// Time to answer panel render
    /// </summary>
    private void OnStartButtonClicked()
    {
        buttonStart.gameObject.SetActive(false);
        timeToReadQuestionPanel.SetActive(false);
        GameUtils.GameTimer.StopCoundownTimer(this);
        noImagePanel.SetActive(false);

        string extention = GameConstants.GetFileExtensionFromUrl(_dataModel.media_link);
        if (extention.Equals(".mp4"))
        {
            DisplayVideoFileForQuestion(_dataModel.id + extention);
        }
        else if (extention.Equals(".jpg"))
        {
            textTimeToAns.color = defaultTextColor;
            Debug.LogError("Display Images for this Question");
            bgImageTimeToAns.gameObject.SetActive(true);
            bgImageTimeToAns.sprite = GameManager.Instance.GetQuestionSprite(_dataModel.id);

            RenderAnsPanelView();

        }
        else
        {
            bgImageTimeToAns.gameObject.SetActive(true);
            noImagePanel.SetActive(true);
            textTimeToAns.color = noImageTextColor;
            RenderAnsPanelView();
        }


    }

    private void OnNoButtonClicked()
    {
        buttonNo.GetComponent<Image>().color = GameConstants.GetColorFromHexCode(ThemeSwitcher.instance.isDarkMode ? darkModeButtonSelectedColor : lightModeButtonSelectedColor);
        Skipped = false;
        if (_dataModel.answer.Equals(0))
        {
            currectSelectedResult = ResultType.CORRECT;
            //  int points = GameConstants.GetSimpleQuestionsPoint(currentQesIndex);
            // GameManager.Instance.UpdateUserSummaryData(currentQesIndex,points, ResultType.CORRECT, QuestionType.BASIC);
        }
        else
        {
            currectSelectedResult = ResultType.WRONG;
            // GameManager.Instance.UpdateUserSummaryData(currentQesIndex,0, ResultType.WRONG, QuestionType.BASIC);
        }
        // OnNextButtonClicked();
    }

    private void OnYesButtonClicked()
    {
        buttonYes.GetComponent<Image>().color = GameConstants.GetColorFromHexCode(ThemeSwitcher.instance.isDarkMode ? darkModeButtonSelectedColor : lightModeButtonSelectedColor);

        Skipped = false;
        if (_dataModel.answer.Equals(1))
        {
            currectSelectedResult = ResultType.CORRECT;
            // int points = GameConstants.GetSimpleQuestionsPoint(currentQesIndex);
            // GameManager.Instance.UpdateUserSummaryData(currentQesIndex, points, ResultType.CORRECT, QuestionType.BASIC);
        }
        else
        {
            currectSelectedResult = ResultType.WRONG;
            // GameManager.Instance.UpdateUserSummaryData(currentQesIndex, 0, ResultType.WRONG, QuestionType.BASIC);
        }
        //OnNextButtonClicked();
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
        optionSimpelObject.SetActive(true);
        optionSpecialObject.SetActive(false);
        specialQuestionPanelView.DisableView();
        buttonNext.gameObject.SetActive(true);
    }

    public void Disable()
    {
        ResultAction -= OnResultSelected;
        QuestionTimeEndAction -= OnQuestionTimeEnd;
        gameObject.SetActive(false);
    }


}
