using System.Collections.Generic;
using UnityEngine;

public abstract class AreaEffectPoint : MonoBehaviour
{
    protected List<GameObject> objectsInArea = new List<GameObject>(); //Objekty wewn�trz punktu
    protected Dictionary<GameObject, float> lastTimePerObject = new Dictionary<GameObject, float>(); //przechowywanie momentu wywo�ania funkcji zmiany czasu dla ka�dej z mr�wek w zasi�gu

    private void OnTriggerEnter2D(Collider2D other) //Funkcja wywo�ywana po wej�ciu obiektu o colliderze na konkretnym layerze w zasi�g
    {
        if (IsValidObject(other.gameObject)) // sprawdzanie czy obiekt jest faktycznie obiektem na kt�rego chcemy nak�ada� jakie� zmiany, np jakby�my dodali inne obiekty typu mr�wkojad to �eby nie 
        {                                      //dostawa�y tezz zmian
            if (!objectsInArea.Contains(other.gameObject))
            {
                objectsInArea.Add(other.gameObject); // dodawanie mrowek
                lastTimePerObject[other.gameObject] = TimeManager.instance.currentTime; //ustawienie ostatniego wywo�ania funkcji czasu
            }
            OnObjectEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D other) //usuwanie mr�wk�w
    {
        if (objectsInArea.Contains(other.gameObject))
        {
            objectsInArea.Remove(other.gameObject);
            lastTimePerObject.Remove(other.gameObject);
        }
        OnObjectEnter();
    }

    private void OnObjectEnter() // funkcja sprawdza czy jest przynajmniej jedna mr�wka w zasi�gu, �eby nie wywo�ywa� kodu bez sensu, gdy i tak nic nie zrobi
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

    private void OnTimeChanged(float currentTime) // funkcja dzia�aj�ca w czasie
    {
        foreach (GameObject obj in objectsInArea)
        {
            float lastTime = lastTimePerObject[obj];
            float deltaHours = currentTime - lastTime; // ile czasu up�yn�o do ostatniego wywo�ania funkcji czasu
            if (deltaHours < 0) deltaHours += 24f;

            ApplyEffect(obj, deltaHours); // funkcja dodaj�ca r�ne wartosci zale�nie od punktu

            lastTimePerObject[obj] = currentTime;
        }
    }

    protected abstract bool IsValidObject(GameObject obj);
    protected abstract void ApplyEffect(GameObject obj, float deltaHours); // nadpisywana dla ka�dego skryptu - nowe efekty
}