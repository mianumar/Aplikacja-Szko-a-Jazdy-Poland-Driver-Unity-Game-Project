using System;
using System.Drawing.Printing;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelView : MonoBehaviour
{
    [SerializeField] private Button buttonBack;
    [SerializeField] private Toggle toggleLightMode;
    [SerializeField] private Toggle toggleDarkMode;

    [SerializeField] private Slider sliderVoulme;
    [SerializeField] private Toggle toggleMusic;

    [SerializeField] private Button buttonSave;

     private Image musicToggleImage;

    // Public Referance
    [SerializeField] private Sprite defaultMusicSetting;
    [SerializeField] private Sprite checkedMusicSetting;

    private float volumePercent;
    private bool isLightMode;
    private bool isMusicOn;
    public void RenderView(GameSettings gameSettings)
    {

        AddListeners();

        musicToggleImage = toggleMusic.GetComponent<Image>();

        sliderVoulme.value = gameSettings.VolumePercent;
        toggleMusic.isOn = gameSettings.IsSoundOn;
        toggleLightMode.isOn = gameSettings.IsLightMode;
        toggleDarkMode.isOn = !gameSettings.IsLightMode;
        musicToggleImage.sprite = gameSettings.IsSoundOn ? checkedMusicSetting : defaultMusicSetting;
        ThemeSwitcher.instance.ToggleMode(!gameSettings.IsLightMode);
        toggleLightMode.GetComponent<Image>().color = gameSettings.IsLightMode ? GameConstants.GetColorFromHexCode("#FFF4CC") : Color.white;
        toggleDarkMode.GetComponent<Image>().color = !gameSettings.IsLightMode ? GameConstants.GetColorFromHexCode("#1BA7E2") : Color.white;
        
        gameObject.SetActive(true);

    }

    private void AddListeners()
    {
        buttonBack.onClick.RemoveAllListeners();
        toggleLightMode.onValueChanged.RemoveAllListeners();
        toggleDarkMode.onValueChanged.RemoveAllListeners();
        buttonSave.onClick.RemoveAllListeners();

        toggleMusic.onValueChanged.RemoveAllListeners();
        sliderVoulme.onValueChanged.RemoveAllListeners();

        buttonBack.onClick.AddListener(OnBackButtonClicked);
        buttonSave.onClick.AddListener(OnSaveButtonClicked);

        toggleMusic.onValueChanged.AddListener(OnMusicToggleUpdated);
        sliderVoulme.onValueChanged.AddListener(OnVolumeUpdate);
        
        toggleDarkMode.onValueChanged.AddListener(OnGameModeUpdate);
    }

    private void OnGameModeUpdate(bool mode)
    { 
        Debug.Log("OnGameModeUpdate :: "+!mode);
        isLightMode = !mode;

        toggleLightMode.GetComponent<Image>().color = isLightMode ? GameConstants.GetColorFromHexCode("#FFF4CC") : Color.white;
        toggleDarkMode.GetComponent<Image>().color = !isLightMode ? GameConstants.GetColorFromHexCode("#1BA7E2") : Color.white;

        ThemeSwitcher.instance.ToggleMode(mode);

    }

    private void OnVolumeUpdate(float volume)
    {
        volumePercent = volume;
    }

    private void OnMusicToggleUpdated(bool isOn)
    {
        isMusicOn = isOn;
       musicToggleImage.sprite = isOn ? checkedMusicSetting : defaultMusicSetting;
    }

    /// <summary>
    /// Save Summary Data to Database
    /// </summary>
    private void OnSaveButtonClicked()
    {
        GameSettings gameSettings = new GameSettings()
        {
            IsLightMode = isLightMode,
            IsSoundOn  = isMusicOn,
            VolumePercent = volumePercent,
        };
        DatabaseHandler.Instance.SaveUserGameSettings(GameManager.Instance.UserID, gameSettings);
    }

 
    private void OnBackButtonClicked()
    {
        UIHandler.Instance.mainMenuView.RenderView();
        Disable();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
