using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GameUtils
{
    public class GameTimer 
    {
        static WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        static Coroutine countdowntimerRoutine;
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
    }
}
