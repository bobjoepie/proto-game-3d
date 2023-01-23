using UnityEngine;

public class ColliderController : MonoBehaviour
{
    public AppendageController appendageController;

    private void Awake()
    {
        appendageController = transform.parent.GetComponent<AppendageController>();
        gameObject.layer = appendageController.collisionLayer.ToLayer();
    }
}
