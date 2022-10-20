/*
* https://qiita.com/k7a/items/80984aaf4abae180816c
*/
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;

namespace NetWorking
{
    public static class UnityWebRequestAsyncOperationExtension
    {
        public static UnityWebRequestAsyncOperationAwaiter ConfigureAwait(this UnityWebRequestAsyncOperation asyncOperation, IProgress<float> progress)
        {
            var progressNotifier = new WebRequestProgressNotifier(asyncOperation, progress);
            ProgressUpdater.Instance.AddItem(progressNotifier);

            return new UnityWebRequestAsyncOperationAwaiter(asyncOperation);
        }
    }
}