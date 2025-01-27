using System;
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
    public void RenderView()
    {

        AddListeners();

        musicToggleImage = toggleMusic.GetComponent<Image>();
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
    }

    private void OnMusicToggleUpdated(bool isOn)
    {
       musicToggleImage.sprite = isOn ? checkedMusicSetting : defaultMusicSetting;
    }

    private void OnSaveButtonClicked()
    {
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
