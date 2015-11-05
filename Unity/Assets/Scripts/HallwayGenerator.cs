using UnityEngine;
using System.Collections;
namespace JumpyWorld
{
    public abstract class HallwayGenerator : MonoBehaviour
    {
        public abstract Platform generateHallway(Vector3 startPosition, Vector3 endPosition);
    }
}
