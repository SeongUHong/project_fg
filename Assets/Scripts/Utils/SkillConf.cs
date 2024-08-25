public class SkillConf
{
    public enum Skill
    {
        FlameBall = 1,
        DummyRange,
    }

    public enum LaunchSkill
    {
        FlameBall = Skill.FlameBall,
    }

    public enum RangeSkill
    {
        DummyRange = Skill.DummyRange,
    }

    public const int MIN_LEVEL = 1;
}
