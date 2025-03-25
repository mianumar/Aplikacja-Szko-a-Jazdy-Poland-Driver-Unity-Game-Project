using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class AfterExamAnswersView : MonoBehaviour
{
    [SerializeField] private Button buttonClose;
    [SerializeField] private Button buttonFilter;

    [SerializeField] private Transform summaryItemContentParent;
    [SerializeField] private GameObject summaryItemPrefab;

    [SerializeField] private FilterView filterView;

    public List<GameObject> generatedConainerList = new List<GameObject>();

    private SummaryData _summaryData;
    public void RenderView(SummaryData summaryData)
    {
        _summaryData = summaryData;
        AddListeners();
        GenerateItems(summaryData.QuestionAttempedList);
        gameObject.SetActive(true);
    }

    private void AddListeners()
    {
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnCloseButtonClicked);

        buttonFilter.onClick.RemoveAllListeners();
        buttonFilter.onClick.AddListener(OnFilterButtonClicked);

    }

    private void GenerateItems(List<QuestionAttempted> qstnList)
    {
        //List<QuestionAttempted> qstnList = summaryData.QuestionAttempedList;
        for (int i = 0; i < qstnList.Count; i++)
        {
            GameObject temp = CreateOrGeneratePrefab(i+1);
            temp.GetComponent<SummaryQestionContainer>().RenderView(qstnList[i] , i+1);
        }
    }

    private GameObject CreateOrGeneratePrefab(int index)
    {
        if (generatedConainerList.Count > index)
        {
            return generatedConainerList[index-1];
        }
        else
        {
            GameObject temp = Instantiate(summaryItemPrefab, summaryItemContentParent);
            temp.transform.localPosition = Vector3.zero;
            temp.transform.localScale = Vector3.one;
            generatedConainerList.Add(temp);
            return temp;
        }
    }

    public void UpdateItemByFilter(FilterItem item)
    {
        List<QuestionAttempted> qstnList = null;
        if (item != null && item.ItemIndex > 0)
        {
            qstnList = _summaryData.QuestionAttempedList.Where(tag => ((int)tag.resultType).Equals(item.ItemIndex - 1)).ToList();
        }
        else
        {
            qstnList = _summaryData.QuestionAttempedList;
        }
        DeleteAllItem();
        GenerateItems(qstnList);
    }

    private void DeleteAllItem()
    {
        for (int i = 0; i < generatedConainerList.Count; i++)
        {
            generatedConainerList[i].SetActive(false);
        }
    }


    private void OnFilterButtonClicked()
    {
        filterView.RenderView();
    }

    private void OnCloseButtonClicked()
    {
        UIHandler.Instance.summaryScreenView.RenderView();
        this.Disable();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
