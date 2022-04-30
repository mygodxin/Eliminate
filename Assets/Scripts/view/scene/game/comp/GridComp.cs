using mm;
using FairyGUI;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 棋盘组件
/// </summary>
public class GridComp : AppComp
{
    private GComponent _comp;
    GList _gridList;
    GList _mapList;
    GLoader3D _finger;
    GComponent _gridLayer;
    GComponent _fingerLayer;
    GComponent _itemLayer;
    int _selIndex = -1;
    string skinName;
    bool _canClick;
    GameMode _gameMode;

    public GridComp(GComponent viewComponent) : base(viewComponent)
    {
        this._comp = viewComponent;

        this._gridList = this.GetList("list");
        // this._gridList.itemRenderer = this.itemRenderer;
        this._gridList.onClickItem.Add(this.onClickItem);
        this._mapList = this.GetList("map");

        this._gridLayer = this.GetComp("gridLayer");
        this._fingerLayer = this.GetComp("fingerLayer");
        this._itemLayer = this.GetComp("itemLayer");
        this._finger = new GLoader3D();
        this._finger.url = "ui://common/shouzhi";
        this._finger.skinName = "default";
        this._finger.animationName = "L";
        // this._finger.animationName.
        this._finger.loop = false;
        this._fingerLayer.AddChild(this._finger);
        this._finger.visible = false;
    }

    public void init(object mode)
    {
        if (mode != null)
        {
            this._gameMode = GameMode.FIGHT;
        }
        else
            this._gameMode = GameMode.PERSON;
        GameCommond.inst.initGame(this._gameMode);

        this._gridList.columnCount = GameDef.GRID_WIDTH;
        this._gridList.lineCount = GameDef.GRID_HEIGHT;
        // Debug.Log("刷新地图==============================" + gridMap.Length);
        this._gridList.numItems = GameDef.GRID_TOTAL_COUNT/* gridMap.Length */;
        this.initMap();
    }

    public void initMap()
    {
        GameCommond.inst.initMap();
        this.updateMap();
        // SkinManager.inst.setCurkin(8);
        this.updateSkin();
        this._canClick = false;
        Timers.inst.Add(0.06f * GameDef.GRID_TOTAL_COUNT, 1, (object obj) =>
            {
                this._canClick = true;
                GameCommond.inst.setCurPlayer();
            });
    }

    void updateMap(bool isFrist = false)
    {
        int[] map = GameCommond.inst.curMap;
        this._mapList.numItems = map.Length;
        for (int i = 0; i < this._mapList.numChildren; i++)
        {
            var mapComp = this._mapList.GetChildAt(i).asCom;
            var val = map[i];
            mapComp.GetChild("icon").asLoader.url = "ui://game/a" + Math.Abs(val);
        }
        Grid[,] gridMap = GameCommond.inst.getMap();
        this._gridList.data = gridMap;

        for (int i = 0; i < this._gridList.numChildren; i++)
        {
            this.updateGrid(i, this._gridList.GetChildAt(i), isFrist);
        }
    }

    void itemRenderer(int index, GObject item)
    {
        this.updateGrid(index, item);
    }

