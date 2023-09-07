using System;
using System.Collections.Generic;
using BaseItems;
using UnityEngine;

namespace Virus
{
    public class VirusShot : BaseBehavior
    {
        [SerializeField] private float speed;
        [SerializeField] private float turnSpeed;
        [SerializeField] private float shotDistanceMultiplier;
        [SerializeField] private float damage;
        
        private VirusShotSetup _setup;
        private Transform _target;
        private bool _exploding;
        private float _explodeTime;

        public void RegisterSetup(VirusShotSetup setup)
        {
            _setup = setup;
        }

        private void Setup(Transform init)
        {
            var pos = (Vector2)init.position;
            _setup.Body.position = pos;
            _setup.Body.rotation = init.rotation.eulerAngles.z;
            _exploding = false;
            _setup.ShotVisual.SetActive(true);
            _setup.ExplodeVisual.SetActive(false);
            _target = GameObject.FindWithTag("Player").transform;
            _explodeTime = Time.fixedTime + ((Vector2)_target.position - pos).magnitude * shotDistanceMultiplier / speed;
        }

        private void Explode()
        {
            _exploding = true;
            _setup.ShotVisual.SetActive(false);
            _setup.ExplodeVisual.SetActive(true);
            _explodeTime = Time.fixedTime + 8.0f / 12.0f;
        }

        private void FixedUpdate()
        {
            if (!_exploding)
            {
                var dir = TurnTowards(transform.rotation.eulerAngles.z,
                    ((Vector2)(_target.position - transform.position)).ToDegrees(), turnSpeed);
                _setup.Body.rotation = dir;
                _setup.Body.MovePosition(_setup.Body.position + dir.DegreesToVector2() * (speed * Time.fixedDeltaTime));
                if (Time.fixedTime >= _explodeTime)
                    Explode();
            }
            else
            {
                if (Time.fixedTime >= _explodeTime)
                    ReturnShot(this);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(!_exploding) Explode();
            else other.gameObject.GetComponent<ILife>()?.ApplyDamage(damage, ILife.DamageType.Electric);
        }

        private static Queue<VirusShot> ShotQueue = new();

        public static void FireShot(VirusShot template, Transform origin)
        {
            if (!ShotQueue.TryDequeue(out var shot))
                shot = Instantiate(template);
            shot.gameObject.SetActive(true);
            shot.Setup(origin);
        }

        private static void ReturnShot(VirusShot shot)
        {
            shot.gameObject.SetActive(false);
            ShotQueue.Enqueue(shot);
        }
    }
}