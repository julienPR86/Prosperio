using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoToADirection : MonoBehaviour
{
    public Transform Path1;
    public float speed = 5f;
    private Vector3 startPosition;

    void Update()
    {
        GoHere(Path1.transform);
    }

    public void GoHere(Transform location)
    {
        startPosition = transform.position;
        Vector3 direction = (location.position - transform.position).normalized;

        transform.position += direction * speed * Time.deltaTime;

        if (transform.position == location.position) 
        {
            Debug.Log("Arrived");
        }
    }
}
