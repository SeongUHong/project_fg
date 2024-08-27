public class CharacterConf
{
    public enum Unit
    {
        Knight = 1,
        Sniper,
        Warrior,
    }

    public enum Monster
    {
        EarthElemental = 1,
        FireElemental,
        IceElemental,
    }

    // 캐릭터 상태의 따른 navMeshAgent의 avoidancePriority값
    public enum AvoidancePriority
    {
        Stop = 50,
        Move,
    }

    // 적 타겟 갱신 간격
    public const float UPDATE_LOCK_ON_INTERVAL = 0.2f;

    // 가장 낮은 레벨
    public const int MIN_LEVEL = 1;
}
