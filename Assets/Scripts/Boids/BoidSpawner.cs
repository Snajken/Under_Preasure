using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    public enum GizmoType { Never, SelectedOnly, Always }
    public BoidManager boidOwner;
    public Boids prefab;
    public float spawnRadius = 10;
    public int spawnCount = 10;
    public Color colour;
    public GizmoType showSpawnRegion;

    void Awake()
    {
        if (boidOwner == null) GetComponent<BoidManager>();
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            Boids boid = Instantiate(prefab);
            boid.transform.position = pos;
            
            boid.transform.forward = Random.insideUnitSphere;
            boidOwner.boids.Add(boid);
            boid.SetColour(colour);
        }
    }

    private void OnDrawGizmos()
    {
        if (showSpawnRegion == GizmoType.Always)
        {
            DrawGizmos();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (showSpawnRegion == GizmoType.SelectedOnly)
        {
            DrawGizmos();
        }
    }

    void DrawGizmos()
    {

        Gizmos.color = new Color(colour.r, colour.g, colour.b, 0.3f);
        Gizmos.DrawSphere(transform.position, spawnRadius);
    }

}
