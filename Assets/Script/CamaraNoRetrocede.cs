using UnityEngine;
using Unity.Cinemachine;

// Impide que la cámara baje más de un límite basado en la altura máxima alcanzada
public class CamaraNoRetrocede : CinemachineExtension
{
    [SerializeField] private Transform jugador;
    [SerializeField] private float margenAbajo = 3f; // Unidades que puede bajar la cámara

    // Récord de altura. MinValue para que cualquier posición inicial lo sobreescriba
    private float maxY = float.MinValue;

    
    // Cinemachine llama a este método en cada paso de su pipeline
    protected override void PostPipelineStageCallback( CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        // Solo actuamos cuando la posición es definitiva
        if (stage != CinemachineCore.Stage.Finalize)
        {
            return;
        }

        // Actualizamos el récord de altura
        if (state.RawPosition.y > maxY)
        {
            maxY = state.RawPosition.y;

        }

     // Cogemos la posición actual de la cámara
        Vector3 pos = state.RawPosition;
     // Calculamos el límite mínimo de bajada (el mayor de los dos, que es el más restrictivo)
        float limiteBajo = Mathf.Max(jugador.position.y - margenAbajo, maxY - margenAbajo);
     // Si la cámara intenta bajar del límite, la clavamos en él
        pos.y = Mathf.Max(pos.y, limiteBajo);
     // Aplicamos la posición corregida
        state.RawPosition = pos;
    }
}