using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JDoddsNAIT.CoroutineTimers
{
    public class Timer : MonoBehaviour, ITimer
    {
        private bool _isRunning;
        private float _startTime;
        private float _timeElapsed;

        [SerializeField] private float _duration;
        [SerializeField] private bool _startOnAwake;
        [SerializeField] private bool _repeat;
        [SerializeField] private UnityEvent _alarm;

        public bool Paused { get; set; }
        public float Duration { get => _duration; set => _duration = value; }

        public event Action OnStart, OnStop, Alarm;

        public bool IsRunning => _isRunning;
        public float TimeElapsed { get => IsRunning ? _timeElapsed : 0; }
        public float TimeRemaining => _duration - TimeElapsed;

        public void StartTimer()
        {
            _isRunning = true;
            _startTime = Time.time;
            StartCoroutine(TimerTick());
        }

        public void StopTimer()
        {
            _isRunning = false;
            StopCoroutine(TimerTick());
            OnStop?.Invoke();
        }

        private IEnumerator TimerTick()
        {
            OnStart?.Invoke();
            do
            {
                if (!Paused)
                    _timeElapsed = Time.time - _startTime;
                else
                    _startTime = Time.time - TimeElapsed;

                yield return null;
            } while (TimeElapsed < _duration);

            Alarm?.Invoke();
            _alarm.Invoke();

            if (_repeat)
                StartTimer();
            else
                StopTimer();
        }
    }
}
