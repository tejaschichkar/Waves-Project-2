using UnityEngine;

public class LightBehaviour : MonoBehaviour
{
    // Array of light sources to be turned off
    public Light[] lightsToTurnOff;

    void Start()
    {
        // Optional: Check if lights are assigned
        if (lightsToTurnOff == null || lightsToTurnOff.Length == 0)
        {
            Debug.LogWarning("No lights assigned to LightBehaviour script.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger collider
        if (other.CompareTag("Player"))
        {
            // Loop through the light sources and turn them off
            foreach (Light light in lightsToTurnOff)
            {
                if (light != null)
                {
                    light.enabled = false;
                }
            }
        }
    }
}
