using UnityEngine;
using System.Collections.Generic;
using mm;
using FairyGUI;
using System;

public class GameCommond
{
    public static readonly GameCommond inst = new GameCommond();

    Grid[,] _gridMap;
    int _otherScore;
    int _myScore;
    public int myScore
    {
        get { return this._myScore; }
        set { this._myScore += value; }
    }
    public int otherScore { get { return this._otherScore; } }
    public int curPlayer = -1;
    public int curStep = 0;
    private int YetiStep = 5;
    private int YetiCreateNeedScore = 500;
    private int IceCubeCreateNeedScore = 1000;
    private int BaoShiCreateNeedScore = 1200;
    private int maxYeTi = 2;
    bool isCheckCountdown;
    int comboCount = 0;
    bool isCreateNew;
    bool isElimEnd;
    int tempScore;
    //无尽模式
    public GameMode gameMode;
    bool isEmActive;
    int recordScore;
    public int[] curMap;
    private bool isElimByItem = false;

    public void initGame(GameMode mode)
    {
        this.isElimByItem = false;
        this.curPlayer = -1;
        this._gridMap = null;
        this.isCheckCountdown = true;
        this.gameMode = mode;
        this._otherScore = 0;
        this._myScore = 0;
        this.curStep = 0;
        this.recordScore = Storage.GetInt(DataDef.RECORD);
        CMap map = ConfigManager.inst.getMapConfig();
        int random = Util.getRandom(0, map.total);
        var p = map.GetType().GetProperty("m1", System.Reflection.BindingFlags.Public);
        // var random = Util.getRandom(0,map.map.Count);
        for (int i = 0; i < map.total; i++)
        {
            if (i == random)
            {
                this.curMap = map.map[i].ToArray();
                break;
            }
        }
        // this.curMap = map.map.get;
        // this.initMap();
    }

    public void setCurPlayer()
    {
        if (this.gameMode == GameMode.PERSON)
        {
            this.curPlayer = GameDef.SELF;
            return;
        }
        if (this.curPlayer == -1)
        {
            this.curPlayer = Util.getRandom(0, 2) > 0 ? GameDef.SELF : GameDef.OTHER;
        }
        else
        {
            // this.curPlayer = this.curPlayer == GameDef.SELF ? GameDef.OTHER : GameDef.OTHER;
            if (this.curPlayer == GameDef.SELF)
            {
                this.curPlayer = GameDef.OTHER;
            }
            else
            {
                this.curPlayer = GameDef.SELF;
                mvc.send(Notification.ShowTurnTip);
            }
        }
        // this.curPlayer = 0;
        //this.curPlayer = GameDef.SELF;
        Debug.Log("当前玩家=" + (this.curPlayer == GameDef.SELF ? "自己" : "机器人"));
        //机器人随机移动
        if (this.curPlayer == GameDef.OTHER)
        {
            this.checkVoidGrid();
        }
    }

    public void checkVoidGrid()
    {
        //判断所有能消除的点
        for (int i = 0; i < this._gridMap.Length; i++)
        {
            int x = i % GameDef.GRID_WIDTH;
            int y = i / GameDef.GRID_WIDTH;
            if (this._gridMap[y, x] == null && this.isOnMapByIndex(i))
            {
                Point p = this.getCanEliminatePoint(x, y);
                if (p != null)
                {
                    // return pointArr[0];
                    Point[] points = { p, new Point(x, y) };
                    // Debug.Log("找到消除点=" + p.x + ",y=" + p.y);
                    mvc.send(Notification.OtherSelect, points);
                    return;
                }
                else
                {
                    //Debug.Log("检查空格子为空");
                }
            }
        }
        for (int i = 0; i < this._gridMap.Length; i++)
        {
            int x = i % GameDef.GRID_WIDTH;
            int y = i / GameDef.GRID_WIDTH;
            if (this._gridMap[y, x] != null)
            {
                Point[] pointArr = this.getRoundMove(x, y);
                if (pointArr.Length > 0)
                {
                    // return pointArr[0];
                    Point[] points = { new Point(x, y), pointArr[0] };
                    mvc.send(Notification.OtherSelect, points);
                    return;
                }
                else
                {
                    Debug.Log("检查空格子为空");
                }
            }
        }
        return;
    }

    public void initMap()
    {
        this._gridMap = new Grid[GameDef.GRID_HEIGHT, GameDef.GRID_WIDTH];
        // int centerX = GameDef.GRID_WIDTH / 2;
        // int centerY = GameDef.GRID_HEIGHT / 2;

        //创建test用地图
        // var map = new int[]{
        //     0,0,0,0,0,1,0,0,0,
        //     0,0,0,0,1,0,0,0,0,
        //     0,0,0,1,0,0,0,0,0,
        //     0,0,1,0,0,0,0,0,0,
        //     0,0,0,1,0,0,0,0,0,
        //     0,0,0,0,1,0,0,0,0,
        //     0,0,0,0,0,1,0,0,0,
        //     0,0,0,0,0,0,0,0,0,
        //     0,0,0,0,0,0,0,0,0
        // };
        // Debug.Log("pp"+p);
        for (int i = 0; i < GameDef.GRID_HEIGHT; i++)
        {
            for (int j = 0; j < GameDef.GRID_WIDTH; j++)
            {
                // 中间不生成
                // if (i == centerY && j == centerX) continue;
                var index = j + i * GameDef.GRID_WIDTH;
                if (this.curMap[index] == -1) continue;
                if (this.curMap[index] == 0) continue;
                Timers.inst.Add(0.05f * index, 1, (object obj) =>
                    {
                        // Debug.Log(index);
                        Point p = this.indexToPoint(index);
                        this.createGrid(p.x, p.y/* , map[p.x + p.y * GameDef.GRID_WIDTH] */);
                    });
            }
        }
    }

