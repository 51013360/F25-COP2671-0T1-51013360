using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages the in-game time cycle, simulating a 24-hour day over a configurable real-time duration.
/// </summary>
public class TimeManager : SingletonMonoBehaviour<TimeManager>
{
    // Constants for time conversion and in-game day length
    public static int HOURS_PER_DAY = 24;
    public static int SECONDS_PER_MINUTE = 60;

    // Events triggered during time updates and hourly changes
    public static UnityEvent<float> OnTimerUpdate = new UnityEvent<float>();       // Normalized time update (0 to 1)
    public static UnityEvent OnNewDay = new UnityEvent();

    [Header("Time Settings")]
    [SerializeField] private float _realTimeMinutesPerDay = 15f; // Duration of one in-game day in real minutes
    [SerializeField] private float _timeMultiplier = 1f;
    [SerializeField] private bool _isCycleActive = true;         // Controls whether the time cycle is running

    // Converts real-time minutes to seconds for internal calculations
    private float DurationOfDayInSeconds => _realTimeMinutesPerDay * SECONDS_PER_MINUTE;

    // Internal state variables
    private Coroutine _dayCycleCoroutine;
    [SerializeField] private float _calculateTime;
    [SerializeField] private float _normalizedTime;   // Value between 0 and 1 representing time progression
    private float _updateTimer;      // Accumulated real-time seconds
    private float _lastNormalizedTime = 0f;

    /// <summary>
    /// Starts the day cycle coroutine when the game begins.
    /// </summary>
    private void Start()
    {
        _realTimeMinutesPerDay = Mathf.Max(1f, _realTimeMinutesPerDay);
        _dayCycleCoroutine = StartCoroutine(TimerRoutine());
    }

    /// <summary>
    /// Coroutine that updates the in-game time based on real-time progression.
    /// </summary>
    private IEnumerator TimerRoutine()
    {
        var wait = new WaitForEndOfFrame(); // Reuse yield instruction to reduce allocations

        while (isActiveAndEnabled)
        {
            // Wait here until the cycle is active again
            yield return new WaitUntil(() => _isCycleActive);

            // Accumulate time since last frame
            _updateTimer += Time.deltaTime;

            // Normalize time to a 0�1 range
            _normalizedTime = (_calculateTime % DurationOfDayInSeconds) / DurationOfDayInSeconds;

            // Detect day wrap (0.99 → 0.00)
            if (_normalizedTime < _lastNormalizedTime)
            {
                OnNewDay?.Invoke();
            }

            _lastNormalizedTime = _normalizedTime;

            // Trigger OnTimerUpdate event every frame
            OnTimerUpdate?.Invoke(_normalizedTime);

            // Calculate time within the current day cycle
            _calculateTime += _updateTimer * _timeMultiplier;
            _calculateTime %= DurationOfDayInSeconds;
            _updateTimer = 0f;

            yield return wait;
        }
    }

    /// <summary>
    /// Toggles the time cycle between paused and active states.
    /// </summary>
    public void TogglePause()
    {
        _isCycleActive = !_isCycleActive;
    }
    /// <summary>
    /// Sets the starting hour
    /// </summary>
    /// <param name="hour"></param>
    public static void SetStartHour(int hour)
    {
        Instance._calculateTime = (Instance.DurationOfDayInSeconds * hour / HOURS_PER_DAY) % Instance.DurationOfDayInSeconds;
    }
}