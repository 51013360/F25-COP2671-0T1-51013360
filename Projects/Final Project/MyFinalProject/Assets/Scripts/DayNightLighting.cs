using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Controls lighting color and intensity transitions over the day-night cycle.
/// </summary>
[RequireComponent(typeof(Light2D))]
public class DayNightLighting : MonoBehaviour
{
    [Header("Lighting Settings")]
    [SerializeField] private Gradient _colorOverDay;         // Color transition over time
    [SerializeField] private AnimationCurve _intensityCurve; // Intensity curve over time

    private Light2D _light;

    private void Awake()
    {
        // Get the Light2D component
        _light = GetComponent<Light2D>();
    }

    private void OnEnable()
    {
        // Subscribe to the time update event
        TimeManager.OnTimerUpdate.AddListener(UpdateLighting);
    }

    private void OnDisable()
    {
        // Unsubscribe from the time update event
        TimeManager.OnTimerUpdate.RemoveListener(UpdateLighting);
    }

    private void UpdateLighting(float normalizedTime)
    {
        // Evaluate color and intensity from 0–1 normalized time
        _light.color = _colorOverDay.Evaluate(normalizedTime);
        _light.intensity = _intensityCurve.Evaluate(normalizedTime);
    }
}