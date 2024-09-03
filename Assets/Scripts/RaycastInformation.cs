using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using TMPro;

public class RaycastInformation : MonoBehaviour
{
    //public ARRaycastManager raycastManager;

    //private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    //private Dictionary<string, GameObject> objs = new Dictionary<string, GameObject>();

    //public TMP_Text textUI;


    //private void Awake()
    //{
    //    foreach(GameObject obj in hits)
    //    {
    //        string name = obj.name;
    //        objs.Add(name, obj);
    //    }
    //}
    //private void Update()
    //{
    //    Vector2 sreenCenterPos = Camera.main.ViewportToScreenPoint(new Vector2 (0.5f, 0.5f));
    //    if (raycastManager.Raycast(sreenCenterPos, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
    //    {
    //        if(hits.Count > 0) 
    //        {
    //            textUI.text = hits[0].trackableId.ToString();
    //        }
    //    }
    //}

}
