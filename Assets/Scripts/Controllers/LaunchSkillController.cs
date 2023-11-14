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
    int[] _layers;


    public void SetSkillStatus(Vector3 startPos, Vector3 dir, float distance, float speed, int damage , int[] layers)
    {
        _startPos = startPos;
         _dir = dir;
        _distance = distance;
        _speed = speed;
        _damage = damage;
        _layers = layers;
    }

    void Start()
    {
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
        int targetLayer = other.gameObject.layer;
        bool canAttack = false;
        foreach (int layer in _layers)
        {
            if(targetLayer == layer)
            {
                canAttack = true;
                break;
            }
        }
        if (canAttack == false) return;

        if (other.gameObject.GetComponent<Stat>().OnAttacked(_damage))
        {
            Cleer();
 
            Managers.Resource.Destroy(gameObject);
        }
    }

    void Cleer()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
