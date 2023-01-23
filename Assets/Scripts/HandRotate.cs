using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRotate : MonoBehaviour
{
    public Camera mainCamera;
    Plane plane = new Plane(Vector3.up, 0);
    Vector3 worldPosition;
    private Ray ray;
    private RaycastHit hit;
    private float distance;
    private LayerMask layerMask;
    private int layer;
    void Start()
    {
        mainCamera = Camera.main;
        layerMask = LayerUtility.Only("Default");
        layer = layerMask.ToLayer();
    }

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

        Debug.DrawRay(transform.position, worldPosition - transform.position, Color.red);
        transform.LookAt(worldPosition);

        Quaternion q = transform.rotation;
        q.eulerAngles = new Vector3(0, q.eulerAngles.y, q.eulerAngles.z);
        transform.rotation = q;
    }
}
