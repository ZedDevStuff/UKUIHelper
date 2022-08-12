using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Helper class for creating menus
/// </summary>
public class Menu
{
    bool isActive = true;
    /// <summary>
    /// Root of the menu
    /// </summary>
    public GameObject root;
    List<GameObject> objects = new List<GameObject>();
    /// <summary>
    /// Adds an object to the menu, will appear in the center. Instanciate before calling this method
    /// </summary>
    public void AddObject(GameObject obj)
    {
        objects.Add(obj);
        obj.transform.SetParent(root.transform);
    }
    /// <summary>
    /// Remove an object from the menu. Use the same object that was added or it will not work
    /// </summary>
    public void RemoveObject(GameObject obj)
    {
        objects.Remove(obj);
    }
    /// <summary>
    /// Switch the current state of the menu
    /// </summary>
    public void Toggle()
    {
        root.SetActive(isActive);
        isActive = !isActive;
    }
}