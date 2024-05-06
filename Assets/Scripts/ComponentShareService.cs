using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Const;
using Gameplay;
using Resource;
using Services;
using Services.DebugMessages;
using Storage;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public class ComponentShareService : Singleton<ComponentShareService>
{
    private readonly Dictionary<Type, object> _components = new();
    private readonly Dictionary<string, object> _componentsWithTag = new();

    void Awake()
    {
        var currentSceneName = SceneManager.GetActiveScene().name;
        switch (currentSceneName)
        {
            case "MainScene":
                RegisterComponent<MainMenu.MainMenu>();
                break;
            case "GridMapScene":
                RegisterComponent<CaptureTerritory>();
                RegisterComponent<ResourcesOnTopPanelHandler>();
                RegisterComponent<GetNormalPos>();
                RegisterComponent<GetTileInfo>();
                RegisterComponent<GameMiddleware>();
                RegisterComponent<BuildUpgradeMenu>();
                RegisterComponent<IncomeManager>();
                RegisterComponent<TileMapPathfinder>();
                RegisterComponent<HexagonTileStorage>();
                RegisterComponent<ResourceIncome>();
                RegisterComponentWithTag<Camera>(Constants.MAIN_CAMERA);
                RegisterComponentWithTag<Tilemap>(Constants.BASE_TILEMAP);
                break;
            case "GeneratorMapScene":
                RegisterComponent<GetTileInfo>();
                RegisterComponent<HexagonTileStorage>();
                RegisterComponentWithTag<Camera>(Constants.MAIN_CAMERA);
                RegisterComponentWithTag<Tilemap>(Constants.BASE_TILEMAP);
                break;
            default:
                Debug.LogWarning("Unknown scene name: " + currentSceneName);
                break;
        }
    }

    public T GetComponentByType<T>() where T : class
    {
        if (_components.TryGetValue(typeof(T), out var component))
        {
            return (T)component;
        }
     
        Debug.LogError($"Component of type {typeof(T)} is null! Called from {GetCallingClassName()}");
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
    
        Debug.LogError($"Component of type {typeof(T)} with tag {tag} is null! Called from {GetCallingClassName()}");
    
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
            Debug.LogWarning($"Component of type {typeof(T)} not found in current Scene!");
        }
    }
    
    private void RegisterComponentWithTag<T>(string tag) where T : Object
    {
        try
        {
            T component = GameObject.FindGameObjectWithTag(tag).GetComponent<T>();
            _componentsWithTag.Add(tag, component);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Component of type {typeof(T)} with tag {tag} not found in current Scene!\n -- Exception message: {e.Message} --");
        }
    }
}