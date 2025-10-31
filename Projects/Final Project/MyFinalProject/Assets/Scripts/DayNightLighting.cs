using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class DayNightLighting : MonoBehaviour
{
    public Gradient dayNightColors;
    public AnimationCurve lightIntensityCurve;

    private Light2D _light;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _light = GetComponent<Light2D>();
    }

    public void Evaluate(float normalizedTime)
    {
        if (_light == null) return;

        _light.color = dayNightColors.Evaluate(normalizedTime);
        _light.intensity = lightIntensityCurve.Evaluate(normalizedTime);
    }
}
