using UnityEngine;
using System;
using UnityEditor;

[ExecuteAlways]
public class UniqueIdentifier : MonoBehaviour
{
    [SerializeField] string uniqueId = "";
    private void Update()
    {
        if (Application.IsPlaying(gameObject)) { return; }
        if(string.IsNullOrEmpty(gameObject.scene.path)) { return; } //we are in a prefab | return

        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty property = serializedObject.FindProperty("uniqueId");

        if (string.IsNullOrEmpty(property.stringValue))
        {
            property.stringValue = Guid.NewGuid().ToString();
            serializedObject.ApplyModifiedProperties();
        }
    }

    public string GetUniqueId()
    {
        return uniqueId;
    }
}