    void updateGrid(int index, GObject item, bool isFirst = false)
    {
        int x = index % GameDef.GRID_WIDTH;
        int y = index / GameDef.GRID_WIDTH;
        GComponent gridComp = item.asCom;
        Grid grid = GameCommond.inst.getGridByIndex(index);
        if (grid != null)
        {
            gridComp.GetChild("icon").asLoader.url = null;
            GLoader3D gridSpine = gridComp.GetChild("icon3D").asLoader3D;
            gridComp.data = grid.color;
            gridSpine.visible = true;
            gridSpine.skinName = this.skinName;
            if (grid.type == GridType.Normal)
            {
                if (isFirst)
                {
                    this.setGridAni(index, EAniType.SHOW, () =>
                    {
                        this.setGridAni(index, EAniType.Default);
                    });
                }
                else
                {
                    this.setGridAni(index, EAniType.Default);
                }
            }
            else if (grid.type == GridType.IceCube)
            {
                if (isFirst)
                {
                    this.setGridAni(index, EAniType.BING_KUAI, () =>
                    {
                        this.setGridAni(index, EAniType.BING_KUAI2);
                    });
                }
                else
                {
                    // this.setGridAni(index, EAniType.BING_KUAI2);
                    this.setGridAni(index, AniManager.inst.getIcecubeEAniType(grid));

                }
            }
            else if (grid.type == GridType.Pollution)
            {
                if (isFirst)
                {
                    this.setGridAni(index, EAniType.YE_TI, () =>
                    {
                        this.setGridAni(index, EAniType.YE_TI2);
                    });
                }
                else
                {
                    // this.setGridAni(index, EAniType.YE_TI2);
                    this.setGridAni(index, AniManager.inst.getYetiEAniType(grid));
                }
            }
            else if (grid.type == GridType.BaoShi)
            {
                if (isFirst)
                {
                    this.setGridAni(index, EAniType.BaoShi, () =>
                    {
                        this.setGridAni(index, EAniType.BaoShi);
                    });
                }
                else
                {
                    // this.setGridAni(index, EAniType.YE_TI2);
                    this.setGridAni(index, EAniType.BaoShi2);
                }
            }
        }
        else
        {
            gridComp.data = null;
            gridComp.GetChild("icon").asLoader.url = null;
            GLoader3D gridSpine = gridComp.GetChild("icon3D").asLoader3D;
            gridSpine.visible = false;
        }
        gridComp.GetChild("title").asTextField.text = /* index + */ "";
    }

    void onClickItem(EventContext obj)
    {
        if (GameCommond.inst.curPlayer != GameDef.SELF) return;

        GComponent gridComp = (obj.data as GObject).asCom;
        this.clickGrid(gridComp);
    }

    void setSelIndex(int index = -1)
    {
        if (this._selComp != null)
        {
            this._selComp.GetChild("icon3D").visible = false;
            this._selComp.visible = false;
        }
        // this._selComp.RemoveFromParent();
        //设为选中
        // for (int i = 0; i < this._gridList.numChildren; i++)
        // {
        //     this.setGridAni(i, i == index ? EAniType.SEL : EAniType.Default);
        // }
        if (this._selIndex > -1)
            this.setGridAni(this._selIndex, EAniType.Default);
        this._selIndex = index;
        if (this._selIndex > -1)
            this.setGridAni(this._selIndex, EAniType.SEL);
    }
    void clickGrid(GComponent gridComp)
    {
        if (!this._canClick)
        {
            // Util.AppTip("please wait");
            return;
        }
        int index = this._gridList.GetChildIndex(gridComp);
        var grid = GameCommond.inst.getGridByIndex(index);
        if (GameCommond.inst.isBarrier(grid))
        {
            return;
        }
        // if (gridComp.GetChild("icon3D").asLoader3D.visible != false//空格子
        if (grid == null)
        {
            if (this._selIndex < 0) return;
            //点击空格根据上左下右移动选中格子
            bool canMove = GameCommond.inst.canMove(this._selIndex, index);
            if (canMove)
            {
                GComponent selGrid = this._gridList.GetChildAt(this._selIndex).asCom;
                GameCommond.inst.move(this._selIndex, index);
            }
            else
            {
                Util.AppTip("There is nothing to be desired");
            }
        }
        else
        {
            this.setSelIndex(index);
        }
    }

