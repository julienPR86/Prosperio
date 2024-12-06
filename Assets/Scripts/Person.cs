using System;
using UnityEditor;
using UnityEngine;

public class Person
{
    private int age { get; set; }
    public Job job {  get; set; }
    private bool isTired { get; set; }

    private GameObject personobject;

    public Person(Job job = Job.Wanderer, int age = 1, bool isTired = false)
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
