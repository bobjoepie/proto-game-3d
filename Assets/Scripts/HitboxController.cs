using UnityEngine;

public class HitboxController : MonoBehaviour
{
    public AppendageController appendageController;

    private void Awake()
    {
        appendageController = transform.parent.GetComponent<AppendageController>();
        gameObject.layer = appendageController.hitboxLayer.ToLayer();
    }

    public void TakeDamage(int damage)
    {
        appendageController.TakeDamage(damage);
    }
}