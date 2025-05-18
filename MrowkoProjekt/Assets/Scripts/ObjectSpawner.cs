using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner instance;

    [SerializeField] int maxWidth = 0;
    [SerializeField] int maxHeight = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    Vector3 findSpawnPoint()
    {
        System.Random rnd = new System.Random();
        int xPosition, yPosition;
        xPosition = rnd.Next(-maxWidth, maxWidth + 1);
        yPosition = rnd.Next(-maxHeight, maxHeight + 1);
        return new Vector3(xPosition, yPosition, 0);
    }
    public GameObject SpawnObject(GameObject prefab)
    {
        return Instantiate(prefab, findSpawnPoint(), Quaternion.identity);
    }
}
