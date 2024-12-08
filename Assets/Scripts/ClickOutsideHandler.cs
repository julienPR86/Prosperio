using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickOutsideHandler : MonoBehaviour
{
    public List<ButtonHoverHandler> buttons; // Référence à tous les boutons
    private ButtonHoverHandler lastClickedButton = null; // Bouton sur lequel on a cliqué dernièrement

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Détecte le clic gauche
        {
            ButtonHoverHandler clickedButton = GetClickedButton();

            if (clickedButton != null)
            {
                // Si c'est un autre bouton, réinitialiser tous les boutons sauf celui-ci
                if (clickedButton != lastClickedButton)
                {
                    foreach (ButtonHoverHandler button in buttons)
                    {
                        button.ResetText(); // Réinitialise les textes de tous les boutons
                    }
                    lastClickedButton = clickedButton; // Mettre à jour le dernier bouton cliqué
                }
            }
            else
            {
                // Si le clic n'a pas eu lieu sur un bouton, réinitialiser tous les boutons
                if (lastClickedButton != null)
                {
                    foreach (ButtonHoverHandler button in buttons)
                    {
                        button.ResetText(); // Réinitialise les textes de tous les boutons
                    }
                    lastClickedButton = null; // Réinitialise la variable pour suivre les futurs clics
                }
            }
        }
    }

    // Méthode pour obtenir le bouton sur lequel on a cliqué
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
