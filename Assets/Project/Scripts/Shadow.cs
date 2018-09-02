using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [RequireComponent(typeof(Renderer))]
    public class Shadow : MonoBehaviour
    {
        [SerializeField]
        float placeAboveGround = 0.01f;

        [Header("Ray Tracing")]
        [SerializeField]
        float maxDistanceFromGround = 30f;
        [SerializeField]
        LayerMask groundLayer;

        Renderer meshRenderer;
        Vector3 originalScale;
        Vector3 position;
        Ray ray = new Ray(Vector3.zero, Vector3.down);
        RaycastHit info;

        // Use this for initialization
        void Start()
        {
            originalScale = transform.localScale;
            meshRenderer = GetComponent<Renderer>();
        }

        // Update is called once per frame
        void Update()
        {
            ray.origin = transform.parent.position;
            meshRenderer.enabled = Physics.Raycast(ray, out info, maxDistanceFromGround, groundLayer);
            if (meshRenderer.enabled == true)
            {
                transform.position = ray.GetPoint(info.distance - placeAboveGround);
            }
        }
    }
}
