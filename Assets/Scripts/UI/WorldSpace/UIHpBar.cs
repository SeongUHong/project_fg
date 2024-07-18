using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHpBar : UIBase
{
    //hp�� ��ġ
    [SerializeField]
    Vector3 _hpPos = new Vector3(0, 0.3f, 0);

    //��� ������Ʈ
    Transform _parent;

    //��� ������Ʈ ����
    Stat _stat;

    //��� ������Ʈ ����
    float _parentHeight;

    enum GameObjects
    {
        HpBar,
    }

    enum Images
    {
        Fill,
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        _stat = transform.parent.GetComponent<Stat>();

        _parent = transform.parent;

        //��� ������Ʈ ���̰� ���
        _parentHeight = _parent.GetComponent<Collider>().bounds.size.y;

        float[] rgb;
        if (Managers.Game.Player == _parent.gameObject)
        {
            rgb = UIConf.HpBarPlayerRgb;
        }
        else if (_parent.gameObject.layer == (int)Define.Layer.Unit)
        {
            rgb = UIConf.HpBarUnitRgb;
        }
        else if (_parent.gameObject.layer == (int)Define.Layer.EnemyStaticObject)
        {
            rgb = UIConf.HpBarEnemyObjectRgb;
            // ũ�� ����
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            rgb = UIConf.HpBarMonsterRgb;
        }

        //hp�� ����
        GetImage((int)Images.Fill).GetComponent<Image>().color = new Color(rgb[0], rgb[1], rgb[2]);
    }

    private void Update()
    {
        transform.SetPositionAndRotation(_parent.position + Vector3.up * _parentHeight + _hpPos, Camera.main.transform.rotation);

        float ratio = _stat.Hp / (float)_stat.MaxHp;
        SetHpRatio(ratio);
    }

    public void SetHpRatio(float ratio)
    {
        GetGameObject((int)GameObjects.HpBar).GetComponent<Slider>().value = ratio;
    }

}
