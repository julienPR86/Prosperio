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
    private ResourcesManager resourcesManager;
    private int DaysPassed = 0;
    private Dictionary<GameObject, Person> personDictionary = new Dictionary<GameObject, Person>();
    void Start()
    {
        gridManager = GetComponent<GridManager>();
        clock = GetComponent<ManageClock>();
        resourcesManager = GetComponent<ResourcesManager>();

        // Beginning: 2 wanderers + 1 harvester + 1 lumberjack + 1 digger + 1 mason
        CreateGameObject(Job.Wanderer);
        CreateGameObject(Job.Wanderer);
        CreateGameObject(Job.Harvester);
        CreateGameObject(Job.Harvester);
        CreateGameObject(Job.Lumberjack);
        CreateGameObject(Job.Digger);
        CreateGameObject(Job.Mason);
    }

    private void Update()
    {
        if (clock.GetTime() == 360 && !isWorking)
        {
            isWorking = true;
            StopAllCoroutines();
            GoToWork(Job.Harvester);
            GoToWork(Job.Lumberjack);
            GoToWork(Job.Digger);
            StartWalkingAround();
        }
        if (clock.GetTime() == 0 && isWorking)
        {
            isWorking = false;
            StopAllCoroutines();
            DaysPassed++;
            if (DaysPassed == 2) // Spawn of wanderers every 2 days.
            {
                DaysPassed = 0;
                CreateGameObject(Job.Wanderer);
                CreateGameObject(Job.Wanderer);
            }
            GoToHouse();
            EatingTime();
            DyingBecauseOfAge();
            AddAge();
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
                    if (!personDictionary[harvestersfolder.GetChild(i).gameObject].isTired)
                    {
                        // Random location
                        int randomIndex = Random.Range(0, gridManager.berriescells.Count);

                        Vector3 randomLocation = gridManager.berriescells[randomIndex].WorldPosition;

                        // Move the harvester to the target chosen
                        GoHere(harvestersfolder.GetChild(i).gameObject, randomLocation);
                    }
                }
                StartCoroutine(GatherResource(Job.Harvester, 3));
                break;
            case Job.Lumberjack:
                Transform lumberjacksfolder = stock.transform.Find("Lumberjacks").transform;
                for (int i = 0; i < lumberjacksfolder.childCount; i++)
                {
                    if (!personDictionary[lumberjacksfolder.GetChild(i).gameObject].isTired)
                    {
                        // Random location
                        int randomIndex = Random.Range(0, gridManager.forestcells.Count);

                        Vector3 randomLocation = gridManager.forestcells[randomIndex].WorldPosition;

                        // Move the lumberjack to the target chosen
                        GoHere(lumberjacksfolder.GetChild(i).gameObject, randomLocation);
                    }
                }
                StartCoroutine(GatherResource(Job.Lumberjack, 4));
                break;
            case Job.Digger:
                Transform diggerfolder = stock.transform.Find("Diggers").transform;
                for (int i = 0; i < diggerfolder.childCount; i++)
                {
                    if (!personDictionary[diggerfolder.GetChild(i).gameObject].isTired)
                    {
                        // Random location
                        int randomIndex = Random.Range(0, gridManager.stonecells.Count);

                        Vector3 randomLocation = gridManager.stonecells[randomIndex].WorldPosition;

                        // Move the digger to the target chosen
                        GoHere(diggerfolder.GetChild(i).gameObject, randomLocation);
                    }
                }
                StartCoroutine(GatherResource(Job.Digger, 5));
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

        foreach (KeyValuePair<GameObject, Person> person in personDictionary)
        {
            if (person.Value.isTired)
            {
                StartCoroutine(WalkRandomly(person.Key));
            }
        }
    }

    private IEnumerator WalkRandomly(GameObject person) // Coroutine to make the wanderers and tired people walking around
    {
        while (true)
        {
            // Randomly pick a location
            int randomX = Random.Range(0, gridManager.width);
            int randomY = Random.Range(0, gridManager.height);

            Vector3 randomLocation = gridManager.cells[randomX, randomY].WorldPosition;

            // Move the wanderer to the new random location
            yield return MoveToTarget(person, randomLocation, 3f); // Adjust speed as needed

            float waitTime = Random.Range(1f, 3f); // Repeat after waiting X seconds
            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator GatherResource(Job job, int interval)
    {
        while (isWorking) // Continue gathering resources while working
        {
            yield return new WaitForSeconds(clock.GetRealSecondsPerInGameHour() * interval);

            switch (job)
            {
                case Job.Harvester:
                    resourcesManager.AddFood(1 * GetActiveWorkers(Job.Harvester));
                    break;
                case Job.Lumberjack:
                    resourcesManager.AddWood(1 * GetActiveWorkers(Job.Lumberjack));
                    break;
                case Job.Digger:
                    resourcesManager.AddStone(1 * GetActiveWorkers(Job.Lumberjack));
                    break;
            }
        }
    }

    private int GetActiveWorkers(Job job)
    {
        int active = 0;

        switch (job)
        {
            case Job.Harvester:
                Transform harvestersfolder = stock.transform.Find("Harvesters").transform;
                for (int i = 0; i < harvestersfolder.childCount; i++)
                {
                    if (!personDictionary[harvestersfolder.GetChild(i).gameObject].isTired)
                    {
                        active++;
                    }
                }
                break;
            case Job.Lumberjack:
                Transform lumberjacksfolder = stock.transform.Find("Lumberjacks").transform;
                for (int i = 0; i < lumberjacksfolder.childCount; i++)
                {
                    if (!personDictionary[lumberjacksfolder.GetChild(i).gameObject].isTired)
                    {
                        active++;
                    }
                }
                break;
            case Job.Digger:
                Transform diggersfolder = stock.transform.Find("Diggers").transform;
                for (int i = 0; i < diggersfolder.childCount; i++)
                {
                    if (!personDictionary[diggersfolder.GetChild(i).gameObject].isTired)
                    {
                        active++;
                    }
                }
                break;
        }

        return active;
    }

    private void GoToHouse()
    {
        int i = 0;
        List<Cell> houses = new List<Cell>();
        foreach (Cell cell in gridManager.cells)  // Storing all the cells that are House.
        {
            if (cell.buildingInCell == Cell.BuildingType.School)
            {
                houses.Add(cell);
            }
        }

        foreach (KeyValuePair<GameObject, Person> person in personDictionary) // Placing every person to a house if this one is not full, else, the person will be tired
        {
            if (houses.Count > 0 || houses[i] != null)
            {
                if (!houses[i].isFull)
                {
                    houses[i].AddPeople();
                    person.Value.isTired = false;
                    GoHere(person.Key, houses[i].WorldPosition);
                }
                else
                {
                    i++;
                }
            }
            else
            {
                person.Value.isTired = true;
            }
        }
    }

    private void EatingTime() // Consume food for each person, else they die.
    {
        List<GameObject> keysToRemove = new List<GameObject>();
        foreach (KeyValuePair<GameObject, Person> person in personDictionary)
        {
            if (resourcesManager.GetFoodCount() > 0)
            {
                resourcesManager.CousumeFood(1);
            }
            else
            {
                keysToRemove.Add(person.Key);
                Destroy(person.Key.gameObject);
            }
        }

        foreach (GameObject key in keysToRemove)
        {
            personDictionary.Remove(key);
        }
    }

    private void AddAge()
    {
        foreach (KeyValuePair<GameObject, Person> person in personDictionary)
        {
            person.Value.age++;
        }
    }

    private void DyingBecauseOfAge()
    {
        List<GameObject> keysToRemove = new List<GameObject>();
        foreach (KeyValuePair<GameObject, Person> person in personDictionary)
        {
            if (person.Value.age >= 4)
            {
                keysToRemove.Add(person.Key);
                Destroy(person.Key.gameObject);
            }
        }

        foreach (GameObject key in keysToRemove)
        {
            personDictionary.Remove(key);
        }
    }
}