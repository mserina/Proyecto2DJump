using UnityEngine;
using Unity.Cinemachine;

public class CamaraNoRetrocede : CinemachineExtension
{
    // float.MinValue es el número más pequeño posible
    // así cualquier posición inicial de la cámara ya será mayor que este valor
    private float maxY = float.MinValue;

    // Este método lo llama Cinemachine automáticamente en cada paso de su pipeline
    // vcam    → la cámara virtual que está procesando
    // stage   → en qué paso del pipeline estamos ahora mismo
    // state   → el estado actual de la cámara (posición, rotación, etc.)
    // deltaTime → tiempo desde el último frame
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime)
    {
        // Solo nos interesa actuar en el último paso (Finalize)
        // En los pasos anteriores ignoramos todo y salimos
        if (stage != CinemachineCore.Stage.Finalize) return;

        // FinalPosition es la posición que Cinemachine ha calculado para este frame
        // Si es mayor que nuestro máximo guardado, actualizamos el máximo
        if (state.RawPosition.y > maxY)
            maxY = state.RawPosition.y;

        // Cogemos la posición calculada por Cinemachine
        Vector3 pos = state.RawPosition;

        // Sustituimos la Y por nuestro máximo, bloqueando cualquier bajada
        pos.y = maxY;

        // RawPosition es la posición que Cinemachine usará finalmente
        // Al asignarla aquí, en Finalize, nadie más la sobreescribirá
        state.RawPosition = pos;
    }
}