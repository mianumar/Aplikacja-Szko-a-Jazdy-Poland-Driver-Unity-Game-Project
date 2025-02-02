using System;
using UnityEngine;
using UnityEngine.UI;

public class ExitPopup : MonoBehaviour
{
    [SerializeField] private Button buttonYes;
    [SerializeField] private Button buttonNo;
    public void RenderView()
    {
        buttonNo.onClick.RemoveAllListeners();
        buttonYes.onClick.RemoveAllListeners();

        buttonNo.onClick.AddListener(OnClickNo);
        buttonYes.onClick.AddListener(OnClickYes);
        gameObject.SetActive(true);
    }

    private void OnClickYes()
    {
        UIHandler.Instance.activitySelectionView.RenderView();
        gameObject.SetActive(false);
    }

    public void OnClickNo()
    {
        UIHandler.Instance.qaPanelView.Disable();
        gameObject.SetActive(false);
    }

}
