using UnityEngine;
using UnityEngine.UI;

public class TogglePanelControl : MonoBehaviour
{
    public Toggle targetToggle;  // Le Toggle qui contr�le la visibilit� du Panel
    public GameObject panelToControl; // Le Panel � contr�ler

    private bool isPanelVisible = false;

    void Start()
    {
        // Initialisation du Panel en fonction de l'�tat du Toggle
        panelToControl.SetActive(targetToggle.isOn);
    }

    void Update()
    {
        // V�rifie si on clique sur la sc�ne (clic gauche)
        if (Input.GetMouseButtonDown(0)) // 0 = clic gauche
        {
            // Si on clique en dehors du Toggle et du Panel, on d�sactive le Toggle et le Panel
            if (!RectTransformUtility.RectangleContainsScreenPoint(targetToggle.GetComponent<RectTransform>(), Input.mousePosition, Camera.main) &&
                !RectTransformUtility.RectangleContainsScreenPoint(panelToControl.GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
            {
                targetToggle.isOn = false; // D�sactive le Toggle
            }
        }

        // Met � jour la visibilit� du Panel en fonction de l'�tat du Toggle
        if (targetToggle.isOn != isPanelVisible)
        {
            isPanelVisible = targetToggle.isOn;
            panelToControl.SetActive(isPanelVisible);
        }
    }
}
