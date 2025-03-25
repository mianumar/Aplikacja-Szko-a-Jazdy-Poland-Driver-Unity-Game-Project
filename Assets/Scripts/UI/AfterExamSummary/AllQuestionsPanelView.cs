using Firebase.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AllQuestionsPanelView : MonoBehaviour
{
    [SerializeField] private Button buttonClose;

    [SerializeField] private Transform summaryItemContentParent;
    [SerializeField] private GameObject summaryItemPrefab;


    public List<GameObject> generatedConainerList = new List<GameObject>();

    public int startIndex = 1;
    public int limit = 20;

    public void RenderView()
    {
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnCloseButtonClicked);

        GetDataListFromServer();

        gameObject.SetActive(true);
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
        else
        {
        }
    }

    public void GetNextSet()
    {
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
            temp.GetComponent<SummaryItem>().RenderView(dataList[i],null, i + 1);
        }
    }

    private GameObject CreateOrGeneratePrefab(int index)
    {
        if (generatedConainerList.Count > index)
        {
            return generatedConainerList[index - 1];
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


}
