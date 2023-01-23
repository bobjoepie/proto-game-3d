using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapNodeController)), CanEditMultipleObjects]
public class MapNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var mapNodeController = target as MapNodeController;
        //myScript.buttonDisplayName = EditorGUILayout.Toggle("Run", myScript.buttonDisplayName);
        //myScript.buttonDisplayName2 = EditorGUILayout.Toggle("Generate", myScript.buttonDisplayName2);

        var createNodeButton = GUILayout.Button("Create Node");
        var linkNodeButton = GUILayout.Button("Link Single-To-Many Node");
        var chainLinkNodeButton = GUILayout.Button("Link Chain Nodes");
        var clearSelectedNodeButton = GUILayout.Button("Clear Selected Node Connections");
        var clearNodeButton = GUILayout.Button("Clear All Node Connections");
        var deleteNodeButton = GUILayout.Button("Delete Node");

        var selection = Selection.objects.OfType<GameObject>().ToList().ConvertAll(o => o.GetComponent<MapNodeController>());

        if (createNodeButton)
        {
            var newNode = mapNodeController.CreateNode();
            EditorUtility.SetDirty(newNode);
            foreach (var parent in selection)
            {
                newNode.LinkNode(parent);
                EditorUtility.SetDirty(newNode);
                EditorUtility.SetDirty(parent);
            }
        }
        else if (linkNodeButton && selection.Count > 1)
        {
            for (int i = 1; i < selection.Count; i++)
            {
                selection[i].LinkNode(selection.First());
                EditorUtility.SetDirty(selection[i]);
                EditorUtility.SetDirty(selection.First());
            }
        }
        else if (chainLinkNodeButton && selection.Count > 1)
        {
            for (int i = 1; i < selection.Count; i++)
            {
                selection[i].LinkNode(selection[i-1]);
                EditorUtility.SetDirty(selection[i]);
                EditorUtility.SetDirty(selection[i-1]);
            }
        }
        else if (clearSelectedNodeButton && selection.Count > 1)
        {
            foreach (var node in selection)
            {
                node.ClearSelectedNodeConnections(selection);
                EditorUtility.SetDirty(node);
            }
        }
        else if (clearNodeButton)
        {
            foreach (var node in selection)
            {
                node.ClearNodeConnections();
                EditorUtility.SetDirty(node);
            }
        }
        else if (deleteNodeButton)
        {
            foreach (var node in selection)
            {
                node.DeleteNode();
            }
        }

        if (GUI.changed && mapNodeController != null)
        {
            EditorUtility.SetDirty(mapNodeController);
        }
    }
}
