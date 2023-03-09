using Cysharp.Threading.Tasks;
using PotionsPlease.Models;
using PotionsPlease.Util.Managers;
using PotionsPlease.Util.Systems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace PotionsPlease.InGame
{
    public class CauldronObject : MonoBehaviour
    {
        [SerializeField] private List<ItemModel> _items;


        [Header("Light")]
        [SerializeField] private Vector2 _lightSinMinMax;
        [SerializeField] private float _lightSinSpeed;
        [SerializeField] private float _lightOnDropItemIntensity;
        [SerializeField] private float _lightFalloffLerpTime;
        [SerializeField] private AnimationCurve _lightOnDropItemCurve;

        [Header("Components")]
        [SerializeField] private ParticleSystem _dropParticleSystem;
        [SerializeField] private Light2D _cauldronLight;

        private float _lightOnDropIntensityCurrent;

        private void Update()
        {
            _lightOnDropIntensityCurrent = Mathf.Lerp(_lightOnDropIntensityCurrent, 0, _lightFalloffLerpTime * Time.deltaTime);
            var minMaxDiffHalf = (_lightSinMinMax.y - _lightSinMinMax.x);
            _cauldronLight.intensity = minMaxDiffHalf + minMaxDiffHalf * Mathf.Sin(Time.time * _lightSinSpeed) + _lightOnDropIntensityCurrent;

            if (Input.GetKeyDown(KeyCode.J))
            {
                _dropParticleSystem.Play();
                _lightOnDropIntensityCurrent = _lightOnDropItemIntensity;
            }
        }

        public void AddItem(ItemModel itemModel)
        {
            AudioSystem.PlaySound("CauldronItemDrop", 0.07f);
            //UIManager.Instance.RecipeInfoPanel.OnItemAddedToCauldron(itemModel);
            _items.Add(itemModel);

            if (_items.Count == 3)
            {
                OrderManager.Instance.EndOrder(_items.ToArray());
                _items.Clear();
            }

            _dropParticleSystem.Play();
            _lightOnDropIntensityCurrent = _lightOnDropItemIntensity;
        }

        public void MenuReset()
        {
            _items.Clear();
        }
    }
}
