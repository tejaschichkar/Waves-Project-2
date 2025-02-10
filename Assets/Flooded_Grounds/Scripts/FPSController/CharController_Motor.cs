using UnityEngine;

public class CharController_Motor : MonoBehaviour
{
    public float speed = 10.0f;
    public float sensitivity = 30.0f;
    public float WaterHeight = 15.5f;

    private CharacterController character;
    public GameObject cam;

    private float moveFB, moveLR;
    private float rotX, rotY;
    private float gravity = -9.8f;

    void Start()
    {
        character = GetComponent<CharacterController>();
        if (Application.isEditor)
        {
            sensitivity *= 1.5f;
        }

        DisableCursor(); // Lock cursor at game start
    }

    void Update()
    {
        moveFB = Input.GetAxis("Vertical") * speed;
        moveLR = Input.GetAxis("Horizontal") * speed;

        rotX = Input.GetAxis("Mouse X") * sensitivity;
        rotY = Input.GetAxis("Mouse Y") * sensitivity;

        CheckForWaterHeight();

        Vector3 movement = new Vector3(moveLR, gravity, moveFB);
        movement = transform.rotation * movement;
        character.Move(movement * Time.deltaTime);

        CameraRotation(cam, rotX, rotY);
    }

    void CheckForWaterHeight()
    {
        gravity = transform.position.y < WaterHeight ? 0f : -9.8f;
    }

    void CameraRotation(GameObject cam, float rotX, float rotY)
    {
        transform.Rotate(0, rotX * Time.deltaTime, 0);
        cam.transform.Rotate(-rotY * Time.deltaTime, 0, 0);
    }

    // Call this when the game starts
    public void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Call this when the game ends (Game Over / Win)
    public void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
