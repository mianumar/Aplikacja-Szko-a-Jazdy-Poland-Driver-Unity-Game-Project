using UnityEngine;

public class SplashScreenView : MonoBehaviour
{
    public void RenderView()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }


}
