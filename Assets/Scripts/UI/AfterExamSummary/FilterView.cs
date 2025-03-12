using System;
using UnityEngine;
using UnityEngine.UI;

public class FilterView : MonoBehaviour
{
    [SerializeField] private Button buttonUpdateFilter;
    public void RenderView()
    {
        AddListeners();
        gameObject.SetActive(true);
    }


    private void AddListeners()
    {
        buttonUpdateFilter.onClick.RemoveAllListeners();
        buttonUpdateFilter.onClick.AddListener(OnUpdateFilter);
    }

    private void OnUpdateFilter()
    {
        Disable();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
