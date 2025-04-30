using System.Collections.Generic;
using UnityEngine;

public class ResPoint : MonoBehaviour
{
    public ResPoint instance;
    public List<GameObject> antsInRestPoint;

    [SerializeField] float lastTime;
    [SerializeField] float energyBoostPerHour = 30f;

    void RegenerateEnergy(float currentTime) {
        foreach(GameObject ant in antsInRestPoint)
        {
            Ant script = ant.GetComponent<Ant>();
            float deltaHours = currentTime - lastTime;
            if (deltaHours < 0) deltaHours += 24f; // przeskok przez pó³noc
            Debug.Log("Regeneruje energie");
            script.desires[Ant.desire.energy] -= energyBoostPerHour * deltaHours;
            lastTime = currentTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ant"))
        {
            if (!antsInRestPoint.Contains(other.gameObject))
            {
                antsInRestPoint.Add(other.gameObject);
            }
            AntEnterTrigger();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (antsInRestPoint.Contains(other.gameObject))
        {
            antsInRestPoint.Remove(other.gameObject);
        }
        AntEnterTrigger();
    }
    private void AntEnterTrigger()
    {
        if(antsInRestPoint.Count > 0)
        {
            TimeManager.instance.OnTimeChanged += RegenerateEnergy;
        }
        else
        {
            TimeManager.instance.OnTimeChanged -= RegenerateEnergy;
        }
    }
}
