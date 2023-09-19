using System.Collections.Generic;
using BaseItems;
using UnityEngine;

namespace VirusSpawner
{
    public class VirusSpawner : BaseBehavior, ILife
    {
        [SerializeField] private Virus.Virus smallVirusTemplate;
        [SerializeField] private Virus.Virus mediumVirusTemplate;
        [SerializeField] private Virus.Virus largeVirusTemplate;
        [SerializeField] private float maxLife;
        [SerializeField] private float mediumLife;
        [SerializeField] private float smallLife;
        [SerializeField] private int initialSpawns;
        [SerializeField] private float spawnTimer;
        [SerializeField] private int maxLockedSpawns;
        [SerializeField] private float anchorRadius;
     
        private enum TransitionStep { Large, LargeToMedium, Medium, MediumToSmall, Small, SmallToDead }
        
        private VirusSpawnerSetup _setup;
        private float _life;
        private float _transitionTime;
        private TransitionStep _step;
        private float _spawnTime;
        private readonly List<GameObject> _lockedSpawns = new();

        public float AnchorRadius => anchorRadius;
        
        public void RegisterSetup(VirusSpawnerSetup setup)
        {
            _setup = setup;
        }

        public void ApplyDamage(float amount, ILife.DamageType type, float direction)
        {
            _life -= amount;
            _setup.LifeBar.SetLife(_life / maxLife);
        }

        public void Detach(GameObject v)
        {
            _lockedSpawns.Remove(v);
        }

        private void Start()
        {
            _life = maxLife;
            _setup.LifeBar.SetLife(1.0f);
            _step = TransitionStep.Large;
            for(int counter=0; counter < initialSpawns; counter++)
                Spawn();
        }

        private void FixedUpdate()
        {
            if (_step == TransitionStep.Large && _life <= mediumLife)
            {
                _transitionTime = Time.fixedTime + 6.0f / 12.0f;
                _step = TransitionStep.LargeToMedium;
                _setup.LargeVisual.SetActive(false);
                _setup.LargeToMediumVisual.SetActive(true);
            }
            if (_step == TransitionStep.LargeToMedium && Time.fixedTime >= _transitionTime)
            {
                _step = TransitionStep.Medium;
                _setup.LargeToMediumVisual.SetActive(false);
                _setup.MediumVisual.SetActive(true);
            }
            if (_step == TransitionStep.Medium && _life <= smallLife)
            {
                _transitionTime = Time.fixedTime + 9.0f / 12.0f;
                _step = TransitionStep.MediumToSmall;
                _setup.MediumVisual.SetActive(false);
                _setup.MediumToSmallVisual.SetActive(true);
            }
            if (_step == TransitionStep.MediumToSmall && Time.fixedTime >= _transitionTime)
            {
                _step = TransitionStep.Small;
                _setup.MediumToSmallVisual.SetActive(false);
                _setup.SmallVisual.SetActive(true);
            }
            if (_step == TransitionStep.Small && _life <= 0)
            {
                foreach (var obj in _lockedSpawns)
                    obj.GetComponent<Virus.Virus>().AnchorDead();
                _transitionTime = Time.fixedTime + 12.0f / 12.0f;
                _step = TransitionStep.SmallToDead;
                _setup.SmallVisual.SetActive(false);
                _setup.SmallToDeadVisual.SetActive(true);
            }
            if(_step == TransitionStep.SmallToDead && Time.fixedTime >= _transitionTime)
            {
                Destroy(gameObject);
            }

            if (Time.fixedTime >= _spawnTime && _lockedSpawns.Count < maxLockedSpawns)
            {
                Spawn();
            }
        }

        private void Spawn()
        {
            var template = VirusTemplate;
            if (template)
            {
                var virus = Instantiate(template, transform.position, Quaternion.identity);
                virus.LockToSpawner(this);
                _lockedSpawns.Add(virus.gameObject);
                _spawnTime = Time.fixedTime + spawnTimer;
            }
        }

        private Virus.Virus VirusTemplate
        {
            get
            {
                if (_step == TransitionStep.Large)
                    return largeVirusTemplate;
                if (_step == TransitionStep.LargeToMedium || _step == TransitionStep.Medium)
                    return mediumVirusTemplate;
                if (_step == TransitionStep.MediumToSmall || _step == TransitionStep.Small)
                    return smallVirusTemplate;
                return null;
            }
        }
    }
}