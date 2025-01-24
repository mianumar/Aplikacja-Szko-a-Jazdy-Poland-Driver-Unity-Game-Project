using UnityEngine;

public class QAPanelView : MonoBehaviour
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
