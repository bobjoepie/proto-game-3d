using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_CameraHolder : MonoBehaviour, IInputController
{
    public InputManager input;
    public float cameraSpeed = 10.0f;
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        input = InputManager.Instance;
        input.Register(this, DefaultActionMaps.BG_CameraActions);

        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        var horizontal = 0f;
        var vertical = 0f;

        if (input.PollKey(this, KeyAction.BG_Left))
        {
            horizontal -= 1f;
        }
        if (input.PollKey(this, KeyAction.BG_Right))
        {
            horizontal += 1f;
        }
        if (input.PollKey(this, KeyAction.BG_Up))
        {
            vertical += 1f;
        }
        if (input.PollKey(this, KeyAction.BG_Down))
        {
            vertical -= 1f;
        }

        if (horizontal == 0f && vertical == 0f) return;

        var movement = new Vector3(horizontal, 0, vertical).normalized;
        var camRot = transform.rotation.eulerAngles.y;
        var rot = Quaternion.Euler(0, camRot, 0);
        var isoMatrix = Matrix4x4.Rotate(rot);
        var res = isoMatrix.MultiplyPoint3x4(movement);

        if (input.PollKey(this, KeyAction.BG_SpeedUpCamera))
        {
            res *= 3f;
        }

        transform.position += new Vector3(res.x * cameraSpeed * Time.deltaTime, 0, res.z * cameraSpeed * Time.deltaTime);
    }

    private void HandleZoom()
    {
        var vertical = 0f;
        var mouseWheel = Input.mouseScrollDelta.y;
        if (input.PollKeyDown(this, KeyAction.BG_ZoomIn) || mouseWheel > 0)
        {
            vertical += 1f;
        }
        if (input.PollKeyDown(this, KeyAction.BG_ZoomOut) || mouseWheel < 0)
        {
            vertical -= 1f;
        }

        if (input.PollKeyDown(this, KeyAction.BG_ResetZoom))
        {
            mainCamera.transform.localPosition = new Vector3(0, 0, 0);
        }

        if (vertical == 0f) return;
        
        mainCamera.transform.localPosition += new Vector3(0, 0, vertical);
    }
}
