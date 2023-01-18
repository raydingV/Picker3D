using System;
using Controllers.Player;
using Enums;
using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Slider _scoreUI;
        [SerializeField] private PlayerMovementController _movementController;
        [SerializeField] private TextMeshProUGUI _scoreUIText;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void Update()
        {
            scoreManagementUI();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize += OnLevelInitialize;
            CoreGameSignals.Instance.onLevelSuccessful += OnLevelSuccessful;
            CoreGameSignals.Instance.onLevelFailed += OnLevelFailed;
            CoreGameSignals.Instance.onReset += OnReset;
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize -= OnLevelInitialize;
            CoreGameSignals.Instance.onLevelSuccessful -= OnLevelSuccessful;
            CoreGameSignals.Instance.onLevelFailed -= OnLevelFailed;
            CoreGameSignals.Instance.onReset -= OnReset;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        public void NextLevel()
        {
            CoreGameSignals.Instance.onNextLevel?.Invoke();
            CoreGameSignals.Instance.onReset?.Invoke();
        }

        public void RestartLevel()
        {
            CoreGameSignals.Instance.onRestartLevel?.Invoke();
            CoreGameSignals.Instance.onReset?.Invoke();
        }

        public void Play()
        {
            CoreGameSignals.Instance.onPlay?.Invoke();
            CoreUISignals.Instance.onClosePanel?.Invoke(1);
            CameraSignals.Instance.onSetCameraTarget?.Invoke();
            describePlayerScripts();
            _movementController._scoreValue = 0;
        }

        private void OnLevelInitialize(int levelValue)
        {
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Level, 0);
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Start, 1);
            UISignals.Instance.onSetNewLevelValue?.Invoke(levelValue);
        }

        private void OnLevelSuccessful()
        {
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Win, 2);
        }

        private void OnLevelFailed()
        {
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Fail, 2);
        }

        private void OnReset()
        {
            CoreUISignals.Instance.onCloseAllPanels?.Invoke();
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Start, 1);
        }

        private void scoreManagementUI()
        {
            _scoreUI.value = _movementController._scoreValue;
            _scoreUIText.text = "% " + (_scoreUI.value * 100).ToString("F0");
        }

        private void describePlayerScripts()
        {
            _movementController = GameObject.Find("LevelHolder/level0(Clone)/PlayerManager").GetComponent<PlayerMovementController>();
        }
    }
}