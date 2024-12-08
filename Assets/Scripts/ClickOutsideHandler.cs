using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickOutsideHandler : MonoBehaviour
{
    public List<ButtonHoverHandler> buttons; // R�f�rence � tous les boutons
    private ButtonHoverHandler lastClickedButton = null; // Bouton sur lequel on a cliqu� derni�rement

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // D�tecte le clic gauche
        {
            ButtonHoverHandler clickedButton = GetClickedButton();

            if (clickedButton != null)
            {
                // Si c'est un autre bouton, r�initialiser tous les boutons sauf celui-ci
                if (clickedButton != lastClickedButton)
                {
                    foreach (ButtonHoverHandler button in buttons)
                    {
                        button.ResetText(); // R�initialise les textes de tous les boutons
                    }
                    lastClickedButton = clickedButton; // Mettre � jour le dernier bouton cliqu�
                }
            }
            else
            {
                // Si le clic n'a pas eu lieu sur un bouton, r�initialiser tous les boutons
                if (lastClickedButton != null)
                {
                    foreach (ButtonHoverHandler button in buttons)
                    {
                        button.ResetText(); // R�initialise les textes de tous les boutons
                    }
                    lastClickedButton = null; // R�initialise la variable pour suivre les futurs clics
                }
            }
        }
    }

    // M�thode pour obtenir le bouton sur lequel on a cliqu�
    private ButtonHoverHandler GetClickedButton()
    {
        foreach (ButtonHoverHandler button in buttons)
        {
            if (EventSystem.current.IsPointerOverGameObject() && button.GetComponent<Collider2D>().OverlapPoint(Input.mousePosition))
            {
                return button;
            }
        }
        return null;
    }
}
