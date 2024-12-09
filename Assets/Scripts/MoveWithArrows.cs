using UnityEngine;

public class MoveWithArrows : MonoBehaviour
{
    public float speed = 200f; // Vitesse de d�placement

    private RectTransform rectTransform;

    void Start()
    {
        // R�cup�re le RectTransform de l'�l�ment d'UI
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // R�cup�re les entr�es des fl�ches directionnelles
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Cr�e un vecteur de d�placement
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f);

        // Applique le d�placement au RectTransform
        rectTransform.anchoredPosition += (Vector2)(movement * speed * Time.deltaTime);
    }
}
