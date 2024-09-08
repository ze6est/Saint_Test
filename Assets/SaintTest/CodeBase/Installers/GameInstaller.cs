using System.Collections.Generic;
using SaintTest.CodeBase.Builders;
using SaintTest.CodeBase.Factories;
using SaintTest.CodeBase.Inputs;
using SaintTest.CodeBase.Items;
using SaintTest.CodeBase.Players;
using SaintTest.CodeBase.Pool;
using UnityEngine;
using Zenject;

namespace SaintTest.CodeBase.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private List<Builder> _builders;
        [Header("Item settings")]
        [SerializeField] private List<Item> _itemPrefabs;
        [SerializeField] private int _countItemInPool = 10;
        
        public override void InstallBindings()
        {
            BindPlayer();

            Container.Bind<ItemFactory>().AsSingle();
            Container.Bind<ItemPool>().AsSingle().WithArguments(_itemPrefabs, _countItemInPool, transform).NonLazy();

            BindBuilders();
        }

        private void BindPlayer()
        {
            Container.Bind<InputHandler>().AsSingle();
            Container.Bind<PlayerMover>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerRotator>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle().NonLazy();
        }

        private void BindBuilders()
        {
            foreach (Builder builder in _builders)
            {
                Container.BindInstance(builder);
            }
        }
    }
}