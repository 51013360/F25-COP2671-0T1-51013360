using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]

public class DayNightController : MonoBehaviour
{
    [Header("References")]
    public TimeManager _timeManager;
    public Gradient _dayNightColors;
    public AnimationCurve _lightIntensityCurve;

    private Light2D _light;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _light = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _light.color = _dayNightColors.Evaluate(_timeManager.Now);
        _light.intensity = _lightIntensityCurve.Evaluate(_timeManager.Now);
    }
}
