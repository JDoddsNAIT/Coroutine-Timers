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

        [SerializeField, Min(0)] private float _duration;
        [SerializeField] private bool _startOnAwake;
        [SerializeField] private bool _repeat;
        [SerializeField] private UnityEvent _alarm;

        public bool Paused { get; set; }
        public float Duration { get => _duration; set => _duration = value; }

        public delegate void TimerTick(Timer timer);
        public event Action OnStart, OnStop, Alarm;
        public event TimerTick Tick;

        public bool IsRunning => _isRunning;
        public float TimeElapsed { get => IsRunning ? Mathf.Min(_timeElapsed, _duration) : 0; }
        public float TimeRemaining => _duration - TimeElapsed;

        protected virtual void Awake()
        {
            if (_startOnAwake)
            {
                StartTimer();
            }
        }

        public void StartTimer()
        {
            _isRunning = true;
            _startTime = Time.time;
            OnStart?.Invoke();
            StartCoroutine(RunTimer());
        }

        public void RestartTimer()
        {
            StopTimer();
            StartTimer();
        }

        public void StopTimer()
        {
            _isRunning = false;
            StopCoroutine(RunTimer());
            OnStop?.Invoke();
        }

        private IEnumerator RunTimer()
        {
            do
            {
                if (!Paused)
                    _timeElapsed = Time.time - _startTime;
                else
                    _startTime = Time.time - TimeElapsed;

                Tick?.Invoke(this);

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
