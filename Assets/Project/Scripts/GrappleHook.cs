using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class GrappleHook : MonoBehaviour
    {
        [SerializeField]
        int numGrapplingHooks = 3;
        [SerializeField]
        Joint originalSpring;

        [Header("To remove")]
        [SerializeField]
        Rigidbody testBody;

        Joint[] allGrapplingHooks;
        //List<Rigidbody> linkedBodies;

        // Use this for initialization
        void Start()
        {
            allGrapplingHooks = new Joint[numGrapplingHooks];
            allGrapplingHooks[0] = originalSpring;
            for(int i = 1; i < allGrapplingHooks.Length; ++i)
            {
                GameObject clone = Instantiate(originalSpring.gameObject, originalSpring.transform.parent);
                clone.transform.localPosition = Vector3.zero;
                clone.transform.localScale = Vector3.one;
                allGrapplingHooks[i] = clone.GetComponent<Joint>();
            }

            //linkedBodies = new List<Rigidbody>(numGrapplingHooks);
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.anyKeyDown)
            {
                allGrapplingHooks[0].connectedBody = testBody;
            }
        }
    }
}
