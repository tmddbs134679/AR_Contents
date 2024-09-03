using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkObject : MonoBehaviour
{
    public GameObject clonedObject; // �̸� ������ ���纻 ������Ʈ
    private bool shouldBlink = false; // ������ ���θ� �����ϴ� bool ��

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
            StartBlinking(); // �����̱� ����
        }
        else
        {
            StopBlinking(); // ������ ����
        }
    }

    void StartBlinking()
    {
        if (!IsInvoking("ToggleObjectVisibility"))
        {
            InvokeRepeating("ToggleObjectVisibility", 0f, 1f); // 1�ʸ��� �����̱�
        }
    }

    void StopBlinking()
    {
        CancelInvoke("ToggleObjectVisibility");
        clonedObject.SetActive(false); // �������� ������ ���� ������Ʈ�� Ȱ��ȭ ���·� ����
    }

    void ToggleObjectVisibility()
    {
        
        clonedObject.SetActive(!clonedObject.activeSelf);
    }
}
