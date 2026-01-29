using _Project.Scripts.Core;
using _Project.Scripts.Data;
using _Project.Scripts.View;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

namespace _Project.Scripts.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private Transform tilesPoolContainer;
        [SerializeField] private Transform slotsContainer;
        [SerializeField] private LevelPicker levelPicker;
        public override void InstallBindings()
        {
            Container.Bind<GameConfig>().FromInstance(gameConfig).AsSingle();
            Container.BindMemoryPool<TileView, TileView.Pool>()
                .WithInitialSize(gameConfig.rowsCount * gameConfig.columnsCount)
                .FromComponentInNewPrefab(gameConfig.tilePrefab)
                .UnderTransform(tilesPoolContainer);
            Container.BindInterfacesAndSelfTo<GridManager>().AsSingle().WithArguments(slotsContainer).NonLazy();
            Container.BindInterfacesAndSelfTo<InputHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelPicker>().FromInstance(levelPicker).AsSingle();
            Container.BindInterfacesAndSelfTo<CollectionService>().AsSingle().NonLazy();
        }
    }
}