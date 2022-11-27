using PotionsPlease.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionsPlease.InGame
{


    public class PotionObject : MonoBehaviour
    {
        [SerializeField] private PotionModel _model;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public List<ItemModel> getNeeded()
        {
            return _model.getNeeded();

        }
    }
}
