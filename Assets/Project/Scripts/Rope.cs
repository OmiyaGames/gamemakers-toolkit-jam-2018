using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [RequireComponent(typeof(Joint))]
    [RequireComponent(typeof(LineRenderer))]
    public class Rope : MonoBehaviour
    {
        Joint joint;
        LineRenderer line;
        bool isVisible = true;

        private void Awake()
        {
            IsVisible = false;
        }

        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                if(isVisible != value)
                {
                    isVisible = value;
                    Line.enabled = value;
                    if(isVisible == false)
                    {
                        Joint.connectedBody = null;
                    }
                }
            }
        }

        public Joint Joint
        {
            get
            {
                if(joint == null)
                {
                    joint = GetComponent<Joint>();
                }
                return joint;
            }
        }

        public LineRenderer Line
        {
            get
            {
                if (line == null)
                {
                    line = GetComponent<LineRenderer>();
                }
                return line;
            }
        }
    }
}
