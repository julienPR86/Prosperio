using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoToADirection : MonoBehaviour
{
    public Transform Path1;
    public float speed = 5f;
    private Vector3 startPosition;
    public GameObject ManagementScripts;
    private GridManager gridManager;

    private void Start()
    {
        gridManager = ManagementScripts.GetComponent<GridManager>();
    }
    void Update()
    {
        GoHere(gridManager.cells[5,5].WorldPosition);
    }

    public void GoHere(Vector3 location)
    {
        startPosition = transform.position;
        Vector3 direction = (transform.position + location).normalized;

        transform.position += direction * speed * Time.deltaTime;

        if (transform.position == location) 
        {
            Debug.Log("Arrived");
        }
    }
}
