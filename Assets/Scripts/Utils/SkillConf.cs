public class SkillConf
{
    public enum Skill
    {
        FlameBall = 1,
        FreezeCircle,
        BeholderAttack,
    }

    public enum LaunchSkill
    {
        FlameBall = Skill.FlameBall,
        BeholderAttack = Skill.BeholderAttack,
    }

    public enum RangeSkill
    {
        FreezeCircle = Skill.FreezeCircle,
    }

    public const int MIN_LEVEL = 1;
}
