using UnityEngine;

public class BaseTower : BaseClass
{
    [SerializeField, Tooltip("Schade die de toren aanricht")]
    protected float damage = 50f;
    [SerializeField, Tooltip("Schietfrequentie (in seconden)")]
    protected float fireRate = 2f;
    [SerializeField, Tooltip("Bereik van de toren")]
    protected float range = 10f;
    [SerializeField, Tooltip("Draaisnelheid (graden per seconde)")]
    protected float rotationSpeed = 20f;
    [SerializeField, Tooltip("Projectiel prefab (optioneel voor standaard aanval)")]
    protected GameObject projectilePrefab;
    [SerializeField, Tooltip("Spawnpunt(en) van het projectiel, meerdere mogelijk")]
    protected Transform[] barrelSpawnPoints; // Nieuwe ondersteuning voor meerdere schietpunten
    [SerializeField, Tooltip("Enkele spawnpunt van het projectiel (fallback)")]
    protected Transform projectileSpawnPoint;
    [SerializeField, Tooltip("Snelheid van het projectiel")]
    protected float projectileSpeed = 10f;
    [SerializeField, Tooltip("Data voor deze toren, inclusief bouwkosten")]
    protected TowerData towerData;

    public TowerData TowerData => towerData; // Voor BuildManager

    protected float nextFireTime;
    protected Transform currentTarget;
    private int currentBarrelIndex = 0; // Index voor meerdere schietpunten

    protected override void Start()
    {
        base.Start();
        nextFireTime = Time.time;
        if (barrelSpawnPoints == null || barrelSpawnPoints.Length == 0)
        {
            if (projectileSpawnPoint == null || projectilePrefab == null)
            {
                Debug.LogError("BaseTower mist essentiële componenten (projectileSpawnPoint of projectilePrefab)!");
            }
        }
        else
        {
            foreach (var point in barrelSpawnPoints)
            {
                if (point == null)
                {
                    Debug.LogError("Een van de barrelSpawnPoints is niet ingesteld!");
                    return;
                }
            }
        }
    }

    protected override void UpdateLogic()
    {
        if (CurrentHP <= 0) return;

        currentTarget = FindTarget();
        if (currentTarget != null)
        {
            RotateTowardsTarget();
            if (Time.time >= nextFireTime && IsRotationComplete())
            {
                Fire();
                nextFireTime = Time.time + fireRate;
            }
        }
        else
        {
            Debug.Log("Geen vijand gevonden binnen bereik!");
        }
    }

    protected virtual Transform FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range, LayerMask.GetMask("Enemy"));
        Transform closest = null;
        float minDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            BaseEnemy enemy = hit.GetComponent<BaseEnemy>();
            if (enemy != null && enemy.isAlive)
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = hit.transform;
                }
            }
        }
        return closest;
    }

    protected virtual void RotateTowardsTarget()
    {
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    protected virtual bool IsRotationComplete()
    {
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        return Quaternion.Angle(transform.rotation, targetRotation) < 1f;
    }

    protected virtual void Fire()
    {
        Transform spawnPoint = (barrelSpawnPoints != null && barrelSpawnPoints.Length > 0) ? barrelSpawnPoints[currentBarrelIndex] : projectileSpawnPoint;
        if (projectilePrefab == null || spawnPoint == null) return;

        Vector3 targetPosition = currentTarget.position;
        BaseEnemy enemy = currentTarget.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            Vector3 enemyVelocity = enemy.Velocity;
            float distance = Vector3.Distance(spawnPoint.position, currentTarget.position);
            float timeToHit = distance / (projectileSpeed - enemyVelocity.magnitude); // Relatieve snelheid
            if (timeToHit > 0)
            {
                targetPosition += enemyVelocity * timeToHit;
            }
        }

        Vector3 direction = (targetPosition - spawnPoint.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.LookRotation(direction));

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = projectile.AddComponent<Rigidbody>();
        }
        rb.useGravity = false;
        rb.velocity = direction * projectileSpeed;

        ProjectileDamage damageScript = projectile.GetComponent<ProjectileDamage>();
        if (damageScript == null)
        {
            damageScript = projectile.AddComponent<ProjectileDamage>();
        }
        damageScript.damage = damage;

        Destroy(projectile, 5f);
        Debug.Log($"Schoot op {currentTarget.name} met voorspelde positie {targetPosition}");

        // Update barrel index als er meerdere schietpunten zijn
        if (barrelSpawnPoints != null && barrelSpawnPoints.Length > 0)
        {
            currentBarrelIndex = (currentBarrelIndex + 1) % barrelSpawnPoints.Length;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    protected class ProjectileDamage : MonoBehaviour
    {
        public float damage;

        private void OnCollisionEnter(Collision collision)
        {
            BaseEnemy enemy = collision.gameObject.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}