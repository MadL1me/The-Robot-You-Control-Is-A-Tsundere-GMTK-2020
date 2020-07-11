using GMTK2020.Level;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GMTK2020
{
    public class PlayerRage : MonoBehaviour
    {
        private const float ENEMY_PRESENCE_CHECK_RADIUS = 4F;

        [SerializeField] private LevelManager _levelManager;

        private PlayerMovement _movement;
        private WeaponBearer _weaponBearer;
        private float _nextRage;

        private void Start()
        {
            _movement = GetComponent<PlayerMovement>();
            _weaponBearer = GetComponent<WeaponBearer>();
            _nextRage = Time.time + Random.Range(7F, 18F);
        }

        public float GetRemainingTimeUntilRage() =>
            _nextRage - Time.time;

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

            var wastedRounds = _weaponBearer.CurrentWeapon?.WeaponType.MagazineRounds - _weaponBearer.CurrentWeapon?.CurrentRounds;

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
                    return Random.Range(2F, 3.5F);
                case GlitchType.RandomWalkBack:
                case GlitchType.RandomWalkForward:
                case GlitchType.RandomWalkLeft:
                case GlitchType.RandomWalkRight:
                    return Random.Range(4F, 15F);
                case GlitchType.RandomReload:
                    return 0.5F;
            }

            return 0F;
        }

        private void Update()
        {
            if (Time.time > _nextRage)
            {
                var type = DecideGlitchType();

                if (type == GlitchType.None)
                {
                    _nextRage = Time.time + 1F;
                    return;
                }

                var duration = DecideGlitchTypeDuration(type);

                Debug.Log($"Glitching {type} for {duration}");

                _nextRage = Time.time + duration * 1.5F + Random.Range(7F, 19F);

                _movement.ApplyGlitch(type, duration);
            }
        }
    }
}
