using UnityEngine;
using UnityEngine.UI;

public class FilterItem : MonoBehaviour
{
    public int ItemIndex;
    public bool IsSelected;

    public Button ItemButton;
    public Image selectedCheckBox;

    public Sprite defaultSprite;
    public Sprite selectedSprite;

    public void RenderView(bool isSelected)
    {
        IsSelected = isSelected;
        selectedCheckBox.sprite = isSelected ? selectedSprite : defaultSprite;

        ItemButton.onClick.RemoveAllListeners();
        ItemButton.onClick.AddListener(() =>
        {
            FilterView.FilterChangedAction?.Invoke(this);
        });
    }
}
