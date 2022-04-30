using System;
using UnityEngine;

namespace mm
{
    public class SourcePreLoader
    {
        protected ASourceLoader[] loaderList;
        protected int _numSources;
        protected int loadPosition;
        readonly int numSources;
        public bool isLoading;
        public bool isComplete;
        public bool hasError;
        public int numRetrys;
        public delegate void PreLoaderCallback(int index, ASourceLoader loader);
        public SourcePreLoader()
        {
            mvc.on(ASourceLoader.EVT_SourceLoader_CompleteEvent, this.onItemLoaded);
        }
        private void onItemLoaded(System.Object obj)
        {
            var index = Array.IndexOf(this.loaderList, obj);
            if (index != -1 && this.loadPosition == index)
            {
                this.preload(index + 1);
            }
        }
        public void addSource(ASourceLoader[] sourceLoader)
        {
            this.loaderList = sourceLoader;
            this._numSources = sourceLoader.Length;
        }
        public void preload(int index = 0)
        {
            if (this.isComplete)
            {
                return;
            }
            if (index >= this._numSources)
            {
                
                for (var i = 0; i < this._numSources; ++i)
                {
                    var item_1 = this.loaderList[i];
                    if (!item_1.loaded)
                    {
                        if (item_1.retry < this.numRetrys)
                        {
                            this.preload(i);
                            return;
                        }
                        else
                        {
                            this.hasError = false;
                        }
                    }
                }
                if (!this.hasError)
                {
                    this.isComplete = true;
                }
                return;
            }
            var item = this.loaderList[index];
            if (item.loading || item.loaded)
            {
                this.preload(index + 1);
                return;
            }
            if (item.retry >= 7)
            {
                this.hasError = true;
                this.preload(index + 1);
                return;
            }
            this.isLoading = true;
            item.Load();
        }
        // protected void onItemLoaded(ASourceLoader sourceLoader)
        // {

        // }
        private void reload()
        {
            if (this.isLoading)
                return;
            if (this.isComplete && this.hasError)
            {
                this.isComplete = false;
                this.hasError = false;
                for (var i = 0; i < this.loaderList.Length; ++i)
                {
                    this.loaderList[i].retry = 0;
                }
                this.preload();
            }
        }
        void forEach(PreLoaderCallback callback)
        {
            for (var i = 0; i < this._numSources; ++i)
            {
                callback(i, this.loaderList[i]);
            }
        }
    }
}
