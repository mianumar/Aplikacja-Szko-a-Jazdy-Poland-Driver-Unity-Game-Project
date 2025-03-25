using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummaryItem : MonoBehaviour
{
    private ThemeSwitcher themeSwitcher;

    public Sprite IconSignBGLight;
    public Sprite IconSignBGDark;
    public Image IconSignBG;
    public Sprite ContainerBGLight;
    public Sprite ContainerBGDark;
    public Image ContainerBG;

    [Header("SummaryQestionContainer Text")]
    public TMP_Text[] texts;

    public string[] options;

    public void RenderView(SimpleQuestionDataModel simpleData, SpecializedQuestionModel specialData, int index)
    {
        texts[1].text = "PYTANIE:" + index + "/" + (GameManager.Instance.totalSimpleQuestionCount + GameManager.Instance.totalSpecialQuestionCount);
        if (simpleData != null)
        {
            texts[0].text = simpleData.question;
            texts[2].text = simpleData.answer.Equals("0") ? "NIE" : "TAK";
            string extention = GameConstants.GetFileExtensionFromUrl(simpleData.media_link);
            if (string.IsNullOrEmpty(extention))
                return;
            if (extention.Equals(".jpg"))
            {
                GameUtils.ImageDownloader.RequestDownload(this, simpleData.media_link, (tex) =>
                {
                    IconSignBG.sprite = GameManager.Instance.TextureToSprite(tex);
                });
            }
        }
        else if (specialData != null)
        {
            texts[0].text = specialData.question;
            if (specialData.answer.Equals("A"))
            {
                texts[2].text = specialData.option_a;
            }
            else if (specialData.answer.Equals("B"))
            {
                texts[2].text = specialData.option_b;
            }
            else
            {
                texts[2].text = specialData.option_c;
            }
            string extention = GameConstants.GetFileExtensionFromUrl(specialData.media_link);

            if (string.IsNullOrEmpty(extention))
                return;
            if (extention.Equals(".jpg"))
            {
                GameUtils.ImageDownloader.RequestDownload(this, specialData.media_link, (tex) =>
                {
                    IconSignBG.sprite = GameManager.Instance.TextureToSprite(tex);
                });
            }

        }
        else
        {
            Debug.LogError("Simple Question and Special Data Not fount ");
        }
        themeSwitcher = ThemeSwitcher.instance;
        ApplyMode(themeSwitcher.isDarkMode);
        gameObject.SetActive(true);
    }

    public void ApplyMode(bool isDarkMode)
    {
        // Apply the correct background image based on the mode
        IconSignBG.sprite = isDarkMode ? IconSignBGDark : IconSignBGLight;
        ContainerBG.sprite = isDarkMode ? ContainerBGDark : ContainerBGLight;

        // Apply text color based on the mode
        foreach (var text in texts)
        {
            //text.color = darkMode ? themeSwitcher.darkTextColor : themeSwitcher.lightTextColor;

            // Check if the text has a "Faded" tag or needs to use faded colors
            if (text.text.Contains("<Faded>"))
            {
                // Apply faded colors based on the mode
                text.color = isDarkMode ? themeSwitcher.darkFaddedTextColor : themeSwitcher.lightTextColor;
            }
            else
            {
                // Apply normal text colors based on the mode
                text.color = isDarkMode ? themeSwitcher.darkTextColor : themeSwitcher.lightTextColor;
            }
        }
    }
}
