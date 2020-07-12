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

        public BulletConfig Config { get; set; }
        public float Angle { get; set; }

        public AudioSource SoundSource { get; set; }
        public ProjectileSide Side { get; set; }

        private SpriteRenderer _renderer;
        private float _appearTime;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();

            _appearTime = Time.time;
        }

        public void UpdateSprite()
        {
            _renderer.sprite = Config.Sprite;
        }

        private void Update()
        {
            var translateVec = new Vector3(Mathf.Cos(Mathf.Deg2Rad * Angle), Mathf.Sin(Mathf.Deg2Rad * Angle)) * Config.BulletSpeed;

            transform.position += translateVec * Time.deltaTime;
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
                    Debug.Log(Config.BulletDamage);
                    if (actor.Damage(Config.BulletDamage))
                        Destroy(gameObject);
                }
            }
            else
            {
                if (Config.ImpactSounds.Length > 0)
                    SoundSource.PlayOneShot(Config.ImpactSounds[UnityEngine.Random.Range(0, Config.ImpactSounds.Length)], 0.3F);
                Destroy(gameObject);
            }
        }
    }
}