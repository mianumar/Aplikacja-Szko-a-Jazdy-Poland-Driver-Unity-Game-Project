using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class AfterExamAnswersView : MonoBehaviour
{
    [SerializeField] private Button buttonClose;
    [SerializeField] private Button buttonFilter;

    [SerializeField] private Transform summaryItemContentParent;
    [SerializeField] private GameObject summaryItemPrefab;

    [SerializeField] private FilterView filterView;

    public List<SummaryQestionContainer> generatedConainerList = new List<SummaryQestionContainer>();
    public void RenderView(SummaryData summaryData)
    {
        AddListeners();
        GenerateItems(summaryData);
        gameObject.SetActive(true);
    }

    private void AddListeners()
    {
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnCloseButtonClicked);

        buttonFilter.onClick.RemoveAllListeners();
        buttonFilter.onClick.AddListener(OnFilterButtonClicked);

    }

    private void GenerateItems(SummaryData summaryData)
    {
        List<QuestionAttempted> qstnList = summaryData.QuestionAttempedList;
        for (int i = 0; i < qstnList.Count; i++)
        {
            GameObject temp = Instantiate(summaryItemPrefab, summaryItemContentParent);
            temp.transform.localPosition = Vector3.zero;
            temp.transform.localScale = Vector3.one;
            temp.GetComponent<SummaryQestionContainer>().RenderView(qstnList[i]);

        }
    }



    private void OnFilterButtonClicked()
    {
        filterView.RenderView();
    }

    private void OnCloseButtonClicked()
    {
        UIHandler.Instance.activitySelectionView.RenderView();
        this.Disable();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
