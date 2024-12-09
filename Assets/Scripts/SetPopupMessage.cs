using UnityEngine;

public class SetPopUpMessage : MonoBehaviour
{
    // Référence au PopupManager
    public PopupManager popupManager;

    void Start()
    {
        // Exemple : Modifier le texte de la première popup dès le début
        popupManager.SetPopupText(0, "Bienvenue dans le jeu avec TextMesh Pro!");

        // Exemple : Modifier le texte de la deuxième popup après 3 secondes
        Invoke("ChangePopupText", 3f);
    }

    void ChangePopupText()
    {
        // Changer le texte de la deuxième popup
        popupManager.SetPopupText(1, "Vous avez commencé votre aventure avec TMP!");
    }
}
