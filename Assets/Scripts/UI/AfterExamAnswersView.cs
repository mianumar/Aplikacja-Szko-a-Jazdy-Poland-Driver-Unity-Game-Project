using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AfterExamAnswersView : MonoBehaviour
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
