using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour{

	public AStar aStar;
	public float speed;
	public PathLimits pathLimits;

	private List<Node> path = null;
	private int currentNode = 0;
	private bool needPath = true;
	
	private Vector3 limit = new Vector3(0.1f, 0.1f , 0.1f);
	private Thread requestPath;

	private PathSettings pathSettings;
	private struct PathSettings{
		public Node startNode;
		public Node endNode;
	}

	[Serializable]
	public struct PathLimits{
		public Vector3 min;
		public Vector3 max;
	}

	private void Update(){
		if(HelperFunctions.gamePaused)
			return;
		
		if(path != null){
			FollowPath();
		} else if(needPath){
			needPath = false;
			FindNewPath();
		}
	}

	public void FindNewPath(Vector3 endPos = new Vector3()){
		if(endPos == Vector3.zero)
			endPos = HelperFunctions.RandomPoint(pathLimits.min, pathLimits.max);

		pathSettings.startNode = aStar.GetClosestNode(transform.position);
		pathSettings.endNode = aStar.GetClosestNode(endPos);
		
		if(requestPath != null && requestPath.IsAlive){
			return;
		}
		requestPath = new Thread(RequestPath);
		requestPath.Start();
	}

	private void FollowPath(){
		Node n = path[currentNode];

		if(Vector3.Distance(transform.position, n.transform.position) > 0.1f){
			transform.LookAt(n.transform);
			transform.position = Vector3.MoveTowards(transform.position, n.transform.position, speed * Time.deltaTime);
		} else{
			currentNode++;
			if(currentNode == path.Count){
				path = null;
				needPath = true;
				currentNode = 0;
			}
		}
		
	}


	public void RequestPath(){
		path = aStar.FindPathFromNodes(pathSettings.startNode, pathSettings.endNode);
		currentNode = 0;
	}

	
	
	
	
	
}
