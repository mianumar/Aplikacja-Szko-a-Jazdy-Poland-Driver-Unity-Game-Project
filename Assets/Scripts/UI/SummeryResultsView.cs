using UnityEngine;
using UnityEngine.UI;
using TMPro;
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

    public string PASS_TEXT = "Pozytywny";
    public string FAILED_TEXT = "Negatywny";


    public void RenderView()
    {
        SetData();
        AddListeners();
        gameObject.SetActive(true);
    }

    private void SetData()
    {
        SummaryData summaryData = GameManager.Instance.GetUserSummaryData();
        bool IsPassed = (summaryData.PointsAchieved >= GameConstants.PASS_MARKS);
        resultText.text = IsPassed ? PASS_TEXT : FAILED_TEXT;
        winLossFrameImage.sprite = IsPassed ? imageWon : imageLoss;
        
        int sec = (int)summaryData.TotalDurations % 60;
        int mint = (int)summaryData.TotalDurations / 60;
        timerText.text = mint + ":" + sec.ToString("00");

        correctAnsCountText.text = summaryData.TotalCorrectAns.ToString();
        correctAnsCountPercentText.text = ((int)((summaryData.TotalCorrectAns / GameConstants.MAX_QUESTION_COUNT) * 100)).ToString();

        totalPointText.text = summaryData.PointsAchieved.ToString();
        totalPointsPercentText.text = ((int)((summaryData.PointsAchieved/GameConstants.MAX_GAME_POINT)*100)).ToString();

        imageBarSkipped.fillAmount = (summaryData.TotalSkipedAns / 20 * 1.0f);
        imageBarRight.fillAmount = (summaryData.TotalCorrectAns / GameConstants.MAX_QUESTION_COUNT * 1.0f);
        imageBarWrong.fillAmount = (summaryData.TotalWrongAns / GameConstants.MAX_QUESTION_COUNT * 1.0f);
    }

    private void AddListeners()
    {
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnCloseButtonClicked);

        repeatButton.onClick.RemoveAllListeners();
        repeatButton.onClick.AddListener(OnStartButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        GameManager.Instance.ShuffleRandomQuestions();
        GameManager.Instance.ResetSummaryData();
        UIHandler.Instance.qaPanelView.RenderView();
        this.Disable();
    }

    private void OnCloseButtonClicked()
    {
        GameManager.Instance.ShuffleRandomQuestions();
        GameManager.Instance.ResetSummaryData();
        UIHandler.Instance.activitySelectionView.RenderView();
        this.Disable();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}

