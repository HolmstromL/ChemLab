﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    private bool selected = false;
    private Shader outlinedShader;
    private Shader defaultShader;

    private void Start()
    {
        outlinedShader = Shader.Find("Outlined");
        defaultShader = Shader.Find("Unlit/Color");
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0)
            && !selected)
        {
            selected = true;
            AtomManager.selectedObjects.Add(gameObject);
            GetComponent<Renderer>().material.shader = outlinedShader;
        }
        else if (Input.GetMouseButton(1)
            && selected)
        {
            selected = false;
            AtomManager.selectedObjects.Remove(gameObject);
            GetComponent<Renderer>().material.shader = defaultShader;
        }
    }
}
