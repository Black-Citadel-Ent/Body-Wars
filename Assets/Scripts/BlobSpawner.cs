using UnityEngine;

public class BlobSpawner : MonoBehaviour
{
    [SerializeField] private BlobSpawnerSetup rootObject;
    [SerializeField] private LifeBar lifeBar;
    [SerializeField] private Animator animator;

    private float _lastSpawn;
    private int _life;

    private static readonly int SizeHash = Animator.StringToHash("Size");

    private void Start()
    {
        _life = rootObject.MaxLife;
        for(int counter=0; counter < rootObject.InitialSpawns; counter++)
            Spawn();
    }

    private void Update()
    {
        if(_life <= 0)
            Destroy(rootObject.gameObject);
        else if(_life <= rootObject.Level1Life)
            animator.SetInteger(SizeHash, 1);
        else if(_life <= rootObject.Level2Life)
            animator.SetInteger(SizeHash, 2);
        else
            animator.SetInteger(SizeHash, 3);
        lifeBar.UpdateLife(rootObject.MaxLife, _life);
        if (Time.time > _lastSpawn + rootObject.SpawnTime &&  rootObject.BlobCount < rootObject.MaxSpawns)
        {
            Spawn();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.gameObject.layer == LayerMask.NameToLayer("PlayerShot"))
            _life -= 1;
    }

    private void Spawn()
    {
        var blob = Instantiate(rootObject.BlobTemplate);
        blob.Spawner = rootObject;
        _lastSpawn = Time.time;
    }
}