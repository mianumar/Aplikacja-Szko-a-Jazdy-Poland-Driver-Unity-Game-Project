using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllQuestionsPanelView : MonoBehaviour
{
    [SerializeField] private Button buttonClose;

    [SerializeField] private Transform summaryItemContentParent;
    [SerializeField] private GameObject summaryItemPrefab;


    public List<GameObject> generatedConainerList = new List<GameObject>();


    public void RenderView()
    {

    }
}
