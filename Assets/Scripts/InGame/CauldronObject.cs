using PotionsPlease.Models;
using System.Collections.Generic;
using UnityEngine;

namespace PotionsPlease.InGame
{
    public class CauldronObject : MonoBehaviour
    {
        [SerializeField] private List<ItemModel> _items;

        [Header("Components")]
        [SerializeField] private ParticleSystem _dropParticleSystem;

        public void AddItem(ItemModel item)
        {
            _items.Add(item);
            _dropParticleSystem.Play();
        }
    }
}
