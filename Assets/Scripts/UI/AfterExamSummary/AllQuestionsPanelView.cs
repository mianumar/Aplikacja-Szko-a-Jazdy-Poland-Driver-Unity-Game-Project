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
        if (startIndex < GameManager.Instance.totalSimpleQuestionCount)
        {
            await ServerHandler.instance.GetSimpleQusetionsInRange(startIndex, limit).ContinueWithOnMainThread(task =>
            {
                SimpleQuestionListDataModel data = task.Result;
                GenerateItem(data.data);
            });
        }
        else if (startIndex > GameManager.Instance.totalSimpleQuestionCount &&
            startIndex < (GameManager.Instance.totalSimpleQuestionCount + GameManager.Instance.totalSpecialQuestionCount))
        {
        }
        else
        {
            Debug.Log("Ypu reached end of the list ");
        }
    }

    public IEnumerator GetNextSet()
    {
        Debug.LogError("GetNextSet");

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

    public void GenerateItem(List<SimpleQuestionDataModel> dataList)
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            GameObject temp = CreateOrGeneratePrefab(i + 1);
            temp.GetComponent<SummaryItem>().RenderView(dataList[i], null, (startIndex + i));
        }
        isLoading = false;
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
