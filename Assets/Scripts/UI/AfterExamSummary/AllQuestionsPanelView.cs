using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AllQuestionsPanelView : MonoBehaviour
{
    [SerializeField] private Button buttonClose;

    [SerializeField] private Transform summaryItemContentParent;
    [SerializeField] private GameObject summaryItemPrefab;

    public ScrollRect scrollRect;

    public List<GameObject> generatedConainerList = new List<GameObject>();

    public int startIndex = 1;
    public int limit = 20;
    private bool isLoading = false;
    private int buffer = 20;

    public void RenderView()
    {
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnCloseButtonClicked);

        scrollRect.onValueChanged.RemoveAllListeners();
        scrollRect.onValueChanged.AddListener(OnScrollChanged);
        GetDataListFromServer();
        scrollRect.verticalNormalizedPosition = 1;
        gameObject.SetActive(true);
    }

    private void OnScrollChanged(Vector2 scrollPosition)
    {
        if (!isLoading && scrollPosition.y <= 0.1f)
        {
            StartCoroutine(GetNextSet());
        }
    }

    public async void GetDataListFromServer()
    {
        GameManager.Instance.loadingPanel.ActiveLoading();
        if (startIndex < GameManager.Instance.totalSimpleQuestionCount)
        {
            await ServerHandler.instance.GetSimpleQusetionsInRange(startIndex, limit).ContinueWithOnMainThread(task =>
            {
                SimpleQuestionListDataModel data = task.Result;
                GenerateItem(data.data, null);
            });
        }
        else if (startIndex > GameManager.Instance.totalSimpleQuestionCount &&
            startIndex < (GameManager.Instance.totalSimpleQuestionCount + GameManager.Instance.totalSpecialQuestionCount))
        {
            await ServerHandler.instance.GetSpecializedQusetionsInRange(startIndex, limit).ContinueWithOnMainThread(task =>
            {
                SpecializedQuestionListDataModel data = task.Result;
                GenerateItem(null, data.data);
            });
        }
        else
        {
            Debug.Log("Ypu reached end of the list ");
        }
    }

    public IEnumerator GetNextSet()
    {
        Debug.Log("GetNextSet");

        isLoading = true;
        yield return new WaitForSeconds(1f);
        startIndex += limit;
        GetDataListFromServer();

    }

    private void OnCloseButtonClicked()
    {
        UIHandler.Instance.activitySelectionView.RenderView();
        gameObject.SetActive(false);
    }

    private int dataLength;
    private int dataLengthSpecial;
    public void GenerateItem(List<SimpleQuestionDataModel> simpleDataList, List<SpecializedQuestionModel> specialDataList)
    {
        if (simpleDataList != null)
        {
            dataLength = simpleDataList.Count;
            for (int i = 0; i < dataLength; i++)
            {
                GameObject temp = CreateOrGeneratePrefab(i + 1);
                temp.GetComponent<SummaryItem>().RenderView(simpleDataList[i], null, (startIndex + i), i, OnItemDoneCallback);
            }
        }
        if (specialDataList != null)
        {
            dataLengthSpecial = specialDataList.Count;
            for (int i = 0; i < dataLengthSpecial; i++)
            {
                GameObject temp = CreateOrGeneratePrefab(i + 1);
                temp.GetComponent<SummaryItem>().RenderView(null, specialDataList[i], (startIndex + i), i, OnItemDoneCallback);
            }
        }

        isLoading = false;
    }

    private void OnItemDoneCallback(int index)
    {
        Debug.Log("OnItemDoneCallback :: index :: " + index);
        if (index == dataLength - 1)
        {
            GameManager.Instance.loadingPanel.DeactiveLoading();
        }
    }

    private GameObject CreateOrGeneratePrefab(int index)
    {
        GameObject temp = Instantiate(summaryItemPrefab, summaryItemContentParent);
        temp.transform.localPosition = Vector3.zero;
        temp.transform.localScale = Vector3.one;
        generatedConainerList.Add(temp);
        return temp;

    }


}
