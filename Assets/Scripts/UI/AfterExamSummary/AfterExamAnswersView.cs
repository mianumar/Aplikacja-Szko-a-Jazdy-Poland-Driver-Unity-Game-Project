using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class AfterExamAnswersView : MonoBehaviour
{
    [SerializeField] private Button buttonClose;
    [SerializeField] private Button buttonFilter;



    [SerializeField] private FilterView filterView;


    public void RenderView()
    {
        AddListeners();
        gameObject.SetActive(true);
    }

    private void AddListeners()
    {
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnCloseButtonClicked);

        buttonFilter.onClick.RemoveAllListeners();
        buttonFilter.onClick.AddListener(OnFilterButtonClicked);

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
