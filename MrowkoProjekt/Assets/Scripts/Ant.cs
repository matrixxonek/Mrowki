using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ant : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float moveSpeed;
    bool readyToMove = true;
    public desire primaryDesiry;
    [SerializeField] public Dictionary<desire, float> desires = new Dictionary<desire, float>();
    [SerializeField] float lastTime;
    [SerializeField] float energyDropPerHour = 10f;
    [SerializeField] float foodDropPerHour = 10f;
    [SerializeField] public float maxDesire;
    Vector2 currentDestination;
    bool hasDestination = false;
    void Start()
    {
        desires[desire.energy] = maxDesire;
        desires[desire.food] = maxDesire;
        primaryDesiry = desire.none;
        lastTime = TimeManager.instance.currentTime;
        TimeManager.instance.OnTimeChanged += StatsUpdate;
    }

    private void OnDisable()
    {
        TimeManager.instance.OnTimeChanged -= StatsUpdate;
    }

    void Update()
    {
        primaryDesiry = CheckPrimaryDesiry();
        Debug.Log("Energia: " + desires[desire.energy]);
        Debug.Log("Jedzenie: " + desires[desire.food]);
    }

    private void FixedUpdate()
    {
        if (readyToMove)
        {
            Move(primaryDesiry);
        }
    }

    private desire CheckPrimaryDesiry()
    {
        desire minDesiry = desires.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        if (desires[minDesiry] < 30)
            return minDesiry;
        return desire.none;
    }

    Vector2 GenerateMovementPoint()
    {
        System.Random rnd = new System.Random();
        int xPosition, yPosition;
        xPosition = rnd.Next(-20,21);
        yPosition = rnd.Next(-10,11);
        return new Vector2(xPosition, yPosition);
    }

    void Move(desire primaryDesiry)
    {
        switch (primaryDesiry) {
            case desire.none:
                if (!hasDestination)
                {
                    currentDestination = GenerateMovementPoint();
                    hasDestination = true;
                }
                BaseMovement(currentDestination);
                break;
            case desire.food:
                FindFood();
                break;
            case desire.energy:
                GoRest();
                break;
            default:
                Console.Error.WriteLine("No movement type found");
                break;
        }
    }

    void StatsUpdate(float currentTime)
    {
        float deltaHours = currentTime - lastTime;
        if (deltaHours < 0) deltaHours += 24f; // przeskok przez pó³noc 
        if(desires[desire.food] > 0)
            desires[desire.food] -= foodDropPerHour * deltaHours;
        if (desires[desire.energy] > 0)
            desires[desire.energy] -= energyDropPerHour * deltaHours;
        lastTime = currentTime;
    }
    public bool CanApply(float desireValue)
    {
        return desireValue < maxDesire;
    }
    void BaseMovement(Vector2 destination)
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.fixedDeltaTime);

        if (Vector2.Distance(transform.position, destination) < 0.1f)
        {
            hasDestination = false; // gdy dotarliœmy, losujemy nowy
        }
    }

    void FindFood()
    {

    }

    void GoRest()
    {

    }
    public enum desire
    {
        none,
        food,
        energy
    }
}
