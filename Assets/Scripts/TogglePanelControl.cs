using UnityEngine;
using UnityEngine.UI;

public class TogglePanelControl : MonoBehaviour
{
    public Toggle targetToggle;  // Le Toggle qui contrôle la visibilité du Panel
    public GameObject panelToControl; // Le Panel à contrôler

    private bool isPanelVisible = false;

    void Start()
    {
        // Initialisation du Panel en fonction de l'état du Toggle
        panelToControl.SetActive(targetToggle.isOn);
    }

    void Update()
    {
        // Vérifie si on clique sur la scène (clic gauche)
        if (Input.GetMouseButtonDown(0)) // 0 = clic gauche
        {
            // Si on clique en dehors du Toggle et du Panel, on désactive le Toggle et le Panel
            if (!RectTransformUtility.RectangleContainsScreenPoint(targetToggle.GetComponent<RectTransform>(), Input.mousePosition, Camera.main) &&
                !RectTransformUtility.RectangleContainsScreenPoint(panelToControl.GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
            {
                targetToggle.isOn = false; // Désactive le Toggle
            }
        }

        // Met à jour la visibilité du Panel en fonction de l'état du Toggle
        if (targetToggle.isOn != isPanelVisible)
        {
            isPanelVisible = targetToggle.isOn;
            panelToControl.SetActive(isPanelVisible);
        }
    }
}
