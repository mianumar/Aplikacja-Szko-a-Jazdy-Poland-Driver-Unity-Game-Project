using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private Button buttonSettings;
    [SerializeField] private Button buttonStart;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void RenderView()
    {
        animator.Play("MM_Open");
        Debug.Log("MainMenuView Splash");
        AddListeners();
        gameObject.SetActive(true);
    }

    private void AddListeners()
    {
        buttonStart.onClick.RemoveAllListeners();
        buttonSettings.onClick.RemoveAllListeners();

        buttonStart.onClick.AddListener(OnStartButtonClicked);
        buttonSettings.onClick.AddListener(OnSettingsClicked);

    }

    /// <summary>
    /// Action on setting button clicked
    /// </summary>
    private void OnSettingsClicked()
    {
        UIHandler.Instance.settingsPanelView.RenderView();
        Disable();
    }


    /// <summary>
    /// Action on start button clicked
    /// </summary>
    private void OnStartButtonClicked()
    {
        UIHandler.Instance.activitySelectionView.RenderView();
        Disable();
    }

    public void Disable()
    {
        Debug.Log("RenderView MainMenu Close");
        animator.SetBool("close", true);
       // StartCoroutine(waitfor(0.30f));
        //Debug.Log("RenderView Splash Close");
        gameObject.SetActive(false);
    }

    IEnumerator waitfor(float secs)
    {
        yield return new WaitForSeconds(secs);
        gameObject.SetActive(false);
    }

}
