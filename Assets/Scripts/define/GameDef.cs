public class GameDef
{
    public const int GRID_WIDTH = 9;//9;
    public const int GRID_HEIGHT = 9;//9;
    public const int GRID_TOTAL_COUNT = GRID_WIDTH * GRID_HEIGHT;
    public const int SCORE = 1;

    public const int SELF = 0;
    public const int OTHER = 1;
    public const int VideoDoubleScoreTimes = 5;
    public const int ScoreToMoney = 10;
}

public class Grid
{
    public int x;
    public int y;

    public int color;

    public GridType type;
    /// <summary>
    /// 需消除次数
    /// </summary>
    public int elimTimes; 
}

public enum GridType
{
    /// <summary>
    /// 正常状态
    /// </summary>
    Normal = 0,
    /// <summary>
    /// 冰块
    /// </summary>
    IceCube = 1,
    /// <summary>
    /// 被消除一次污染
    /// </summary>
    Pollution = 2,
    BaoShi = 3,
}

public enum GridColor
{
    RED,
    ORANGE,
    YELLOW,
    GREEN,
    BLUE,
    PURPLE
}
// A是出场动画
// B是选中动画
// C-D是向下移动时的动画，移动时间0.5秒，
// C-up是向上移动
// C-L是向左移动
// C-R是向右移动
// D是消除前
public enum AniDef
{

}

public enum GameMode
{
    FIGHT,
    PERSON
}

