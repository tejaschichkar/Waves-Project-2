using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightManager : MonoBehaviour
{
    public List<Transform> lightPoints; // Assign lights in Inspector
    public float lightDuration = 5f;    // Time each light stays ON
    public int activeLightCount = 4;    // Number of active lights at a time

    private List<Transform> currentLights = new List<Transform>();  // Active lights

    void Start()
    {
        if (lightPoints.Count > 0)
        {
            StartCoroutine(SwitchLights());
        }
    }

    IEnumerator SwitchLights()
    {
        while (true)
        {
            // Turn OFF all lights first
            foreach (Transform light in lightPoints)
            {
                light.gameObject.SetActive(false);
            }

            // Pick 'activeLightCount' number of lights
            List<Transform> selectedLights = new List<Transform>();
            while (selectedLights.Count < activeLightCount)
            {
                int randomIndex = Random.Range(0, lightPoints.Count);
                Transform selectedLight = lightPoints[randomIndex];

                if (!selectedLights.Contains(selectedLight)) // Avoid duplicates
                {
                    selectedLights.Add(selectedLight);
                    selectedLight.gameObject.SetActive(true);  // Turn this light ON
                }
            }

            // Update the current lights list
            currentLights = new List<Transform>(selectedLights);

            // Wait before switching
            yield return new WaitForSeconds(lightDuration);
        }
    }

    // Check if the player is safe (under any active light)
    public bool IsPlayerSafe(Vector3 playerPosition)
    {
        return IsPositionSafe(playerPosition);
    }

    // Check if ANY position (e.g., ghost's target) is inside a lighted area
    public bool IsPositionSafe(Vector3 position)
    {
        if (currentLights.Count == 0) return false;

        float safeRadius = 3.0f; // Adjust as needed

        foreach (Transform light in currentLights)
        {
            if (Vector3.Distance(position, light.position) <= safeRadius)
            {
                return true; // Position is within a light
            }
        }

        return false; // Position is in the dark
    }
}
