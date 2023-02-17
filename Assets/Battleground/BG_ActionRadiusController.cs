using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BG_ActionRadiusController : MonoBehaviour
{
    public List<Transform> storedTargets = new List<Transform>();
    public bool isManagingTargets = true;
    public bool autoSortEnabled = true;
    public int autoSortFrequency = 5;

    private CancellationTokenSource cancellationToken;
    private SphereCollider sphereCollider;

    private void Awake()
    {
        if (autoSortEnabled)
        {
            StartAutoSortTargets();
        }

        sphereCollider = GetComponent<SphereCollider>();
    }
    private void OnEnable()
    {
        if (transform.root.TryGetComponent<BG_EntityController>(out var entity))
        {
            entity.Register(this);
        }
    }

    private void Start()
    {
        if (transform.root.TryGetComponent<BG_EntityController>(out var entity))
        {
            gameObject.layer = entity.actionRadiusLayer.ToLayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isManagingTargets && other.transform.root.TryGetComponent<BG_EntityController>(out var entity))
        {
            AddTarget(entity.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isManagingTargets && other.transform.root.TryGetComponent<BG_EntityController>(out var entity))
        {
            RemoveTarget(entity.transform);
        }
    }

    private void OnDisable()
    {
        CancelAutoSortTargets();
        if (transform.root.TryGetComponent<BG_EntityController>(out var entity))
        {
            entity.Unregister(this);
        }
    }

    public void AddTarget(Transform t)
    {
        storedTargets.Add(t);
    }

    public void RemoveTarget(Transform t)
    {
        storedTargets.Remove(t);
    }

    public void FillTargets()
    {
        List<Collider> colliders = Physics.OverlapSphere(transform.position, sphereCollider.radius, gameObject.layer).ToList();
        storedTargets = colliders.Select(c => c.transform).ToList();
    }

    public void ClearTargets()
    {
        storedTargets.Clear();
    }

    public void SortTargets()
    {
        storedTargets = storedTargets
            .Where(c => c != null)
            .OrderBy(t => Vector3.Distance(transform.position, t.transform.position))
            .ToList();
    }

    private async UniTask AutoSortTargets()
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (autoSortEnabled)
            {
                SortTargets();
            }
            await UniTask.Delay(TimeSpan.FromSeconds(autoSortFrequency));
        }
    }

    public void StartAutoSortTargets()
    {
        if (cancellationToken != null) return;
        cancellationToken = new CancellationTokenSource();
        AutoSortTargets().Forget();
    }

    public void CancelAutoSortTargets()
    {
        cancellationToken.Cancel();
        cancellationToken.Dispose();
        autoSortEnabled = false;
    }

    public Transform GetClosestTarget()
    {
        var target = storedTargets.FirstOrDefault();
        if (target == null)
        {
            //ClearTargets();
            //FillTargets();
            SortTargets();
            target = storedTargets.FirstOrDefault();
        }

        return target;
    }

    public List<Transform> GetClosestTargets(int num)
    {
        var targets = storedTargets.GetRange(0, num);
        if (targets.Contains(null))
        {
            ClearTargets();
            FillTargets();
            targets = storedTargets.GetRange(0, num);
        }
        return targets;
    }
}
