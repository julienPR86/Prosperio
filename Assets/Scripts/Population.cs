using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Person;

public class Population : MonoBehaviour
{
    // Scripts needed
    private GridManager gridManager;
    private ManageClock clock;
    private ResourcesManager resourcesManager;
    public GameObject PopupManager; // Contains PopupManager script
    private PopupManager popupManagerText;

    // UI elements + gameobjects needed
    public Sprite circleSprite;
    public GameObject populationFolder; // Contains empty objects acting like folders to stock Wanderers, Lumberjacks etc gameobjects
    public GameObject unitPanelPrefab;

    public TextMeshProUGUI ageText;
    public TextMeshProUGUI jobText;
    public TextMeshProUGUI isTiredText;
    private Person Person2;
    // Link ALL the gameObjects (key) to an instance of Person class (Value)
    public Dictionary<GameObject, Person> personDictionary = new Dictionary<GameObject, Person>();

    // To update number of workers per category in the UI
    public TextMeshProUGUI harvesterText;
    public TextMeshProUGUI lumberjackText;
    public TextMeshProUGUI diggerText;
    public TextMeshProUGUI wandererText;
    public TextMeshProUGUI masonText;

    public bool isWorking = false;
    public bool isTimerStopped = false;
    private int numberOfDeadToday = 0;

    void Start()
    {
        gridManager = GetComponent<GridManager>();
        clock = GetComponent<ManageClock>();
        resourcesManager = GetComponent<ResourcesManager>();
        popupManagerText = PopupManager.GetComponent<PopupManager>();
    }

    public void CreateGameObject(Job job)
    {
        GameObject toggleGroupObject = GameObject.Find("Population");
        GameObject personObject = new GameObject();
        personObject.transform.position = new Vector3(5, 5);
        personObject.transform.localScale = new Vector3(0.3f, 0.3f);
        SpriteRenderer spriteRenderer = personObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = circleSprite;
        Toggle toggle = personObject.AddComponent<Toggle>();
        Image toggleImage = personObject.AddComponent<Image>();
        Person person = null;

        GameObject unitPanel = Instantiate(unitPanelPrefab, personObject.transform);
        unitPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(6f, 0f);
        unitPanel.GetComponent<RectTransform>().localScale = new Vector3(0.005f, 0.005f, 0.005f);
        unitPanel.SetActive(true);

        // Update the job, age, and fatigue texts
        ageText = unitPanel.transform.Find("AgeText").GetComponent<TextMeshProUGUI>();
        jobText = unitPanel.transform.Find("JobText").GetComponent<TextMeshProUGUI>();
        isTiredText = unitPanel.transform.Find("IsTiredText").GetComponent<TextMeshProUGUI>();

        // Toggle navigation to none
        toggleImage.sprite = circleSprite;
        toggle.navigation = new Navigation { mode = Navigation.Mode.None };
        toggle.image = toggleImage;
        toggle.group = toggleGroupObject.GetComponent<ToggleGroup>();
        toggle.onValueChanged.AddListener(isOn => OnToggleValueChanged(unitPanel, isOn));

        // Assigning the job and updating texts
        switch (job)
        {
            case Job.Harvester:
                spriteRenderer.color = new Color(9f / 255f, 166f / 255f, 3f / 255f);
                personObject.transform.SetParent(populationFolder.transform.Find("Harvesters"));
                person = new Person(Job.Harvester);
                break;
            case Job.Wanderer:
                spriteRenderer.color = new Color(139f / 255f, 140f / 255f, 139f / 255f);
                personObject.transform.SetParent(populationFolder.transform.Find("Wanderers"));
                person = new Person();
                break;
            case Job.Lumberjack:
                spriteRenderer.color = new Color(133f / 255f, 75f / 255f, 36f / 255f);
                personObject.transform.SetParent(populationFolder.transform.Find("Lumberjacks"));
                person = new Person(Job.Lumberjack);
                break;
            case Job.Digger:
                spriteRenderer.color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                personObject.transform.SetParent(populationFolder.transform.Find("Diggers"));
                person = new Person(Job.Digger);
                break;
            case Job.Mason:
                spriteRenderer.color = new Color(252f / 255f, 130f / 255f, 0f / 255f);
                personObject.transform.SetParent(populationFolder.transform.Find("Masons"));
                person = new Person(Job.Mason);
                break;
        }

        // Update UI text elements
        ageText.text = "Age: " + person.age;
        jobText.text = "Job: " + person.job.ToString();
        isTiredText.text = person.isTired ? "Tired" : "Not Tired";

        personDictionary.Add(personObject, person);
        UpdateText();
    }


    private void OnToggleValueChanged(GameObject unitPanel, bool isOn)
    {
        if (unitPanel != null)
        {
            unitPanel.SetActive(isOn);
        }
        else
        {
            Debug.LogWarning("Panel not assigned, impossible to manipulate");
        }
    }

