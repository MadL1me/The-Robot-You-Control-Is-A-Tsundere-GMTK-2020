using System;
using System.Collections;
using UnityEngine;

namespace GMTK2020
{
    [Flags]
    public enum GlitchType
    {
        None = 0,
        RandomShoot = 1 << 0,
        RandomWalkLeft = 1 << 1,
        RandomWalkRight = 1 << 2,
        RandomWalkForward = 1 << 3,
        RandomWalkBack = 1 << 4,
        RandomReload = 1 << 5
    }

    public class PlayerMovement : MonoBehaviour
    {
        private const int RAGE_INPUT_GLITCH_FREQUENCY = 30;

        public Vector3 MoveVector { get; protected set; }

        private Rigidbody2D _rigidbody;
        private WeaponBearer _bearer;
        private Player _player;
        private CameraFollow _cameraFollow;
        
        [SerializeField] private float _movingSpeed;
        [SerializeField] private float _dashingSpeed;
        [SerializeField] private float _dashingLength;
        [SerializeField] private float _dashTimeout;
        [SerializeField] private Camera _camera;
        
        private bool _isDashing;
        private float _horizontalMove;
        private float _verticalMove;

        private GlitchType _glitch;
        private float _glitchStart;
        private float _glitchDuration;
        private float _glitchEffectCounter;

        private HealthComponent _healthComponent;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _healthComponent = GetComponent<HealthComponent>();
            _bearer = GetComponent<WeaponBearer>();
            _player = GetComponent<Player>();
            _cameraFollow = _camera.GetComponent<CameraFollow>();
        }

        private void Update()
        {
            HandleInputs();

            if (_glitch != GlitchType.None)
            {
                if (_glitchEffectCounter > 0F)
                    _glitchEffectCounter -= Time.deltaTime;

                if (Time.time - _glitchStart > _glitchDuration)
                    _glitch = GlitchType.None;
            }
        }

        public float GetGlitchProgress() =>
            (Time.time - _glitchStart) / _glitchDuration;

        public GlitchType GetCurrentGlitch() =>
            _glitch;

        public void ApplyGlitch(GlitchType glitch, float duration)
        {
            _glitch = glitch;
            _glitchStart = Time.time;
            _glitchDuration = duration;
        }

        private void HandleInputs()
        {
            if ((Input.GetKeyDown(KeyCode.R) || _glitch.HasFlag(GlitchType.RandomReload)) && _bearer.CanReload())
                _bearer.Reload();

            if (Input.GetKeyDown(KeyCode.Alpha1))
                _bearer.TrySetWeapon(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                _bearer.TrySetWeapon(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                _bearer.TrySetWeapon(2);

            if ((_bearer.CurrentWeapon?.WeaponConfig.IsAutomatic == true ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1"))
                || _glitch.HasFlag(GlitchType.RandomShoot))
            {
                var vecDiff = Input.mousePosition - new Vector3(_camera.pixelWidth, _camera.pixelHeight) / 2F;

                var direction = (Mathf.Atan2(vecDiff.y, vecDiff.x) * Mathf.Rad2Deg + 360) % 360;

                if (_bearer.Shoot(direction))
                    _cameraFollow.Shake(_bearer.CurrentWeapon.WeaponConfig.ScreenShakeAmount);
            }

            if (_isDashing)
                return;
            
            _horizontalMove = Input.GetAxisRaw("Horizontal");
            _verticalMove = Input.GetAxisRaw("Vertical");

            if (_glitch.HasFlag(GlitchType.RandomWalkLeft))
            {
                if (_glitchEffectCounter > 0F)
                    _horizontalMove = Math.Max(_horizontalMove, 0F) - 1F;
                else if (UnityEngine.Random.Range(0, RAGE_INPUT_GLITCH_FREQUENCY) == 0)
                    _glitchEffectCounter += UnityEngine.Random.Range(0.05F, 0.5F);
            }
            else if (_glitch.HasFlag(GlitchType.RandomWalkRight))
            {
                if (_glitchEffectCounter > 0F)
                    _horizontalMove = Math.Min(_horizontalMove, 0F) + 1F;
                else if (UnityEngine.Random.Range(0, RAGE_INPUT_GLITCH_FREQUENCY) == 0)
                    _glitchEffectCounter += UnityEngine.Random.Range(0.05F, 0.5F);
            }

            if (_glitch.HasFlag(GlitchType.RandomWalkForward))
            {
                if (_glitchEffectCounter > 0F)
                    _verticalMove = Math.Min(_verticalMove, 0F) + 1F;
                else if (UnityEngine.Random.Range(0, RAGE_INPUT_GLITCH_FREQUENCY) == 0)
                    _glitchEffectCounter += UnityEngine.Random.Range(0.05F, 0.5F);
            }
            else if (_glitch.HasFlag(GlitchType.RandomWalkBack))
            {
                if (_glitchEffectCounter > 0F)
                    _verticalMove = Math.Max(_verticalMove, 0F) - 1F;
                else if (UnityEngine.Random.Range(0, RAGE_INPUT_GLITCH_FREQUENCY) == 0)
                    _glitchEffectCounter += UnityEngine.Random.Range(0.05F, 0.5F);
            }

            MoveVector = new Vector3(_horizontalMove, _verticalMove).normalized;
            
            if (Input.GetKeyDown(KeyCode.Space))
                StartCoroutine(Dash());
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = (MoveVector * _movingSpeed * Time.fixedDeltaTime);
        }

        private IEnumerator Dash()
        {
            _isDashing = true;
            _healthComponent.IsInvisible = true;
            
            var curPosition = transform.position;
            var nextPosition = MoveVector * _dashingLength * _dashingSpeed;
            var difference = (nextPosition - curPosition).normalized;
            var wait = new WaitForFixedUpdate();
            
            for (float i = 0; i < 1; i += Time.deltaTime)
            {
                _rigidbody.velocity = difference * _dashingSpeed;
                yield return wait;
            }

            _healthComponent.IsInvisible = false;
            yield return new WaitForSeconds(_dashTimeout);
            _isDashing = false;
        }
    }
}