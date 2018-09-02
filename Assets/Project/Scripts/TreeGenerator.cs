using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OmiyaGames;
using OmiyaGames.Global;

namespace Project
{
    public class TreeGenerator : MonoBehaviour
    {
        [SerializeField]
        float checkEverySeconds = 5f;

        [Header("Tree Ranges")]
        [SerializeField]
        ElementStatus prefab;
        [SerializeField]
        int startingNumTrees = 25;
        [SerializeField]
        int maxNumTrees = 50;

        [Header("Position")]
        [SerializeField]
        Bounds range;

        WaitForSeconds waitFor;

        public HashSet<ElementStatus> AllElements { get; } = new HashSet<ElementStatus>();

        // Use this for initialization
        void Start()
        {
            for (int i = 0; i < startingNumTrees; ++i)
            {
                GenerateTree();
            }
            waitFor = new WaitForSeconds(checkEverySeconds);
            StartCoroutine(CheckWhetherToTree());
        }

        IEnumerator CheckWhetherToTree()
        {
            yield return waitFor;
            if(AllElements.Count < maxNumTrees)
            {
                GenerateTree();
            }
        }

        void GenerateTree()
        {
            Vector3 nextPosition = range.center;
            nextPosition.x = Random.Range(range.min.x, range.max.x);
            nextPosition.y = range.max.y;
            nextPosition.z = Random.Range(range.min.z, range.max.z);

            ElementStatus newElement = Singleton.Get<PoolingManager>().GetInstance<ElementStatus>(prefab, nextPosition, Quaternion.identity);
            newElement.OnEnergyDepletion += RemoveElement;
            AllElements.Add(newElement);
        }

        void RemoveElement(ElementStatus status)
        {
            AllElements.Remove(status);
            status.OnEnergyDepletion -= RemoveElement;
        }
    }
}
