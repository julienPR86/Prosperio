using System;
using UnityEngine;

public class Person
{
    private int age { get; set; }
    private Job job {  get; set; }
    private bool isTired { get; set; }

    public Person(int age = 1, Job job = Job.Wanderer, bool isTired = false)
    {
        this.age = age;
        this.job = job;
        this.isTired = isTired;
    }

    public enum Job
    {
        Wanderer,
        Harvester,
        Lumberjack,
        Digger,
        Mason
    }
}
