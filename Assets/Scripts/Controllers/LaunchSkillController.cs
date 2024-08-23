using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchSkillController : MonoBehaviour
{
    //발사한 위치
    Vector3 _startPos;

    //방향
    Vector3 _dir;

    //사거리
    float _distance;

    //투사체 속도
    float _speed;

    //피해량
    int _damage;

    //대상 레이어
    int _layerBit;

    public void SetSkillStatus(Vector3 startPos, Vector3 dir, float distance, float speed, int damage , Define.Layer[] layers)
    {
        _startPos = startPos;
         _dir = dir;
        _distance = distance;
        _speed = speed;
        _damage = damage;

        // 대상 레이어를 비트로 산출
        int layerBit = 0;
        foreach (int layer in layers)
        {
            if (layer == 1)
            {
                layerBit |= 1;
                continue;
            }

            int bit = 1 << (layer - 1);
            layerBit |= bit;
        }

        _layerBit = layerBit;
    }

    void Update()
    {
        if(_distance < (_startPos - transform.position).magnitude)
        {
            Cleer();
            Managers.Resource.Destroy(gameObject);
        }
    }

    public void StartLaunch()
    {
        gameObject.transform.rotation = Quaternion.LookRotation(_dir);
        gameObject.GetComponent<Rigidbody>().AddForce(_dir * _speed, ForceMode.VelocityChange);
    }

    void OnTriggerEnter(Collider other)
    {
        //대상 레이어가 아니면 리턴
        if (!IsTarget(other.gameObject.layer))
            return;

        if (other.gameObject.GetComponent<Stat>().OnAttacked(_damage))
        {
            Cleer();
            Managers.Resource.Destroy(gameObject);
        }
    }

    // 스킬 피격 대상인가
    public bool IsTarget(int layer)
    {
        int targetBit = 0;
        if (layer == 1)
        {
            targetBit = 1;
        }
        else
        {
            targetBit = 1 << (layer - 1);
        }

        if ((_layerBit & targetBit) > 0)
            return true;

        return false;
    }

    void Cleer()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
