using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Person;

public class Population : MonoBehaviour
{
    public Sprite circleSprite;
    public GameObject stock;
    private GridManager gridManager;
    private ManageClock clock;
    private bool isWorking = false;
    void Start()
    {
        gridManager = GetComponent<GridManager>();
        clock = GetComponent<ManageClock>();
        Person wanderer = new Person();
        Person harvester = new Person(Job.Harvester);
        Person lumberjack = new Person(Job.Lumberjack);
        Person digger = new Person(Job.Digger);
        Person mason = new Person(Job.Mason);
    }

    private void Update()
    {
        if (clock.GetTime() == 360 && !isWorking)
        {
            GoToWork(Job.Harvester);
            GoToWork(Job.Lumberjack);
            GoToWork(Job.Digger);
            isWorking = true;
        }
        if (clock.GetTime() == 0 && isWorking) 
        {
            // go to sleep
        }
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

        switch (job) 
        {
            case Job.Harvester:
                spriteRenderer.color = new Color(9f / 255f, 166f / 255f, 3f / 255f);
                personobject.transform.SetParent(stock.transform.Find("Harvesters"));
                break;
            case Job.Wanderer:
                spriteRenderer.color = new Color(139f / 255f, 140f / 255f, 139f / 255f);
                personobject.transform.SetParent(stock.transform.Find("Wanderers"));
                break;
            case Job.Lumberjack:
                spriteRenderer.color = new Color(133f / 255f, 75f / 255f, 36f / 255f);
                personobject.transform.SetParent(stock.transform.Find("Lumberjacks"));
                break;
            case Job.Digger:
                spriteRenderer.color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                personobject.transform.SetParent(stock.transform.Find("Diggers"));
                break;
            case Job.Mason:
                spriteRenderer.color = new Color(252f / 255f, 130f / 255f, 0f / 255f);
                personobject.transform.SetParent(stock.transform.Find("Masons"));
                break;
        }
    }

    private void GoToWork(Job job)
    {

        switch (job)
        {
            case Job.Harvester:
                Transform harvestersfolder = stock.transform.Find("Harvesters").transform;
                for (int i = 0; i < harvestersfolder.childCount; i++)
                {
                    // Random location
                    int randomIndex = Random.Range(0, gridManager.berriescells.Count);

                    Vector3 randomLocation = gridManager.berriescells[randomIndex].WorldPosition;

                    // Move the harvester to the target chosen
                    GoHere(harvestersfolder.GetChild(i).gameObject, randomLocation);
                }
                break;
            case Job.Lumberjack:
                Transform lumberjacksfolder = stock.transform.Find("Lumberjacks").transform;
                for (int i = 0; i < lumberjacksfolder.childCount; i++)
                {
                    // Random location
                    int randomIndex = Random.Range(0, gridManager.forestcells.Count);

                    Vector3 randomLocation = gridManager.forestcells[randomIndex].WorldPosition;

                    // Move the lumberjack to the target chosen
                    GoHere(lumberjacksfolder.GetChild(i).gameObject, randomLocation);
                }
                break;
            case Job.Digger:
                Transform diggerfolder = stock.transform.Find("Diggers").transform;
                for (int i = 0; i < diggerfolder.childCount; i++)
                {
                    // Random location
                    int randomIndex = Random.Range(0, gridManager.stonecells.Count);

                    Vector3 randomLocation = gridManager.stonecells[randomIndex].WorldPosition;

                    // Move the digger to the target chosen
                    GoHere(diggerfolder.GetChild(i).gameObject, randomLocation);
                }
                break;
        }
    }

    public void GoHere(GameObject o, Vector3 location)
    {
        float speed = 5f;
        StartCoroutine(MoveToTarget(o, location, speed));
    }

    private IEnumerator MoveToTarget(GameObject o, Vector3 location, float speed)
    {
        while (Vector3.Distance(o.transform.position, location) > 0.1f) // While still not having reached the destination
        {
            // Calculate the direction to the target location
            Vector3 direction = (location - o.transform.position).normalized;

            // Move the object towards the target
            o.transform.position += direction * speed * Time.deltaTime;

            yield return null;
        }

        // If object is close, setting it at the location
        o.transform.position = location;
    }




}
