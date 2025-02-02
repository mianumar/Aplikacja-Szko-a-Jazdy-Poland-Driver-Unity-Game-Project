using System;
using UnityEngine;
using UnityEngine.UI;

public class GameRulesPage : MonoBehaviour
{
    [SerializeField] private Button buttonClose;
    [SerializeField] private Button buttonStart;
    public void RenderView()
    {
        AddListeners();
        gameObject.SetActive(true);
    }

    private void AddListeners()
    {
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnCloseButtonClicked);

        buttonStart.onClick.RemoveAllListeners();
        buttonStart.onClick.AddListener(OnStartButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        UIHandler.Instance.qaPanelView.RenderView();
        this.Disable();
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
