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
    private Dictionary<GameObject, Person> personDictionary = new Dictionary<GameObject, Person>();
    void Start()
    {
        gridManager = GetComponent<GridManager>();
        clock = GetComponent<ManageClock>();

        // Beginning: 2 wanderers + 1 harvester + 1 lumberjack + 1 digger + 1 mason
        CreateGameObject(Job.Wanderer);
        CreateGameObject(Job.Wanderer);
        CreateGameObject(Job.Harvester);
        CreateGameObject(Job.Lumberjack);
        CreateGameObject(Job.Digger);
        CreateGameObject(Job.Mason);
    }

    private void Update()
    {
        if (clock.GetTime() == 360 && !isWorking)
        {
            GoToWork(Job.Harvester);
            GoToWork(Job.Lumberjack);
            GoToWork(Job.Digger);
            StartWalkingAround();
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
        Person person = null;

        switch (job) 
        {
            case Job.Harvester:
                spriteRenderer.color = new Color(9f / 255f, 166f / 255f, 3f / 255f);
                personobject.transform.SetParent(stock.transform.Find("Harvesters"));
                person = new Person(Job.Harvester);
                break;
            case Job.Wanderer:
                spriteRenderer.color = new Color(139f / 255f, 140f / 255f, 139f / 255f);
                personobject.transform.SetParent(stock.transform.Find("Wanderers"));
                person = new Person();
                break;
            case Job.Lumberjack:
                spriteRenderer.color = new Color(133f / 255f, 75f / 255f, 36f / 255f);
                personobject.transform.SetParent(stock.transform.Find("Lumberjacks"));
                person = new Person(Job.Lumberjack);
                break;
            case Job.Digger:
                spriteRenderer.color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                personobject.transform.SetParent(stock.transform.Find("Diggers"));
                person = new Person(Job.Digger);
                break;
            case Job.Mason:
                spriteRenderer.color = new Color(252f / 255f, 130f / 255f, 0f / 255f);
                personobject.transform.SetParent(stock.transform.Find("Masons"));
                person = new Person(Job.Mason);
                break;
        }

        personDictionary.Add(personobject, person);
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
        float speed = 3f;
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

    private void StartWalkingAround()
    {
        Transform wanderersFolder = stock.transform.Find("Wanderers").transform;
        for (int i = 0; i < wanderersFolder.childCount; i++)
        {
            GameObject wanderer = wanderersFolder.GetChild(i).gameObject;
            StartCoroutine(WalkRandomly(wanderer));
        }
    }

    private IEnumerator WalkRandomly(GameObject wanderer) // Coroutine to make the wanderers walking around
    {
        while (true)
        {
            // Randomly pick a location
            int randomX = Random.Range(0, gridManager.width);
            int randomY = Random.Range(0, gridManager.height);

            Vector3 randomLocation = gridManager.cells[randomX, randomY].WorldPosition;

            // Move the wanderer to the new random location
            yield return MoveToTarget(wanderer, randomLocation, 3f); // Adjust speed as needed

            float waitTime = Random.Range(1f, 3f); // Repeat after waiting X seconds
            yield return new WaitForSeconds(waitTime);
        }
    }


    public void GoToSleep()
    {
        // function called at the end of the day to make people go to sleep
    }


}
