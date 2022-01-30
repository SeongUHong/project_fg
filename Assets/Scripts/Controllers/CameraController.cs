using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //�÷��̾� ĳ����
    [SerializeField]
    private GameObject _player = null;

    //ī�޶� �Ÿ�
    [SerializeField]
    private Vector3 _delta = new Vector3(0, 8.0f, -5.0f);

    public void SetPlayer(GameObject player)
    {
        _player = player;
    }

    void LateUpdate()
    {
        if (Util.IsValid(_player) == false)
        {
            return;
        }
        transform.position = _player.transform.position + _delta;
        transform.LookAt(_player.transform);
    }
}
