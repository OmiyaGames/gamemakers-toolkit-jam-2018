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
        [SerializeField]
        TreeGenerator treeList;
        [SerializeField]
        float minDistanceFromTree = 15f;

        Rope[] allRopes;

        public ElementStatus ClosestTree
        {
            get
            {
                float minDistance = minDistanceFromTree;
                float compareDistance;
                ElementStatus returnTree = null;
                foreach (ElementStatus tree in treeList.AllElements)
                {
                    compareDistance = Vector3.Distance(transform.position, tree.transform.position);
                    if(compareDistance < minDistance)
                    {
                        returnTree = tree;
                        minDistance = compareDistance;
                    }
                }
                return returnTree;
            }
        }

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
        }

        // Update is called once per frame
        void Update()
        {
            ElementStatus tree = ClosestTree;
            if (tree != null)
            {
                // FIXME: add something to indicate this is the closest tree
            }
            if (CrossPlatformInputManager.GetButtonDown("Fire1") == true)
            {
                if (tree != null)
                {
                    foreach (Rope rope in allRopes)
                    {
                        if (rope.IsVisible == false)
                        {
                            rope.IsVisible = true;
                            rope.Joint.connectedBody = tree.Body;
                            break;
                        }
                    }
                }
            }
            else if (CrossPlatformInputManager.GetButtonDown("Fire2") == true)
            {
                foreach(Rope rope in allRopes)
                {
                    rope.IsVisible = false;
                }
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
