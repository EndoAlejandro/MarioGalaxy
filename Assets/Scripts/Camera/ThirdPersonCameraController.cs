using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    public Transform player; // El objeto que la cámara seguirá
    public float distanceFromPlayer = 5f; // Distancia de la cámara al jugador
    public float height = 2f; // Altura de la cámara sobre el jugador
    public float sensitivity = 100f; // Sensibilidad del mouse

    private float currentX = 0f;
    private float currentY = 0f;
    public float minY = -20f;
    public float maxY = 80f;

    void Start()
    {
        // Oculta y bloquea el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Obtener la entrada del mouse
        currentX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        currentY -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // Limitar la rotación en el eje Y
        currentY = Mathf.Clamp(currentY, minY, maxY);
    }

    void LateUpdate()
    {
        // Calcular la posición y rotación de la cámara
        Vector3 direction = new Vector3(0, 0, -distanceFromPlayer);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = player.position + rotation * direction + Vector3.up * height;
        transform.LookAt(player.position + Vector3.up * height);
    }
}