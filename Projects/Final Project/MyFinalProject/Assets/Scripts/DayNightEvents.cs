using UnityEngine;
using UnityEngine.Events;

public class DayNightEvents : MonoBehaviour
{
    private const int MIDNIGHT = 0;

    [Header("Event Hours")]
    [SerializeField] private int sunRiseHour = 6;
    [SerializeField] private int sunSetHour = 18;

    [Header("Day-Night Events")]
    public UnityEvent OnSunRise;
    public UnityEvent OnSunset;
    public UnityEvent<int> OnHour;
    public UnityEvent OnNewDay;

    [field: SerializeField] public int CurrentHour { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentHour = -1;
    }

    public void Evaluate(float normalizedTime)
    {
        int calculatedHour = Mathf.FloorToInt(normalizedTime * TimeManager.HOURS_PER_DAY) % TimeManager.HOURS_PER_DAY;

        if (calculatedHour == CurrentHour) return;

        TriggerMidnightEvent(calculatedHour);
        TriggerSunriseEvent(calculatedHour);
        TriggerSunset(calculatedHour);
        TriggerHourlyEvent(calculatedHour);

        CurrentHour = calculatedHour;
    }

    private void TriggerSunset(int hour)
    {
        if (hour == sunSetHour)
            OnSunset?.Invoke();
    }
    
    private void TriggerSunriseEvent(int hour)
    {
        if (hour == sunRiseHour)
            OnSunRise?.Invoke();
    }

    private void TriggerMidnightEvent(int hour)
    {
        if (hour == MIDNIGHT)
            OnNewDay?.Invoke();
    }

    private void TriggerHourlyEvent(int hour)
    {
        OnHour?.Invoke(hour);
    }
}
