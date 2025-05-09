using UnityEngine;

public class ResPoint1 : AreaEffectPoint
{
    [SerializeField] float energyBoostPerHour = 30f;

    protected override bool IsValidObject(GameObject obj)
    {
        return obj.CompareTag("Ant");
    }

    protected override void ApplyEffect(GameObject obj, float deltaHours) //efekt dla res pointu
    {
        Ant script = obj.GetComponent<Ant>();
        Debug.Log("Regeneruje energie");
        script.desires[Ant.desire.energy] += energyBoostPerHour * deltaHours;
    }
}
