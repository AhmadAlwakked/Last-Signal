using UnityEngine;

public class LaserTower : BaseTower
{
    [SerializeField, Tooltip("Schade per seconde")]
    private float damagePerSecond = 10f;
    [SerializeField, Tooltip("Startpunt van de laser (de bal bovenop)")]
    private Transform laserOrigin;
    [SerializeField, Tooltip("HDR-materiaal voor de laser")]
    private Material laserMaterial;
    [SerializeField, Tooltip("Breedte van de laser")]
    private float laserWidth = 0.2f;

    private LineRenderer laserRenderer;
    private float laserDuration = 0.1f;

    protected override void Start()
    {
        base.Start();
        laserRenderer = gameObject.AddComponent<LineRenderer>();
        laserRenderer.material = laserMaterial;
        laserRenderer.startWidth = laserWidth;
        laserRenderer.endWidth = laserWidth;
        laserRenderer.enabled = false;
    }

    protected override void Fire()
    {
        if (laserOrigin == null || currentTarget == null) return;

        BaseEnemy enemy = currentTarget.GetComponent<BaseEnemy>();
        if (enemy != null && CurrentHP > 0)
        {
            enemy.TakeDamage(damagePerSecond * laserDuration);

            laserRenderer.SetPosition(0, laserOrigin.position);
            laserRenderer.SetPosition(1, currentTarget.position);
            laserRenderer.enabled = true;

            Invoke("DisableLaser", laserDuration);
        }
    }

    private void DisableLaser()
    {
        laserRenderer.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}