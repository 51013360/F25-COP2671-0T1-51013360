using UnityEngine;

public class DayNightController : SingletonMonoBehaviour<DayNightController>
{
    public static DayNightEvents Events => Instance._events;
    public static DayNightLighting Lighting => Instance._lighting;

    [SerializeField] private int StartingHour;

    private DayNightEvents _events;
    private DayNightLighting _lighting;

    private void OnEnable()
    {
        TimeManager.OnTimerUpdate.AddListener(UpdateDayCycle);
    }

    private void OnDisable()
    {
        TimeManager.OnTimerUpdate.RemoveListener(UpdateDayCycle);
    }

    protected override void InitializeAfterAwake()
    {
        TimeManager.SetStartHour(StartingHour);

        _events = GetComponent<DayNightEvents>();
        _lighting = GetComponent<DayNightLighting>();
    }

    private void UpdateDayCycle(float normalizedTime)
    {
        _events.Evaluate(normalizedTime);
        _lighting.Evaluate(normalizedTime);
    }
}
