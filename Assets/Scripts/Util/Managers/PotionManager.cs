using PotionsPlease.InGame;
using PotionsPlease.Models;
using System.Collections.Generic;
using UnityEngine;

namespace PotionsPlease.Util.Managers
{
    public class PotionManager : ManagerBase<PotionManager>
    {
        [field: SerializeField] public PotionOverlay PotionOverlay { get; private set; }

        public async void NewPotion(ItemModel[] items)
        {
            PotionOverlay.gameObject.SetActive(true);
            await PotionOverlay.ShowNewPotionProcedure();
            PotionOverlay.gameObject.SetActive(false);

            OrderManager.Instance.NextOrder();
        }
    }
}
