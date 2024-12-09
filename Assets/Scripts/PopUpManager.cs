using System.Collections;
using UnityEngine;
using TMPro;  // N'oublie pas d'ajouter cette ligne pour utiliser TextMesh Pro

public class PopupManager : MonoBehaviour
{
    // Liste des CanvasGroups pour les popups
    public CanvasGroup[] popups;

    // Liste des TMP_Text pour chaque popup (TextMesh Pro)
    public TMP_Text[] popupTexts;

    // Dur�e d'affichage de chaque popup
    public float displayDuration = 5f;

    // Dur�e de disparition progressive
    public float fadeDuration = 1f;

    private bool isRunning = false;

    void Start()
    {
        SetPopupText(0, "Bienvenue dans le jeu !");
        // Assure que toutes les popups sont d�sactiv�es et invisibles au d�part
        foreach (var popup in popups)
        {
            popup.gameObject.SetActive(false);
            popup.alpha = 0f;
        }
    }

    void Update()
    {
    }

    IEnumerator ShowPopup(int currentIndex)
    {
        isRunning = true;

        // Active et affiche la popup actuelle
        CanvasGroup currentPopup = popups[currentIndex];
        currentPopup.gameObject.SetActive(true);
        currentPopup.alpha = 1f;

        // Attend la dur�e d'affichage
        yield return new WaitForSeconds(displayDuration - fadeDuration);

        // Lance la disparition progressive
        yield return StartCoroutine(FadeOut(currentPopup));

        // D�sactive la popup apr�s le fade-out
        currentPopup.gameObject.SetActive(false);

        // Passe � la popup suivante, ou revient � la premi�re si on a fini le cycle
        currentIndex = (currentIndex + 1) % popups.Length;

        isRunning = false;
    }

    IEnumerator FadeOut(CanvasGroup popup)
    {
        float elapsed = 0f;
        float startAlpha = popup.alpha;

        while (elapsed < fadeDuration)
        {
            popup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        popup.alpha = 0f;
    }

    // Fonction publique pour modifier le texte de la popup actuelle avec TMP_Text
    public void SetPopupText(int index, string newText)
    {
        if (index >= 0 && index < popupTexts.Length)
        {
            popupTexts[index].text = newText;
            StartCoroutine(ShowPopup(index));
        }
    }
}