    public Grid[,] getMap()
    {
        return this._gridMap;
    }

    public Grid getGridByPoint(Point p)
    {
        if (this._gridMap == null) return null;
        if (!this.isOnMap(p)) return null;
        return this._gridMap[p.y, p.x];
    }
    public Grid getGridByIndex(int index)
    {
        Point p = new Point(index % GameDef.GRID_WIDTH, (int)index / GameDef.GRID_WIDTH);
        return this.getGridByPoint(p);
    }
    void createGrid(int x, int y, int color = -1, GridType type = GridType.Normal)
    {
        // if (color == 0) return;
        Grid grid = new Grid();
        grid.color = color == -1 ? this.getColorWithPos(x, y) : color;
        grid.x = x;
        grid.y = y;
        grid.type = type;
        if (type == GridType.Normal || type == GridType.BaoShi)
        {
            grid.elimTimes = 1;
        }
        else if (type == GridType.IceCube)
        {
            grid.elimTimes = 2;
        }
        else if (type == GridType.Pollution)
        {
            grid.elimTimes = 2;
        }
        this._gridMap[y, x] = grid;
        mvc.send(Notification.CreateNewGrid, new Point(x, y));
    }

    int getColorWithPos(int x, int y)
    {
        int[] all = { 0, 1, 2, 3, 4, 5 };
        int random = Util.getRandom(0, all.Length);
        if (x == 0 && y == 0)
        {
            return all[random];
        }

        //根据上左下右获取不同色的球
        Point[] roundPoint = this.getRoundGrid(x, y);
        List<int> round = new List<int>();
        for (int i = 0; i < roundPoint.Length; i++)
        {
            Point point = roundPoint[i];
            if (this.isOnMap(point) && this._gridMap[point.y, point.x] != null)
            {
                int color = this._gridMap[point.y, point.x].color;
                round.Add(color);
            }
        }
        //去除相同的颜色
        List<int> tar = new List<int>();
        for (int i = 0; i < all.Length; i++)
        {
            int color = all[i];
            if (round.IndexOf(color) < 0)
            {
                tar.Add(color);
            }
        }
        //剩余颜色中随机一个
        random = Util.getRandom(0, tar.ToArray().Length);
        return tar.ToArray()[random];
    }

    Point getCanEliminatePoint(int x, int y)
    {
        //查找上方可移动消除的点
        //4
        //3 2 1 0 4 3 2 1
        int m = 0;
        List<Point> shield = new List<Point>();
        for (int i = y - 1; i >= 0; i--)
        {
            Point p = new Point(x, i);
            if (!this.isOnMap(p)) break;
            if (this._gridMap[p.y, p.x] == null)
            {
                if ((y - 1 - m) != p.y)
                {
                    break;
                }
                m++;
                continue;
            }
            shield.Add(p);
            bool can = this.canEliminate(x, i + 1 + m, p, shield);
            if (can)
            {
                return p;
            }
        }
        //查找下方可移动消除的点
        // 3
        //4 5 6  3 4 5
        m = 0;
        shield = new List<Point>();
        for (int i = y + 1; i < GameDef.GRID_HEIGHT; i++)
        {
            Point p = new Point(x, i);
            if (!this.isOnMap(p)) break;
            if (this._gridMap[p.y, p.x] == null)
            {
                if ((y + 1 + m) != p.y)
                {
                    break;
                }
                m++;
                continue;
            }
            //从上1遍历到0 尝试移动
            shield.Add(p);
            bool can = this.canEliminate(x, i - 1 - m, p, shield);
            if (can)
            {
                return p;
            }
        }
        //查找左方可移动消除的点
        //3
        //2 1 0 1 2 3
        m = 0;
        shield = new List<Point>();
        for (int i = x - 1; i >= 0; i--)
        {
            Point p = new Point(i, y);
            if (!this.isOnMap(p)) break;
            if (this._gridMap[p.y, p.x] == null)
            {
                if ((x - 1 - m) != p.x)
                {
                    break;
                }
                m++;
                continue;
            }
            shield.Add(p);
            bool can = this.canEliminate(i + 1 + m, y, p, shield);
            if (can)
            {
                return p;
            }
        }
        //查找下方可移动消除的点
        // 3
        //4 5 6  3 4 5
        m = 0;
        shield = new List<Point>();
        for (int i = x + 1; i < GameDef.GRID_HEIGHT; i++)
        {
            Point p = new Point(i, y);
            if (!this.isOnMap(p)) break;
            if (this._gridMap[p.y, p.x] == null)
            {
                if ((x + 1 + m) != p.x)
                {
                    break;
                }
                m++;
                continue;
            }
            //从上1遍历到0 尝试移动
            shield.Add(p);
            bool can = this.canEliminate(i - 1 - m, y, p, shield);
            if (can)
            {
                return p;
            }
        }
        return null;
    }
    bool canEliminate(int x, int y, Point existPoint, List<Point> shiled = null)
    {
        Grid existGrid = this._gridMap[existPoint.y, existPoint.x];
        int color = existGrid.color;
        Point[] pointArr = this.getRoundGrid(x, y);
        for (int i = 0; i < pointArr.Length; i++)
        {
            Point p = pointArr[i];
            if (p.x == existPoint.x && p.y == existPoint.y) continue;
            if (shiled != null)
            {
                bool c = false;
                foreach (Point s in shiled)
                {
                    if (s.x == p.x && s.y == p.y)
                    {
                        c = true;
                        break;
                    }
                }
                if (c)
                {
                    continue;
                }
            }
            if (this.isOnMap(p))
            {
                Grid grid = this._gridMap[p.y, p.x];

                if (grid != null && grid.color == color && grid.type == GridType.Normal)
                {
                    return true;
                }
            }
        }
        return false;
    }
    Point getTop(Point p)
    {
        int x = p.x;
        int y = p.y - 1;
        if (isOnMapByXY(x, y))
        {
            return new Point(x, y);
        }
        return null;
    }
    Point getBottom(Point p)
    {
        int x = p.x;
        int y = p.y + 1;
        if (isOnMapByXY(x, y))
        {
            return new Point(x, y);
        }
        return null;
    }
    Point getLeft(Point p)
    {
        int x = p.x - 1;
        int y = p.y;
        if (isOnMapByXY(x, y))
        {
            return new Point(x, y);
        }
        return null;
    }
    Point getRight(Point p)
    {
        int x = p.x + 1;
        int y = p.y;
        if (isOnMapByXY(x, y))
        {
            return new Point(x, y);
        }
        return null;
    }
    bool isOnMapByXY(int x, int y)
    {
        return x >= 0
            && y >= 0
            && x < GameDef.GRID_WIDTH
            && y < GameDef.GRID_HEIGHT;
    }
    bool isOnMap(Point point)
    {
        return point.x >= 0
            && point.y >= 0
            && point.x < GameDef.GRID_WIDTH
            && point.y < GameDef.GRID_HEIGHT && this.isMap(point);
    }
    bool isOnMapByIndex(int index)
    {
        var point = this.indexToPoint(index);
        return this.isOnMap(point);
    }

