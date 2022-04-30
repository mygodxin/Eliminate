[System.Serializable]
public class CShop
{
    public CSkin[] skin;
    public CBg[] bg;
    public int[] price;
    public int[] video;
    public int[] item;
}
[System.Serializable]
public class CSkin
{
    public string name;
    public string icon;
    public int[] price;
    public int[] video;
    public int times;
}

[System.Serializable]
public class CBg
{
    public string name;
    public string icon;
    public int[] price;
    public int[] video;
    public int times;
}