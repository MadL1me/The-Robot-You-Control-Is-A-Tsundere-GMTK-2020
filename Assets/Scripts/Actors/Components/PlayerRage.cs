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

        private PlayerMovement _mov;
        private float _nextRage;

        private void Start()
        {
            _mov = GetComponent<PlayerMovement>();
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
            var enemiesNear = Physics2D
                .OverlapBoxAll(transform.position, new Vector2(ENEMY_PRESENCE_CHECK_RADIUS, ENEMY_PRESENCE_CHECK_RADIUS), 0F)
                .Where(x => x.CompareTag("Enemy"))
                .Count();

            var moveDesire = Random.Range(0.5F, 1.3F);
            var shootDesire = 0.2F;

            shootDesire += enemiesNear * 0.75F;

            if (shootDesire > moveDesire)
                return GlitchType.RandomShoot;
            else if (moveDesire > shootDesire)
                return GetRandomMoveGlitchType(); // TODO: Pathfind to objective
            else
                return GlitchType.None;
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
            }

            return 0F;
        }

        private void Update()
        {
            if (Time.time > _nextRage)
            {
                var type = DecideGlitchType();
                var duration = DecideGlitchTypeDuration(type);

                Debug.Log($"Glitching {type} for {duration}");

                _nextRage = Time.time + duration * 1.5F + Random.Range(7F, 19F);

                _mov.ApplyGlitch(type, duration);
            }
        }
    }
}
