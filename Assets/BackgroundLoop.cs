using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    [SerializeField] private Transform fondo1;
    [SerializeField] private Transform fondo2;
    [SerializeField] private float alturaSprite = 10f; // Ajusta según el tamaño de tu imagen

    private Transform camara;

    void Start()
    {
        camara = Camera.main.transform;
    }

    void Update()
    {
        // Si el fondo queda por debajo de la cámara, lo recoloca arriba
        RecolocarSiNecesario(fondo1, fondo2);
        RecolocarSiNecesario(fondo2, fondo1);
    }

    private void RecolocarSiNecesario(Transform actual, Transform otro)
    {
        // Si este fondo ha quedado una altura por debajo de la cámara, lo movemos arriba
        if (actual.position.y + alturaSprite < camara.position.y)
        {
            actual.position = new Vector3(
                actual.position.x,
                otro.position.y + alturaSprite, // Se coloca justo encima del otro
                actual.position.z
            );
        }
    }
}