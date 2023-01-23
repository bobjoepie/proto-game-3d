using System;
using UnityEngine;

public class ProjectileController : WeaponBase
{
    [Header("Properties")]
    public WeaponType weaponType;
    public CollisionType collisionType;
    public GameObject weaponGameObject;
    public int damage;
    public SpawnLocation weaponSpawn;
    public int iterationNum;

    [Header("Projectile Properties")]
    public bool cur_hasCollision;
    [Range(0, 5)] public float cur_lifeTime;
    [Range(0, 20)] public float cur_speed;
    public TargetType cur_targetType; //TODO
    public AudioClip cur_attackSound;

    [Header("Pre-Attack")]
    public bool pre_hasCollision; //TODO
    [Range(0, 5)] public float pre_lifeTime;
    [Range(0, 20)] public float pre_speed;
    public TargetType pre_targetType;
    public AudioClip pre_attackSound;

    [Header("Post-Attack")]
    public WeaponSO post_subWeapon;

    private Camera MainCamera;
    private AudioManager audioManager;

    private float ElapsedTime;

    public Quaternion initDir;
    public Vector3 initPos;
    public Vector3 initMousePos;

    private bool isPrepped;
    
    void Start()
    {
        ElapsedTime = 0f;
        MainCamera = Camera.main;
        audioManager = AudioManager.Instance;
        
        if (Physics.Raycast(MainCamera.ScreenPointToRay(Input.mousePosition), out var hit, 999f, LayerUtility.Only("Default")))
        {
            initMousePos = hit.point;
        }

        switch (weaponSpawn)
        {
            case SpawnLocation.MousePoint:
                transform.position = initMousePos;
                break;
            case SpawnLocation.None:
            default:
                break;
        }

        initDir = transform.rotation;
        initPos = transform.position;
        if (pre_attackSound != null)
        {
            audioManager.Play(pre_attackSound);
        }
    }
    
    void Update()
    {
        if (!isPrepped)
        {
            ProcessPreAttack();
        }
        if (!isPrepped) return;

        ProcessAttack();

        if (ElapsedTime > cur_lifeTime)
        {
            ProcessPostAttack();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //var closestPoint = other.ClosestPoint(transform.position);
        if (other.TryGetComponent<HitboxController>(out var hitbox)) hitbox.TakeDamage(damage);

        switch (isPrepped)
        {
            case true when cur_hasCollision:
            case false when pre_hasCollision:
                ProcessPostAttack();
                break;
        }
    }

    private void ProcessPreAttack()
    {
        var step = pre_speed * Time.deltaTime;
        transform.position += transform.forward * step;

        if (pre_lifeTime <= 0f)
        {
            InitAttack();
        }
        pre_lifeTime -= Time.deltaTime;
    }

    private void ProcessAttack()
    {
        var step = cur_speed * Time.deltaTime;
        transform.position += transform.forward * step;

        ElapsedTime += Time.deltaTime;
    }

    private void ProcessPostAttack()
    {
        Destroy(gameObject);
        var weaponParts = WeaponSO.ConvertWeaponToParts(post_subWeapon);
        WeaponSO.InstantiateWeaponParts(weaponParts, gameObject.transform.position, gameObject.transform.rotation, gameObject.layer, iterationNum);
    }

    private void InitAttack()
    {
        isPrepped = true;
        if (cur_attackSound != null)
        {
            audioManager.Play(cur_attackSound);
        }

        switch (cur_targetType)
        {
            case TargetType.TowardsMouse:
                Vector3 mousePos = initMousePos;
                if (Physics.Raycast(MainCamera.ScreenPointToRay(Input.mousePosition), out var hit, 999f, LayerUtility.Only("Default")))
                {
                    mousePos = hit.point;
                }
                transform.LookAt(mousePos);
                break;
            case TargetType.TowardsInitialMouse:
                transform.LookAt(initMousePos);
                break;
            case TargetType.None:
            default:
                transform.rotation = initDir;
                break;
        }
    }
}
