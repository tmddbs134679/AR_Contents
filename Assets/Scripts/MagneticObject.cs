using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MagneticObject : MonoBehaviour
{
    public Transform target; // ���� ��� ������Ʈ
    public float particleDistance = 2.5f; // ��ü�� ���۵� �Ÿ�
    public float magneticDistance = 1.0f; // ���� �Ÿ�
    public float moveSpeed = 1.0f; // ���� ���� �̵� �ӵ�
    private bool isMagnetActive = true; // �ڼ� ȿ�� Ȱ��ȭ ����

    public GameObject combineParticleObj;
    private GameObject spawnedParticleObj;

    private bool hasRotated = false;
    private bool isRotating = false;
    public float rotationDuration = 1.0f;
    private Quaternion originalRotationThisObject; // ���� ȸ�� �� ����
    private Quaternion originalRotationTarget; // Ÿ���� ���� ȸ�� �� ����
    public TextMeshProUGUI txt;


    void Update()
    {
        if (isMagnetActive)
        {

            // �� ������Ʈ ������ �Ÿ� ���
            float distance = Vector3.Distance(transform.position, target.position);
            if(txt != null )
            {
                txt.text = distance.ToString();
            }
           

            // ��ü �Ÿ��� �����ϸ� ��ƼŬ �ý��� Ȱ��ȭ
            if (distance <= particleDistance)
            {
                ActivateParticles();
            }
            else
            {
                DeactivateParticles();
            }

            // ���� �Ÿ� ������ ��
            if (!hasRotated && distance <= magneticDistance && !isRotating)
            {

              
                // Ÿ���� ���� ������Ʈ�� ���ʿ� �ִ��� Ȯ��
                Vector3 directionToTarget = target.position - transform.position;

                // �������̳� ���� �Ǵ� ������ transform.right(������Ʈ�� ������ ���� ����)�� ���
                float dotProduct = Vector3.Dot(transform.right, directionToTarget.normalized);

                originalRotationThisObject = transform.rotation;
                originalRotationTarget = target.rotation;

                // �� ������Ʈ�� �Ÿ� ���� ���� ���� ȸ��
                if (dotProduct < 0) // Ÿ���� ���ʿ� ���� ��
                {
                    StartCoroutine(RotateObjects(Vector3.forward * -90, Vector3.forward * 90));
                }
                else if (dotProduct > 0) // Ÿ���� �����ʿ� ���� ��
                {
                    StartCoroutine(RotateObjects(Vector3.forward * 90, Vector3.forward * -90));
                }

                hasRotated = true;
            }
            // ���� �Ÿ� ������ ��� ��
            else if (hasRotated && distance > magneticDistance && !isRotating)
            {
                StartCoroutine(ResetRotation());
            }

        

        }
    }


    void OnTriggerEnter(Collider other)
    {
        // Ʈ���� �浹 �� �ڼ� ȿ�� ���߱�
        isMagnetActive = false;
    }



    private IEnumerator RotateObjects(Vector3 targetRotationThisObject, Vector3 targetRotationTarget)
    {
        isRotating = true; // ȸ�� �� ���·� ����

        Quaternion startRotationThisObject = transform.rotation;
        Quaternion startRotationTarget = target.rotation;

        Quaternion endRotationThisObject = Quaternion.Euler(transform.rotation.eulerAngles + targetRotationThisObject);
        Quaternion endRotationTarget = Quaternion.Euler(target.rotation.eulerAngles + targetRotationTarget);

        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / rotationDuration;

            transform.rotation = Quaternion.Slerp(startRotationThisObject, endRotationThisObject, t);
            target.rotation = Quaternion.Slerp(startRotationTarget, endRotationTarget, t);

            yield return null;
        }

        transform.rotation = endRotationThisObject;
        target.rotation = endRotationTarget;

        if(spawnedParticleObj == null)
        {
            TrySummon();
        }
       
        isRotating = false; // ȸ�� �Ϸ� ���·� ����
    }


    private IEnumerator ResetRotation()
    {
        isRotating = true;

        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / rotationDuration;

            // ���� ȸ�� ������ ����
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotationThisObject, t);
            target.rotation = Quaternion.Slerp(target.rotation, originalRotationTarget, t);

            yield return null;
        }

        transform.rotation = originalRotationThisObject;
        target.rotation = originalRotationTarget;

        if(spawnedParticleObj != null)
        {
            Destroy(spawnedParticleObj);
            spawnedParticleObj = null;
        }
        

        isRotating = false;
        hasRotated = false; // ȸ�� �Ϸ� �� ���¸� �ʱ�ȭ
    }



    // ��ƼŬ �ý����� Ȱ��ȭ�ϴ� �Լ�
    void ActivateParticles()
    {
        Transform waterfallEffectTransform1 = transform.Find("WaterfallEffect");
        Transform waterfallEffectTransform2 = target.Find("WaterfallEffect");

        ParticleSystem particle1 = waterfallEffectTransform1.GetComponent<ParticleSystem>();
        ParticleSystem particle2 = waterfallEffectTransform2.GetComponent<ParticleSystem>();
        if (waterfallEffectTransform1 != null && !particle1.isPlaying)
        {
            waterfallEffectTransform1.gameObject.SetActive(true);
            particle1.Play();
        }

        if (waterfallEffectTransform2 != null && !particle2.isPlaying)
        {
            waterfallEffectTransform2.gameObject.SetActive(true);
            particle2.Play();
        }
    }

    void DeactivateParticles()
    {
        Transform waterfallEffectTransform1 = transform.Find("WaterfallEffect");
        Transform waterfallEffectTransform2 = target.Find("WaterfallEffect");

        ParticleSystem particle1 = waterfallEffectTransform1.GetComponent<ParticleSystem>();
        ParticleSystem particle2 = waterfallEffectTransform2.GetComponent<ParticleSystem>();

        if (waterfallEffectTransform1 != null && particle1.isPlaying)
        {
            waterfallEffectTransform1.gameObject.SetActive(false);
            particle1.Stop();
        }

        if (waterfallEffectTransform2 != null && particle2.isPlaying)
        {
            waterfallEffectTransform2.gameObject.SetActive(false);
            particle2.Stop();
        }

    }


    void TrySummon()
    {
        // ���� ������Ʈ�� Ÿ���� �߰� ��ġ�� ���
        Vector3 midPoint = (transform.position + target.position) / 2;

        // combineParticleObj�� �߰� ��ġ�� ����
        spawnedParticleObj = Instantiate(combineParticleObj, midPoint, Quaternion.identity);

        // ������ ������Ʈ���� ��ƼŬ �ý��� Ȱ��ȭ
        ParticleSystem[] particles = spawnedParticleObj.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particles)
        {
            if (!particle.isPlaying)
            {
                particle.Play();
            }
        }
    }

}