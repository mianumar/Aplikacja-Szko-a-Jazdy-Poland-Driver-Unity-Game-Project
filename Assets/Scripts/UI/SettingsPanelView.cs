using UnityEngine;

public class SettingsPanelView : MonoBehaviour
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
