using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class GetNormalPos : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D clickedCollider = Physics2D.OverlapPoint(clickPosition);

            if (clickedCollider != null)
            {
                Bounds bounds;
                Vector2 normalizedPosition = (bounds = clickedCollider.bounds).ClosestPoint(clickPosition) - bounds.min;
                normalizedPosition /= bounds.size;


                var X = normalizedPosition.x.ToString(CultureInfo.InvariantCulture).Replace(",", ".");
                var Y = normalizedPosition.y.ToString(CultureInfo.InvariantCulture).Replace(",", ".");

                Debug.Log($"\"x\" : {X},\"y\" : {Y}");
            }
        }
    }
}
