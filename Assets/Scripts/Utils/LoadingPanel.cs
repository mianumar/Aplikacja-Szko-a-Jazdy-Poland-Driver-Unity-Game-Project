using UnityEngine;

public class LoadingPanel : MonoBehaviour
{
   public GameObject loadingIcon;
   [Range(10,100)]public float speed = 5f;

    private bool IsLoading = false;
    // Update is called once per frame
    void Update()
    {
        if (IsLoading)
        {
            loadingIcon.transform.eulerAngles = new Vector3(0, 0, loadingIcon.transform.eulerAngles.z + speed * Time.deltaTime);
        }
    }

    public void ActiveLoading()
    {
        IsLoading = true;
        gameObject.SetActive(true);
    }

    public void DeactiveLoading()
    {
        IsLoading = false;
        gameObject.SetActive(false);
    }
}
