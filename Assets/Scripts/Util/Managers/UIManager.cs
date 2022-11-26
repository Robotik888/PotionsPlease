using PotionsPlease.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PotionsPlease.InGame
{
    public class UIManager : ManagerBase<UIManager>
    {
        [SerializeField] public TextMeshProUGUI label;
        [SerializeField] public TextMeshProUGUI description;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void changeTexts(string label, string description)
        {
            this.label.text = label;
            this.description.text = description;
        }

    }
}
