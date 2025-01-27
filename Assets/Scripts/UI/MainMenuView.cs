using System.Collections;
using UnityEngine;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void RenderView()
    {
        animator.Play("MM_Open");
        Debug.Log("MainMenuView Splash");
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        Debug.Log("RenderView MainMenu Close");
        animator.SetBool("close", true);
        StartCoroutine(waitfor(0.30f));
        //Debug.Log("RenderView Splash Close");

    }

    IEnumerator waitfor(float secs)
    {
        yield return new WaitForSeconds(secs);
        gameObject.SetActive(false);
    }

}