    Point[] getRoundMove(int x, int y)
    {
        List<Point> pointArr = new List<Point>();

        //根据上左下右获取不同色的球
        Point[] roundPoint = this.getRoundGrid(x, y);

        List<Point> movePointArr = new List<Point>();
        for (int i = 0; i < roundPoint.Length; i++)
        {
            Point point = roundPoint[i];
            if (this.isOnMap(point) && this._gridMap[point.y, point.x] == null)
            {
                // int color = this._gridMap[point.y, point.x].color;
                movePointArr.Add(point);
            }
        }

        return movePointArr.ToArray();
    }

    public bool canMove(int curIndex, int tarIndex)
    {
        int x = curIndex % GameDef.GRID_WIDTH;
        int y = curIndex / GameDef.GRID_HEIGHT;

        int tarX = tarIndex % GameDef.GRID_WIDTH;
        int tarY = tarIndex / GameDef.GRID_HEIGHT;
        //选中点与目标点在一条直线上,并且目标点为空即可移动
        int curX = curIndex % GameDef.GRID_WIDTH;
        int curY = curIndex / GameDef.GRID_HEIGHT;

        // if (curX != tarX && curY != tarY) return;

        Point[] startPoints;
        List<Point> endPoints = new List<Point>();
        if (curX == tarX)
        {//x轴方向
            if (tarY < curY)
            {//上
                //查找障碍
                Grid grid;
                List<Point> line = new List<Point>();
                int obstacleIndex = tarY;
                //获取需要移动的点
                for (int i = curY; i >= tarY; i--)
                {
                    grid = this._gridMap[i, curX];
                    var p = new Point(curX, i);
                    if (this.isBarrier(grid) || !this.isMap(p))
                    {
                        line.Clear();
                        break;
                    }
                    if (grid != null)
                    {
                        line.Add(new Point(curX, i));
                    }
                    else
                    {
                        obstacleIndex = i;
                        break;
                    }
                }
                startPoints = line.ToArray();
            }
            else
            {//下
                //查找障碍
                Grid grid;
                List<Point> line = new List<Point>();
                int obstacleIndex = tarY;
                //获取需要移动的点
                for (int i = curY; i <= tarY; i++)
                {
                    grid = this._gridMap[i, curX];
                    var p = new Point(curX, i);
                    if (this.isBarrier(grid) || !this.isMap(p))
                    {
                        line.Clear();
                        break;
                    }
                    if (grid != null)
                    {
                        line.Add(new Point(curX, i));
                    }
                    else
                    {
                        obstacleIndex = i;
                        break;
                    }
                }
                startPoints = line.ToArray();
            }
        }
        else
        {//y轴方向
            if (tarX < curX)
            {//左
                //查找障碍
                Grid grid;
                List<Point> line = new List<Point>();
                int obstacleIndex = tarX;
                //获取需要移动的点
                for (int i = curX; i >= tarX; i--)
                {
                    grid = this._gridMap[curY, i];
                    var p = new Point(i, curY);
                    if (this.isBarrier(grid) || !this.isMap(p))
                    {
                        line.Clear();
                        break;
                    }
                    if (grid != null)
                    {
                        line.Add(new Point(i, curY));
                    }
                    else
                    {
                        obstacleIndex = i;
                        break;
                    }
                }
                startPoints = line.ToArray();
            }
            else
            {//右
                //查找障碍
                Grid grid;
                List<Point> line = new List<Point>();
                int obstacleIndex = tarX;
                //获取需要移动的点
                for (int i = curX; i <= tarX; i++)
                {
                    grid = this._gridMap[curY, i];
                    var p = new Point(i, curY);
                    if (this.isBarrier(grid) || !this.isMap(p))
                    {
                        line.Clear();
                        break;
                    }
                    if (grid != null)
                    {
                        line.Add(new Point(i, curY));
                    }
                    else
                    {
                        obstacleIndex = i;
                        break;
                    }
                }
                startPoints = line.ToArray();
            }
        }
        // if (x == tarX || y == tarY)
        //     return true;
        if (startPoints.Length > 0) return true;
        return false;
    }
    public void move(int curIndex, int tarIndex)
    {
        int curX = curIndex % GameDef.GRID_WIDTH;
        int curY = curIndex / GameDef.GRID_HEIGHT;

        int tarX = tarIndex % GameDef.GRID_WIDTH;
        int tarY = tarIndex / GameDef.GRID_HEIGHT;

        if (curX != tarX && curY != tarY) return;

        Point[] startPoints;
        List<Point> endPoints = new List<Point>();
        if (curX == tarX)
        {//x轴方向
            if (tarY < curY)
            {//上
                //查找障碍
                Grid grid;
                List<Point> line = new List<Point>();
                int obstacleIndex = tarY;
                //获取需要移动的点
                for (int i = curY; i > tarY; i--)
                {
                    grid = this._gridMap[i, curX];
                    var point = new Point(curX, i);
                    if (this.isBarrier(grid) || !this.isMap(point))
                    {
                        line.Clear();
                        break;
                    }
                    if (grid != null)
                    {
                        line.Add(new Point(curX, i));
                    }
                    else
                    {
                        obstacleIndex = i;
                        break;
                    }
                }
                //获取障碍点
                for (int i = obstacleIndex; i >= tarY; i--)
                {
                    grid = this._gridMap[i, curX];
                    var point = new Point(curX, i);
                    if (grid != null || this.isBarrier(grid) || !this.isMap(point))
                    {
                        break;
                    }
                    else
                    {
                        obstacleIndex = i;
                    }
                }
                //开始移动点
                Point p;
                startPoints = line.ToArray();
                for (int i = startPoints.Length - 1; i >= 0; i--)
                {
                    p = startPoints[i];
                    endPoints.Add(new Point(p.x, obstacleIndex));
                    this._gridMap[obstacleIndex, p.x] = this._gridMap[p.y, p.x];
                    this._gridMap[p.y, p.x] = null;
                    obstacleIndex++;
                }
            }
            else
            {//下
                //查找障碍
                Grid grid;
                List<Point> line = new List<Point>();
                int obstacleIndex = tarY;
                //获取需要移动的点
                for (int i = curY; i < tarY; i++)
                {
                    grid = this._gridMap[i, curX];
                    var point = new Point(curX, i);
                    if (this.isBarrier(grid) || !this.isMap(point))
                    {
                        line.Clear();
                        break;
                    }
                    if (grid != null)
                    {
                        line.Add(new Point(curX, i));
                    }
                    else
                    {
                        obstacleIndex = i;
                        break;
                    }
                }
                //获取障碍点
                for (int i = obstacleIndex; i <= tarY; i++)
                {
                    grid = this._gridMap[i, curX];
                    var point = new Point(curX, i);
                    if (grid != null || this.isBarrier(grid) || !this.isMap(point))
                    {
                        break;
                    }
                    else
                    {
                        obstacleIndex = i;
                    }
                }
                //开始移动点
                Point p;
                startPoints = line.ToArray();
                for (int i = startPoints.Length - 1; i >= 0; i--)
                {
                    p = startPoints[i];
                    endPoints.Add(new Point(p.x, obstacleIndex));
                    this._gridMap[obstacleIndex, p.x] = this._gridMap[p.y, p.x];
                    this._gridMap[p.y, p.x] = null;
                    obstacleIndex--;
                }
            }
        }
        else
        {//y轴方向
            if (tarX < curX)
            {//左
                //查找障碍
                Grid grid;
                List<Point> line = new List<Point>();
                int obstacleIndex = tarX;
                //获取需要移动的点
                for (int i = curX; i > tarX; i--)
                {
                    grid = this._gridMap[curY, i];
                    var point = new Point(i, curY);
                    if (this.isBarrier(grid) || !this.isMap(point))
                    {
                        line.Clear();
                        break;
                    }
                    if (grid != null)
                    {
                        line.Add(new Point(i, curY));
                    }
                    else
                    {
                        obstacleIndex = i;
                        break;
                    }
                }
                //获取障碍点
                for (int i = obstacleIndex; i >= tarX; i--)
                {
                    grid = this._gridMap[curY, i];
                    var point = new Point(i, curY);
                    if (grid != null || this.isBarrier(grid) || !this.isMap(point))
                    {
                        break;
                    }
                    else
                    {
                        obstacleIndex = i;
                    }
                }
                //开始移动点
                Point p;
                startPoints = line.ToArray();
                for (int i = startPoints.Length - 1; i >= 0; i--)
                {
                    p = startPoints[i];
                    endPoints.Add(new Point(obstacleIndex, p.y));
                    this._gridMap[p.y, obstacleIndex] = this._gridMap[p.y, p.x];
                    this._gridMap[p.y, p.x] = null;
                    obstacleIndex++;
                }
            }
            else
            {//右
                //查找障碍
                Grid grid;
                List<Point> line = new List<Point>();
                int obstacleIndex = tarX;
                //获取需要移动的点
                for (int i = curX; i < tarX; i++)
                {
                    grid = this._gridMap[curY, i];
                    var point = new Point(i, curY);
                    if (this.isBarrier(grid) || !this.isMap(point))
                    {
                        line.Clear();
                        break;
                    }
                    if (grid != null)
                    {
                        line.Add(new Point(i, curY));
                    }
                    else
                    {
                        obstacleIndex = i;
                        break;
                    }
                }
                //获取障碍点
                for (int i = obstacleIndex; i <= tarX; i++)
                {
                    grid = this._gridMap[curY, i];
                    var point = new Point(i, curY);
                    if (grid != null || this.isBarrier(grid) || !this.isMap(point))
                    {
                        break;
                    }
                    else
                    {
                        obstacleIndex = i;
                    }
                }
                //开始移动点
                Point p;
                startPoints = line.ToArray();
                for (int i = startPoints.Length - 1; i >= 0; i--)
                {
                    p = startPoints[i];
                    endPoints.Add(new Point(obstacleIndex, p.y));
                    this._gridMap[p.y, obstacleIndex] = this._gridMap[p.y, p.x];
                    this._gridMap[p.y, p.x] = null;
                    obstacleIndex--;
                }
            }
        }
        Dictionary<string, Point[]> moveList = new Dictionary<string, Point[]>();
        var endArr = endPoints.ToArray();
        Array.Reverse(endArr);
        moveList.Add("end", endArr);
        moveList.Add("start", startPoints);
        if (startPoints.Length > 0
        && this.gameMode == GameMode.PERSON
        && this._myScore >= this.YetiCreateNeedScore)
        {
            this.curStep += 1;
        }
        this.isCreateNew = true;
        this.tempScore = 0;
        mvc.send(Notification.MoveEnd, moveList);
    }
    private void checkObstacleGrid(int curIndex, int tarIndex)
    {

    }

