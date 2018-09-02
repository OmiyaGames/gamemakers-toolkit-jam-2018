using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
//using UnityStandardAssets.CrossPlatformInput;

namespace Project
{
    public class GrappleHook : MonoBehaviour
    {
        [SerializeField]
        int numRopes = 3;
        [SerializeField]
        Rope originalRope;
        [SerializeField]
        Transform handPosition;

        [Header("To remove")]
        [SerializeField]
        Rigidbody testBody;

        Rope[] allRopes;
        //List<Rigidbody> linkedBodies;

        // Use this for initialization
        void Start()
        {
            allRopes = new Rope[numRopes];
            allRopes[0] = originalRope;
            for(int i = 1; i < allRopes.Length; ++i)
            {
                GameObject clone = Instantiate(originalRope.gameObject, originalRope.transform.parent);
                clone.transform.localPosition = Vector3.zero;
                clone.transform.localScale = Vector3.one;
                allRopes[i] = clone.GetComponent<Rope>();
            }

            //linkedBodies = new List<Rigidbody>(numGrapplingHooks);
        }

        // Update is called once per frame
        void Update()
        {
            if(CrossPlatformInputManager.GetButtonDown("Fire1") == true)
            {
                allRopes[0].IsVisible = true;
                allRopes[0].Joint.connectedBody = testBody;
            }
            else if (CrossPlatformInputManager.GetButtonDown("Fire2") == true)
            {
                allRopes[0].IsVisible = false;
            }

            // Update visuals
            int i = 0;
            float lerp;
            foreach (Rope rope in allRopes)
            {
                if(rope.IsVisible == true)
                {
                    for(; i < rope.Line.positionCount; ++i)
                    {
                        lerp = i;
                        lerp /= (rope.Line.positionCount - 1);
                        rope.Line.SetPosition(i, Vector3.Lerp(handPosition.position, rope.Joint.connectedBody.position, lerp));
                    }
                }
            }
        }
    }
}