    private GComponent _selComp;
    void setGridAni(int index, EAniType aniType, Action complete = null)
    {
        GComponent foo = this._gridList.GetChildAt(index).asCom;
        var icon3D = foo.GetChild("icon3D").asLoader3D;
        if (foo.data == null) return;

        AniManager.inst.playAni(foo, aniType, complete, false);

        if (aniType == EAniType.SEL)
        {
            if (this._selComp == null)
            {
                this._selComp = UIPackage.CreateObject("game", "Grid").asCom;
                this._selComp.GetChild("icon").asLoader.url = "";
                this._selComp.GetChild("title").asTextField.text = "";
                this._gridLayer.AddChild(this._selComp);
            }
            var selIcon3D = this._selComp.GetChild("icon3D").asLoader3D;
            // selIcon3D.align = AlignType.Center;
            // selIcon3D.verticalAlign = VertAlignType.Middle;
            selIcon3D.url = icon3D.url;
            selIcon3D.skinName = icon3D.skinName;

            int color = (int)foo.data;//GameCommond.inst.getGridByIndex(index).color;

            selIcon3D.animationName = AniManager.inst.getAniName(EAniType.SEL, color);//icon3D.animationName;
            this._selComp.x = this._gridList.x + foo.x + this._gridList.margin.left;
            this._selComp.y = this._gridList.y + foo.y + this._gridList.margin.top;

            this._selComp.visible = true;
            this._selComp.GetChild("icon3D").visible = true;
            foo.visible = false;
        }
        else
        {
            foo.visible = true;
        }
    }

    Stack<GComponent> _moveCompList = new Stack<GComponent>();
    Stack<GComponent> _flyCompList = new Stack<GComponent>();
    Stack<GGraph> _boomGraphList = new Stack<GGraph>();
    readonly int _speed = 1000;
    void moveItem(int start, int end, bool isEnd)
    {
        GComponent startComp = this._gridList.GetChildAt(start).asCom;
        GLoader3D startIcon3D = startComp.GetChild("icon3D").asLoader3D;
        GComponent endComp = this._gridList.GetChildAt(end).asCom;

        var moveComp = this._moveCompList.Count > 0 ? this._moveCompList.Pop() : null;
        if (moveComp == null)
        {
            moveComp = UIPackage.CreateObject("game", "Grid").asCom;
        }
        var icon3D = moveComp.GetChild("icon3D").asLoader3D;
        moveComp.GetChild("icon").asLoader.url = "";
        moveComp.GetChild("title").asTextField.text = "";
        icon3D.url = startIcon3D.url;
        icon3D.skinName = startIcon3D.skinName;

        moveComp.x = this._gridList.x + startComp.x + this._gridList.margin.left;
        moveComp.y = this._gridList.y + startComp.y + this._gridList.margin.top;
        moveComp.GetChild("icon3D").asLoader3D.visible = true;
        this._gridLayer.AddChild(moveComp);
        moveComp.data = startComp.data;

        float endX = this._gridList.x + endComp.x + this._gridList.margin.left;
        float endY = this._gridList.y + endComp.y + this._gridList.margin.top;
        EAniType aniType;
        EAniType aniType2;
        float time;
        if (startComp.y != endComp.y)
        {
            aniType = startComp.y > endComp.y ? EAniType.MOVE_UP : EAniType.MOVE_DOWN;
            time = (startComp.y - endComp.y) / _speed;
            aniType2 = startComp.y > endComp.y ? EAniType.MOVE_UP2 : EAniType.MOVE_DOWN2;
        }
        else
        {
            aniType = startComp.x > endComp.x ? EAniType.MOVE_LEFT : EAniType.MOVE_RIGHT;

            time = (startComp.x - endComp.x) / _speed;
            aniType2 = startComp.x > endComp.x ? EAniType.MOVE_LEFT2 : EAniType.MOVE_RIGHT2;
        }
        if (time < 0)
        {
            time = -time;
        }

        icon3D.loop = false;
        icon3D.animationName = AniManager.inst.getAniName(aniType, (int)startComp.data);

        startIcon3D.visible = false;

        GTween.To(new Vector3(moveComp.x, moveComp.y), new Vector3(endX, endY), time)
            .SetEase(EaseType.Linear)
            .SetTarget(moveComp, TweenPropType.Position)
            .OnComplete((tween) =>
            {
                // moveComp.GetChild("icon3D").asLoader3D.visible = false;

                AniManager.inst.playAni(moveComp, aniType2, () =>
                {

                    moveComp.GetChild("icon3D").asLoader3D.visible = false;
                    moveComp.RemoveFromParent();
                    this._moveCompList.Push(moveComp);
                    this.updateMap();


                    if (isEnd)
                    {
                        GameCommond.inst.checkAndRemove();
                    }
                }, false);
            });
    }
    public void otherMove(Point[] pointArr)
    {
        Point cur = pointArr[0];
        Point tar = pointArr[1];

        GComponent curComp = this._gridList.GetChildAt(cur.x + cur.y * GameDef.GRID_WIDTH).asCom;
        GComponent tarComp = this._gridList.GetChildAt(tar.x + tar.y * GameDef.GRID_WIDTH).asCom;
        this.clickGrid(curComp);

        // Timers.inst.Add(1, 1, (object obj) =>
        // {
        //播放手指动画
        var animationName = "";
        if (curComp.y != tarComp.y)
        {
            animationName = tarComp.y > curComp.y ? EFingerType.DOWN : EFingerType.UP;
        }
        else
            animationName = tarComp.x > curComp.x ? EFingerType.RIGHT : EFingerType.LEFT;
        this._finger.animationName = animationName;
        // Debug.Log("手指动画="+animationName);
        this._finger.x = this._gridList.x + curComp.x - 393 / 2 + this._gridList.margin.left;
        this._finger.y = this._gridList.y + curComp.y + this._gridList.margin.top;
        this._finger.visible = true;
        Spine.AnimationState.TrackEntryDelegate boomComplete = null;
        boomComplete = delegate (Spine.TrackEntry trackEntry)
                {
                    this._finger.spineAnimation.state.Complete -= boomComplete;

                    this._finger.visible = false;
                    this.clickGrid(tarComp);
                };
        this._finger.spineAnimation.state.Complete += boomComplete;


        // });
    }

