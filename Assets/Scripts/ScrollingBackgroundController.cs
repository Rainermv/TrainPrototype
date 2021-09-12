using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ScrollingBackgroundController : MonoBehaviour
    {

        //prrivate float ScrollSpeed = -0.5f;
        private Vector2 _savedOffset;
        private Renderer _renderer;
        public float Ratio;
        public float BaseSpeed;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _savedOffset = _renderer.material.mainTextureOffset;
        }

        private void Start()
        {
        }

        private void FixedUpdate()
        {
            var worldSpeed = BaseSpeed * Ratio;
            var worldTime = worldSpeed * Time.fixedTime;

            var repeat = Mathf.Repeat(worldTime, 1);
            var offset = new Vector2(repeat, _savedOffset.y);

            _renderer.material.mainTextureOffset = offset;
        }

        

        private void OnDisable()
        {
            _renderer.material.mainTextureOffset = _savedOffset;
        }


    }
}
