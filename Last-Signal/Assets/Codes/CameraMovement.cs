using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speedX = 5f; // Snelheid voor horizontale beweging (X-as)
    [SerializeField] private float speedY = 5f; // Snelheid voor verticale beweging (Y-as, optioneel)
    [SerializeField] private float minXLimit = -2000f; // Minimale X-limiet via Inspector
    [SerializeField] private float maxXLimit = 2100f; // Maximale X-limiet via Inspector

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal"); // A = -1, D = 1
        float inputY = Input.GetAxisRaw("Vertical");   // W = 1, S = -1

        Vector3 position = transform.position;

        // Beweging op X-as, richting omgedraaid
        position.x += -inputX * speedX * Time.deltaTime; // Vermenigvuldigen met -1 omdraait de richting
        position.x = Mathf.Clamp(position.x, minXLimit, maxXLimit);

        // Beweging op Y-as (optioneel, kan worden uitgeschakeld door speedY op 0 te zetten)
        position.y += inputY * speedY * Time.deltaTime;

        transform.position = position;
    }
}