    private Point[] getRoundGrid(int x, int y)
    {
        Point top = new Point(x, y - 1);
        Point left = new Point(x - 1, y);
        Point down = new Point(x, y + 1);
        Point right = new Point(x + 1, y);
        Point[] roundPoint = { top, left, down, right };
        return roundPoint;
    }

    public void checkAndRemove()
    {
        int totalScore = 0;
        //消除x轴和y轴颜色一致的球
        List<Point> tempBoomList = new List<Point>();
        List<Grid> tempGridList = new List<Grid>();
        for (int j = 0; j < GameDef.GRID_TOTAL_COUNT; j++)
        {
            int score = 0;
            int x = j % GameDef.GRID_WIDTH;
            int y = (int)j / GameDef.GRID_WIDTH;
            Grid grid = this._gridMap[y, x];
            Point current = new Point(x, y);
            var sameList = new List<Point>();
            if (grid != null && grid.type == GridType.Normal)
            {
                this.FillSameItemsList(sameList, current);
                // var boomList = this.FillBoomList(sameList, current);
                var boomList = sameList;
                //消除并加分
                if (boomList.Count > 1)
                {
                    tempGridList.AddRange(this.removeBoomList(boomList));
                    score += (boomList.Count - 1);
                    tempBoomList.AddRange(boomList);
                }
            }
            if (score > 0)
            {
                totalScore += score;
            }
        }
        if (totalScore > 0)
        {
            //计算连击
            int combo = this.getCombo(tempGridList.ToArray());
            totalScore = totalScore * combo * 8;
            // if (this.gameMode == GameMode.PERSON)
            this.tempScore += totalScore;
            // else
            //     this.addScore(totalScore);
        }
        // if (!this.isCreateNew)
        //     this.addScore(this.tempScore);
        Debug.Log("触发消除检查=========");
        mvc.send(Notification.CheckAndMoveEnd, tempBoomList);
    }
    int getCombo(Grid[] boomArr)
    {
        int combo = 0;
        int[] colorArr = { 0, 0, 0, 0, 0, 0 };
        for (int m = 0; m < boomArr.Length; m++)
        {
            Grid grid = boomArr[m];
            if (grid == null)
                continue;
            int color = grid.color;
            colorArr[color]++;
        }
        for (int n = 0; n < colorArr.Length; n++)
        {
            if (colorArr[n] > 0)
            {
                combo++;
            }
        }
        return combo;
    }

