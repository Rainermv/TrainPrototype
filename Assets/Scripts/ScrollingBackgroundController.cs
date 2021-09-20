using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ScrollingBackgroundController : MonoBehaviour
    {
        public float ParallaxRatio = 1;

        private Vector2 _initialOffset;
        private Renderer _renderer;

        [HideInInspector]
        public float Scale = 1;

        public void UpdateScreenPosition(float playerWorldPosition)
        {
            var scaledPosition = playerWorldPosition * ParallaxRatio * (1 / Scale);
            var repeat = Mathf.Repeat(scaledPosition, 1);
            var offset = new Vector2(repeat, _initialOffset.y);
            _renderer.material.mainTextureOffset = offset;
        }

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _initialOffset = _renderer.material.mainTextureOffset;
        }
        

        private void OnDisable()
        {
            _renderer.material.mainTextureOffset = _initialOffset;
        }


        
    }
}
