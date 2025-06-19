using UnityEngine;

public class TankTurret : MonoBehaviour
{
    [SerializeField, Tooltip("Barrel spawnpoints")]
    private Transform[] barrelSpawnPoints;
    [SerializeField, Tooltip("Projectiel prefab")]
    private GameObject projectilePrefab;
    [SerializeField, Tooltip("Layer voor towers")]
    private LayerMask towerLayer;
    [SerializeField, Tooltip("Detectie- en schietbereik")]
    private float shootRange = 10f;
    [SerializeField, Tooltip("Tijd tussen schoten (seconden)")]
    private float fireRate = 2f;
    [SerializeField, Tooltip("Schade per projectiel")]
    private float damage = 20f;
    [SerializeField, Tooltip("Snelheid van projectielen")]
    private float projectileSpeed = 10f;
    [SerializeField, Tooltip("Rotatiesnelheid turret (graden/seconde)")]
    private float turretRotationSpeed = 20f;

    private float nextFireTime;
    private int currentBarrelIndex = 0;
    private Transform currentTarget;

    void Start()
    {
        if (barrelSpawnPoints == null || barrelSpawnPoints.Length == 0 || projectilePrefab == null)
        {
            Debug.LogError("TankTurret mist essentiële componenten (barrels of projectilePrefab)!");
            return;
        }

    }

    void Update()
    {
        currentTarget = FindTarget();
        if (currentTarget != null)
        {
            RotateTurretTowardsTarget();
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
            if (distanceToTarget <= shootRange && Time.time >= nextFireTime && IsAlignedWithTarget())
            {
                Fire();
                nextFireTime = Time.time + fireRate;
            }
        }

    }

    private Transform FindTarget()
    {
        Collider[] towerHits = Physics.OverlapSphere(transform.position, shootRange, towerLayer);
        Transform closestTower = null;
        float minDistance = shootRange;

        foreach (var hit in towerHits)
        {
            BaseTower tower = hit.GetComponent<BaseTower>();
            if (tower != null && tower.CurrentHP > 0)
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTower = hit.transform;
                }
            }
        }

        if (closestTower != null)
        {
            return closestTower;
        }

        GameObject goldenUnit = GameObject.FindWithTag("GoldenUnit");
        if (goldenUnit != null)
        {
            return goldenUnit.transform;
        }

        return null;
    }

    private void RotateTurretTowardsTarget()
    {
        if (currentTarget == null) return;

        Vector3 direction = currentTarget.position - transform.position;
        direction.y = 0f; // Beperk rotatie tot Y-as
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turretRotationSpeed * Time.deltaTime);
    }

    private bool IsAlignedWithTarget()
    {
        if (currentTarget == null) return false;

        Vector3 directionToTarget = (currentTarget.position - transform.position).normalized;
        directionToTarget.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        float angleDiff = Quaternion.Angle(transform.rotation, targetRotation);
        return angleDiff < 5f;
    }

    private void Fire()
    {
        if (projectilePrefab == null || barrelSpawnPoints.Length == 0) return;

        Transform spawnPoint = barrelSpawnPoints[currentBarrelIndex];
        currentBarrelIndex = (currentBarrelIndex + 1) % barrelSpawnPoints.Length;

        Vector3 direction = (currentTarget.position - spawnPoint.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.LookRotation(direction));

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb == null) rb = projectile.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = direction * projectileSpeed;

        ProjectileDamage damageScript = projectile.GetComponent<ProjectileDamage>();
        if (damageScript == null)
        {
            damageScript = projectile.AddComponent<ProjectileDamage>();
            damageScript.damage = damage; // Debug: log schade
        }

        Destroy(projectile, 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }

    private class ProjectileDamage : MonoBehaviour
    {
        public float damage;

        private void OnCollisionEnter(Collision collision)
        {
            BaseTower tower = collision.gameObject.GetComponent<BaseTower>();
            GoldenUnit golden = collision.gameObject.GetComponent<GoldenUnit>();
            if (tower != null)
            {
                tower.TakeDamage(damage);

            }
            else if (golden != null)
            {
                golden.TakeDamage(damage);

            }
            Destroy(gameObject);
        }
    }
}