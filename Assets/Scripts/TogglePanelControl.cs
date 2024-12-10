using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TogglePanelControl : MonoBehaviour, IPointerDownHandler
{
    public Toggle targetToggle;  // The Toggle that controls the visibility of the Panel
    public GameObject panelToControl; // The Panel to control

    private bool isPanelVisible = false;

    void Start()
    {
        // Initialize the Panel based on the state of the Toggle
        panelToControl.SetActive(targetToggle.isOn);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // If clicking outside the Toggle and the Panel, deactivate the Toggle and the Panel
        if (!IsPointerOverUIElement(targetToggle.gameObject) && !IsPointerOverUIElement(panelToControl))
        {
            targetToggle.isOn = false; // Deactivate the Toggle
            panelToControl.SetActive(false); // Deactivate the Panel
        }
    }

    void Update()
    {
        // Update the visibility of the Panel based on the state of the Toggle
        if (targetToggle.isOn != isPanelVisible)
        {
            isPanelVisible = targetToggle.isOn;
            panelToControl.SetActive(isPanelVisible);
        }
    }

    // Check if the pointer (click) is over a UI element
    bool IsPointerOverUIElement(GameObject uiElement)
    {
        RectTransform rectTransform = uiElement.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, Camera.main);
    }
}
