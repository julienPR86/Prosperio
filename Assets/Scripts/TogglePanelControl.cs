using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TogglePanelControl : MonoBehaviour, IPointerDownHandler
{
    public Toggle targetToggle;  // Le Toggle qui contrôle la visibilité du Panel
    public GameObject panelToControl; // Le Panel à contrôler

    private bool isPanelVisible = false;

    void Start()
    {
        // Initialisation du Panel en fonction de l'état du Toggle
        panelToControl.SetActive(targetToggle.isOn);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Si on clique en dehors du Toggle et du Panel, on désactive le Toggle et le Panel
        if (!IsPointerOverUIElement(targetToggle.gameObject) && !IsPointerOverUIElement(panelToControl))
        {
            targetToggle.isOn = false; // Désactive le Toggle
            panelToControl.SetActive(false); // Désactive le Panel
        }
    }

    void Update()
    {
        // Met à jour la visibilité du Panel en fonction de l'état du Toggle
        if (targetToggle.isOn != isPanelVisible)
        {
            isPanelVisible = targetToggle.isOn;
            panelToControl.SetActive(isPanelVisible);
        }
    }

    // Vérifie si le pointeur (clic) est sur un élément UI
    bool IsPointerOverUIElement(GameObject uiElement)
    {
        RectTransform rectTransform = uiElement.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, Camera.main);
    }
}
