public class SkillConf
{
    public enum Skill
    {
        FlameBall = 1,
        FreezeCircle,
    }

    public enum LaunchSkill
    {
        FlameBall = Skill.FlameBall,
    }

    public enum RangeSkill
    {
        FreezeCircle = Skill.FreezeCircle,
    }

    public const int MIN_LEVEL = 1;
}
