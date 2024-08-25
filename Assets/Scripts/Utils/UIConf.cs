public class UIConf
{
    public enum UIEvent
    {
        Click,
        Drag,
        Press,
        PointerDown,
        PointerUp,
    }

    public static readonly float[] HpBarPlayerRgb = new float[]{ 117 / 255f, 1, 84 / 255f };
    public static readonly float[] HpBarUnitRgb = new float[] { 84 / 255f, 153 / 255f, 1 };
    public static readonly float[] HpBarMonsterRgb = new float[] { 1, 83 / 255f, 83 / 255f };
    public static readonly float[] HpBarEnemyObjectRgb = new float[] { 199 / 255f, 83 / 255f, 1 };

    public const string SKILL_LEVEL_UP_TXT = "Level Up";
    public const string SKILL_LEVEL_MAX_TXT = "Max";
    public const string SKILL_NO_LEVEL_TXT = "None";
    public const string SKILL_GET_TXT = "Get";
}
