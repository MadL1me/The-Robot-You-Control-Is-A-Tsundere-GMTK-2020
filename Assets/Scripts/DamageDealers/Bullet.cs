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
        public int GetDamage => Config.BulletDamage;

        public BulletConfig Config;
        public float Angle;

        public ProjectileSide Side { get; set; }

        private SpriteRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();

            _renderer.sprite = Config.Sprite;
        }

        private void Update()
        {
            var translateVec = new Vector3(Mathf.Cos(Mathf.Deg2Rad * Angle), Mathf.Sin(Mathf.Deg2Rad * Angle)) * Config.BulletSpeed * 0.01F;

            transform.position += translateVec;
            transform.rotation = Quaternion.Euler(0F, 0F, Angle);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var obj = collision.gameObject;
            var actor = obj.GetComponent<Actor>();

            if (actor != null)
            {
                // Damage actor

                if (Side != actor.Side)
                    Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}