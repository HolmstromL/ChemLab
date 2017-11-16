﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    [HideInInspector] public bool selected = false;
    [HideInInspector] public bool merged = false;
    public Shader outlinedShader;
    public Shader defaultShader;

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0)
            && !selected)
        {
            if (merged)
            {
                GroupSelect();
            }
            else
            {
                Select();
            }
        }
        else if (Input.GetMouseButton(1)
            && selected)
        {
            if (merged)
            {
                GroupDeSelect();
            }
            else
            {
                DeSelect();
            }
        }
    }

    public void Select()
    {
        selected = true;
        ObjectManager.selectedObjects.Add(gameObject);
        UpdateShader(gameObject, ManagerStorage.OutlinedShader);
    }

    public void DeSelect(bool mergeRemove = false)
    {
        selected = false;
        UpdateShader(gameObject, ManagerStorage.DefaultShader);
        if (!mergeRemove)
        {
            ObjectManager.selectedObjects.Remove(gameObject);
        }
    }

    public void GroupSelect()
    {
        Transform parent = transform.parent;
        foreach(Transform child in parent)
        {
            child.gameObject.GetComponent<Selector>().selected = true;
            UpdateShader(child.gameObject, ManagerStorage.OutlinedShader);
        }
    }

    public void GroupDeSelect()
    {
        Transform parent = transform.parent;
        foreach (Transform child in parent)
        {
            child.gameObject.GetComponent<Selector>().selected = false;
            UpdateShader(child.gameObject, ManagerStorage.DefaultShader);
        }
    }


    private void UpdateShader(GameObject target, Shader shader)
    {
        target.GetComponent<Renderer>().material.shader = shader;
    }
}
