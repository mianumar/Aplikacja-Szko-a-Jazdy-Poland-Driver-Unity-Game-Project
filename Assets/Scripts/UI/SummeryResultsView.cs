using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SummeryResultsView : MonoBehaviour
{
    [SerializeField] private Button buttonClose;
    [SerializeField] private Button repeatButton;

    [SerializeField] private TMP_Text relustText;
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

    [SerializeField] private TMP_Text ignoreObtainText;
    [SerializeField] private TMP_Text ignoreTotelText;

    [SerializeField] private TMP_Text attempObtainText;
    [SerializeField] private TMP_Text attempTotelText;

    [SerializeField] private TMP_Text resultObtainText;

    [SerializeField] private Button viewAnswerButton;

    [SerializeField] private Sprite imageLoss;
    [SerializeField] private Sprite imageWon;


    public void RenderView()
    {
        AddListeners();
        gameObject.SetActive(true);
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

