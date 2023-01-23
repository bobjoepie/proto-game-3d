using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseIndicator : MonoBehaviour
{
    private Camera mainCamera;
    Plane plane = new Plane(Vector3.up, 0);
    Vector3 worldPosition;
    private Ray ray;
    private RaycastHit hit;
    private float distance;
    private LayerMask layerMask;
    private int layer;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        layerMask = LayerUtility.Only("Default");
        layer = layerMask.ToLayer();
    }

    // Update is called once per frame
    void Update()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 999f, layerMask))
        {
            worldPosition = hit.point;
        }
        else if (plane.Raycast(ray, out distance))
        {
            worldPosition = ray.GetPoint(distance);
        }

        transform.position = worldPosition;
    }
}
