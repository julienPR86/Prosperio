using UnityEngine;

public class SetPopUpMessage : MonoBehaviour
{
    // R�f�rence au PopupManager
    public PopupManager popupManager;

    void Start()
    {
        // Exemple : Modifier le texte de la premi�re popup d�s le d�but
        popupManager.SetPopupText(0, "Bienvenue dans le jeu avec TextMesh Pro!");

        // Exemple : Modifier le texte de la deuxi�me popup apr�s 3 secondes
        Invoke("ChangePopupText", 3f);
    }

    void ChangePopupText()
    {
        // Changer le texte de la deuxi�me popup
        popupManager.SetPopupText(1, "Vous avez commenc� votre aventure avec TMP!");
    }
}
