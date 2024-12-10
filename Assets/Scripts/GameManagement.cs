using System.Collections.Generic;
using System.Resources;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;
using static Person;

public class GameManagement : MonoBehaviour
{
    // Scripts needed
    private ManageClock clock;
    private Population population;
    private ResourcesManager resourcesManager;
    private BuildingSystem buildingSystem;
    private PopupManager popupManagerText;
    private GridManager gridManager;

    // GameObjects needed
    public GameObject popupManager; // that contains PopupManager Script

    // Times (in minutes) when they work/sleep.
    private int workTime = 360;
    private int sleepTime = 0;


    private int daysPassed = 0; // Useful to spawn 2 wanderers every two days

    // Some booleans to avoid functions in update being called multiple times
    private bool isWorking = false;

    // UI elements needed
    public Button pauseButton;
    public Slider prosperitySlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clock = GetComponent<ManageClock>();
        resourcesManager = GetComponent<ResourcesManager>();
        population = GetComponent<Population>();
        popupManagerText = popupManager.GetComponent<PopupManager>();
        buildingSystem = GetComponent<BuildingSystem>();
        gridManager = GetComponent<GridManager>();

        // Beginning: 2 wanderers + 1 harvester + 1 lumberjack + 1 digger + 1 mason
        population.CreateGameObject(Job.Wanderer);
        population.CreateGameObject(Job.Wanderer);
        population.CreateGameObject(Job.Harvester);
        population.CreateGameObject(Job.Lumberjack);
        population.CreateGameObject(Job.Digger);
        population.CreateGameObject(Job.Mason);

        // Pause button linked to PauseGame Method.
        pauseButton.onClick.AddListener(population.PauseGame);

        prosperitySlider.value = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (clock.GetTime() == workTime && !isWorking)
        {
            //Update buildings in progress list
            if (buildingSystem != null)
            {
                UpdatePersonAtSchool();
                buildingSystem.UpdateBuildingsInProgress();
            }

            popupManagerText.SetPopupText(2, "Workers go to work!");
            population.ResetDeadNumber();

            isWorking = true;
            population.isWorking = isWorking;

            population.StopAllCoroutines();

            // Every worker goes to work:
            population.GoToWork(Job.Harvester);
            population.GoToWork(Job.Lumberjack);
            population.GoToWork(Job.Digger);
            // Wanderers and Tired people just walk around:
            population.StartWalkingAround();
        }
        if (clock.GetTime() == sleepTime && isWorking)
        {
            popupManagerText.SetPopupText(2, "Workers go to sleep!");

            isWorking = false;
            population.isWorking = isWorking;

            population.StopAllCoroutines();

            daysPassed++;
            if (daysPassed == 2) // Spawn of wanderers every 2 days.
            {
                daysPassed = 0;
                SpawnWanderers();
                popupManagerText.SetPopupText(1, "2 Wanderers appeared!");
            }

            population.GoToHouse(); // People move to houses: if they do not find, they become tired
            population.EatingTime(); // Consume food
            population.AddAge(); // Add +1 age to every person
            population.DyingBecauseOfAge(); // Check if people reached 25+ age = they die

            UpdateProsperity();
        }
    }

    private void SpawnWanderers()
    {
        population.CreateGameObject(Job.Wanderer);
        population.CreateGameObject(Job.Wanderer);
    }

    private void UpdateProsperity()
    {
        int museumCount = 0;
        int libraryCount = 0;

        foreach (Cell cell in gridManager.cells)
        {
            if (cell.buildingInCell == Cell.BuildingType.Library)
            {
                libraryCount++;
            }
            if (cell.buildingInCell == Cell.BuildingType.Museum)
            {
                museumCount++;
            }
        }

        foreach (KeyValuePair<GameObject, Person> person in population.personDictionary)
        {
            if (!person.Value.isTired)
            {
                prosperitySlider.value += 1;
            }
            else
            {
                prosperitySlider.value -= 1;
            }
        }

        prosperitySlider.value -= population.GetNumberOfDeadPeople(); // Remove one per death
        prosperitySlider.value += 2 * libraryCount;
        prosperitySlider.value += 3 * museumCount;
    }

    //Method that apply(update) job to person for each element of dictonary of person at school
    private void UpdatePersonAtSchool()
    {
        //Check to update each element
        foreach (KeyValuePair<Person, Job> element in population.personAtSchool)
        {
            element.Key.job = element.Value;
            population.personAtSchool.Remove(element.Key);
            element.Key.atSchool = false;
        }
    }
}

