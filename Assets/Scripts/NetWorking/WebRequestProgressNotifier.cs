/*
* https://qiita.com/k7a/items/80984aaf4abae180816c
*/
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;

namespace NetWorking{
    public class WebRequestProgressNotifier
    {
        UnityWebRequestAsyncOperation _asyncOp;
        IProgress<float> _progress;

        public WebRequestProgressNotifier(UnityWebRequestAsyncOperation asyncOp, IProgress<float> progress)
        {
            _asyncOp = asyncOp;
            _progress = progress;
        }

        public bool NotifyProgress()
        {
            _progress.Report(_asyncOp.progress);

            return _asyncOp.isDone;
        }
    }

    public class ProgressUpdater : MonoBehaviour
    {
        static ProgressUpdater instance;
        List<WebRequestProgressNotifier> items = new List<WebRequestProgressNotifier>();

        public static ProgressUpdater Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("ProgressUpdater").AddComponent<ProgressUpdater>();
                }

                return instance;
            }
        }

        public void AddItem(WebRequestProgressNotifier item)
        {
            if (!item.NotifyProgress())
            {
                items.Add(item);
            }
        }

        void Update()
        {
            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (item.NotifyProgress())
                {
                    items[i] = null;
                }
            }

            // パフォーマンス的にあまりよろしくない実装なのでどうにかしたい感
            items.RemoveAll(item => item == null);
        }
    }
}