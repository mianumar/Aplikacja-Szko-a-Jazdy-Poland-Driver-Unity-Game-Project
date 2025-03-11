using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemeSwitcher : MonoBehaviour
{
    [Header("Colors References")]
    public Canvas canvas;
    public Color lightBackgroundColor = Color.white;
    public Color darkBackgroundColor = Color.black;
    public Color lightTextColor = Color.black;
    public Color darkTextColor = Color.white;
    public Color darkFaddedTextColor = Color.gray;

    [Header("MainMenu Title Colors References")]
    public Color darkMainMenuTitleTextColor = Color.gray;
    public Color lightMainMenuTitleTextColor = Color.blue;

    [Header("Button Sprites Reference")]
    public Sprite lightButtonSprite;
    public Sprite darkButtonSprite;

    [Header("Background Reference")]
    public Image commonBackgroundImage;

    [Header("UI References")]
    public TMP_Text[] texts;
    public TMP_Text mainMenuTitletext;
    public Button[] buttons;
    public Image[] CrossImage;

    [Header("Cross Image References")]
    public Sprite CrossImageLight;
    public Sprite CrossImageDark;

    [Header("Exit Popup Image References")]
    public Sprite ExitPopupImageLight;
    public Sprite ExitPopupImageDark;
    public Image ExitPopupImage;


    [SerializeField] private bool isDarkMode = false;

    private bool isPanelHidden = false;

    [SerializeField] private int textSizeThreshold = 20;


    [SerializeField] private float transitionDuration = 0.5f;


    public static ThemeSwitcher instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        ApplyMode(isDarkMode, false); // Initialize with no animation on start
    }

    public void ToggleMode()
    {
        isDarkMode = !isDarkMode;
        StartCoroutine(FadeToMode(isDarkMode));
    }

    // Coroutine to smoothly transition between light and dark mode
    IEnumerator FadeToMode(bool darkMode)
    {
        // Start fading transition for background
        Color targetBackgroundColor = darkMode ? darkBackgroundColor : lightBackgroundColor;
        Color initialBackgroundColor = commonBackgroundImage.color;
        float timeElapsed = 0f;

        while (timeElapsed < transitionDuration)
        {
            commonBackgroundImage.color = Color.Lerp(initialBackgroundColor, targetBackgroundColor, timeElapsed / transitionDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        commonBackgroundImage.color = targetBackgroundColor; // Ensure it reaches the target color

        // Start fading text and buttons
        ApplyMode(darkMode, true); // Apply text/button changes during the transition
    }

    void ApplyMode(bool darkMode, bool isTransitioning)
    {
        if (!isTransitioning)
        {
            // Apply the color change immediately without fading
            commonBackgroundImage.color = darkMode ? darkBackgroundColor : lightBackgroundColor;
        }
        mainMenuTitletext.color = darkMode ? darkMainMenuTitleTextColor : lightMainMenuTitleTextColor;


        // Apply text color changes with a fade
        foreach (var text in texts)
        {
            StartCoroutine(FadeTextColor(text, darkMode));
        }

        foreach (var cross in CrossImage)
        {
            //cross.sprite = darkMode ? CrossImageDark : CrossImageLight;
            if (cross != null)
            {
                cross.sprite = darkMode ? CrossImageDark : CrossImageLight;
            }
        }

        ExitPopupImage.sprite = darkMode ? ExitPopupImageDark : ExitPopupImageLight;

        // Apply button color and sprite changes with a fade
        foreach (var button in buttons)
        {
            StartCoroutine(FadeButton(button, darkMode));
        }
    }

    // Coroutine to fade the text color smoothly
    IEnumerator FadeTextColor(TMP_Text text, bool darkMode)
    {
        Color targetColor = text.fontSize > textSizeThreshold ? (darkMode ? darkTextColor : lightTextColor) : (darkMode ? darkFaddedTextColor : lightTextColor);
        Color initialColor = text.color;
        float timeElapsed = 0f;

        while (timeElapsed < transitionDuration)
        {
            text.color = Color.Lerp(initialColor, targetColor, timeElapsed / transitionDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        text.color = targetColor; // Ensure it reaches the target color
    }

    // Coroutine to fade the button text and sprite smoothly
    IEnumerator FadeButton(Button button, bool darkMode)
    {
        var buttonImage = button.GetComponent<Image>();
        Sprite targetSprite = darkMode ? darkButtonSprite : lightButtonSprite;
        Color targetButtonTextColor = darkMode ? darkTextColor : lightTextColor;

        Color initialButtonColor = buttonImage.color;
        Sprite initialSprite = buttonImage.sprite;
        float timeElapsed = 0f;

        // Fade button background color
        while (timeElapsed < transitionDuration)
        {
            buttonImage.color = Color.Lerp(initialButtonColor, targetButtonTextColor, timeElapsed / transitionDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        buttonImage.sprite = targetSprite;
        buttonImage.color = targetButtonTextColor; // Ensure final color after fade

        // Fade button text color
        var buttonText = button.GetComponentInChildren<TMP_Text>();
        if (buttonText != null)
        {
            StartCoroutine(FadeTextColor(buttonText, darkMode));
        }
    }
}
