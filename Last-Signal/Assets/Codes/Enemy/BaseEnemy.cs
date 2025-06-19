using UnityEngine;

public class BaseEnemy : BaseClass
{
    [SerializeField, Tooltip("Waypoints voor de vijand")]
    protected Transform[] waypoints; // Beschermd voor afgeleide klassen
    [SerializeField, Tooltip("Bewegingssnelheid")]
    protected float moveSpeed = 5f; // Beschermd voor afgeleide klassen
    [SerializeField, Tooltip("Materialen verdiend bij dood")]
    private int rewardMaterials = 10;
    [SerializeField, Tooltip("Draaisnelheid (graden per seconde)")]
    private float turnSpeed = 5f;

    protected int currentWaypointIndex = 0; // Beschermd voor afgeleide klassen
    protected bool isMoving = true; // Veranderd van private naar protected
    private Vector3 previousPosition;
    private Vector3 velocity;

    public float MoveSpeed => moveSpeed;
    public Vector3 Velocity => velocity;
    public bool isAlive => currentHP > 0;
    public bool IsMoving => isMoving; // Public property blijft voor externe toegang

    protected override void Start()
    {
        base.Start();
        previousPosition = transform.position;
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("Geen waypoints ingesteld!");
            isMoving = false;
        }
        else
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
                rb.isKinematic = true; // Kinematic voor controle
            }
            Collider col = gameObject.GetComponent<Collider>();
            if (col == null)
            {
                gameObject.AddComponent<BoxCollider>();
            }
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }
    }

    protected override void UpdateLogic()
    {
        if (!isMoving || waypoints == null || currentWaypointIndex >= waypoints.Length) return;

        Vector3 target = waypoints[currentWaypointIndex].position;
        Vector3 direction = (target - transform.position).normalized;

        // Roteer naar de bewegingsrichting
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);

        // Beweeg in lokale as (vooruit)
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);

        // Controleer of waypoint is bereikt
        if (Vector3.Distance(transform.position, target) < 0.5f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                isMoving = false;
            }
        }

        // Bereken snelheid
        velocity = (transform.position - previousPosition) / Time.deltaTime;
        previousPosition = transform.position;
    }

    protected override void Die()
    {
        base.Die();
        if (MaterialManager.Instance != null)
        {
            MaterialManager.Instance.AddMaterials(rewardMaterials);
            Debug.Log($"Verdiende {rewardMaterials} materials bij dood");
        }
        else
        {
            Debug.LogWarning("MaterialManager niet gevonden!");
        }
    }

    // Publieke methode om waypoints in te stellen
    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
        currentWaypointIndex = 0;
        isMoving = waypoints != null && waypoints.Length > 0;
        if (!isMoving)
        {
            Debug.LogError("Ongeldige waypoints ontvangen!");
        }
    }
}