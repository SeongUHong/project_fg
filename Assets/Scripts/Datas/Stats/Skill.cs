using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace data
{
	[Serializable]
	public class Skill
	{
		// レベル(主キー)
		public int level;
		// 攻撃力
		public int offence;
		// 速度
		public float move_speed;
		// 最大距離(影響範囲)
		public float distance;
		// 活性時間
		public float active_time;
		// ダメージを与える間隔
		public float tick_interval;
		// クールタイム
		public float cooltime;
	}

	[Serializable]
	public class SkillLoader : ILoader<int, Skill>
	{
		public List<Skill> skills = new List<Skill>();

		public Dictionary<int, Skill> MakeDict()
		{
			Dictionary<int, Skill> dict = new Dictionary<int, Skill>();

			foreach (Skill skill in skills)
			{
				dict.Add(skill.level, skill);
			}

			return dict;
		}
	}
}
