/*
* https://qiita.com/k7a/items/80984aaf4abae180816c
*/
using System;
using System.Runtime.CompilerServices;
using UnityEngine.Networking;

namespace NetWorking
{
    public class UnityWebRequestAsyncOperationAwaiter : INotifyCompletion
    {
        UnityWebRequestAsyncOperation _asyncOperation;

        public UnityWebRequestAsyncOperationAwaiter GetAwaiter()
        {
            return this;
        }
        public bool IsCompleted
        {
            get { return _asyncOperation.isDone; }
        }

        public UnityWebRequestAsyncOperationAwaiter(UnityWebRequestAsyncOperation asyncOperation)
        {
            _asyncOperation = asyncOperation;
        }

        public void GetResult()
        {
            // NOTE: 結果はUnityWebRequestからアクセスできるので、ここで返す必要性は無い
        }

        public void OnCompleted(Action continuation)
        {
            _asyncOperation.completed += _ => { continuation(); };
        }
    }
}