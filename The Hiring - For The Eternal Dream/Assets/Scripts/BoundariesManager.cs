using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundariesManager : MonoBehaviour
{
        // Private

        //Public
        [Header("Object")]
        public PolygonCollider2D cameraConfiner;

        [Space(10)]
        [Header("Array")]
        public Vector2[] points;


        private void Update() {
            cameraConfiner.points = points;
        }
}
