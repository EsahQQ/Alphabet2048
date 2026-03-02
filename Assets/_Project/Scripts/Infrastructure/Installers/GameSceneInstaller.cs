using _Project.Scripts.Data;
using _Project.Scripts.GamePlay.Grid;
using _Project.Scripts.GamePlay.Inputs;
using _Project.Scripts.GamePlay.LevelsLogic;
using _Project.Scripts.GamePlay.View;
using _Project.Scripts.Infrastructure.Services;
using _Project.Scripts.UI.Windows;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure.Installers
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
            Container.BindInterfacesAndSelfTo<InputHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LevelPicker>().FromInstance(levelPicker).AsSingle();
            Container.BindInterfacesAndSelfTo<CollectionService>().AsSingle().NonLazy();
            Container.Bind<WindowManager>().FromComponentInHierarchy().AsSingle();
        }
    }
}