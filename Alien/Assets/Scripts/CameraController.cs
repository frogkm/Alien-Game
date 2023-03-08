using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float horizontalSensitivity;
    [SerializeField] private float verticalSensitivity;

    [SerializeField] private Transform orientation;

    private float x_rotation;
    private float y_rotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouse_x = Input.GetAxisRaw("Mouse X") * Time.deltaTime * horizontalSensitivity;
        float mouse_y = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * verticalSensitivity;

        y_rotation += mouse_x;
        x_rotation -= mouse_y;

        x_rotation = Mathf.Clamp(x_rotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(x_rotation, y_rotation, 0);
        orientation.rotation = Quaternion.Euler(0, y_rotation, 0);
    }
}
