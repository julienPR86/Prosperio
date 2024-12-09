using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    // Liste des CanvasGroups pour les popups
    public CanvasGroup[] popups;

    // Dur�e d'affichage de chaque popup
    public float displayDuration = 5f;

    // Dur�e de disparition progressive
    public float fadeDuration = 1f;

    private int currentIndex = 0;
    private bool isRunning = false;

    void Start()
    {
        // Assure que toutes les popups sont d�sactiv�es et invisibles au d�part
        foreach (var popup in popups)
        {
            popup.gameObject.SetActive(false);
            popup.alpha = 0f;
        }
    }

    void Update()
    {
        // D�tecte l'appui sur la barre d'espace pour d�marrer ou continuer le cycle
        if (Input.GetKeyDown(KeyCode.Space) && !isRunning)
        {
            StartCoroutine(ShowPopup());
        }
    }

    IEnumerator ShowPopup()
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
}
