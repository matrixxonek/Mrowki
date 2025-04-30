using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    [Header("Czas gry")]
    [Range(0f, 24f)]
    public float currentTime = 6f;  // Start np. o 6:00
    public float dayLengthInMinutes = 10f;  // Ile minut prawdziwego czasu trwa doba

    public delegate void TimeChanged(float time);
    public event TimeChanged OnTimeChanged;

    private float timeSpeed;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        timeSpeed = 24f / (dayLengthInMinutes * 60);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += timeSpeed * Time.deltaTime;
        if (currentTime > 24f)
            currentTime -= 24f;
        OnTimeChanged?.Invoke(currentTime);
        
    }

    public bool IsDayTime()
    {
        return (currentTime >= 6f || currentTime <= 20f);
    }
    
    public bool IsNightTime()
    {
        return !IsDayTime();
    }
}
