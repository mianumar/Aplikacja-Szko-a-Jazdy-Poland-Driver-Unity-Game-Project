using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FilterView : MonoBehaviour
{
    [SerializeField] private Button buttonUpdateFilter;

    public List<FilterItem> filterItems;
    public static Action<FilterItem> FilterChangedAction;

    public FilterItem currentSelectedFilter;
    public void RenderView()
    {
        FilterChangedAction += OnFilterChanged;
        AddListeners();
        SetDefaultFilterData();
        gameObject.SetActive(true);
    }

    private void OnFilterChanged(FilterItem item)
    {
        currentSelectedFilter = item;
        for (int i = 0; i < filterItems.Count; i++)
        {
            filterItems[i].RenderView(false);
        }
        filterItems[item.ItemIndex].RenderView(true);
    }

    private void AddListeners()
    {
        buttonUpdateFilter.onClick.RemoveAllListeners();
        buttonUpdateFilter.onClick.AddListener(OnUpdateFilter);
    }

    private void SetDefaultFilterData()
    {
        for (int i = 0; i < filterItems.Count; i++)
        {
            filterItems[i].RenderView(false);
        }
        filterItems[0].RenderView(true);
        currentSelectedFilter = filterItems[0];
    }


    private void OnUpdateFilter()
    {
        UIHandler.Instance.afterExamAnswersView.UpdateItemByFilter(currentSelectedFilter);
        Disable();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
