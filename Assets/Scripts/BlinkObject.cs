using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkObject : MonoBehaviour
{
    public GameObject clonedObject; // 미리 지정된 복사본 오브젝트
    private bool shouldBlink = false; // 깜빡임 여부를 제어하는 bool 값

    public void BlinkOn()
    {
        shouldBlink = true;
    }

    public void BlinkOut()
    {
        shouldBlink = false;
    }
    void Update()
    {
        if (shouldBlink)
        {
            StartBlinking(); // 깜빡이기 시작
        }
        else
        {
            StopBlinking(); // 깜빡임 중지
        }
    }

    void StartBlinking()
    {
        if (!IsInvoking("ToggleObjectVisibility"))
        {
            InvokeRepeating("ToggleObjectVisibility", 0f, 1f); // 1초마다 깜빡이기
        }
    }

    void StopBlinking()
    {
        CancelInvoke("ToggleObjectVisibility");
        clonedObject.SetActive(false); // 깜빡임이 중지될 때는 오브젝트를 활성화 상태로 유지
    }

    void ToggleObjectVisibility()
    {
        
        clonedObject.SetActive(!clonedObject.activeSelf);
    }
}
