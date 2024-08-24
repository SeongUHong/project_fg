public class SkillConf
{
    public enum LaunchSkill
    {
        Attack = 1,
        FlameBall,
    }

    public enum RangeSkill
    {
        DummyRange = 1,
    }

    public const float FLAME_BALL_COOLTIME = 3.0f;
    public const float DUMMY_RANGE_ACTIVE_TIME = 3.0f;
    public const float DUMMY_RANGE_DAMAGE_TICK_INTERVAL = 0.2f;
    public const float DUMMY_RANGE_COOLTIME = 3.0f;
}
