using System;
using UnityEngine;

namespace GMTK2020
{
    public interface IDamageDealer
    { 
        int GetDamage { get; }
    }

    public enum ProjectileSide
    {
        Friend,
        Opponent
    }
    
    [RequireComponent(typeof(Collider2D)), RequireComponent(typeof(SpriteRenderer))]
    public class Bullet : MonoBehaviour, IDamageDealer
    {
        public const float KEEP_ALIVE_TIME = 10F;

        public int GetDamage => Config.BulletDamage;

        public BulletConfig Config;
        public float Angle;

        public ProjectileSide Side { get; set; }

        private SpriteRenderer _renderer;
        private float _appearTime;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();

            _renderer.sprite = Config.Sprite;

            _appearTime = Time.time;
        }

        private void Update()
        {
            var translateVec = new Vector3(Mathf.Cos(Mathf.Deg2Rad * Angle), Mathf.Sin(Mathf.Deg2Rad * Angle)) * Config.BulletSpeed * 0.01F;

            transform.position += translateVec;
            transform.rotation = Quaternion.Euler(0F, 0F, Angle);

            if (Time.time - _appearTime > KEEP_ALIVE_TIME)
                Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var obj = collision.gameObject;
            var actor = obj.GetComponent<Actor>();

            if (actor != null)
            {
                if (Side != actor.Side)
                {
                    actor.HealthStats.Health -= Config.BulletDamage;
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}