using Data.ValueObjects;
using Keys;
using Managers;
using Signals;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace Controllers.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerManager manager;
        [SerializeField] private PlayerPhysicsController physicsController;

        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private new Collider collider;

        #endregion

        #region Private Variables

        [ShowInInspector] private MovementData _data;

        [ShowInInspector] private bool _isReadyToMove, _isReadyToPlay, _finalMovement, _collectableScore;

        private float _xValue;
        private float2 _clampValues;
        public float _scoreValue;

        #endregion

        #endregion

        internal void SetMovementData(MovementData movementData)
        {
            _data = movementData;
        }

        private void FixedUpdate()
        {
            if(_scoreValue >= 1)
            {
                _scoreValue = 1;
            }

            if (!_isReadyToPlay)
            {
                StopPlayer();
                return;
            }

            if (_isReadyToMove)
            {
                MovePlayer();
            }
            else
            {
                StopPlayerHorizontaly();
            }

            if(_collectableScore)
            {
                _scoreValue += 0.015f;
                CollectableScore(false);
            }

            if(_finalMovement)
            {
                SlowDownPlayer();
                MovementSpeedFinal();
            }

            if (_data.ForwardSpeed <= 0)
            {
                endGame();
            }
        }

        private void MovePlayer()
        {
            var velocity = rigidbody.velocity;
            velocity = new float3(_xValue * _data.SidewaysSpeed, velocity.y,
                _data.ForwardSpeed);
            rigidbody.velocity = velocity;

            float3 position;
            position = new float3(
                Mathf.Clamp(rigidbody.position.x, _clampValues.x,
                    _clampValues.y),
                (position = rigidbody.position).y,
                position.z);
            rigidbody.position = position;
        }

        private void StopPlayerHorizontaly()
        {
            rigidbody.velocity = new float3(0, rigidbody.velocity.y, _data.ForwardSpeed);
            rigidbody.angularVelocity = float3.zero;
        }

        private void SlowDownPlayer()
        {
            _scoreValue -= Time.deltaTime / 10f;
        }

        private void StopPlayer()
        {
            rigidbody.velocity = float3.zero;
            rigidbody.angularVelocity = float3.zero;
        }

        internal void IsReadyToPlay(bool condition)
        {
            _isReadyToPlay = condition;
        }

        internal void IsReadyToMove(bool condition)
        {
            _isReadyToMove = condition;
        }

        internal void FinalMovement(bool condition)
        {
            _finalMovement = condition;
        }

        internal void CollectableScore(bool condition)
        {
            _collectableScore = condition;
        }

        internal void UpdateInputParams(HorizontalnputParams inputParams)
        {
            _xValue = inputParams.HorizontalInputValue;
            _clampValues = new float2(inputParams.HorizontalInputClampNegativeSide,
                inputParams.HorizontalInputClampPositiveSide);
        }

        internal void OnReset()
        {
            StopPlayer();
            _isReadyToPlay = false;
            _isReadyToMove = false;
            _finalMovement = false;
        }

        private void MovementSpeedFinal()
        {
            _data.ForwardSpeed = _scoreValue * _data.MiniGameMultiplier;
        }

        private void endGame()
        {
            StopPlayer();
            CoreGameSignals.Instance.onLevelSuccessful?.Invoke();
        }
    }
}
