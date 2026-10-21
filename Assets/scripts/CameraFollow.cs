using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; 
    public float smoothSpeed = 5f;
    
    [Header("Ajuste de Cámara")]
    public float offsetY = 1.5f; // <--- Aquí ajustas cuánto sube o baja la cámara

    void LateUpdate()
    {
        if (target == null) return; 

        // Toma la X del jugador, le suma el offset a la Y, y mantiene la Z en -10
        Vector3 posicionDeseada = new Vector3(target.position.x, target.position.y + offsetY, -10f);
        
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, smoothSpeed * Time.deltaTime);
        
        transform.position = posicionSuavizada;
    }
}