using UnityEngine;

public class ActivitySelectionView : MonoBehaviour
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