    public void GoToWork(Job job)
    {
        switch (job)
        {
            case Job.Harvester:
                Transform harvestersFolder = populationFolder.transform.Find("Harvesters").transform;
                for (int i = 0; i < harvestersFolder.childCount; i++)
                {
                    if (!personDictionary[harvestersFolder.GetChild(i).gameObject].isTired)
                    {
                        // Random location
                        int randomIndex = Random.Range(0, gridManager.berriescells.Count);

                        Vector3 randomLocation = gridManager.berriescells[randomIndex].WorldPosition;

                        // Move the harvester to the target chosen
                        GoHere(harvestersFolder.GetChild(i).gameObject, randomLocation);
                    }
                }
                StartCoroutine(GatherResource(Job.Harvester, 3));
                break;
            case Job.Lumberjack:
                Transform lumberjacksFolder = populationFolder.transform.Find("Lumberjacks").transform;
                for (int i = 0; i < lumberjacksFolder.childCount; i++)
                {
                    if (!personDictionary[lumberjacksFolder.GetChild(i).gameObject].isTired)
                    {
                        // Random location
                        int randomIndex = Random.Range(0, gridManager.forestcells.Count);

                        Vector3 randomLocation = gridManager.forestcells[randomIndex].WorldPosition;

                        // Move the lumberjack to the target chosen
                        GoHere(lumberjacksFolder.GetChild(i).gameObject, randomLocation);
                    }
                }
                StartCoroutine(GatherResource(Job.Lumberjack, 4));
                break;
            case Job.Digger:
                Transform diggersFolder = populationFolder.transform.Find("Diggers").transform;
                for (int i = 0; i < diggersFolder.childCount; i++)
                {
                    if (!personDictionary[diggersFolder.GetChild(i).gameObject].isTired)
                    {
                        // Random location
                        int randomIndex = Random.Range(0, gridManager.stonecells.Count);

                        Vector3 randomLocation = gridManager.stonecells[randomIndex].WorldPosition;

                        // Move the digger to the target chosen
                        GoHere(diggersFolder.GetChild(i).gameObject, randomLocation);
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
        while (Vector3.Distance(o.transform.position, location) > 0.1f)
        {
            // Pause if the game is paused
            while (isTimerStopped)
            {
                yield return null; // Wait until the game is resumed
            }

            Vector3 direction = (location - o.transform.position).normalized;
            o.transform.position += direction * speed * Time.deltaTime;

            yield return null;
        }

        o.transform.position = location;
    }


    public void StartWalkingAround()
    {
        Transform wanderersFolder = populationFolder.transform.Find("Wanderers").transform;
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

    private IEnumerator WalkRandomly(GameObject person)
    {
        while (true)
        {
            while (isTimerStopped)
            {
                yield return null; // Wait until the game is resumed
            }

            int randomX = Random.Range(0, gridManager.width);
            int randomY = Random.Range(0, gridManager.height);

            Vector3 randomLocation = gridManager.cells[randomX, randomY].WorldPosition;

            yield return MoveToTarget(person, randomLocation, 3f);

            float waitTime = Random.Range(1f, 3f);
            yield return new WaitForSeconds(waitTime);
        }
    }


    private IEnumerator GatherResource(Job job, int interval)
    {
        float elapsedTime = 0f;
        float targetTime = clock.GetRealSecondsPerInGameHour() * interval;

        while (isWorking)
        {
            while (isTimerStopped)
            {
                yield return null; // Wait until the game is resumed
            }

            // Increment elapsed time based on real-time passage : This is to avoid that the timer of the Coroutine resets.
            elapsedTime += Time.deltaTime;

            // Check if the elapsed time has reached or exceeded the target time
            if (elapsedTime >= targetTime)
            {
                elapsedTime = 0f;

                switch (job)
                {
                    case Job.Harvester:
                        resourcesManager.AddFood(1 * GetActiveWorkers(Job.Harvester) * Mathf.Max(1, GetNumberOfFarms()));
                        break;
                    case Job.Lumberjack:
                        resourcesManager.AddWood(1 * GetActiveWorkers(Job.Lumberjack));
                        break;
                    case Job.Digger:
                        resourcesManager.AddStone(1 * GetActiveWorkers(Job.Digger));
                        break;
                }
            }

            yield return null; // Wait until the next frame
        }
    }

    private int GetNumberOfFarms() // Return the number of farms
    {
        int numberOfFarms = 0;
        foreach (Cell cell in gridManager.cells)
        {
            if (cell.buildingInCell == Cell.BuildingType.Farm)
            {
                numberOfFarms++;
            }
        }

        return numberOfFarms;
    }



    private int GetActiveWorkers(Job job)
    {
        int active = 0;

        switch (job)
        {
            case Job.Harvester:
                Transform harvestersFolder = populationFolder.transform.Find("Harvesters").transform;
                for (int i = 0; i < harvestersFolder.childCount; i++)
                {
                    if (!personDictionary[harvestersFolder.GetChild(i).gameObject].isTired)
                    {
                        active++;
                    }
                }
                break;
            case Job.Lumberjack:
                Transform lumberjacksFolder = populationFolder.transform.Find("Lumberjacks").transform;
                for (int i = 0; i < lumberjacksFolder.childCount; i++)
                {
                    if (!personDictionary[lumberjacksFolder.GetChild(i).gameObject].isTired)
                    {
                        active++;
                    }
                }
                break;
            case Job.Digger:
                Transform diggersFolder = populationFolder.transform.Find("Diggers").transform;
                for (int i = 0; i < diggersFolder.childCount; i++)
                {
                    if (!personDictionary[diggersFolder.GetChild(i).gameObject].isTired)
                    {
                        active++;
                    }
                }
                break;
        }

        return active;
    }

    public void GoToHouse()
    {
        int i = 0;
        List<Cell> houses = new List<Cell>();
        foreach (Cell cell in gridManager.cells)  // Storing all the cells that are House.
        {
            if (cell.buildingInCell == Cell.BuildingType.Home)
            {
                houses.Add(cell);
            }
        }

        Debug.Log("Build count: " + houses.Count);

        if (houses.Count == 0)
        {
            foreach (KeyValuePair<GameObject, Person> person in personDictionary)
            {
                if (person.Value.job != Job.Wanderer)
                {
                    person.Value.isTired = true;
                    popupManagerText.SetPopupText(0, "Couldn't sleep: House missing");
                }
            }
            return; // Exit early since there are no houses
        }

        foreach (KeyValuePair<GameObject, Person> person in personDictionary) // Placing every person to a house if this one is not full, else, the person will be tired
        {
            if (person.Value.job != Job.Wanderer)
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
                    popupManagerText.SetPopupText(0, "Couldn't sleep: House missing");
                }
            }
        }
    }

    public void EatingTime() // Consume food for each person, else they die.
    {
        List<GameObject> peopleToRemove = new List<GameObject>();
        foreach (KeyValuePair<GameObject, Person> person in personDictionary)
        {
            if (resourcesManager.GetFoodCount() > 0)
            {
                resourcesManager.CousumeFood(1);
            }
            else
            {
                peopleToRemove.Add(person.Key);
                Destroy(person.Key.gameObject);
            }
        }

        foreach (GameObject key in peopleToRemove)
        {
            personDictionary.Remove(key);
            numberOfDeadToday++;
        }

        if (peopleToRemove.Count > 0)
        {
            popupManagerText.SetPopupText(0, $"{peopleToRemove.Count} people died of hunger.");
        }

        UpdateText();
    }

    public void AddAge()
    {
        foreach (KeyValuePair<GameObject, Person> person in personDictionary)
        {
            person.Value.age++;
            var unitPanel = person.Key.transform.GetComponentInChildren<Transform>().Find("UnitPanelPrefab");
            if (unitPanel != null)
            {
                unitPanel.transform.GetComponent<TextMeshProUGUI>().text = $"Age : {person.Value.age++}";
            }
        }
    }

    public void DyingBecauseOfAge()
    {
        List<GameObject> peopleToRemove = new List<GameObject>();
        foreach (KeyValuePair<GameObject, Person> person in personDictionary)
        {
            if (person.Value.age >= 25)
            {
                peopleToRemove.Add(person.Key);
                Destroy(person.Key.gameObject);
            }
        }

        foreach (GameObject key in peopleToRemove)
        {
            personDictionary.Remove(key);
            numberOfDeadToday++;
        }

        if (peopleToRemove.Count > 0)
        {
            popupManagerText.SetPopupText(0, $"{peopleToRemove.Count} people died of age.");
        }
        UpdateText();
    }

    private void UpdateText()
    {
        harvesterText.text = populationFolder.transform.Find("Harvesters").transform.childCount.ToString();
        lumberjackText.text = populationFolder.transform.Find("Lumberjacks").transform.childCount.ToString();
        diggerText.text = populationFolder.transform.Find("Diggers").transform.childCount.ToString();
        masonText.text = populationFolder.transform.Find("Masons").transform.childCount.ToString();
        wandererText.text = populationFolder.transform.Find("Wanderers").transform.childCount.ToString();
    }

    public void PauseGame()
    {
        if (!isTimerStopped)
        {
            clock.PauseGameClock();
            isTimerStopped = true;
            popupManagerText.SetPopupText(0, "Time paused.");
        }
        else
        {
            clock.ResumeGameClock();
            isTimerStopped = false;
            popupManagerText.SetPopupText(0, "Time resumed.");
        }
    }

    public int GetNumberOfDeadPeople()
    {
        return numberOfDeadToday;
    }

    public void ResetDeadNumber()
    {
        numberOfDeadToday = 0;
    }
}
