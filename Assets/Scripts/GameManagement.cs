using UnityEngine;

public class GameManagement : MonoBehaviour
{

    private ManageClock clock;
    private int worktime = 360;
    private int sleeptime = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clock = GetComponent<ManageClock>();
    }

    // Update is called once per frame
    void Update()
    {
        if (clock.GetTime() == worktime) 
        {
            Debug.Log("It's time to work!");
        }
        if (clock.GetTime() == sleeptime)
        {
            Debug.Log("It's time to sleep");
        }
    }
}
