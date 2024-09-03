using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit;

public class ImageRecog : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;

    public List<GameObject> hits = new List<GameObject>();
    private Dictionary<string, GameObject> objs = new Dictionary<string, GameObject>();


    private void Awake()
    {
        foreach (GameObject obj in hits)
        {
            string name = obj.name;
            objs.Add(name, obj);
        }
    }

   

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }
    
    private void ImgChanged(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        GameObject obj = objs[name];
        obj.transform.position = trackedImage.transform.position;
        obj.SetActive(true);

     

    }
    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {

            ImgChanged(trackedImage);
               
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {

            ImgChanged(trackedImage);

        }
    }

   

}
