using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummaryQestionContainer : MonoBehaviour
{
    [Header("SummaryQestionContainer Image References")]
    public Sprite IconSignBGLight;
    public Sprite IconSignBGDark;
    public Image IconSignBG;
    public Sprite ContainerBGLight;
    public Sprite ContainerBGDark;
    public Image ContainerBG;

    private ThemeSwitcher themeSwitcher;


    [Header("SummaryQestionContainer Text")]
    public TMP_Text[] texts;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the ThemeSwitcher instance
        themeSwitcher = ThemeSwitcher.instance;
        ApplyMode(themeSwitcher.isDarkMode);
    }


    // Function to toggle the theme (light/dark mode)
    public void ToggleMode()
    {
        if (themeSwitcher != null)
        {
           // themeSwitcher.ToggleMode();  // Call the ThemeSwitcher's ToggleMode method

            // Apply the updated mode to this container
            ApplyMode(themeSwitcher.isDarkMode);
        }
    }

    // Method to immediately apply mode changes (color and background updates)
    // Method to immediately apply mode changes (color and background updates)
    void ApplyMode(bool darkMode)
    {
        // Apply the correct background image based on the mode
        IconSignBG.sprite = darkMode ? IconSignBGDark : IconSignBGLight;
        ContainerBG.sprite = darkMode ? ContainerBGDark : ContainerBGLight;

        // Apply text color based on the mode
        foreach (var text in texts)
        {
            //text.color = darkMode ? themeSwitcher.darkTextColor : themeSwitcher.lightTextColor;

            // Check if the text has a "Faded" tag or needs to use faded colors
            if (text.text.Contains("<Faded>"))
            {
                // Apply faded colors based on the mode
                text.color = darkMode ? themeSwitcher.darkFaddedTextColor : themeSwitcher.lightTextColor;
            }
            else
            {
                // Apply normal text colors based on the mode
                text.color = darkMode ? themeSwitcher.darkTextColor : themeSwitcher.lightTextColor;
            }
        }
    }


}
