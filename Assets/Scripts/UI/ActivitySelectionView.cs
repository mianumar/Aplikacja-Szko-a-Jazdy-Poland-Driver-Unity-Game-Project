using System;
using UnityEngine;
using UnityEngine.UI;

public class ActivitySelectionView : MonoBehaviour
{
    [SerializeField] private Button buttonBack;
    [SerializeField] private Button buttonRulesPages;
    [SerializeField] private Button buttonAllSummary;
    public void RenderView()
    {
        AddListeners();
        gameObject.SetActive(true);
    }

    private void AddListeners()
    {
        buttonAllSummary.onClick.RemoveAllListeners();
        buttonBack.onClick.RemoveAllListeners();    
        buttonRulesPages.onClick.RemoveAllListeners();

        buttonBack.onClick.AddListener(OnBackButtonClicked);
        buttonRulesPages.onClick.AddListener(OnRulesButtonClicked);
        buttonAllSummary.onClick.AddListener(OnSummaryButtonClicked);
    }

    /// <summary>
    /// Action on summary button clicked
    /// </summary>
    private void OnSummaryButtonClicked()
    {
       // UIHandler.Instance.summaryScreenView.RenderView();
       // Disable();
    }


    /// <summary>
    /// Action on Rules button clicked
    /// </summary>
    private void OnRulesButtonClicked()
    {
        UIHandler.Instance.gameRulesPage.RenderView();
        Disable();
    }

    /// <summary>
    /// Action on back button clicked
    /// </summary>
    private void OnBackButtonClicked()
    {
        UIHandler.Instance.mainMenuView.RenderView();
        Disable();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
