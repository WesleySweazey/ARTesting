using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;

public class ARTapToPlace : MonoBehaviour
{
    public GameObject planeTarget;
    public GameObject objectToPlace;
    private ARSessionOrigin arOrigin;
    public ARRaycastManager arRaycastManager;
    private Pose targetPose;
    private bool planeFound = false;
    // Start is called before the first frame update
    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTargetPose();
        UpdateTargetPosition();
        if(planeFound && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }

    private void PlaceObject()
    {
        Instantiate(objectToPlace, targetPose.position,targetPose.rotation);
    }

    private void UpdateTargetPosition()
    {
        if (planeFound)
        {
            planeTarget.SetActive(true);
            planeTarget.transform.SetPositionAndRotation(targetPose.position, targetPose.rotation);
        }
        else
        {
            planeTarget.SetActive(false);
        }
    }

    private void UpdateTargetPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arRaycastManager.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
        planeFound = hits.Count > 0;
        if (planeFound)
        {
            targetPose = hits[0].pose;
        }
    }
}
