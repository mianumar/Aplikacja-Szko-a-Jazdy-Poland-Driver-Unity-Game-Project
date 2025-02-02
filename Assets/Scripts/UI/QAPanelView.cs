using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QAPanelView : MonoBehaviour
{
    [SerializeField] private Button buttonClose;
    [SerializeField] private Button buttonYes;
    [SerializeField] private Button buttonNo;
    [SerializeField] private Button buttonStart;
    [SerializeField] private Button buttonNext;


    [SerializeField] private TextMeshProUGUI textTimer;
    [SerializeField] private TextMeshProUGUI textCurrentQuestion;
    [SerializeField] private TextMeshProUGUI testDescription;
    [SerializeField] private TextMeshProUGUI textQuestion;

    [SerializeField] private TextMeshProUGUI textReamingTimeToRead;
    [SerializeField] private TextMeshProUGUI textReamingTimeToAnswer;
    [SerializeField] private Image remainQuestionSlider;

    [SerializeField] private Slider sliderRemainingQuestionsToAns;

    [SerializeField] private GameObject timeToReadQuestionPanel;
    [SerializeField] private GameObject timeToAnsPanel;

    // Ans Panel referance
    [SerializeField] private Image remainTimeToAnsSlider;
    [SerializeField] private TextMeshProUGUI textTimeReamingAns;



    private int MaxTotalPoints;
    private int MinTotalPoints;
    private int TotalQusetionsCount;

    private List<QADataModel> qADataModels;
    public void RenderView()
    {

        var instance = GameManager.Instance;
        if (instance != null)
        {
            MaxTotalPoints = instance.MaxTotalPoints;
            MinTotalPoints = instance.MinPointsToPass;
            qADataModels = instance.GetAllQuestionAns();

            TotalQusetionsCount = qADataModels.Count;
        }
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
