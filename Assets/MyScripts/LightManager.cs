using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightManager : MonoBehaviour
{
    public List<Transform> lightPoints; // Assign lights in Inspector
    public float lightDuration = 5f;    // Time each light stays ON
    public int activeLightCount = 4;    // Number of active lights at a time (default to 3)

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

                // Ensure no duplicates (no same light selected twice)
                if (!selectedLights.Contains(selectedLight))
                {
                    selectedLights.Add(selectedLight);
                    selectedLight.gameObject.SetActive(true);  // Turn this light ON
                }
            }

            // Update the current lights list
            currentLights = new List<Transform>(selectedLights);

            // Wait for the duration before switching
            yield return new WaitForSeconds(lightDuration);
        }
    }

    // Check if the player is in a safe zone under one of the active lights
    public bool IsPlayerSafe(Vector3 playerPosition)
    {
        if (currentLights.Count == 0) return false;

        float safeRadius = 3.0f; // Adjust as needed

        foreach (Transform light in currentLights)
        {
            if (Vector3.Distance(playerPosition, light.position) <= safeRadius)
            {
                return true; // Player is safe if near any of the active lights
            }
        }

        return false; // Player is not safe
    }
}