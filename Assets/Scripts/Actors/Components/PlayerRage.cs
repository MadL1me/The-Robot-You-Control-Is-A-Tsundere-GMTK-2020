using System.Collections;
using GMTK2020.Level;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMTK2020.HUD;
using UnityEngine;

namespace GMTK2020
{
    public class PlayerRage : MonoBehaviour
    {
        private const float ENEMY_PRESENCE_CHECK_RADIUS = 4F;

        [SerializeField] private LevelManager _levelManager;

        private PlayerMovement _movement;
        private WeaponBearer _weaponBearer;
        
        [SerializeField] private float _nextRage;
        [SerializeField] private float _rageLength;
        [SerializeField] private OutOfControlTextSetter _outOfControl;
        
        private float _rageTimer = 0;
        
        private void Start()
        {
            _rageTimer = 0;
            _movement = GetComponent<PlayerMovement>();
            _weaponBearer = GetComponent<WeaponBearer>();
        }

        public float GetRemainingTimeUntilRage() =>
            _nextRage - _rageTimer;

        private GlitchType GetRandomMoveGlitchType()
        {
            switch (Random.Range(0, 5))
            {
                case 0: return GlitchType.RandomWalkLeft;
                case 1: return GlitchType.RandomWalkRight;
                case 2: return GlitchType.RandomWalkForward;
                case 3: return GlitchType.RandomWalkBack;
            }

            return GlitchType.RandomWalkLeft;
        }

        private GlitchType DecideGlitchType()
        {
            if (_levelManager.IsCompleted)
                return GlitchType.None;

            var enemiesNear = Physics2D
                .OverlapBoxAll(transform.position, new Vector2(ENEMY_PRESENCE_CHECK_RADIUS, ENEMY_PRESENCE_CHECK_RADIUS), 0F)
                .Where(x => x.CompareTag("Enemy"))
                .Count();

            var wastedRounds = _weaponBearer.CurrentWeapon?.WeaponConfig.MagazineRounds - _weaponBearer.CurrentWeapon?.CurrentRounds;

            if (enemiesNear >= 1 && Random.Range(0, 3) == 0)
                return GlitchType.RandomShoot;
            else if (wastedRounds != 0 && wastedRounds <= 5)
                return GlitchType.RandomReload;
            else
                return GetRandomMoveGlitchType(); // TODO: Pathfind to objective
            /*else
                return GlitchType.None;*/
        }

        private float DecideGlitchTypeDuration(GlitchType type)
        {
            switch (type)
            {
                case GlitchType.RandomShoot:
                    return _rageLength;
                case GlitchType.RandomWalkBack:
                case GlitchType.RandomWalkForward:
                case GlitchType.RandomWalkLeft:
                case GlitchType.RandomWalkRight:
                    return _rageLength;
                case GlitchType.RandomReload:
                    return 0.5F;
            }

            return 0F;
        }

        private void Update()
        {
            _rageTimer += Time.deltaTime;
            
            if (_rageTimer > _nextRage)
            {
                Debug.Log("Rage AAAAAAAAAA");
                
                var type = DecideGlitchType();

                if (type == GlitchType.None)
                {
                    _rageTimer = 0;
                    return;
                }

                var duration = DecideGlitchTypeDuration(type);

                Debug.Log($"Glitching {type} for {duration}");

                 //_nextRage = Time.time + duration * 1.5F + Random.Range(7F, 19F);
                _outOfControl.SetText(type);
                 _rageTimer = -_rageLength;
                _movement.ApplyGlitch(type, duration);
                StartCoroutine(ChangeNameAfterGlitch(_rageLength));
            }
        }

        private IEnumerator ChangeNameAfterGlitch(float time)
        {
            yield return new WaitForSeconds(time);
            _outOfControl.SetText(GlitchType.None);
        }
    }
}
