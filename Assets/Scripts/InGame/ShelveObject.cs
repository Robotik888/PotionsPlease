using PotionsPlease.Models;
using System.Collections.ObjectModel;
using UnityEngine;
using static UnityEditor.Progress;

namespace PotionsPlease.InGame
{
    [SelectionBase]
    public class ShelveObject : MonoBehaviour
    {
        public int Size => _size;
        public ItemObject ItemObjectPrefab => _itemObjectPrefab;
        
        [field: SerializeField] public ItemObject[] ItemObjects { get; set; }

        [SerializeField] private float _edgeOffset;
        [SerializeField, Range(0, 10)] private int _size;
        [SerializeField] private ReadOnlyCollection<ItemObject> _itemObjectPrefabs;

        [Space]
        [SerializeField] private Transform _shelve;
        [SerializeField] private ItemObject _itemObjectPrefab;

        public Vector2[] GetItemObjectPositions() 
        {
            Vector2[] positions = new Vector2[_size];

            float minX = _shelve.position.x - _shelve.localScale.x / 2.0f + _edgeOffset;
            float maxX = _shelve.position.x + _shelve.localScale.x / 2.0f - _edgeOffset;
            float shift = (maxX - minX) / (_size - 1);
            float xPos = minX;

            for(int i = 0; i < _size; i++)
            {
                positions[i] = transform.position.SetX(xPos);
                xPos += shift;
            }

            return positions;
        }

        public void SetItems(ItemModel[] items)
        {
            Debug.Assert(items.Length == ItemObjects.Length, "Item model count must match item object count!");

            for (int i = 0; i < items.Length; i++) 
                ItemObjects[i].ResetState(items[i]);
        }

        public void SetItemsInactive()
        {
            for (int i = 0; i < ItemObjects.Length; i++)
                ItemObjects[i].SetIncactive();
        }
    }
}
