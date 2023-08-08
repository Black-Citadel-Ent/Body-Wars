using UnityEngine;

public class BlobSpawnerSetup : MonoBehaviour
{
    [SerializeField] private float spawnTime;
    [SerializeField] private int maxSpawns;
    [SerializeField] private float maxSpawnDistance;
    [SerializeField] private BlobSetup blobTemplate;
    [SerializeField] private int initialSpawns;
    [SerializeField] private int maxLife;
    [SerializeField] private int level2Life;
    [SerializeField] private int level1Life;

    private int _blobCount;
        
    public float SpawnTime => spawnTime;
    public int MaxSpawns => maxSpawns;
    public float MaxSpawnDistance => maxSpawnDistance;
    public BlobSetup BlobTemplate => blobTemplate;
    public int InitialSpawns => initialSpawns;
    public int BlobCount => _blobCount;
    public int MaxLife => maxLife;
    public int Level2Life => level2Life;
    public int Level1Life => level1Life;

    private void Update()
    {
        _blobCount = 0;
    }

    public void MarkBlob()
    {
        _blobCount++;
    }
}