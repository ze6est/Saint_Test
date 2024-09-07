using System.Collections;
using System.Collections.Generic;
using SaintTest.CodeBase.Builders;
using SaintTest.CodeBase.Items;
using TMPro;
using UnityEngine;

namespace SaintTest.CodeBase
{
    public class Hud : MonoBehaviour
    {
        [SerializeField] private List<Builder> _builders;

        [SerializeField] private TextMeshProUGUI _ironFull;
        [SerializeField] private TextMeshProUGUI _woodFull;
        [SerializeField] private TextMeshProUGUI _stoneFull;
        
        [SerializeField] private TextMeshProUGUI _woodBuiderEmptyIron;
        [SerializeField] private TextMeshProUGUI _stoneBuiderEmptyIron;
        [SerializeField] private TextMeshProUGUI _stoneBuiderEmptyWood;

        private TextMeshProUGUI[] _allText;
        
        private void Awake()
        {
            foreach (Builder builder in _builders)
            {
                builder.StorageFulled += OnStorageFulled;
                builder.StorageEmpted += OnStorageEmpted;
            }

            StartCoroutine(ClearAllText(_ironFull, _woodFull, _stoneFull, 
                _woodBuiderEmptyIron, _stoneBuiderEmptyIron, _stoneBuiderEmptyWood));
        }
        
        private void OnDestroy()
        {
            foreach (Builder builder in _builders)
            {
                builder.StorageFulled -= OnStorageFulled;
                builder.StorageEmpted -= OnStorageEmpted;
            }
        }

        private void OnStorageFulled(ItemType storageType)
        {
            switch (storageType)
            {
                case ItemType.Iron:
                    _ironFull.text = "Iron full";
                    break;
                case ItemType.Wood:
                    _woodFull.text = "Wood full";
                    break;
                case ItemType.Stone:
                    _stoneFull.text = "Stone full";
                    break;
            }
        }
        
        private void OnStorageEmpted(ItemType builderType, ItemType storageType)
        {
            switch (builderType)
            {
                case ItemType.Wood:
                    _woodBuiderEmptyIron.text = "At Wood Builder empty Iron Storage";
                    break;
                
                case ItemType.Stone:
                    
                    switch (storageType)
                    {
                        case ItemType.Iron:
                            _stoneBuiderEmptyIron.text = "At Stone Builder empty Iron Storage";
                            break;
                        
                        case ItemType.Wood:
                            _stoneBuiderEmptyWood.text = "At Stone Builder empty Wood Storage";
                            break;
                    }
                    break;
            }
        }

        private IEnumerator ClearAllText(params TextMeshProUGUI[] texts)
        {
            WaitForSeconds wait = new WaitForSeconds(1f);
            
            while (true)
            {
                foreach (TextMeshProUGUI text in texts)
                {
                    text.text = "";
                }
                
                yield return wait;
            }
        }
    }
}