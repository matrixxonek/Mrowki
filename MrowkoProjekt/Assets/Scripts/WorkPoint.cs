using UnityEngine;

public class WorkPoint : AreaEffectPoint
{
    [SerializeField] float energyLostPerHour = 30f;
    [SerializeField] float foodLostPerHour = 15f;

    protected override bool IsValidObject(GameObject obj)
    {
        return obj.CompareTag("Ant");
    }

    protected override void ApplyEffect(GameObject obj, float deltaHours) //efekt dla work pointu
    {
        Ant script = obj.GetComponent<Ant>();
        Debug.Log("Zabieram energiê");
        Debug.Log("Zabieram jedzenie");
        script.desires[Ant.desire.energy] -= energyLostPerHour * deltaHours;
        script.desires[Ant.desire.food] -= foodLostPerHour * deltaHours;
    }
}