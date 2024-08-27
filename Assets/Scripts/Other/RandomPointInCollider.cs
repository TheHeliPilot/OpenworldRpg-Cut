using UnityEngine;

public static class RandomPointInCollider
{
  public static Vector2 GetPointInsideCollider(Collider2D collider)
  {
    Vector2 point;
    do
    {
      Bounds bounds = collider.bounds;
      point.x = Random.Range(bounds.min.x, bounds.max.x);
      point.y = Random.Range(collider.bounds.min.y, bounds.max.y);
    } while (collider.ClosestPoint(point) != point);

    return point;
  }
}