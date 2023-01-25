using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class AppendageController : MonoBehaviour
{
    public int appendageHp;
    public bool canTakeDamage;
    public EntityController entityController;
    public List<AttackController> attacks = new List<AttackController>();

    public bool detach;
    public bool affectsEntityHP;
    public LayerMask hitboxLayer;
    public LayerMask hurtboxLayer;
    public LayerMask collisionLayer;

    private void OnEnable()
    {
        entityController = transform.root.GetComponent<EntityController>();
        if (entityController != null)
        {
            entityController.Register(this);
        }
    }

    void Start()
    {
        if (detach)
        {
            this.transform.parent = null;
            var rigidbody = this.GetOrAddComponent<Rigidbody2D>();
            rigidbody.gravityScale = 0;
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void OnDisable()
    {
        entityController.Unregister(this);
    }

    public void PerformAttacks(Vector3? targetPos)
    {
        if (targetPos == null) return;
        foreach (var attack in attacks)
        {
            attack.Attack(targetPos).Forget();
        }
    }

    public void TakeDamage(int damage)
    {
        if (canTakeDamage)
        {
            appendageHp -= damage;
        }

        if (affectsEntityHP)
        {
            entityController.TakeDamage(damage);
        }

        if (canTakeDamage && appendageHp <= 0)
        {
            DestroyAll();
        }
    }

    public void DestroyAll()
    {
        Destroy(gameObject);
    }

    public void Register(AttackController attackController)
    {
        attacks.Add(attackController);
    }

    public void Unregister(AttackController attackController)
    {
        attacks.Remove(attackController);
    }
}