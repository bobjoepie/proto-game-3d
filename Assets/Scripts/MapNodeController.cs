using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using Object = UnityEngine.Object;

[ExecuteInEditMode]
public class MapNodeController : MonoBehaviour
{
    public GameObject nodePrefab;
    //public bool needsConfirmation = false;
    private GameObject tempPrefab;
    
    public List<MapNodeController> children = new List<MapNodeController>();
    public List<MapNodeController> parents = new List<MapNodeController>();

    private MeshRenderer meshRenderer;

    public bool root = false;
    public bool selectable = false;
    public bool inactive = false;
    public bool active = false;
    public Material inactiveMaterial;
    public Material selectableMaterial;
    public Material activeMaterial;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        if (root && Application.isPlaying)
        {
            Activate();
        }
    }

    void Update()
    {
#if UNITY_EDITOR
        //if (Event.current.type == EventType.MouseDown && needsConfirmation == true)
        //{
        //    ConfirmPrefabPosition();
        //}
        //else if (tempPrefab != null && needsConfirmation == true)
        //{
        //    Vector3 mousePos = Vector3.zero;
        //    var mouse = Event.current.mousePosition;
        //    Ray ray = HandleUtility.GUIPointToWorldRay(mouse);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit, 999f, LayerUtility.Only("Default")))
        //    {
        //        mousePos = hit.point;
        //    }

        //    tempPrefab.transform.position = mousePos;
        //}
#endif
        foreach (var child in children)
        {
            ForDebug(transform.position, child.transform.position - transform.position, Color.green);
        }
    }

    public MapNodeController CreateNode()
    {
#if UNITY_EDITOR
        string prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(nodePrefab);
        //Get prefab object from path
        Object prefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(Object));
        //Instantiate the prefab in the scene, as a sibling of current gameObject
        tempPrefab = PrefabUtility.InstantiatePrefab(prefab, transform) as GameObject;
        tempPrefab.transform.parent = transform.parent;
        var tempNode = tempPrefab.GetComponent<MapNodeController>();
        Selection.activeGameObject = tempPrefab.gameObject;

        return tempNode;

       // needsConfirmation = true;
#endif
    }

    public void ConfirmPrefabPosition()
    {
#if UNITY_EDITOR
        //needsConfirmation = false;
#endif
    }

    public void LinkNode(MapNodeController root)
    {
        if (!this.parents.Contains(root))
        {
            this.parents.Add(root);
        }

        if (!root.children.Contains(this))
        {
            root.children.Add(this);
        }
        
    }

    public void ClearSelectedNodeConnections(List<MapNodeController> nodes)
    {
        foreach (var parent in nodes)
        {
            parent.children.Remove(this);
        }

        foreach (var child in nodes)
        {
            child.parents.Remove(this);
        }
    }

    public void ClearNodeConnections()
    {
        foreach (var parent in parents)
        {
            parent.children.Remove(this);
        }
        parents.Clear();

        foreach (var child in children)
        {
            child.parents.Remove(this);
        }
        children.Clear();
    }

    public void DeleteNode()
    {
        ClearNodeConnections();
        DestroyImmediate(this.transform.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (selectable)
        {
            Activate();
        }
    }

    public void Activate()
    {
        meshRenderer.material = activeMaterial;
        active = true;
        selectable = false;

        foreach (var parent in parents)
        {
            parent.Deactivate();
        }

        foreach (var child in children)
        {
            child.MakeSelectable();
        }
    }

    public void MakeSelectable()
    {
        meshRenderer.material = selectableMaterial;
        selectable = true;
    }

    public void Deactivate()
    {
        meshRenderer.material = inactiveMaterial;
        inactive = true;
        foreach (var child in children.Where(c => c.selectable))
        {
            child.Deactivate();
        }
    }

    public static void ForDebug(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 1.75f, float arrowHeadAngle = 20.0f)
    {
        Debug.DrawRay(pos, direction, color);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Debug.DrawRay(pos + direction, right * arrowHeadLength, Color.red);
        Debug.DrawRay(pos + direction, left * arrowHeadLength, Color.red);
    }
}
