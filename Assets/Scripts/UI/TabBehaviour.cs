using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Sprite defaultSprite, selected, hover;
    Image tabImage; // Ensure this is UnityEngine.UI.Image  
    public UIManager.Tab windowToOpen;

    public static UnityEvent onTabStateChange = new UnityEvent(); // Simplified 'new' expression  

    private void Awake()
    {
        tabImage = GetComponent<Image>(); // UnityEngine.UI.Image is a Unity Component  
        onTabStateChange.AddListener(RenderTabState);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onTabStateChange?.Invoke();
        tabImage.sprite = selected;
        UIManager.Instance.OpenWindow(windowToOpen);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onTabStateChange?.Invoke();
        tabImage.sprite = hover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //tabImage.sprite = defaultSprite;
        onTabStateChange?.Invoke();
        //RenderTabState();
    }

    void RenderTabState()
    {
        if (UIManager.Instance.selectedTab == windowToOpen)
        {
            tabImage.sprite = selected;
            return;
        }
        tabImage.sprite = defaultSprite;
    }
}
