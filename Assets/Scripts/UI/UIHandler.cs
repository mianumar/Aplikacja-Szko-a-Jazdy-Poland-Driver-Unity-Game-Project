using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private SplashScreenView splashScreenView;
    [SerializeField] private MainMenuView MainMenuView;
    [SerializeField] private ActivitySelectionView activitySelectionView;
    [SerializeField] private GameRulesPage gameRulesPage;
    [SerializeField] private SettingsPanelView settingsPanelView;
    [SerializeField] private QAPanelView qaPanelView;
    [SerializeField] private SummaryScreenView summaryScreenView;
    [SerializeField] private ExitPopup exitPopup;
    public RawImage testDownloader;
    public Text testText;
    string url = "https://photographylife.com/wp-content/uploads/2014/09/Nikon-D750-Image-Samples-2.jpg";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameUtils.ImageDownloader.RequestDownload(this, url, (tex2D) =>
        {
            testDownloader.texture = tex2D;
        });

        GameUtils.GameTimer.CoutDownTimer(this, 10f, CountDownCallback);

    }

    public void CountDownCallback(float remainingTime)
    {
        testText.text = "Remaining Time = " + remainingTime;
    }

}
