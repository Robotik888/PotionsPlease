using PotionsPlease.Models;
using System.Collections.ObjectModel;
using UnityEngine;

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
        [SerializeField, Min(0)] private float _width;
        [SerializeField] private ItemObject _itemObjectPrefab;

        public Vector2[] GetItemObjectPositions() 
        {
            Vector2[] positions = new Vector2[_size];

            float minX = -_width / 2 + _edgeOffset;
            float maxX = _width / 2 - _edgeOffset;
            float shift = (maxX - minX) / (_size - 1);
            float xPos = minX;

            for(int i = 0; i < _size; i++)
            {
                positions[i] = transform.position.SetX(xPos);
                xPos += shift;
            }

            return positions;
        }

        public void SetItemsActive(bool value)
        {
            for (int i = 0; i < ItemObjects.Length; i++)
                ItemObjects[i].SetActive(value);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position - Vector3.right * _width / 2, transform.position + Vector3.right * _width / 2);
        }
    }
}
