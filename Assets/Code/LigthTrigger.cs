using UnityEngine;

public class LigthTrigger : MonoBehaviour
{

    public float radius = 1f;     // Radio del SphereCollider
    public float maxDamage = 5f;  // Da�o en el epicentro

    private SphereCollider spCol;
    private Transform playerInside; // Player actualmente dentro del trigger
    private Renderer objRenderer;

    void Start()
    {

        TriggerAudioThenAlarma.LightsEvent += TurnUpDwonLigths;

        objRenderer = GetComponent<Renderer>();
        spCol = GetComponent<SphereCollider>();

        if (spCol == null)
        {
            Debug.LogError("No hay SphereCollider en este GameObject.");
            return;
        }

        // Convertir a posici�n local del collider
        Vector3 localPos = transform.InverseTransformPoint(
            new Vector3(transform.position.x, 0f, transform.position.z)
        );

        spCol.center = localPos;
        spCol.radius = radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = other.transform;
            InvokeRepeating(nameof(DetectAndDamage), 0f, 0.1f
                );
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = null;
            CancelInvoke(nameof(DetectAndDamage));
        }
    }

    private void DetectAndDamage()
    {
        if (playerInside == null) return;

        Vector3 worldCenter = transform.TransformPoint(spCol.center);
        float distance = Vector3.Distance(playerInside.position, worldCenter);
        distance = Mathf.Clamp(distance, 0f, radius);

        float damage = maxDamage * (1f - (distance / radius));

        float visibility = VisibilityFactor();
        damage *= (1f + visibility);

        //Debug.Log("Da�o aplicado (visibilidad " + visibility + "): " + damage);

        // ?? Aplicar da�o real al jugador:
        playerInside.GetComponent<ScriptPlayerLive>()?.TakeDamage(damage);


    }


    private float VisibilityFactor()
    {
        if (playerInside == null) return 0f;

        Camera cam = playerInside.GetComponentInChildren<Camera>();
        if (cam == null) return 0f;

        // Centro visual del mesh
        Vector3 worldCenter = objRenderer.bounds.center;

        // Convertir a coordenadas del viewport (0-1)
        Vector3 viewportPos = cam.WorldToViewportPoint(worldCenter);

        // Si est� detr�s de la c�mara o fuera de pantalla ? 0
        if (viewportPos.z < 0 ||
            viewportPos.x < 0 || viewportPos.x > 1 ||
            viewportPos.y < 0 || viewportPos.y > 1)
        {
            return 0f;
        }

        // Distancia al centro del viewport
        float dx = viewportPos.x - 0.5f;
        float dy = viewportPos.y - 0.5f;

        float distanceFromCenter = Mathf.Sqrt(dx * dx + dy * dy);
        float maxDistance = 0.707f; // distancia desde centro a una esquina

        float factor = 1f - (distanceFromCenter / maxDistance);

        return Mathf.Clamp01(factor);
    }

    private void TurnUpDwonLigths(bool stateLigths) 
    {
        //Si stateLigths es true, se activa el gameObject, si es false se desactiva
        //Encender luces
        GetComponent<Light>().enabled = stateLigths;
        GetComponent<SphereCollider>().enabled = stateLigths;

    }

}
