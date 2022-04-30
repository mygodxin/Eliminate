using System.Collections.Generic;
/// <summary>
/// 地图说明
/// 0为空格子
/// 大于0时是地图图片后缀
/// 小于0时转正数为地图图片后缀，不放置球
/// </summary>
[System.Serializable]
public class CMap
{
    public int total;
    public List<List<int>> map;
}
[System.Serializable]
public class CGrid
{
    public int[] grid;
}