using PotionsPlease.Models;
using PotionsPlease.Util.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PotionsPlease.InGame
{
    public class CauldronObject : MonoBehaviour
    {
        [SerializeField] private List<ItemModel> _items;

        [Header("Components")]
        [SerializeField] private ParticleSystem _dropParticleSystem;


        public void AddItem(ItemModel itemModel)
        {
            UIManager.Instance.RecipeInfoPanel.OnItemAddedToCauldron(itemModel);
            _items.Add(itemModel);

            if (_items.Count == 2)
            {
                OrderManager.Instance.EndOrder(_items.ToArray());
                _items.Clear();
            }

            _dropParticleSystem.Play();
        }
    }
}
