using UnityEngine;
using UnityEngine.EventSystems;

public class HoverUnderlineButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Hover UI")]
    public GameObject underlineImage;

    private void Awake()
    {
        HideUnderline();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowUnderline();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideUnderline();
    }

    private void ShowUnderline()
    {
        if (underlineImage != null)
            underlineImage.SetActive(true);
    }

    private void HideUnderline()
    {
        if (underlineImage != null)
            underlineImage.SetActive(false);
    }
}