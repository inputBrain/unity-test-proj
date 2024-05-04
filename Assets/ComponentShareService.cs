using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Const;
using Services.DebugMessages;
using Storage;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public class ComponentShareService : Singleton<ComponentShareService>
{
    private readonly Dictionary<Type, object> _components = new();
    private readonly Dictionary<string, object> _componentsWithTag = new();

    void Awake()
    {
        RegisterComponent<HexagonTileStorage>();
        RegisterComponent<GetTileInfo>();
        RegisterComponent<BuildUpgradeMenu>();
        
        RegisterComponentWithTag<Camera>(Constants.MAIN_CAMERA);
        RegisterComponentWithTag<Tilemap>(Constants.BASE_TILEMAP);
        RegisterComponentWithTag<Tilemap>(Constants.CASTLE_TILEMAP);
    }

    public T GetComponentByType<T>() where T : class
    {
        if (_components.TryGetValue(typeof(T), out var component))
        {
            return (T)component;
        }
     
        Debug.LogWarning($"Component of type {typeof(T)} is null! Called from {GetCallingClassName()}");
        return null;
    }

    private static string GetCallingClassName()
    {
        StackTrace stackTrace = new();
        return stackTrace.GetFrame(2).GetMethod().DeclaringType?.Name;
    }

    public T GetComponentByTypeAndTag<T>(string tag) where T : class
    {
        foreach (var kvp in _componentsWithTag.Where(kvp => kvp.Key == tag))
        {
            if (kvp.Value is Component component && component.CompareTag(tag))
            {
                return (T)kvp.Value;
            }
        }
    
        Debug.LogWarning($"Component of type {typeof(T)} with tag {tag} is null! Called from {GetCallingClassName()}");
    
        return null;
    }

    private void RegisterComponent<T>() where T : Object
    {
        T component = FindObjectOfType<T>();
        if (component != null)
        {
            _components.Add(typeof(T), component);
        }
        else
        {
            Debug.LogError($"Component of type {typeof(T)} not found in current Scene!");
        }
    }
    
    private void RegisterComponentWithTag<T>(string tag) where T : Object
    {
        T component = GameObject.FindGameObjectWithTag(tag).GetComponent<T>();
        if (component != null)
        {
            _componentsWithTag.Add(tag, component);
        }
        else
        {
            Debug.LogError($"Component of type {typeof(T)} with tag {tag} not found in current Scene!");
        }
    }
}