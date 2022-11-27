using PotionsPlease.Models;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace PotionsPlease.InGame
{
    public class ShelveObject : MonoBehaviour
    {
        [SerializeField] private Transform _shelve;
        [SerializeField] private int _size;
        [SerializeField] private ReadOnlyCollection<ItemObject> _itemObjectPrefabs;
        // Start is called before the first frame update
        void Start()
        {
            fillShelves();
        }

        private List<float> GetPositions() {
            List<float> positions = new List<float>();
            float minimalX = _shelve.position.x - _shelve.localScale.x / 2.0f + 0.3f;
            float maximalX = _shelve.position.x + _shelve.localScale.x / 2.0f - 0.3f;
            float shift = (maximalX - minimalX) / (_size - 1);
            float position = minimalX;

            for(int i = 0; i < _size; i++)
            {
                positions.Add(position);
                Debug.Log(position);
                position += shift;

            }


            return positions;

        } 
        public void fillObjects(ReadOnlyCollection<ItemObject> collection)
        {
            _itemObjectPrefabs = collection;
            _size = _itemObjectPrefabs.Count;
        } 

        public void fillShelves()
        {
            List<float> positions = GetPositions();
            float y = _shelve.position.y;
            for (int i = 0; i < _itemObjectPrefabs.Count; i++)
            {
                Instantiate(_itemObjectPrefabs[i], new Vector3(positions[i], y, 0), new Quaternion());
            }
        }
    }
}
