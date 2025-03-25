using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance;

    [SerializeField] private SplashScreenView splashScreenView;
    [SerializeField] public MainMenuView mainMenuView;
    [SerializeField] public ActivitySelectionView activitySelectionView;
    [SerializeField] public GameRulesPage gameRulesPage;
    [SerializeField] public SettingsPanelView settingsPanelView;
    [SerializeField] public QAPanelView qaPanelView;
    [SerializeField] public SummeryResultsView summaryScreenView;
    [SerializeField] public ExitPopup exitPopup;
    [SerializeField] public AfterExamAnswersView afterExamAnswersView;
    [SerializeField] public AllQuestionsPanelView allQuestionsView;

    private Image splashScreenImage; // Reference to the child splash image

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        splashScreenView.RenderView();
        splashScreenImage = splashScreenView.GetComponentInChildren<Image>(); // Find child Image component
        //GameUtils.ImageDownloader.RequestDownload(this, url, (tex2D) =>
        //{
        //    testDownloader.texture = tex2D;
        //});

        //GameUtils.GameTimer.CoutDownTimer(this, 10f, CountDownCallback);

        Invoke("ShowMainMenu", 2f);

    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    
    void ShowMainMenu()
    {
/*        // Ensure splashScreenView has an Image component
        if (splashScreenView.GetComponent<Image>() != null && splashScreenImage != null)
        {
            splashScreenView.GetComponent<Image>().CrossFadeAlpha(0f, 1f,true);
            splashScreenImage.CrossFadeAlpha(0f, 1f, true); // Fade out child image
        }
        else
        {
            Debug.LogError("SplashScreenView object is missing an Image component for fade effect.");
        }*/
        mainMenuView.RenderView();
        //Destroy(splashScreenView.gameObject, 1f);
        splashScreenView.Disable();
    }

}
