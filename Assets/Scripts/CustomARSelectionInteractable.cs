using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;


public class CustomARSelectionInteractable : ARSelectionInteractable
{
    private float lastClickTime = 0f;

    private const float doubleClick = 0.5f;
    private const float rotTime = 1f;
    private bool bselected;

    public GameObject particleObj;

    private Quaternion initialRotation;
    private bool isDragging = false;
    private bool bselectedenter = false;

    private bool benotexit = false;

    private bool isTouching = false;

    private void Update()
    {
        if(bselectedenter)
        {
          
        }

      
    }


    //transform.position = transform.position + new Vector3(transform.position.x + t, 0f, 0f);
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        GetComponent<BlinkObject>().BlinkOn();

        base.OnSelectEntered(args);

        //파티클 Play();
        this.transform.GetComponent<ParticleSystem>().Play();

        //회전
        StartCoroutine(RotateOverTime(90f, rotTime));

        bselectedenter = true;

    }


    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        GetComponent<BlinkObject>().BlinkOut();
        //transform.rotation = initialRotation;


        this.transform.GetComponent<ParticleSystem>().Stop();

    }

    //private void OnSingleClick(SelectEnterEventArgs args)
    //{
    //    //파티클 Play();
    //    this.transform.GetComponent<ParticleSystem>().Play();

    //    //회전
    //    StartCoroutine(RotateOverTime(90f, rotTime));

    //    bselectedenter = true;

    //    initialRotation = transform.rotation;

    //}

    //private void OnDoubleClick(SelectEnterEventArgs args)
    //{
    //    float t = Time.time;

    //    transform.position = transform.position + new Vector3(t, 0,0);  

    //}


    private System.Collections.IEnumerator RotateOverTime(float angle, float duration)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(transform.eulerAngles.x + angle, transform.eulerAngles.y, transform.eulerAngles.z);
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, timeElapsed / duration);
            yield return null;
        }


        transform.rotation = endRotation;

    }


}
