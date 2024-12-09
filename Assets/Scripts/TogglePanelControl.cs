using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TogglePanelControl : MonoBehaviour, IPointerDownHandler
{
    public Toggle targetToggle;  // Le Toggle qui contr�le la visibilit� du Panel
    public GameObject panelToControl; // Le Panel � contr�ler

    private bool isPanelVisible = false;

    void Start()
    {
        // Initialisation du Panel en fonction de l'�tat du Toggle
        panelToControl.SetActive(targetToggle.isOn);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Si on clique en dehors du Toggle et du Panel, on d�sactive le Toggle et le Panel
        if (!IsPointerOverUIElement(targetToggle.gameObject) && !IsPointerOverUIElement(panelToControl))
        {
            targetToggle.isOn = false; // D�sactive le Toggle
            panelToControl.SetActive(false); // D�sactive le Panel
        }
    }

    void Update()
    {
        // Met � jour la visibilit� du Panel en fonction de l'�tat du Toggle
        if (targetToggle.isOn != isPanelVisible)
        {
            isPanelVisible = targetToggle.isOn;
            panelToControl.SetActive(isPanelVisible);
        }
    }

    // V�rifie si le pointeur (clic) est sur un �l�ment UI
    bool IsPointerOverUIElement(GameObject uiElement)
    {
        RectTransform rectTransform = uiElement.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, Camera.main);
    }
}
