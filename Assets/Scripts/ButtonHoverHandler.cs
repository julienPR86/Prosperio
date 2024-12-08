using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TMP_Text hoverText; // Assignez le texte TMP via l'inspecteur
    private bool isClicked = false;

    void Start()
    {
        // Assurez-vous que le texte est invisible au d�part
        hoverText.color = new Color(hoverText.color.r, hoverText.color.g, hoverText.color.b, 0);
    }

    // Appel� au survol
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClicked)
        {
            hoverText.color = new Color(hoverText.color.r, hoverText.color.g, hoverText.color.b, 1); // Rendre le texte visible
        }
    }

    // Appel� lorsque le survol s'arr�te
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isClicked)
        {
            hoverText.color = new Color(hoverText.color.r, hoverText.color.g, hoverText.color.b, 0); // Masquer le texte si pas cliqu�
        }
    }

    // Appel� au clic du bouton
    public void OnPointerClick(PointerEventData eventData)
    {
        isClicked = true; // Maintenir le texte visible apr�s un clic
    }

    // Fonction pour r�initialiser le texte
    public void ResetText()
    {
        isClicked = false;
        hoverText.color = new Color(hoverText.color.r, hoverText.color.g, hoverText.color.b, 0); // Masquer le texte
    }
}