    void flyGrid(int index, GGraph myGraph, GGraph otherGraph)
    {
        GComponent startComp = this._gridList.GetChildAt(index).asCom;
        GLoader3D startIcon3D = startComp.GetChild("icon3D").asLoader3D;

        var flyComp = this._flyCompList.Count > 0 ? this._flyCompList.Pop() : null;
        if (flyComp == null)
        {
            flyComp = UIPackage.CreateObject("game", "Grid").asCom;
        }
        var icon3D = flyComp.GetChild("icon3D").asLoader3D;
        flyComp.GetChild("icon").asLoader.url = "";
        flyComp.GetChild("title").asTextField.text = "";
        flyComp.data = startComp.data;
        icon3D.url = startIcon3D.url;
        icon3D.skinName = startIcon3D.skinName;
        icon3D.animationName = AniManager.inst.getAniName(EAniType.Default, (int)flyComp.data);

        flyComp.x = this._gridList.x + startComp.x + this._gridList.margin.left;
        flyComp.y = this._gridList.y + startComp.y + this._gridList.margin.top;
        flyComp.GetChild("icon3D").asLoader3D.visible = true;
        this._gridLayer.AddChild(flyComp);

        bool isMelf = GameCommond.inst.curPlayer == GameDef.SELF;
        float endX = /* this._gridList.x +  */(isMelf ? myGraph.x : otherGraph.x) - startComp.width / 2;
        float endY = /* this._gridList.y +  */(isMelf ? myGraph.y : otherGraph.y) - startComp.height / 2;
        GTween.To(new Vector3(flyComp.x, flyComp.y), new Vector3(endX, endY), 0.5f)
            .SetEase(EaseType.Linear)
            .SetTarget(flyComp, TweenPropType.Position)
            .OnComplete((tween) =>
            {
                flyComp.GetChild("icon3D").asLoader3D.visible = false;
                flyComp.RemoveFromParent();
                this._flyCompList.Push(flyComp);

                GameObject defen = Resources.Load<GameObject>("EF/baozha/DeFenFeiXing_dx");
                GameObject gameObject = UnityEngine.Object.Instantiate(defen);
                GoWrapper go = new GoWrapper(gameObject);
                if (isMelf)
                {
                    myGraph.SetNativeObject(go);
                    Timers.inst.Add(1f, 1, (object obj) =>
                    {
                        UnityEngine.Object.Destroy(go.wrapTarget);
                    });
                }
                else
                {
                    otherGraph.SetNativeObject(go);
                    Timers.inst.Add(1f, 1, (object obj) =>
                    {
                        UnityEngine.Object.Destroy(go.wrapTarget);
                    });
                }
            });
    }

