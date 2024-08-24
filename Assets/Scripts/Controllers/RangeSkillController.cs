using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSkillController : MonoBehaviour
{
	// スキル使用者
	protected GameObject _owner;
	// 持続時間
	protected float _activeTime;
	// ダメージ付与間隔
	protected float _tickInterval;
	// ダメージ
	protected int _damage;
	// 対象レイヤー
	int _layerBit;
	// 範囲内に入った対象リスト
	List<Collider> _targetList = new List<Collider>();
	
	public void SetSkillStatus(GameObject owner, float activeTime, float tickInterval, int damage, Define.Layer[] layers)
	{
		_owner = owner;
		_activeTime = activeTime;
		_damage = damage;
		_tickInterval = tickInterval;

		// 対象レイヤーをBitで算出
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
		// スキル使用者を追う
		gameObject.transform.position = _owner.GetComponent<Collider>().bounds.min;
	}

	// スキルを発動させる
	public void ActiveSkill()
	{

		StartCoroutine(DisableAfterTime());
		StartCoroutine(StartDamageTick());
	}

	void OnTriggerEnter(Collider target)
	{
		OnEnterRange(target);
	}
	
	void OnTriggerExit(Collider target)
	{
		OnRangeExit(target);
	}

	// 対象が範囲内に入った時
	protected virtual void OnEnterRange(Collider target)
    {
		// 対象レイヤーでなければReturn
		if (!IsTarget(target.gameObject.layer))
			return;

		// ダメージを与える対象に追加する
		_targetList.Add(target);
	}

	// 対象が範囲から離れた時
	protected virtual void OnRangeExit(Collider target)
    {
		_targetList.Remove(target);
	}

	// 一定間隔で対象にダメージを与える
	IEnumerator StartDamageTick()
	{
		while (true)
		{
			foreach (Collider target in _targetList)
			{
				target.GetComponent<Stat>().OnAttacked(_damage);
			}

			yield return new WaitForSeconds(_tickInterval);
		}
	}

	// 一定時間後にスキル非活性のフラグを立てる
	IEnumerator DisableAfterTime()
	{
		yield return new WaitForSeconds(_activeTime);

		// スキル非活性
		Cleer();
		Managers.Resource.Destroy(gameObject);
	}

	// スキル対象レイヤーなのか
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

	protected void Cleer()
	{
		StopCoroutine(StartDamageTick());
	}
}
