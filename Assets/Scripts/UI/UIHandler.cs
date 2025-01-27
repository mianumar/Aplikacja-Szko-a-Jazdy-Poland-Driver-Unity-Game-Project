using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private SplashScreenView splashScreenView;
    [SerializeField] private MainMenuView mainMenuView;
    [SerializeField] private ActivitySelectionView activitySelectionView;
    [SerializeField] private GameRulesPage gameRulesPage;
    [SerializeField] private SettingsPanelView settingsPanelView;
    [SerializeField] private QAPanelView qaPanelView;
    [SerializeField] private SummaryScreenView summaryScreenView;
    [SerializeField] private ExitPopup exitPopup;
    public RawImage testDownloader;
    public Text testText;
    string url = "https://photographylife.com/wp-content/uploads/2014/09/Nikon-D750-Image-Samples-2.jpg";

    private Image splashScreenImage; // Reference to the child splash image

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        splashScreenView.RenderView();
        splashScreenImage = splashScreenView.GetComponentInChildren<Image>(); // Find child Image component
        GameUtils.ImageDownloader.RequestDownload(this, url, (tex2D) =>
        {
            testDownloader.texture = tex2D;
        });

        GameUtils.GameTimer.CoutDownTimer(this, 10f, CountDownCallback);

        Invoke("ShowMainMenu", 2f);

    }

    public void CountDownCallback(float remainingTime)
    {
        testText.text = "Remaining Time = " + remainingTime;
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
