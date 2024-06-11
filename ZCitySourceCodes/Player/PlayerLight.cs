using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    [SerializeField] private Light flashLight;
    [SerializeField] private DayNightCycle day;

    private void Update()
    {
        if (day.time >= 0.75f || day.time <= 0.25f)
            flashLight.enabled = true;
        else
            flashLight.enabled = false;
    }
}