    void addScore(int score)
    {
        switch (this.curPlayer)
        {
            case GameDef.SELF:

                int doubleTimes = DataManager.inst.getDoubleScoreTimes();
                if (doubleTimes > 0)
                {
                    score *= 10;
                    DataManager.inst.addDoubleScoreTimes(-1);
                }

                this._myScore += score;
                break;
            case GameDef.OTHER:
                this._otherScore += score;
                break;
        }
        this.tempScore = 0;
        mvc.send(Notification.ScoreUpdate, this.curPlayer);
        if (this.recordScore != 0 && this.myScore > this.recordScore)
        {
            Storage.SetInt(DataDef.RECORD, this.myScore);
            mvc.send(Notification.NewRecord);
        }
    }

    public void FillSameItemsList(List<Point> sameList, Point current)
    {
        //如果已存在，跳过
        foreach (var p in sameList)
        {
            if (p.x == current.x && p.y == current.y)
                return;
        }
        //添加到列表
        sameList.Add(current);
        //上下左右的Item
        Point[] tempItemList = new Point[4]{
    getTop(current),getBottom(current),
    getLeft(current),getRight(current)};

        Grid curGrid = this._gridMap[current.y, current.x];
        for (int i = 0; i < tempItemList.Length; i++)
        {
            Point p = tempItemList[i];
            //如果Item不合法，跳过
            if (p == null)
                continue;
            Grid tempGird = this._gridMap[p.y, p.x];
            if (tempGird == null || tempGird.type != GridType.Normal)
                continue;
            if (curGrid.color == tempGird.color)
            {
                FillSameItemsList(sameList, tempItemList[i]);
            }
        }
    }