    private GameObject _boomObj;
    // private GoWrapper go;
    public void onCheckAndMoveEnd(List<Point> pointList, GGraph my, GGraph other)
    {
        if (pointList.Count > 0)
        {
            foreach (Point p in pointList)
            {
                int index = GameCommond.inst.pointToIndex(p);
                this.setGridAni(index, EAniType.REMOVE_BEFORE, () =>
                {
                    this.flyGrid(index, my, other);
                });
            }
            Timers.inst.Add(1.0f, 1, (object obj) =>
            {
                this.endCallback();
            });
        }
        else
        {
            this.endCallback();
        }
    }

    void endCallback()
    {
        this.updateMap();
        this._canClick = true;

        GameCommond.inst.checkFinish();
    }

    public void updateSkin()
    {
        this.skinName = SkinManager.inst.getCurSkinName();
    }


    public void onMoveEnd(object moves)
    {
        this._canClick = false;


        var moveList = moves as Dictionary<string, Point[]>;
        var moveStartPoints = moveList["start"];
        var moveEndPoints = moveList["end"];

        this.setSelIndex();
        for (int i = 0; i < moveStartPoints.Length; i++)
        {
            int start = GameCommond.inst.pointToIndex(moveStartPoints[i]);
            int end = GameCommond.inst.pointToIndex(moveEndPoints[i]);
            this.moveItem(start, end, i == (moveStartPoints.Length - 1));
        }
    }

    public void onCreateNewGrid(Point p)
    {
        int index = GameCommond.inst.pointToIndex(p);
        this.updateGrid(index, this._gridList.GetChildAt(index), true);
    }

    public void onElimnateGrid(Point p)
    {
        int index = GameCommond.inst.pointToIndex(p);
        // this.updateGrid(index, this._gridList.GetChildAt(index), true);
        Grid[,] gridMap = GameCommond.inst.getMap();//this._gridList.data as Grid[,];

        int x = index % GameDef.GRID_WIDTH;
        int y = index / GameDef.GRID_WIDTH;
        Grid grid = null;
        if (gridMap != null) grid = gridMap[y, x];
        if (grid != null)
        {
            if (grid.type == GridType.IceCube)
            {
                if (grid.elimTimes == 2)
                {
                    this.setGridAni(index, EAniType.BING_KUAI3);
                }
            }
            this.setGridAni(index, EAniType.Default);
        }
        else
        {
            this.setGridAni(index, EAniType.Default, () =>
            {
                this.updateGrid(index, this._gridList.GetChildAt(index), true);
            });
        }
    }

    public void onElimIceCube(Point p)
    {
        int index = GameCommond.inst.pointToIndex(p);
        // this.updateGrid(index, this._gridList.GetChildAt(index), true);
        Grid[,] gridMap = GameCommond.inst.getMap();//this._gridList.data as Grid[,];

        int x = index % GameDef.GRID_WIDTH;
        int y = index / GameDef.GRID_WIDTH;
        Grid grid = null;
        if (gridMap != null) grid = gridMap[y, x];
        if (grid != null)
        {
            this.setGridAni(index, EAniType.BING_KUAI3, () =>
            {
                this.setGridAni(index, AniManager.inst.getIcecubeEAniType(grid));
            });
        }
        else
        {
            this.setGridAni(index, EAniType.BING_KUAI5, () =>
                {
                    this.updateGrid(index, this._gridList.GetChildAt(index), true);
                });
        }
    }
    public void onElimPollution(Point p)
    {
        int index = GameCommond.inst.pointToIndex(p);
        // this.updateGrid(index, this._gridList.GetChildAt(index), true);
        Grid[,] gridMap = GameCommond.inst.getMap();//this._gridList.data as Grid[,];

        int x = index % GameDef.GRID_WIDTH;
        int y = index / GameDef.GRID_WIDTH;
        Grid grid = null;
        if (gridMap != null) grid = gridMap[y, x];
        if (grid != null)
        {
            this.setGridAni(index, EAniType.YE_TI3, () =>
            {
                this.setGridAni(index, AniManager.inst.getYetiEAniType(grid));
            });
        }
        else
        {
            this.setGridAni(index, EAniType.YE_TI5, () =>
            {
                this.updateGrid(index, this._gridList.GetChildAt(index), true);
            });
        }
    }

