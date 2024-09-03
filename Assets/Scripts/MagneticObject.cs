using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MagneticObject : MonoBehaviour
{
    public Transform target; // 붙을 대상 오브젝트
    public float particleDistance = 2.5f; // 합체가 시작될 거리
    public float magneticDistance = 1.0f; // 붙을 거리
    public float moveSpeed = 1.0f; // 붙을 때의 이동 속도
    private bool isMagnetActive = true; // 자석 효과 활성화 여부

    public GameObject combineParticleObj;
    private GameObject spawnedParticleObj;

    private bool hasRotated = false;
    private bool isRotating = false;
    public float rotationDuration = 1.0f;
    private Quaternion originalRotationThisObject; // 원래 회전 값 저장
    private Quaternion originalRotationTarget; // 타겟의 원래 회전 값 저장
    public TextMeshProUGUI txt;


    void Update()
    {
        if (isMagnetActive)
        {

            // 두 오브젝트 사이의 거리 계산
            float distance = Vector3.Distance(transform.position, target.position);
            if(txt != null )
            {
                txt.text = distance.ToString();
            }
           

            // 합체 거리에 도달하면 파티클 시스템 활성화
            if (distance <= particleDistance)
            {
                ActivateParticles();
            }
            else
            {
                DeactivateParticles();
            }

            // 일정 거리 이하일 때
            if (!hasRotated && distance <= magneticDistance && !isRotating)
            {

              
                // 타겟이 현재 오브젝트의 왼쪽에 있는지 확인
                Vector3 directionToTarget = target.position - transform.position;

                // 오른쪽이나 왼쪽 판단 기준은 transform.right(오브젝트의 오른쪽 방향 벡터)를 사용
                float dotProduct = Vector3.Dot(transform.right, directionToTarget.normalized);

                originalRotationThisObject = transform.rotation;
                originalRotationTarget = target.rotation;

                // 두 오브젝트가 거리 내에 있을 때만 회전
                if (dotProduct < 0) // 타겟이 왼쪽에 있을 때
                {
                    StartCoroutine(RotateObjects(Vector3.forward * -90, Vector3.forward * 90));
                }
                else if (dotProduct > 0) // 타겟이 오른쪽에 있을 때
                {
                    StartCoroutine(RotateObjects(Vector3.forward * 90, Vector3.forward * -90));
                }

                hasRotated = true;
            }
            // 일정 거리 밖으로 벗어날 때
            else if (hasRotated && distance > magneticDistance && !isRotating)
            {
                StartCoroutine(ResetRotation());
            }

        

        }
    }


    void OnTriggerEnter(Collider other)
    {
        // 트리거 충돌 시 자석 효과 멈추기
        isMagnetActive = false;
    }



    private IEnumerator RotateObjects(Vector3 targetRotationThisObject, Vector3 targetRotationTarget)
    {
        isRotating = true; // 회전 중 상태로 설정

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
       
        isRotating = false; // 회전 완료 상태로 설정
    }


    private IEnumerator ResetRotation()
    {
        isRotating = true;

        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / rotationDuration;

            // 원래 회전 값으로 복구
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
        hasRotated = false; // 회전 완료 후 상태를 초기화
    }



    // 파티클 시스템을 활성화하는 함수
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
        // 현재 오브젝트와 타겟의 중간 위치를 계산
        Vector3 midPoint = (transform.position + target.position) / 2;

        // combineParticleObj를 중간 위치에 생성
        spawnedParticleObj = Instantiate(combineParticleObj, midPoint, Quaternion.identity);

        // 생성된 오브젝트에서 파티클 시스템 활성화
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