using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchSkillController : MonoBehaviour
{
    //�߻��� ��ġ
    Vector3 _startPos;

    //����
    Vector3 _dir;

    //��Ÿ�
    float _distance;

    //����ü �ӵ�
    float _speed;

    //���ط�
    int _damage;

    //��� ���̾�
    int _layerBit;

    public void SetSkillStatus(Vector3 startPos, Vector3 dir, float distance, float speed, int damage , Define.Layer[] layers)
    {
        _startPos = startPos;
         _dir = dir;
        _distance = distance;
        _speed = speed;
        _damage = damage;

        // ��� ���̾ ��Ʈ�� ����
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
        //��� ���̾ �ƴϸ� ����
        if (!IsTarget(other.gameObject.layer))
            return;

        if (other.gameObject.GetComponent<Stat>().OnAttacked(_damage))
        {
            Cleer();
            Managers.Resource.Destroy(gameObject);
        }
    }

    // ��ų �ǰ� ����ΰ�
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
