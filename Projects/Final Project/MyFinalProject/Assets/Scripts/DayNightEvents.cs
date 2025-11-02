using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles events that occur at sunrise and sunset based on the in-game time.
/// </summary>
public class DayNightEvents : MonoBehaviour
{
    [Header("Event Hours")]
    [Range(0, 23)][SerializeField] private int _sunriseHour = 6;
    [Range(0, 23)][SerializeField] private int _sunsetHour = 18;

    [Header("Day-Night Events")]
    public UnityEvent OnSunrise;
    public UnityEvent OnSunset;

    private bool _sunriseTriggered;
    private bool _sunsetTriggered;

    private void OnEnable()
    {
        TimeManager.OnTimerUpdate.AddListener(HandleTimeUpdate);
    }

    private void OnDisable()
    {
        TimeManager.OnTimerUpdate.RemoveListener(HandleTimeUpdate);
    }

    private void HandleTimeUpdate(float normalizedTime)
    {
        // Convert normalized time (0–1) into an in-game hour
        float currentHour = normalizedTime * TimeManager.HOURS_PER_DAY;

        // Trigger sunrise event
        if (!_sunriseTriggered && currentHour >= _sunriseHour && currentHour < _sunsetHour)
        {
            OnSunrise?.Invoke();
            _sunriseTriggered = true;
            _sunsetTriggered = false;
        }
        // Trigger sunset event
        else if (!_sunsetTriggered && (currentHour >= _sunsetHour || currentHour < _sunriseHour))
        {
            OnSunset?.Invoke();
            _sunsetTriggered = true;
            _sunriseTriggered = false;
        }
    }
}