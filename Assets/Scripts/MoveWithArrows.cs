using UnityEngine;

public class MoveWithArrows : MonoBehaviour
{
    public float speed = 200f; // Vitesse de déplacement

    private RectTransform rectTransform;

    void Start()
    {
        // Récupère le RectTransform de l'élément d'UI
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Récupère les entrées des flèches directionnelles
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Crée un vecteur de déplacement
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f);

        // Applique le déplacement au RectTransform
        rectTransform.anchoredPosition += (Vector2)(movement * speed * Time.deltaTime);
    }
}
