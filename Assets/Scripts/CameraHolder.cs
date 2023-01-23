using DG.Tweening;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CameraHolder : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Camera mainCamera;
    public float maxLeashDistance = 5f;
    [Range(1f, 20f)] public float rangeMultiplier = 1f;
    [Range(0f, 1f)] public float cameraSmoothingTime = 0.5f;

    private float isometricAngle = 45f;
    private Quaternion isometricRotation;
    private Tween curTween;

    private void Start()
    {
        mainCamera = Camera.main;
        isometricAngle = mainCamera.transform.rotation.eulerAngles.y;
        isometricRotation = Quaternion.Euler(0, isometricAngle, 0);
    }

    private void Update()
    {
        if (target == null) return;
        var mouseScreenPoint = mainCamera.ScreenToViewportPoint(Input.mousePosition);
        var hor = mouseScreenPoint.x;
        var vert = mouseScreenPoint.y;

        var mousePos = new Vector3(hor, 0, vert);

        var targetPos = mainCamera.WorldToViewportPoint(target.position);
        targetPos = new Vector3(targetPos.x, 0, targetPos.y);
        var dir = mousePos - targetPos;

        var magnitude = dir.magnitude;
        if (magnitude > maxLeashDistance) {
            dir = dir * (maxLeashDistance / magnitude);
        }

        var offsetPos = isometricRotation * dir.normalized * (magnitude * rangeMultiplier);
        if (offsetPos.magnitude > maxLeashDistance)
        {
            offsetPos = offsetPos.normalized * maxLeashDistance;
        }

        curTween?.Kill();
        curTween = transform.DOMove(target.position + offsetPos, cameraSmoothingTime);
    }
}
