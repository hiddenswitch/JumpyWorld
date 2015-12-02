using UnityEngine;
using System.Collections;


namespace JumpyWorld { 
    [RequireComponent (typeof(GridLocateable))]
    [RequireComponent(typeof(ICondition))]
    public class ConditionalBuildForwardPlatform : MonoBehaviour {
        public ICondition condition;
        public TileDrawer tileDrawer;
        public TilePool tilePool;

        public Collects woodCollector;

        GridLocateable gridLocatable;

        void Start()
        {
            gridLocatable = GetComponent<GridLocateable>();
        }

        void Update()
        {
            if (condition.evaluate()) {
                Vector3 forward = gridLocatable.PositionGrid;// + transform.forward;
                if (!tileDrawer.Contains(forward))
                {
                    tileDrawer.DrawTerrain(prefab: tilePool.defaultGround,at: forward);
                    woodCollector.decrement(1);
                }
                
            }
        }


        
    }
}
