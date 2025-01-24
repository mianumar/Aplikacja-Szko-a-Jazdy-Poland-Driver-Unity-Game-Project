using UnityEngine;

public class SummaryScreenView : MonoBehaviour
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
