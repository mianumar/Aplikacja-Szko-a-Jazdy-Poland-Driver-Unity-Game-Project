using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GameUtils
{
    public class GameTimer 
    {
        static WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        static Coroutine countdowntimerRoutine;

        static Coroutine clockTimerRoutine;
        public static void CoutDownTimer(MonoBehaviour currentBehavior , float countdownTime , UnityAction<float> countdownEvent)
        {
            if (countdowntimerRoutine != null)
            {
                currentBehavior.StopCoroutine(countdowntimerRoutine);
            }
            countdowntimerRoutine = currentBehavior.StartCoroutine(StartCountDown(countdownTime , countdownEvent));
        }

        private static IEnumerator StartCountDown(float countDown , UnityAction<float> countdownEvent)
        {
            while (countDown >= 0)
            {
                countdownEvent?.Invoke(countDown);
                yield return waitForSeconds;
                countDown--;
            }
        }

        /// <summary>
        /// Start game timer 
        /// </summary>
        /// <param name="currentBehavior"></param>
        /// <param name="clockEvent"></param>
        public static void GameClockTimer(MonoBehaviour currentBehavior,float beginTime, UnityAction<float> clockEvent)
        {
            if (clockTimerRoutine != null)
            {
                currentBehavior.StopCoroutine(clockTimerRoutine);
            }
            clockTimerRoutine = currentBehavior.StartCoroutine(StartClock(clockEvent,beginTime));
        }

        private static IEnumerator StartClock (UnityAction<float> clockEvent , float beginTime)
        {
            while (true)
            {
                beginTime += 1f;
                yield return waitForSeconds;
                clockEvent?.Invoke(beginTime);
               
            }
        }

        /// <summary>
        /// Stop Game Clock
        /// </summary>
        /// <param name="currentBehavior"></param>
        public static void StopClock(MonoBehaviour currentBehavior)
        {
            currentBehavior.StopCoroutine(clockTimerRoutine);
        }
    }
}
