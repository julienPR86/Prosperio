using System;
using UnityEditor;
using UnityEngine;

public class Person
{
    public int age { get; set; }
    public Job job {  get; set; }
    public bool isTired { get; set; }
    public bool atSchool { get; set; }

    public GameObject unitpanel;

    public Person(Job job = Job.Wanderer, int age = 1, bool isTired = false)
    {
        this.age = age;
        this.job = job;
        this.isTired = isTired;
        this.atSchool = false;
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
