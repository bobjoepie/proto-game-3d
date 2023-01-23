using UnityEngine;

public class HurtboxController : MonoBehaviour
{
    public AppendageController appendageController;

    private void Awake()
    {
        appendageController = transform.parent.GetComponent<AppendageController>();
        gameObject.layer = appendageController.hurtboxLayer.ToLayer();
    }
}