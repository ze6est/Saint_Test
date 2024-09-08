using System.Collections;
using System.Collections.Generic;
using SaintTest.CodeBase.Builders;
using SaintTest.CodeBase.Items;
using TMPro;
using UnityEngine;

namespace SaintTest.CodeBase.UI
{
    public class Hud : MonoBehaviour
    {
        [SerializeField] private List<Builder> _builders;
        [SerializeField] private TextMeshProUGUI _mesagges; 
        [SerializeField] private float _clearMessageInterval = 1f;

        private HashSet<string> _eventMessages;
        
        private Coroutine _clearMessagesCoroutine;

        private void Awake()
        {
            _eventMessages = new HashSet<string>();
            
            foreach (Builder builder in _builders)
            {
                builder.StorageFulled += OnStorageFulled;
                builder.StorageEmpted += OnStorageEmpted;
            }
        }
        
        private void Start() => 
            _clearMessagesCoroutine = StartCoroutine(ClearMessages());

        private void OnDestroy()
        {
            foreach (Builder builder in _builders)
            {
                builder.StorageFulled -= OnStorageFulled;
                builder.StorageEmpted -= OnStorageEmpted;
            }

            if (_clearMessagesCoroutine != null)
                StopCoroutine(_clearMessagesCoroutine);
        }

        private void OnStorageFulled(ItemData storageType)
        {
            _eventMessages.Add($"{storageType.name} full");
            UpdateInfoText();
        }

        private void OnStorageEmpted(ItemData builderType, ItemData storageType)
        {
            _eventMessages.Add($"At {builderType.name} Builder empty {storageType.name} Storage");
            UpdateInfoText();
        }
        
        private void UpdateInfoText() => 
            _mesagges.text = string.Join("\n", _eventMessages);

        private IEnumerator ClearMessages()
        {
            WaitForSeconds wait = new WaitForSeconds(_clearMessageInterval);
            
            while (true)
            {
                yield return wait;
                _eventMessages.Clear();
                UpdateInfoText();
            }
        }
    }    
}