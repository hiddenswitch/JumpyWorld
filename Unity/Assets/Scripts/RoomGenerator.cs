using UnityEngine;
using System.Collections;
namespace JumpyWorld
{
    public abstract class RoomGenerator : MonoBehaviour
    {

        //assuming rectangular rooms...
        public abstract Platform generateRoom(Vector3 roomCenterPosition, int xSize, int ySize);
    }
}
