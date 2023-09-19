using System.Collections.Generic;
using BaseItems;
using UnityEngine;

namespace Player
{
    public class PlayerShot : BaseBehavior
    {
        [SerializeField] private float speed;
        [SerializeField] private float range;
        [SerializeField] private float damage;

        private PlayerShotSetup _setup;
        private float _destroyTime;
        private bool _hit;

        public void RegisterSetup(PlayerShotSetup setup)
        {
            _setup = setup;
        }

        private void Setup(Transform t)
        {
            transform.SetPositionAndRotation(t.position, t.rotation);
            _setup.BaseObject.SetActive(true);
            _setup.HitObject.SetActive(false);
            _destroyTime = Time.fixedTime + range / speed;
            _hit = false;
        }

        private void FixedUpdate()
        {
            if (!_hit)
                _setup.Body.velocity = transform.rotation.eulerAngles.z.DegreesToVector2() * speed;
            else
                _setup.Body.velocity = Vector2.zero;
            if (Time.fixedTime >= _destroyTime)
                ReturnShot(this);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _setup.BaseObject.SetActive(false);
            _setup.HitObject.SetActive(true);
            _hit = true;
            _destroyTime = Time.fixedTime + 5.0f / 12.0f;

            // TODO NEXT Collision objects not necessarily in order
            var life = collision.gameObject.GetComponent<ILife>() ?? collision.otherRigidbody.gameObject.GetComponent<ILife>();
            life?.ApplyDamage(damage, ILife.DamageType.Energy);
        }

        private static readonly Queue<PlayerShot> _shotQueue = new();

        public static PlayerShot FireShot(PlayerShot shotTemplate, Transform origin)
        {
            if (!_shotQueue.TryDequeue(out var shot))
                shot = Instantiate(shotTemplate);
            shot.gameObject.SetActive(true);
            shot.Setup(origin);
            return shot;
        }

        private static void ReturnShot(PlayerShot shot)
        {
            _shotQueue.Enqueue(shot);
            shot.gameObject.SetActive(false);
        }
    }
}