using PotionsPlease.InGame;
using PotionsPlease.Models;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace PotionsPlease.Util.Managers
{
    public class OrderManager : MonoBehaviour
    {
        [SerializeField] public List<PotionObject> potions;
        private PotionObject _currentPotion;
        [SerializeField] public List<ItemObject> ingredients;
        [SerializeField] public List<ShelveObject> shelves;
        [SerializeField] public int shelveMax;

        private void Awake()
        {
            newPotion();
            storeItems();
        }


        // Start is called before the first frame update
        void Start()
        {

        }

        private void newPotion()
        {
            int potionIndex = Random.Range(0, potions.Count);
            _currentPotion = potions[potionIndex];
        }

        public void storeItems()
        {
            ReadOnlyCollection<ItemObject> items = getItems();
            shelves[0].fillObjects(items);
            shelves[1].fillObjects((new List<ItemObject>()).AsReadOnly());
        }

        public ReadOnlyCollection<ItemObject> getItems()
        {
            int maxCount = shelveMax * shelves.Count;
            if (maxCount > ingredients.Count)
            {
                return ingredients.AsReadOnly();
            }
            List<ItemObject> toShelves = new List<ItemObject>();
            List<ItemModel> models =_currentPotion.getNeeded();
            foreach (ItemModel model in models)
            {
                foreach (ItemObject itemObject in ingredients)
                {
                    if (itemObject.getModel() == model)
                    {
                        toShelves.Add(itemObject);
                    }
                }
            }
            return returnList(toShelves);
        }

        private ReadOnlyCollection<ItemObject> returnList(List<ItemObject> toShelves)
        {
            List<ItemObject> toReturn = new List<ItemObject>();
            while (toShelves.Count != 0)
            {
                int random = Random.Range(0, toShelves.Count - 1);
                toReturn.Add(toShelves[random]);
                toShelves.RemoveAt(random);

            }
            return toReturn.AsReadOnly();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown("c"))
            {
                newPotion();
                storeItems();
                foreach(ShelveObject shelve in shelves)
                {
                    shelve.fillShelves();
                }
            }
        }
    }
}
