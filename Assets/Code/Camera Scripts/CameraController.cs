﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private Vector3 moveDir, newPosition, mousePos;
    private Camera mainCamera;
    private float horAxis, verAxis, mouseScroll;
    private float yLim, xLim, aspectRatio, wallWidth;
    private float newZoom, startTime;
    private float camSize, previousCamSize;
    private bool zoomChanging = false;

    public Transform topWall, rightWall;
    public float camSpeed = 1;
    public float zoomDuration = 10;

    private void Start()
    {
        //Lock newPosition Z value
        newPosition.z = transform.position.z;

        wallWidth = topWall.gameObject.GetComponent<BoxCollider>().size.y;

        //Setup for camera limitations;
        yLim = topWall.position.y - (wallWidth / 2);
        xLim = rightWall.position.x - (wallWidth / 2);
        mainCamera = Camera.main;
        newZoom = mainCamera.orthographicSize;
    }

    void Update ()
    {

#region Setup

        aspectRatio = Screen.width / (float)Screen.height;
        horAxis = Input.GetAxis("Horizontal");
        verAxis = Input.GetAxis("Vertical");
        mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        mousePos = Input.mousePosition;
        camSize = mainCamera.orthographicSize;

        #endregion

#region Zoom
        
        if (mouseScroll > 0 
            && camSize > 1)
        {
            //Zoom out from cursor position
            ZoomCamera(mainCamera.ScreenToWorldPoint(mousePos), 1);
        }
        else if (mouseScroll < 0
                && camSize < topWall.position.y - (wallWidth / 2))
        {
            //Zoom in to cursor position
            ZoomCamera(mainCamera.ScreenToWorldPoint(mousePos), -1);
        }

        float zoomVelocity = 0f;
        mainCamera.orthographicSize = Mathf.SmoothDamp(
            camSize,
            newZoom,
            ref zoomVelocity,
            Time.deltaTime * Mathf.Pow(zoomDuration, 2));
        
        zoomChanging = (previousCamSize == camSize) ? false : true;

        #endregion

#region Movement
        //Set the new position
        float speedMultiplier = Time.deltaTime * camSpeed * (camSize * 10);
        newPosition.x += horAxis * Time.deltaTime * speedMultiplier;
        newPosition.y += verAxis * Time.deltaTime * speedMultiplier;

        //Lerp to the new position deoending on whether the zoom is active or not
        if (zoomChanging)
        {
            Vector3 moveVel = new Vector3(0, 0, 0);
            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref moveVel, Time.deltaTime * Mathf.Pow(zoomDuration, 2));
        }
        else if (!zoomChanging)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                newPosition,
                0.25f);
        }

        #endregion

    }

    private void LateUpdate()
    {
        //Assign the limit of the camera's movement
        float maxY = yLim - camSize;
        float minY = maxY * -1;
        float maxX = xLim - (camSize * aspectRatio);
        float minX = maxX * -1;

        //Clamp the newPosition vector to the limits
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        previousCamSize = camSize;
    }

    /// <summary>
    /// Method for zooming the camera towards the mouse position
    /// </summary>
    /// <param name="zoomTowards">The mouse position</param>
    /// <param name="amount">How much the camera size should change per scroll</param>
    private void ZoomCamera(Vector3 zoomTowards, float amount)
    {
        if (!zoomChanging)
            startTime = Time.time;
        newZoom -= amount;
        newZoom = Mathf.Clamp(newZoom,
            1,
            topWall.position.y - (wallWidth / 2));

        float multiplier = (1.0f / newZoom * amount);

        newPosition += (zoomTowards - transform.position) * multiplier;
        
    }
}