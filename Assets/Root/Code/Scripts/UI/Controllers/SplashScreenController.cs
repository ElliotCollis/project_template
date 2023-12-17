using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace HowlingMan.UI
{
    public class SplashScreenController : MonoBehaviour
    {
        public float fadeTime = 1f;
        public float waitTime = 1f;
        public Image uiImage;
        public Sprite[] sprites;

        private Queue<Sprite> splashes;
        private Tween currentTween;

        private void Awake()
        {
            splashes = new Queue<Sprite>(sprites);

            StartCoroutine(PlaySplashSequence());
        }

        private IEnumerator PlaySplashSequence()
        {
            while (splashes.Count > 0)
            {
                Sprite splash = splashes.Dequeue();
                uiImage.sprite = splash;

                // Fade in
                currentTween = uiImage.DOFade(1, fadeTime).SetEase(Ease.InOutQuad);
                yield return currentTween.WaitForCompletion();

                // Wait a moment while the image is fully visible
                yield return new WaitForSeconds(waitTime);

                // Fade out
                currentTween = uiImage.DOFade(0, fadeTime).SetEase(Ease.InOutQuad);
                yield return currentTween.WaitForCompletion();
            }

            OnComplete();
        }

        private void OnComplete()
        {
            GameManager.instance.levelManager.LoadHome();
        }
    }
}
