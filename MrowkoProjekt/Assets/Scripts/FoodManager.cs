using System;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    GameObject[] foodObjects;
    float[] respawnTimes;
    [SerializeField] GameObject foodPrefab;
    [SerializeField] int numberOfFood;
    [SerializeField] float respawnDelayInHours;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foodObjects = new GameObject[numberOfFood];
        respawnTimes = new float[numberOfFood];
        for (int i = 0; i < numberOfFood; i++)
            respawnTimes[i] = -1f;
        for (int i = 0; i < numberOfFood; i++)
        {
            //foodObjects[i] = ObjectSpawner.instance.SpawnObject(foodPrefab);
            RespownOnIndex(i);
        }
        TimeManager.instance.OnTimeChanged += OnTimeChanged;
    }
    void OnTimeChanged(float currentTime)
    {
        Debug.Log("OnTimeChanged");
        for (int i = 0; i < numberOfFood; i++)
        {
            if (foodObjects[i] == null && ShouldRespawn(respawnTimes[i], currentTime))
            {
                RespownOnIndex(i);
            }
        }
    }
    void RespownOnIndex(int i)
    {
        foodObjects[i] = ObjectSpawner.instance.SpawnObject(foodPrefab);
        FoodItem foodItem = foodObjects[i].GetComponent<FoodItem>();
        foodItem.OnCollected = () =>
        {
            Debug.Log("Wywoluje respawn on index - wazne");
            foodObjects[i] = null;
            respawnTimes[i] = TimeManager.instance.currentTime + respawnDelayInHours;
            if (respawnTimes[i] > 24)
                respawnTimes[i] -= 24;
        };
        //respawnTimes[i] = -1;
    }
    private bool ShouldRespawn(float targetTime, float currentTime)
    {
        //if (targetTime < 0f) return false;
        return Mathf.Abs(currentTime - targetTime) < 0.01f || (currentTime > targetTime);
    }
    private void OnDestroy()
    {
        if(TimeManager.instance != null)
            TimeManager.instance.OnTimeChanged -= OnTimeChanged;
    }
}