    public void onElimBaoShi(Point p)
    {
        int index = GameCommond.inst.pointToIndex(p);
        // this.updateGrid(index, this._gridList.GetChildAt(index), true);
        Grid[,] gridMap = GameCommond.inst.getMap();//this._gridList.data as Grid[,];

        int x = index % GameDef.GRID_WIDTH;
        int y = index / GameDef.GRID_WIDTH;
        Grid grid = null;
        if (gridMap != null) grid = gridMap[y, x];
        if (grid != null)
        {
            // this.setGridAni(index, EAniType.YE_TI3, () =>
            // {
            //     this.setGridAni(index, AniManager.inst.getYetiEAniType(grid));
            // });
        }
        else
        {
            this.setGridAni(index, EAniType.BaoShi3, () =>
            {
                this.updateGrid(index, this._gridList.GetChildAt(index), true);
            });
        }
    }

    private GLoader _hammerComp;
    public void enableHammer()
    {
        this._canClick = false;
        // if (this._hammerComp == null)
        // {
        //     this._hammerComp = new GLoader();
        //     this._hammerComp.url = "ui://common/daoju3";
        // }
        // this._itemLayer.AddChild(this._hammerComp);
        // this._itemLayer.draggable = true;
        // this._itemLayer.onDragStart.Add(onDragStart);
    }
    void onDragStart(EventContext context)
    {
        //取消掉源拖动
        context.PreventDefault();

        //icon是这个对象的替身图片，userData可以是任意数据，底层不作解析。context.data是手指的id。
        DragDropManager.inst.StartDrag(this._itemLayer, "ui://common/daoju3", 1, (int)context.data);

        DragDropManager.inst.dragAgent.onDragEnd.Add(onDragEnd);
    }
    void onDragEnd()
    {
        Debug.Log("拖动结束");
    }
    public void unableHammer()
    {
        var obj = GRoot.inst.touchTarget;
        int index = -1;
        for (int i = 0; i < this._gridList.numItems; i++)
        {
            var gridComp = this._gridList.GetChildAt(i) as GComponent;
            if (gridComp.IsAncestorOf(obj))
            {
                index = i;
                break;
            }
        }
        if (index >= 0)
        {
            CShop shop = ConfigManager.inst.getShopConfig();
            // this._sameBallLabel.title = shop.item[0] + "";
            DataManager.inst.addMoney(-shop.item[2]);
            var chuizi = new GLoader3D();
            chuizi.url = "ui://game/chuizi";
            chuizi.skinName = "default";
            Spine.AnimationState.TrackEntryDelegate boomComplete = null;
            boomComplete = delegate (Spine.TrackEntry trackEntry)
                    {
                        chuizi.RemoveFromParent();
                        GameCommond.inst.hitGrid(index);
                        mvc.send(Notification.UseHammer, index > 0);
                    };
            chuizi.spineAnimation.state.Complete += boomComplete;
            chuizi.animationName = "chuizi";
            this._itemLayer.AddChild(chuizi);
            var gridComp = this._gridList.GetChildAt(index) as GComponent;
            chuizi.x = this._gridList.x + gridComp.x - gridComp.width / 2 + this._gridList.margin.left;
            chuizi.y = this._gridList.y + gridComp.y - gridComp.height / 2 + this._gridList.margin.top;
        }
    }
}