using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Threading.Tasks;
public class SummeryResultsView : MonoBehaviour
{
    [SerializeField] private Button buttonClose;
    [SerializeField] private Button repeatButton;

    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text timerText;

    [SerializeField] private Image imageBarWrong;

    [SerializeField] private Image imageBarSkipped;

    [SerializeField] private Image imageBarRight;

    [SerializeField] private TMP_Text correctAnsCountText;
    [SerializeField] private TMP_Text correctAnsCountPercentText;

    [SerializeField] private TMP_Text totalPointText;
    [SerializeField] private TMP_Text totalPointsPercentText;

    [SerializeField] private Button viewAnswerButton;

    [SerializeField] private Image winLossFrameImage;

    [SerializeField] private Sprite imageLoss;
    [SerializeField] private Sprite imageWon;
    public Sprite defaultButtonSprite;

    public Image ansButtonBarImage;

    public string PASS_TEXT = "Pozytywny";
    public string FAILED_TEXT = "Negatywny";

    public const string darkModeColor = "#E6F8FF";
    public const string lightModeColor = "#2CBCF8";

    SummaryData summaryData = null;
    public void RenderView()
    {
        repeatButton.GetComponent<Image>().sprite = defaultButtonSprite;
        viewAnswerButton.GetComponent<TextMeshProUGUI>().color = GameConstants.GetColorFromHexCode(ThemeSwitcher.instance.isDarkMode ? darkModeColor : lightModeColor);
        ansButtonBarImage.color = GameConstants.GetColorFromHexCode(ThemeSwitcher.instance.isDarkMode ? darkModeColor : lightModeColor);
        SetData();
        AddListeners();
        gameObject.SetActive(true);
    }

    private void SetData()
    {
         summaryData = GameManager.Instance.GetUserSummaryData();
        if (summaryData != null)
        {
            bool IsPassed = (summaryData.PointsAchieved >= GameConstants.PASS_MARKS);
            resultText.text = IsPassed ? PASS_TEXT : FAILED_TEXT;
            winLossFrameImage.sprite = IsPassed ? imageWon : imageLoss;

            int sec = (int)summaryData.TotalDurations % 60;
            int mint = (int)summaryData.TotalDurations / 60;
            timerText.text = mint + ":" + sec.ToString("00");

            correctAnsCountText.text = summaryData.TotalCorrectAns.ToString();
            correctAnsCountPercentText.text = "/" + GameConstants.MAX_QUESTION_COUNT + " (" + ((int)((summaryData.TotalCorrectAns / (GameConstants.MAX_QUESTION_COUNT * 1.0f)) * 100)).ToString() + "%)";

            totalPointText.text = summaryData.PointsAchieved.ToString();
            totalPointsPercentText.text = "/" + GameConstants.MAX_GAME_POINT + " (" + ((int)((summaryData.PointsAchieved / (GameConstants.MAX_GAME_POINT * 1.0f)) * 100)).ToString() + "%)";

          //  imageBarSkipped.fillAmount = (summaryData.TotalSkipedAns / (GameConstants.MAX_QUESTION_COUNT * 1.0f));
          //  imageBarRight.fillAmount = (summaryData.TotalCorrectAns / (GameConstants.MAX_QUESTION_COUNT * 1.0f));
          //  imageBarWrong.fillAmount = (summaryData.TotalWrongAns / (GameConstants.MAX_QUESTION_COUNT * 1.0f));
          
            imageBarRight.GetComponent<RectTransform>().sizeDelta = new Vector2(imageBarRight.GetComponent<RectTransform>().sizeDelta.x,(summaryData.TotalCorrectAns / (GameConstants.MAX_QUESTION_COUNT * 1.0f))* 260.0f);
            imageBarWrong.GetComponent<RectTransform>().sizeDelta = new Vector2(imageBarWrong.GetComponent<RectTransform>().sizeDelta.x,(summaryData.TotalWrongAns / (GameConstants.MAX_QUESTION_COUNT * 1.0f))* 260.0f);
            imageBarSkipped.GetComponent<RectTransform>().sizeDelta = new Vector2(imageBarSkipped.GetComponent<RectTransform>().sizeDelta.x,(summaryData.TotalSkipedAns / (GameConstants.MAX_QUESTION_COUNT * 1.0f))* 260.0f);
        }
    }

    private void AddListeners()
    {
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnCloseButtonClicked);

        repeatButton.onClick.RemoveAllListeners();
        repeatButton.onClick.AddListener(OnStartButtonClicked);

        viewAnswerButton.onClick.RemoveAllListeners();
        viewAnswerButton.onClick.AddListener(OnViewAnsButtonClicked);
    }

  

    private async void OnStartButtonClicked()
    {
        GameManager.Instance.FetchRandomQuestions();
        GameManager.Instance.ResetSummaryData();

        GameManager.Instance.loadingPanel.ActiveLoading();
        await Task.Delay(new TimeSpan(0, 0, 5));
        GameManager.Instance.loadingPanel.DeactiveLoading();
        UIHandler.Instance.qaPanelView.RenderView();
        this.Disable();
    }

    private void OnCloseButtonClicked()
    {
        GameManager.Instance.FetchRandomQuestions();
        GameManager.Instance.ResetSummaryData();
        UIHandler.Instance.activitySelectionView.RenderView();
        this.Disable();
    }

    private void OnViewAnsButtonClicked()
    {
       // GameManager.Instance.ShuffleRandomQuestions();

        UIHandler.Instance.afterExamAnswersView.RenderView(summaryData);
        this.Disable();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}

