using System.Collections;
using Mirror;
using UnityEngine;

namespace UI
{
    public class Crossfade : MonoBehaviour
    {
        public Animator transition;
        private static readonly int Black = Animator.StringToHash("Black");

        public void FadeIn()
        {
            transition.SetBool(Black, false);
        }

        public void FadeOut()
        {
            transition.SetBool(Black, true);
        }

        public void StartCrossfade(int seconds)
        {
            StartCoroutine(RunCrossfade(seconds));
        }

        IEnumerator RunCrossfade(int seconds)
        {
            FadeOut();

            yield return new WaitForSeconds(seconds);
            
            FadeIn();
        }
    }
}