using UnityEngine;

public class Item : MonoBehaviour
{
   [SerializeField] private int pointsValue = 10;
   public int PointsValue => pointsValue;
}
