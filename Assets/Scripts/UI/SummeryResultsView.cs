using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SummeryResultsView : MonoBehaviour
{
    [SerializeField] private Button buttonClose;
    [SerializeField] private Button repeatButton;

    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text timerText;

    [SerializeField] private Image middleBarImage;
    [SerializeField] private Image middleBarIcon;
    [SerializeField] private TMP_Text middleBartext;

    [SerializeField] private Image leftBarImage;
    [SerializeField] private Image leftBarIcon;
    [SerializeField] private TMP_Text leftBartext;

    [SerializeField] private Image rigthBarImage;
    [SerializeField] private Image rightBarIcon;
    [SerializeField] private TMP_Text rightBartext;

    [SerializeField] private TMP_Text correctAnsCountText;
    [SerializeField] private TMP_Text correctAnsCountPercentText;

    [SerializeField] private TMP_Text totalPointText;
    [SerializeField] private TMP_Text totalPointsPercentText;

    [SerializeField] private Button viewAnswerButton;

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

        resultText.text = (summaryData.PointsAchieved >= GameConstants.PASS_MARKS) ? PASS_TEXT : FAILED_TEXT;

        int sec = (int)summaryData.TotalDurations % 60;
        int mint = (int)summaryData.TotalDurations / 60;
        timerText.text = mint + ":" + sec.ToString("00");
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
        UIHandler.Instance.qaPanelView.RenderView();
        this.Disable();
    }

    private void OnCloseButtonClicked()
    {
        UIHandler.Instance.activitySelectionView.RenderView();
        this.Disable();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}

