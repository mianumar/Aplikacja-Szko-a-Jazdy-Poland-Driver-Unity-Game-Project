using UnityEngine;

public class ExitPopup : MonoBehaviour
{
    public void RenderView()
    {
        gameObject.SetActive(true);
    }

    public void OnExit()
    {
        gameObject.SetActive(false);
    }

}
