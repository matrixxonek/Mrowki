using System.Collections.Generic;
using UnityEngine;

public abstract class AreaEffectPoint : MonoBehaviour
{
    protected List<GameObject> objectsInArea = new List<GameObject>(); //Objekty wewn¹trz punktu
    protected Dictionary<GameObject, float> lastTimePerObject = new Dictionary<GameObject, float>(); //przechowywanie momentu wywo³ania funkcji zmiany czasu dla ka¿dej z mrówek w zasiêgu

    private void OnTriggerEnter2D(Collider2D other) //Funkcja wywo³ywana po wejœciu obiektu o colliderze na konkretnym layerze w zasiêg
    {
        if (IsValidObject(other.gameObject)) // sprawdzanie czy obiekt jest faktycznie obiektem na którego chcemy nak³adaæ jakieœ zmiany, np jakbyœmy dodali inne obiekty typu mrówkojad to ¿eby nie 
        {                                      //dostawa³y tezz zmian
            if (!objectsInArea.Contains(other.gameObject))
            {
                objectsInArea.Add(other.gameObject); // dodawanie mrowek
                lastTimePerObject[other.gameObject] = TimeManager.instance.currentTime; //ustawienie ostatniego wywo³ania funkcji czasu
            }
            OnObjectEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D other) //usuwanie mrówków
    {
        if (objectsInArea.Contains(other.gameObject))
        {
            objectsInArea.Remove(other.gameObject);
            lastTimePerObject.Remove(other.gameObject);
        }
        OnObjectEnter();
    }

    private void OnObjectEnter() // funkcja sprawdza czy jest przynajmniej jedna mrówka w zasiêgu, ¿eby nie wywo³ywaæ kodu bez sensu, gdy i tak nic nie zrobi
    {
        if (objectsInArea.Count > 0)
        {
            TimeManager.instance.OnTimeChanged += OnTimeChanged; // dodawanie eventu do zmiany czasu
        }
        else
        {
            TimeManager.instance.OnTimeChanged -= OnTimeChanged; // usuwanie
        }
    }

    private void OnTimeChanged(float currentTime) // funkcja dzia³aj¹ca w czasie
    {
        foreach (GameObject obj in objectsInArea)
        {
            float lastTime = lastTimePerObject[obj];
            float deltaHours = currentTime - lastTime; // ile czasu up³ynê³o do ostatniego wywo³ania funkcji czasu
            if (deltaHours < 0) deltaHours += 24f;

            ApplyEffect(obj, deltaHours); // funkcja dodaj¹ca ró¿ne wartosci zale¿nie od punktu

            lastTimePerObject[obj] = currentTime;
        }
    }

    protected abstract bool IsValidObject(GameObject obj);
    protected abstract void ApplyEffect(GameObject obj, float deltaHours); // nadpisywana dla ka¿dego skryptu - nowe efekty
}