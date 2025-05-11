using System.Collections;
using UnityEngine;

namespace SpongeScene.Tutorial
{
    public class GuidingArrow : MonoBehaviour
    {
        private Renderer _renderer;
        void Start()
        {
            _renderer = GetComponent<Renderer>();
            StartCoroutine(FadeInAndOut());
        }

        private IEnumerator FadeInAndOut()
        {
            while (gameObject.activeInHierarchy)
            {
                yield return StartCoroutine(UtilityFunctions.FadeImage(_renderer, 1, 0, 1));
                yield return StartCoroutine(UtilityFunctions.FadeImage(_renderer, 0, 1, 1));
            }
        }
    }
}
