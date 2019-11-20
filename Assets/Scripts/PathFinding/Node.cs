using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Node : MonoBehaviour{

	public LayerMask obsMask;
	public float obsRange;
	
	public Node parent;
	public int gCost, hCost;
	public int xPos, yPos;
	public NodeSettings.NodeType nodeType = NodeSettings.NodeType.Default;
	
	public int fCost{
		get{ return gCost + hCost; }
	}
	
	public MeshRenderer mRend;
	private List<Neighbour> neighbours = new List<Neighbour>();
	
	[Serializable]
	public struct Neighbour{
		public Node node;
		public bool diagonal;
	}
	
	private void Awake(){
		mRend = GetComponent<MeshRenderer>();
	}

	private void Update(){
		switch(nodeType){
			case NodeSettings.NodeType.Default:
				mRend.material.color = NodeSettings.nodeDef;
				mRend.enabled = NodeSettings.showNodeGrid;
				break;
			case NodeSettings.NodeType.Start:
				mRend.material.color = NodeSettings.nodeStart;
				mRend.enabled = NodeSettings.showNodePath || NodeSettings.showNodeGrid;
				break;
			case NodeSettings.NodeType.End:
				mRend.material.color = NodeSettings.nodeEnd;
				mRend.enabled = NodeSettings.showNodePath || NodeSettings.showNodeGrid;
				break;
			case NodeSettings.NodeType.Path:
				mRend.material.color = NodeSettings.nodePath;
				mRend.enabled = NodeSettings.showNodePath || NodeSettings.showNodeGrid;
				break;
			case NodeSettings.NodeType.Checked:
				mRend.material.color = NodeSettings.nodeChecked;
				mRend.enabled = NodeSettings.showNodeGrid;
				break;
		}
	}
	
	public List<Neighbour> GetNeighbours(){
		return neighbours;
	}

	public void SetPosition(int x, int y){
		xPos = x;
		yPos = y;
	}
	
	public void SetNeighbour(Node obj, bool diagonal){
		Neighbour n;
		n.node = obj;
		n.diagonal = diagonal;
		neighbours.Add(n);
	}

	public void DeleteNeighbour(GameObject obj){
		foreach(Neighbour n in neighbours){
			if(n.node.gameObject == obj){
				neighbours.Remove(n);
				break;
			}
		}
	}

	public void ObstacleCheck(){
		DetectObstacles();
	}

	private void DetectObstacles(){
		List<GameObject> obstacles = HelperFunctions.GetObjectsInRange(transform.position, obsRange, obsMask);
		if(obstacles.Count > 0){
			foreach(Neighbour n in neighbours){
				n.node.DeleteNeighbour(gameObject);
			}
			Destroy(gameObject);
		}
	}
	
	
	
	
}
