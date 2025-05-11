using System;
using System.Collections;
using System.Diagnostics.Eventing.Reader;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummaryQestionContainer : MonoBehaviour
{
    [Header("SummaryQestionContainer Image References")]
    public Sprite IconSignBGLight;
    public Sprite IconSignBGDark;
    public Image IconSignBG;
    public Image IconNoImage;
    public Image IconFrameImage;
    public Sprite ContainerBGLight;
    public Sprite ContainerBGDark;
    public Image ContainerBG;


    private ThemeSwitcher themeSwitcher;

    public Image ansStatusIcon;
    public TextMeshProUGUI correctAns;
    public TextMeshProUGUI answered;
    public TextMeshProUGUI correctAnsIfSkipped;


    [Header("SummaryQestionContainer Text")]
    public TMP_Text[] texts;

    public Sprite[] statusIcon; // 0 - correct , 1 - wrong , 2 - skipped

    public Color[] AnsColor; // 0 - Correct

    private string[] headingText = new string[] { "Odpowiedź poprawna", "Odpowiedź niepoprawna", "Nieprzerobione" };

    //QuestionType 1- BASIC , 2 - SPECIAL

    private SimpleQuestionDataModel simpleQuestionData;
    private SpecializedQuestionModel specializedQuestionData;
    public void RenderView(QuestionAttempted question, int index)
    {
        IconNoImage.gameObject.SetActive(true);
        IconFrameImage.gameObject.SetActive(false);
        int resultIndex = (int)question.resultType;
        ansStatusIcon.sprite = statusIcon[resultIndex];
        texts[1].text = "PYTANIE:" + index + "/32";
        texts[4].text = headingText[resultIndex];
        //  texts[0].text = (int)question.questionType == 0 ? GameManager.Instance.GetSimpleQuestionFromList(question.QusetionNo).question : GameManager.Instance.GetSpecialQuestionFromList(question.QusetionNo).question;
        if ((int)question.questionType == 1)
        {
            simpleQuestionData = GameManager.Instance.GetSimpleQuestionFromList(question.QusetionNo);
            texts[0].text = simpleQuestionData.question;
            Sprite sp = GameManager.Instance.GetQuestionSprite(question.QuestionID);
            if (sp != null)
            {
                IconNoImage.gameObject.SetActive(false);
                IconFrameImage.sprite = sp;
                IconFrameImage.gameObject.SetActive(true);
            }
        }
        else
        {
            specializedQuestionData = GameManager.Instance.GetSpecialQuestionFromList(question.QusetionNo - 20);
            texts[0].text = specializedQuestionData.question;
            Sprite sp = GameManager.Instance.GetSpecializedQuestioSprite(question.QuestionID);
            if (sp != null)
            {
                IconNoImage.gameObject.SetActive(false);
                IconFrameImage.sprite = sp ;
                IconFrameImage.gameObject.SetActive(true);
            }

        }

        if (resultIndex == 0)
        {
            correctAnsIfSkipped.text = "";
            if ((int)question.questionType == 1)
            {
                answered.text = simpleQuestionData.answer.Equals("0") ? "NIE" : "TAK";
            }
            else
            {
                answered.text = specializedQuestionData.answer;
            }
            answered.color = AnsColor[resultIndex];

            correctAns.gameObject.SetActive(false);
        }
        else if (resultIndex == 1)
        {
            correctAnsIfSkipped.text = "";
            if ((int)question.questionType == 1)
            {
                correctAns.gameObject.SetActive(true);
                correctAns.text = simpleQuestionData.answer.Equals("0") ? "NIE" : "TAK";
                correctAns.color = AnsColor[2];
                answered.text = question.selectedAns;
                answered.fontStyle = FontStyles.Strikethrough;
                answered.color = AnsColor[1];
            }
            else
            {
                correctAns.gameObject.SetActive(true);
                correctAns.text = specializedQuestionData.answer;
                if (specializedQuestionData.answer.Equals("A"))
                {
                    correctAnsIfSkipped.text = specializedQuestionData.option_a;
                }
                else if (specializedQuestionData.answer.Equals("B"))
                {
                    correctAnsIfSkipped.text = specializedQuestionData.option_b;
                }
                else if (specializedQuestionData.answer.Equals("C"))
                {
                    correctAnsIfSkipped.text = specializedQuestionData.option_c;
                }
                correctAnsIfSkipped.color = AnsColor[3];
                correctAns.color = AnsColor[3];

                answered.text = question.selectedAns;
                answered.fontStyle = FontStyles.Strikethrough;
                answered.color = AnsColor[1];
            }

        }
        else if (resultIndex == 2)
        {
            correctAnsIfSkipped.text = "";
            correctAns.gameObject.SetActive(true);
            if ((int)question.questionType == 1)
            {
                correctAns.text = simpleQuestionData.answer.Equals("0") ? "NIE" : "TAK";
            }
            else
            {
                correctAns.text = specializedQuestionData.answer;
                if (specializedQuestionData.answer.Equals("A"))
                {
                    correctAnsIfSkipped.text = specializedQuestionData.option_a;
                }
                else if (specializedQuestionData.answer.Equals("B"))
                {
                    correctAnsIfSkipped.text = specializedQuestionData.option_b;
                }
                else if (specializedQuestionData.answer.Equals("C"))
                {
                    correctAnsIfSkipped.text = specializedQuestionData.option_c;
                }

            }
            correctAnsIfSkipped.color = AnsColor[3];
            correctAns.color = AnsColor[3];
            answered.gameObject.SetActive(false);
        }

        // Get the ThemeSwitcher instance
        themeSwitcher = ThemeSwitcher.instance;
        ApplyMode(themeSwitcher.isDarkMode);
        gameObject.SetActive(true);
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
