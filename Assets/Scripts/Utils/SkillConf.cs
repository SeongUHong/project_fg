public class SkillConf
{
    public enum Skill
    {
        Attack = 1,
        FlameBall,
        DummyRange,
    }

    public enum LaunchSkill
    {
        Attack = 1,
        FlameBall = Skill.FlameBall,
    }

    public enum RangeSkill
    {
        DummyRange = Skill.DummyRange,
    }
}
