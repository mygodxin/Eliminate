public class Notification
{
    private static int _index = 0;
    private static string Next()
    {
        return "N" + _index++;
    }
    public static string UpdateMoney = Notification.Next();
    public static string UpdateGem = Notification.Next();
    public static string UpdateSkinTimes = Notification.Next();
    public static string UpdateSkin = Notification.Next();
    public static string AddSkin = Notification.Next();
    public static string AddBg = Notification.Next();
    public static string UpdateBgTimes = Notification.Next();
    public static string UpdateBg = Notification.Next();
    public static string GameEnter = Notification.Next();
    public static string GameStart = Notification.Next();
    public static string ScoreUpdate = Notification.Next();
    public static string MoveEnd = Notification.Next();
    public static string GameEnd = Notification.Next();
    public static string OtherSelect = Notification.Next();
    public static string OherSelect = Notification.Next();
    public static string Eliminate = Notification.Next();
    public static string CheckAndMoveEnd = Notification.Next();
    public static string CloseGameEnd = Notification.Next();
    public static string CreateNewGrid = Notification.Next();

    public static string ElimnateGrid = Notification.Next();
    public static string ElimIceCube = Notification.Next();
    public static string ElimPollution = Notification.Next();
    public static string ElimBaoShi = Notification.Next();
    public static string ShowTurnTip = Notification.Next();
    public static string ShowCountdown = Notification.Next();
    public static string UseItem = Notification.Next();
    public static string EnableHammer = Notification.Next();
    public static string UnableHammer = Notification.Next();
    public static string UseHammer = Notification.Next();
    public static string UseLightning = Notification.Next();
    public static string UseSameBall = Notification.Next();
    public static string ActiveEM = Notification.Next();
    public static string NewRecord = Notification.Next();
    public static string InitMap = Notification.Next();
}