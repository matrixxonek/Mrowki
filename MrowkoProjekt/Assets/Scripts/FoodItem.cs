using UnityEngine;
using System;

public class FoodItem : MonoBehaviour
{
    public Action OnCollected;
    [SerializeField] float energyRegeneration;
    [SerializeField] float foodRegeneration;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ant"))
        {
            OnCollected?.Invoke();
            ApplyEffect(other);
            Destroy(gameObject);
        }
    }
    void ApplyEffect(Collider2D antCollider)
    {
        Ant script = antCollider.gameObject.GetComponent<Ant>();
        Debug.Log("Jedzenie i Energia zregenerowane");
        script.desires[Ant.desire.energy] += energyRegeneration;
        script.desires[Ant.desire.food] += foodRegeneration;
        if(!script.CanApply(script.desires[Ant.desire.energy]))
            script.desires[Ant.desire.energy] = script.maxDesire;
        if (!script.CanApply(script.desires[Ant.desire.food]))
            script.desires[Ant.desire.food] = script.maxDesire;
    }
}