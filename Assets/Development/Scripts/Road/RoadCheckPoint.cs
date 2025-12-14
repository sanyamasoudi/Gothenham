using UnityEngine;

namespace Development.Scripts.Road
{
    public class RoadCheckPoint : MonoBehaviour
    {
        [SerializeField] private GameObject road;

        public void OnDestroyRoad()
        {
            Destroy(road);
        }
    }
}
