using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JumpyWorld
{
	public class MazeForRoom : Generator
	{
		public Floor room;
		public float height = 1f;
		[Header("Runtime")]
		public HashSet<Vector3>
			obstaclePositions = new HashSet<Vector3> ();

		// builds a maze using a blockwise growing tree algorithm:
		// http://weblog.jamisbuck.org/2015/10/31/mazes-blockwise-geometry
		// http://weblog.jamisbuck.org/2011/1/27/maze-generation-growing-tree-algorithm

		public override void Generate (int seed)
		{
			base.Generate (seed);
			
			int randomMazeSeed = (int)System.DateTime.Now.Ticks;
			Random.seed = randomMazeSeed;

			// keep track of possible points that can be used for the maze path
			var mazePositions = new List<Vector3>();

			// start the room with a checkerboard pattern,
			// which we can later create tunnels within to build the maze
			// OXOX
			// XXXX
			// OXOX
			// XXXX
			foreach (var point in Floor.Rectangle(room.size,1f,height)) {
				if (point.position.x % 2 == 0 && point.position.z % 2 == 0) {
					mazePositions.Add(point.position); 
				} else {
					obstaclePositions.Add(point.position);
				}
			}

			// activeMazePositions keeps track of parts of the maze path that we can still expand
			List<Vector3> activeMazePositions = new List<Vector3>();
			activeMazePositions.Add(mazePositions[0]);

			// visitedMazePositions keeps track of which points have been traveled to, so we know
			// when to stop creating paths to the same point
			List<Vector3> visitedMazePositions = new List<Vector3>();

			// keep iterating until we have exhausted the possible maze positions
			while (activeMazePositions.Count > 0) {

				// choose part of the path to expand
				var selectedPos = activeMazePositions[activeMazePositions.Count-1];

				// mark the cell as visited so we can avoid creating more paths to it
				visitedMazePositions.Add(selectedPos);

				// check which neighboring cells are unvisited
				var isNorthNeighborUnvisited = mazePositions.Contains(selectedPos + 2 * Directions.North.ToVector())
					&& !visitedMazePositions.Contains(selectedPos + 2 * Directions.North.ToVector());
				
				var isEastNeighborUnvisited = mazePositions.Contains(selectedPos + 2 * Directions.East.ToVector())
					&& !visitedMazePositions.Contains(selectedPos + 2 * Directions.East.ToVector());
				
				var isSouthNeighborUnvisited = mazePositions.Contains(selectedPos + 2 * Directions.South.ToVector())
					&& !visitedMazePositions.Contains(selectedPos + 2 * Directions.South.ToVector());
				
				var isWestNeighborUnvisited = mazePositions.Contains(selectedPos + 2 * Directions.West.ToVector())
					&& !visitedMazePositions.Contains(selectedPos + 2 * Directions.West.ToVector());

				// for each unvisited direction, add to a list for we can randomly choose one
				List<Vector3> possibleDirections = new List<Vector3>();
				if (isNorthNeighborUnvisited) {
					possibleDirections.Add(Directions.North.ToVector());
				}
				if (isEastNeighborUnvisited) {
					possibleDirections.Add(Directions.East.ToVector());
				}
				if (isSouthNeighborUnvisited) {
					possibleDirections.Add(Directions.South.ToVector());
				}
				if (isWestNeighborUnvisited) {
					possibleDirections.Add(Directions.West.ToVector());
				}
				
				var direction = Directions.South.ToVector();
				int randomDirectionIndex;
				if (possibleDirections.Count > 0) {
					randomDirectionIndex = Random.Range(0,possibleDirections.Count);
					direction = possibleDirections[randomDirectionIndex];
				}

				// calculate the position of the adjacent maze cell, and the wall between
				// the current cell and the adjacent cell
				var newPos = selectedPos + 2 * direction;
				var wallPos = selectedPos + direction;

				// if the new cell hasn't been visited yet, remove the wall to create a tunnel
				var alreadyVisited = visitedMazePositions.Contains(newPos);
				if (!alreadyVisited) {
					obstaclePositions.Remove(wallPos);

					// if the new cell position is within the bounds of possible maze positions,
					// add it to activeMazePositions so it can be expanded next
					var isNewPosInMaze = mazePositions.Contains(newPos);
					if (isNewPosInMaze) {
						activeMazePositions.Add(newPos);
					}
				}

				// if the current cell has no more unvisited neighbors, remove it from 
				// activeMazePositions so we stop trying to expand it
				var hasUnvisitedNeighbors = isNorthNeighborUnvisited
										|| isEastNeighborUnvisited 
										|| isSouthNeighborUnvisited
										|| isWestNeighborUnvisited;
				
				if (!hasUnvisitedNeighbors) {
					activeMazePositions.Remove(selectedPos);
				}
			}
		}

		public override void Draw (TileDrawer tileDrawer, TilePool tilePool)
		{
			base.Draw (tileDrawer, tilePool);
			
			foreach (var point in obstaclePositions) {
				tileDrawer.DrawTerrain (tilePool.defaultGround, at: point, isDynamic: false);
			}
		}
		
		public override void OnDrawGizmos ()
		{
			base.OnDrawGizmos ();			
			
		}
	}
}