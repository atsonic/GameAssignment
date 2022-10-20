using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace Utils{
    public class CountDownTimer : MonoBehaviour
    {
        public IntReactiveProperty CountDownTime = new IntReactiveProperty(30);
        private IDisposable _disposableTimer;
        public void StartTimer(){
            _disposableTimer = Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(_ => {
                CountDownTime.Value--;
            }).AddTo(this);
        }
        public void StopTimer(){
            _disposableTimer.Dispose();
        }
    }
}