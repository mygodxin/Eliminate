using FairyGUI;
using System;
using UnityEditor;
using UnityEngine;

namespace mm
{
    public class FairyLoader : ASourceLoader
    {
        public FairyLoader(string name)
        {
            this.fileName = name;
        }
        protected override void Start()
        {
            // UIPackage.AddPackage(mm.Global.fairyPath + this.fileName, this.onPackLoaded);
            // UIPackage.LoadResource _loadFromAssetsPath = (string name, string extension, System.Type type, out DestroyMethod destroyMethod) =>
            // {
            //     destroyMethod = DestroyMethod.Unload;
            //     /* return  */
            //     var obj = AssetDatabase.LoadAssetAtPath(name + extension, type);
            //     if (obj != null)
            //     {
            //         return obj;
            //     }
            //     return null;
            // };
            UIPackage.AddPackage(mm.Global.fairyPath + this.fileName);//, _loadFromAssetsPath);
            //Debug.Log("包加载完成" + name + "," + extension + "," + type);
            UIPackage pkg = UIPackage.GetByName(this.fileName);
            this.onPackLoaded(pkg);
        }

        void onPackLoaded(UIPackage pkg)
        {
            base.Complete();
            mvc.send(ASourceLoader.EVT_SourceLoader_CompleteEvent, this);
        }

        protected override void Failed()
        {

        }
    }
}