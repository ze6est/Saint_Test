using System.Threading;
using Cysharp.Threading.Tasks;
using SaintTest.CodeBase.Configs;
using SaintTest.CodeBase.Items;
using SaintTest.CodeBase.Logic;
using UnityEngine;

namespace SaintTest.CodeBase.Transitions
{
    public class Transition
    {
        private readonly ISender _sender;
        private readonly ITaker _taker;
        private readonly Transform _to;

        private readonly float _transferTime = GlobalGameConfigs.TRANSFER_TIME;
        
        public Transition(ISender sender, ITaker taker, Transform to)
        {
            _sender = sender;
            _taker = taker;
            _to = to;
        }

        public async UniTask Run(CancellationToken token)
        {
            float elapsedTime = 0f;

            Item item = _sender.Send();

            Vector3 startPosition = item.transform.position;
            
            while (elapsedTime < _transferTime)
            {
                float progress = elapsedTime / _transferTime;
                item.transform.position = Vector3.Lerp(startPosition, _to.position, progress);

                await UniTask.Yield(token);
                elapsedTime += Time.deltaTime;
            }

            item.transform.position = _to.position;
            _taker.Take(item);
        }
    }
}