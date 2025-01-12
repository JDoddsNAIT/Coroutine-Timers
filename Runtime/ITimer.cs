using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JDoddsNAIT.CoroutineTimers
{
    public interface ITimer
    {
        public bool IsRunning { get; }
        public void StartTimer();
        public void StopTimer();
    }
}
