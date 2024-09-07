using System.Threading;
using Cysharp.Threading.Tasks;
using SaintTest.CodeBase.Items;
using UnityEngine;

namespace SaintTest.CodeBase
{
    public class Transition
    {
        private ISender _sender;
        private ITaker _taker;
        private Transform _to;

        private float _transferTime = 1f;
        
        public Transition(ISender sender, ITaker taker, Transform to)
        {
            _sender = sender;
            _taker = taker;
            _to = to;
        }

        public async UniTask Run(CancellationToken cancellationToken)
        {
            float elapsedTime = 0f;

            Item item = _sender.Send();

            Vector3 startPosition = item.transform.position;
            
            while (elapsedTime < _transferTime)
            {
                float progress = elapsedTime / _transferTime;
                item.transform.position = Vector3.Lerp(startPosition, _to.position, progress);

                await UniTask.Yield(cancellationToken);
                elapsedTime += Time.deltaTime;
            }

            item.transform.position = _to.position;
            _taker.Take(item);
        }
    }
}