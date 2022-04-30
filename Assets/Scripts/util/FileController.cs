using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileController : MonoBehaviour
{
    public static FileController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    /// <summary>
    /// 对资源的写入操作
    /// </summary>
    /// <typeparam name="T">数据的类型</typeparam>
    /// <param name="data">需要写入的数据</param>
    /// <param name="path">需要写入数据的地址</param>
    public void Write<T>(T data, string path)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }


    /// <summary>
    /// 创建数据文件
    /// </summary>
    /// <typeparam name="T">数据的类型</typeparam>
    /// <param name="data">需要写入的数据</param>
    /// <param name="path">需要写入数据的地址</param>
    public void CreateFile<T>(T data, string path)
    {
        if (!File.Exists(path))
        {
            File.Create(path).Dispose();

            string json = JsonUtility.ToJson(data);
            File.WriteAllText(path, json);
        }
    }



    /// <summary>
    /// 读取资源
    /// </summary>
    /// <typeparam name="T">数据的类型</typeparam>
    /// <param name="path">需要读取数据的地址</param>
    /// <returns></returns>
    public T ReadFile<T>(string path)
    {
        string json = File.ReadAllText(path);
        T data = JsonUtility.FromJson<T>(json);
        return data;
    }
}
