using System;
using UnityEngine;
using UnityEngine.UI;

public class GameRulesPage : MonoBehaviour
{
    [SerializeField] private Button buttonClose;
    public void RenderView()
    {
        AddListeners();
        gameObject.SetActive(true);
    }

    private void AddListeners()
    {
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnCloseButtonClicked);
    }

    private void OnCloseButtonClicked()
    {
        UIHandler.Instance.activitySelectionView.RenderView();
        Disable();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
