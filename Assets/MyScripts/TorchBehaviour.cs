using UnityEngine;

public class TorchBehaviour : MonoBehaviour
{
    public GameObject torch; // Reference to the torch GameObject
    private bool isTorchOn = false; // Keeps track of whether the torch is on

    // Start is called once before the first execution of Update
    void Start()
    {
        if (torch != null)
        {
            torch.SetActive(isTorchOn); // Initialize the torch's state
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // Press 'F' to toggle the torch
        {
            ToggleTorch();
        }
    }

    void ToggleTorch()
    {
        if (torch != null)
        {
            isTorchOn = !isTorchOn; // Toggle the state
            torch.SetActive(isTorchOn); // Set the active state of the torch
        }
        else
        {
            Debug.LogWarning("Torch GameObject is not assigned!");
        }
    }
}
