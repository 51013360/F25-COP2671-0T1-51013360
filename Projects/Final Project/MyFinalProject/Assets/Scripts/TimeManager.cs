using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public const int HOURSPERDAY = 24;
    public const float SECONDSINMINUTE = 60f;

    public UnityEvent<float> OnUpdateTrigger;

    [Header("Time Settings")]
    [SerializeField] private float _realTimeMinutesPerDay = 15f;
    [SerializeField] private float _timeMultiplier = 1f;
    [SerializeField] private bool _isCycleActive = true;
    [SerializeField] private float _updateInterval = 0.1f;
    [SerializeField] private float _normalizedTime;

    private float DurationInSeconds => _realTimeMinutesPerDay * SECONDSINMINUTE; // 15 * 60 = 900
    private float _calculateTime;
    public float Now => _normalizedTime;

    private float _updateTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        while (enabled)
        {
            if (_isCycleActive)
            {
                _updateTimer += Time.deltaTime;

                if (_updateTimer >= _updateInterval)
                {
                    _normalizedTime = (_calculateTime % DurationInSeconds) / DurationInSeconds;
                    OnUpdateTrigger.Invoke(_normalizedTime);

                    _calculateTime += _updateTimer * _timeMultiplier;
                    _calculateTime %= DurationInSeconds;
                    _updateTimer = 0f;
                }
            }
        yield return null;
        }
    }
}
