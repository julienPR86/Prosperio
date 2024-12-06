using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Person;

public class Population : MonoBehaviour
{
    public Sprite circleSprite;
    
    void Start()
    {
        Person person = new Person();
        SpawnPerson(person);
        Person person2 = new Person(Job.Harvester);
        SpawnPerson(person2);
    }

    private void SpawnPerson(Person persontospawn)
    {
        CreateGameObject(persontospawn.job);
    }

    private void CreateGameObject(Job job)
    {
        GameObject personobject = new GameObject();
        personobject.transform.position = new Vector3(5, 5);
        personobject.transform.localScale = new Vector3(0.3f, 0.3f);
        SpriteRenderer spriteRenderer = personobject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = circleSprite;
        
    }
}
