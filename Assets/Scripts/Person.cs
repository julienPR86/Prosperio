using System;
using UnityEditor;
using UnityEngine;

public class Person
{
    public int age = 1; //{ get; set; }
    public Job job {  get; set; }
    public bool isTired { get; set; }
    public GameObject unitpanel;

    public Person(Job job = Job.Wanderer, bool isTired = false)
    {
        //this.age = age;
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