    // 填充待消除列表
    public List<Point> FillBoomList(List<Point> sameList, Point current)
    {
        //计数器
        int rowCount = 0;
        int columnCount = 0;
        //临时列表
        List<Point> rowTempList = new List<Point>();
        List<Point> columnTempList = new List<Point>();
        //横向纵向检测
        foreach (var p in sameList)
        {
            //如果在同一行
            if (p.y == current.y)
            {
                //判断该点与Curren中间有无间隙
                bool rowCanBoom = CheckItemsInterval(true, current, p);
                if (rowCanBoom)
                {
                    //计数
                    rowCount++;
                    //添加到行临时列表
                    rowTempList.Add(p);
                }
            }
            //如果在同一列
            if (p.x == current.x)
            {
                //判断该点与Curren中间有无间隙
                bool columnCanBoom = CheckItemsInterval(false, current, p);
                if (columnCanBoom)
                {
                    //计数
                    columnCount++;
                    //添加到列临时列表
                    columnTempList.Add(p);
                }
            }
        }

        //横向消除
        List<Point> boomList = new List<Point>();
        bool horizontalBoom = false;
        //如果横向三个以上
        if (rowCount > 1)
        {
            //将临时列表中的Item全部放入BoomList
            boomList.AddRange(rowTempList);
            //横向消除
            horizontalBoom = true;
        }
        //如果纵向三个以上
        if (columnCount > 1)
        {
            if (horizontalBoom)
            {
                //剔除自己
                boomList.Remove(current);
            }
            //将临时列表中的Item全部放入BoomList
            boomList.AddRange(columnTempList);
        }
        //如果没有消除对象，返回
        if (boomList.Count == 0)
        {
            return null;
        }
        //创建临时的BoomList
        List<Point> tempBoomList = new List<Point>();
        //转移到临时列表
        tempBoomList.AddRange(boomList);
        //开启处理BoomList的协程
        // StartCoroutine(ManipulateBoomList(tempBoomList));
        return tempBoomList;
    }
    private List<Grid> removeBoomList(List<Point> boomList)
    {
        List<Grid> gridList = new List<Grid>();
        if (boomList == null || boomList.Count == 0) return gridList;

        foreach (var p in boomList)
        {
            // if(this._gridMap[p.y,p.x] != null) {

            // }
            //周围有冰块或者液体进行消除
            Point[] roundPoint = this.getRoundGrid(p.x, p.y);
            List<int> round = new List<int>();
            for (int i = 0; i < roundPoint.Length; i++)
            {
                Point point = roundPoint[i];
                if (this.isOnMap(point) && this._gridMap[point.y, point.x] != null)
                {
                    var grid = this._gridMap[point.y, point.x];
                    if (grid.type == GridType.IceCube || grid.type == GridType.Pollution || grid.type == GridType.BaoShi)
                    {
                        Debug.Log("待消除次数=" + grid.elimTimes + ",x=" + point.x + ",y=" + point.y);
                        if (grid.elimTimes > 1)
                            grid.elimTimes--;
                        else
                            this._gridMap[point.y, point.x] = null;
                        if (grid.type == GridType.IceCube)
                            mvc.send(Notification.ElimIceCube, point);
                        else if (grid.type == GridType.BaoShi)
                        {
                            DataManager.inst.addGem();
                            mvc.send(Notification.ElimBaoShi, point);
                        }
                        else
                            mvc.send(Notification.ElimPollution, point);
                    }
                }
            }
            gridList.Add(this._gridMap[p.y, p.x]);
            this._gridMap[p.y, p.x] = null;
        }
        return gridList;
        // mvc.send(Notification.Eliminate, boomList);
    }
    private bool CheckItemsInterval(bool isHorizontal, Point begin, Point end)
    {
        Grid beginGrid = this._gridMap[begin.y, begin.x];
        Grid endGrid = this._gridMap[end.y, end.x];
        Grid[,] gridMap = this._gridMap.Clone() as Grid[,];
        //获取图案
        int color = beginGrid.color; //如果是横向
        if (isHorizontal)
        {
            //起点终点列号
            int beginIndex = begin.x;
            int endIndex = end.x;
            //如果起点在右，交换起点终点列号
            if (beginIndex > endIndex)
            {
                beginIndex = end.x;
                endIndex = begin.x;
            }
            //遍历中间的Item
            for (int i = beginIndex + 1; i < endIndex; i++)
            {
                //异常处理(中间未生成，标识为不合法)
                if (gridMap[begin.y, i] == null)
                {
                    return false;
                }
                //如果中间有间隙(有图案不一致的)
                if (gridMap[begin.y, i].color != color)
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            //起点终点行号
            int beginIndex = begin.y;
            int endIndex = end.y;
            //如果起点在上，交换起点终点列号
            if (beginIndex > endIndex)
            {
                beginIndex = end.y;
                endIndex = begin.y;
            }
            //遍历中间的Item
            for (int i = beginIndex + 1; i < endIndex; i++)
            {
                //如果中间有间隙(有图案不一致的)
                if (gridMap[i, begin.x].color != color)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public bool isFinished()
    {
        int[] colorNumList = { 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < this._gridMap.Length; i++)
        {
            int y = (int)i / GameDef.GRID_WIDTH;
            int x = i % GameDef.GRID_WIDTH;
            if (this._gridMap[y, x] == null)
                continue;
            int color = this._gridMap[y, x].color;
            colorNumList[color]++;
            if (colorNumList[color] >= 2)
            {
                return false;
            }
        }
        Debug.Log("比赛结束==========================");
        return true;
    }

    public int pointToIndex(Point p)
    {
        return p.x + p.y * GameDef.GRID_WIDTH;
    }
    public Point indexToPoint(int index)
    {
        return new Point(index % GameDef.GRID_WIDTH, (int)index / GameDef.GRID_WIDTH);
    }
    /// <summary>
    /// 消除随机同色球
    /// </summary>
    public void elimRandomColorGrid()
    {
        int tarColor = -1;
        //获取场上有的颜色
        List<int> colorList = new List<int>();
        for (int i = 0; i < this._gridMap.Length; i++)
        {
            Point p = this.indexToPoint(i);
            if (this._gridMap[p.y, p.x] == null)
                continue;
            int color = this._gridMap[p.y, p.x].color;
            if (colorList.IndexOf(color) < 0)
            {
                colorList.Add(color);
            }
        }

        int random = Util.getRandom(0, colorList.Count - 1);
        tarColor = colorList.ToArray()[random];

        List<Point> tempBoomList = new List<Point>();
        int score = 0;
        for (int i = 0; i < this._gridMap.Length; i++)
        {
            Point p = this.indexToPoint(i);
            if (this._gridMap[p.y, p.x] == null)
                continue;
            int color = this._gridMap[p.y, p.x].color;
            if (tarColor == color)
            {
                this._gridMap[p.y, p.x] = null;
                tempBoomList.Add(p);
                score++;
            }
        }
        if (score > 0)
        {
            this.isElimByItem = true;
            // this.addScore(score);
            this.tempScore += score;
            mvc.send(Notification.CheckAndMoveEnd, tempBoomList);
        }
        this.isReset();
    }

    int _minLightningElimCount = 3;
    int _maxLightningElimCount = 5;
    /// <summary>
    /// 消除随机数量球
    /// </summary>
    public void elimRandomCount()
    {
        int count = Util.getRandom(this._minLightningElimCount, this._maxLightningElimCount + 1);
        count = Math.Min(count, this._gridMap.Length);
        // List<int> list = Util.getRandomWithoutRep(0, this._gridMap.Length, count);
        this.elimCount(count, true);
        this.isReset();
    }

    public void elimCount(int count, bool isItem = false)
    {
        List<int> list = new List<int>();
        while (list.Count < count)
        {
            int random = Util.getRandom(0, this._gridMap.Length);
            Point p = this.indexToPoint(random);
            if (this._gridMap[p.y, p.x] != null && list.IndexOf(random) < 0)
                list.Add(random);
        }

        int score = 0;
        List<Point> tempBoomList = new List<Point>();
        for (int i = 0; i < this._gridMap.Length; i++)
        {
            if (list.IndexOf(i) >= 0)
            {
                Point p = this.indexToPoint(i);
                if (this._gridMap[p.y, p.x] == null)
                    continue;
                this._gridMap[p.y, p.x] = null;
                tempBoomList.Add(p);
                score++;
            }
        }

        if (score > 0)
        {
            if (isItem)
            {
                this.isElimByItem = true;
            }
            // this.addScore(score);
            this.tempScore += score;
            mvc.send(Notification.CheckAndMoveEnd, tempBoomList);
        }
    }

    public bool hitGrid(int index)
    {
        Point p = this.indexToPoint(index);
        Grid grid = this._gridMap[p.y, p.x];
        if (grid == null) return false;
        if (grid.type == GridType.Pollution)
        {
            this._gridMap[p.y, p.x] = null;
            mvc.send(Notification.ElimPollution, p);
        }
        else if (grid.type == GridType.IceCube)
        {
            this._gridMap[p.y, p.x] = null;
            mvc.send(Notification.ElimIceCube, p);
        }
        else if (grid.type == GridType.BaoShi)
        {
            this._gridMap[p.y, p.x] = null;
            DataManager.inst.addGem();
            mvc.send(Notification.ElimBaoShi, p);
        }
        else
        {
            this._gridMap[p.y, p.x] = null;
            // mvc.send(Notification.ElimnateGrid, p);

            List<Point> tempBoomList = new List<Point>();
            tempBoomList.Add(p);
            this.tempScore += 1;
            mvc.send(Notification.CheckAndMoveEnd, tempBoomList);
        }
        return true;
    }

    public bool createNewGridInPerson()
    {
        //生成新球
        this.createNewGrid();

        //生成污染物
        if (this.curStep == this.YetiStep)
        {
            this.createYeTi();
            this.curStep = 0;
        }
        //生成冰块
        if (this._myScore >= this.IceCubeCreateNeedScore)
        {
            this.createIceCube();
        }
        //生成宝石
        if (this._myScore >= this.BaoShiCreateNeedScore)
        {
            this.createBaoShi();
        }
        return true;
    }
    bool isMap(Point p)
    {
        var val = this.curMap[this.pointToIndex(p)];
        return val != 0;
    }
    void createNewGrid()
    {
        //生成球
        int createCount = this.getCreateCount();
        //筛选空格子
        List<Point> pList = new List<Point>();
        for (int i = 0; i < this._gridMap.Length; i++)
        {
            Point p = this.indexToPoint(i);
            if (this._gridMap[p.y, p.x] == null && this.isMap(p))
            {
                pList.Add(p);
            }
        }
        if (pList.Count > 1)
        {
            if (pList.Count > createCount)
            {
                var list = Util.getRandomWithoutRep(0, pList.Count, createCount).ToArray();
                for (int i = 0; i < list.Length; i++)
                {
                    var p = pList[list[i]];
                    if (i == 0 && this._myScore >= this.IceCubeCreateNeedScore && this.isCreateIceCube())
                    {
                        this.createGrid(p.x, p.y, -1, GridType.IceCube);
                    }
                    else
                        this.createGrid(p.x, p.y, Util.getRandom(0, SkinManager.COLOR_COUNT));
                }
            }
            else
            {
                var pArr = pList.ToArray();
                for (int i = 0; i < pArr.Length; i++)
                {
                    var p = pArr[i];
                    if (i == 0 && this._myScore >= this.IceCubeCreateNeedScore && this.isCreateIceCube())
                    {
                        this.createGrid(p.x, p.y, -1, GridType.IceCube);
                    }
                    else
                        this.createGrid(p.x, p.y, Util.getRandom(0, SkinManager.COLOR_COUNT));
                }
            }
        }
    }

    private bool isCreateIceCube()
    {
        int random = Util.getRandom(0, 5);
        return random > 3;
        // return false;
    }

    public void createYeTi()
    {
        //生成球
        int createCount = this.getCreateCount();
        //筛选空格子
        List<Point> pList = new List<Point>();
        for (int i = 0; i < this._gridMap.Length; i++)
        {
            Point p = this.indexToPoint(i);
            if (this._gridMap[p.y, p.x] != null && !this.isBarrier(this._gridMap[p.y, p.x]) && this.isMap(p))
            {
                pList.Add(p);
            }
        }
        if (pList.Count > this.maxYeTi)
        {
            var list = Util.getRandomWithoutRep(0, pList.Count, this.maxYeTi).ToArray();
            for (int i = 0; i < list.Length; i++)
            {
                var p = pList[list[i]];
                this.createGrid(p.x, p.y, -1, GridType.Pollution);
            }
        }
        else
        {
            var pArr = pList.ToArray();
            for (int i = 0; i < pArr.Length; i++)
            {
                var p = pArr[i];
                this.createGrid(p.x, p.y, -1, GridType.Pollution);
            }
        }
    }

    private void createIceCube()
    {

    }

    private void createBaoShi()
    {
        //生成球
        int createCount = this.getCreateCount();
        //筛选空格子
        List<Point> pList = new List<Point>();
        for (int i = 0; i < this._gridMap.Length; i++)
        {
            Point p = this.indexToPoint(i);
            if (this._gridMap[p.y, p.x] != null && !this.isBarrier(this._gridMap[p.y, p.x]) && this.isMap(p))
            {
                pList.Add(p);
            }
        }
        if (pList.Count > 0)
        {
            //判断概率
            var random = Util.getRandom(0, 10);
            if (random < 2)
            {
                var list = Util.getRandomWithoutRep(0, pList.Count, 1).ToArray();
                for (int i = 0; i < list.Length; i++)
                {
                    var p = pList[list[i]];
                    this.createGrid(p.x, p.y, -1, GridType.BaoShi);
                }
            }
        }
    }

    public int getCreateCount()
    {
        int[] create = ConfigManager.inst.getGameConfig().create;
        int score = this._myScore;
        for (int i = create.Length - 1; i >= 0; i--)
        {
            if (create[i] >= 0 && score >= create[i])
                return i;
        }
        return 0;
    }

    public bool isPersonMode()
    {
        return this.gameMode == GameMode.PERSON;
    }
    /// <summary>
    /// 是否是障碍
    /// </summary>
    /// <returns></returns>
    public bool isBarrier(Grid grid)
    {
        if (grid != null
        && (grid.type == GridType.IceCube
        || grid.type == GridType.Pollution
        || grid.type == GridType.BaoShi
        ))
            return true;
        return false;
    }
    public void checkCountdown()
    {
        if (!this.isCheckCountdown) return;
        int totalCount = 0;
        for (int i = 0; i < this._gridMap.Length; i++)
        {
            int y = (int)i / GameDef.GRID_WIDTH;
            int x = i % GameDef.GRID_WIDTH;
            if (this._gridMap[y, x] == null)
                continue;
            totalCount++;
        }
        if (totalCount <= ConfigManager.inst.getGameConfig().countdownCount)
        {
            mvc.send(Notification.ShowCountdown);
            this.isCheckCountdown = false;
        }
    }

    public void checkFinish()
    {
        if (this.isPersonMode())
        {
            if (!this.isCreateNew)
            {

                this.addScore(this.tempScore);
                if (this._myScore >= this.YetiCreateNeedScore)
                {
                    mvc.send(Notification.ActiveEM);
                }
                this.isReset();
            }
        }
        else
        {
            this.addScore(this.tempScore);
        }
        if (this.isElimByItem)
        {
            if (this.resetMap())
            {
                return;
            }
        }
        if (this.isFinished())
        {
            if (this.recordScore == 0)
            {
                Storage.SetInt(DataDef.RECORD, this.myScore);
            }
            mvc.send(Notification.GameEnd);
            return;
        }
        if (this.isPersonMode() && this.isCreateNew)
        {
            this.isCreateNew = false;
            if (this.createNewGridInPerson())
            {
                Timers.inst.Add(1, 1, (object obj) =>
                {
                    this.checkAndRemove();
                });
            }
        }
        else
        {
            this.checkCountdown();
            this.setCurPlayer();
        }
    }

    private void isReset()
    {

    }
    private bool resetMap()
    {
        // int count = 0;
        for (int i = 0; i < this._gridMap.Length; i++)
        {
            var p = this.indexToPoint(i);
            if (this._gridMap[p.y, p.x] != null)
            {
                return false;
            }
        }
        mvc.send(Notification.InitMap);
        return true;
    }
}