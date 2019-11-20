using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class AStar : MonoBehaviour{
	
	public LayerMask mask;
	public Node start, end;
	public PathFinder pathFinder;

	private Camera main;
	private List<Node> path = new List<Node>(), oldPath = new List<Node>();
	private bool newPath = false;

	private List<Node> checkedList = new List<Node>();
	private List<Node> oldCheckedList = new List<Node>();
	
	private void Awake(){
		main = Camera.main;
	}

	private void Update(){
		if(Input.GetMouseButton(1)){
			Vector3 v = ShootRay();
			if(v != Vector3.zero){
				pathFinder.FindNewPath(v);
			}
		}

		if(start != null && end != null && newPath){
			newPath = false;
			FindNewPath();
		}
	}

	public List<Node> FindPathFromWorld(Vector3 s, Vector3 e){
		start = GetClosestNode(s);
		end = GetClosestNode(e);
		FindNewPath();
		return path;
	}

	public List<Node> FindPathFromNodes(Node nodeA, Node nodeB){
		start = nodeA;
		end = nodeB;
		FindNewPath();
		return path;
	}

	private void FindNewPath(){
		oldPath = path;
		path = FindPath(start, end);
		if(path.Count < 1){
			print("No path found");
			return;
		}
		DrawPath();
	}

	private void DrawPath(){
		if(oldCheckedList.Count > 0){
			foreach(Node node in oldCheckedList){
				if(!checkedList.Contains(node))
					node.nodeType = NodeSettings.NodeType.Default;
			}
		}
		if(oldPath.Count > 0){
			foreach(Node node in oldPath){
				node.nodeType = NodeSettings.NodeType.Default;
			}
		}
		if(checkedList.Count > 0){
			foreach(Node node in checkedList){
				node.nodeType = NodeSettings.NodeType.Checked;
			}
		}
		if(path.Count > 0){
			foreach(Node node in path){
				node.nodeType = NodeSettings.NodeType.Path;
			}

			path[path.Count - 1].nodeType = NodeSettings.NodeType.End;
			path[0].nodeType = NodeSettings.NodeType.Start;
		}
	}

	private Vector3 ShootRay(){
		Vector3 v = Vector3.zero;
		Ray ray = main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, 1000f)){
			v = hit.point;
		}
		return v;
	}

	public Node GetClosestNode(Vector3 point){
		Collider[] cols = Physics.OverlapSphere(point, 5f, mask);
		if(cols.Length > 0){
			GameObject closestObject = null;
			float closestDistance = float.MaxValue;
			foreach(Collider col in cols){
				float dis = Vector3.Distance(col.transform.position, point);
				if(dis < closestDistance){
					closestDistance = dis;
					closestObject = col.gameObject;
				}
			}
			if(closestObject == null)
				return null;
			return closestObject.GetComponent<Node>();
		}
		
		return null;
	}


	private List<Node> FindPath(Node startNode, Node endNode){
		if(endNode == startNode || endNode == null){
			List<Node> nodeList = new List<Node>();
			nodeList.Add(startNode);
			return nodeList;
		}
		oldCheckedList = checkedList;
		checkedList = new List<Node>();
		List<Node> openList = new List<Node>();
		List<Node> closedList = new List<Node>();

		openList.Add(startNode);

		while(openList.Count > 0){
			Node node = openList[0];
			for(int i = 1; i < openList.Count; i++){
				if(openList[i].fCost < node.fCost || openList[i].fCost == node.fCost){
					node = openList[i];
				}
			}
			
			
			openList.Remove(node);
			closedList.Add(node);

			if(endNode == node){
				return RetracePath(startNode, endNode);
			}

			foreach(Node.Neighbour neighbour in node.GetNeighbours()){
				if(closedList.Contains(neighbour.node)){
					continue;
				}

				Node n = neighbour.node;
				if(n == null)
					print("Node neighbour is null");
				int neighbourCost = node.gCost + GetDistance(node, n);
				if(neighbourCost < n.gCost || !openList.Contains(n)){
					n.gCost = neighbourCost;
					n.hCost = GetDistance(n, endNode);
					n.parent = node;
					checkedList.Add(n);

					if(!openList.Contains(n)){
						openList.Add(n);
					}
				}
			}
		}

		//No path found
		return new List<Node>();
	}

	private List<Node> RetracePath(Node startNode, Node endNode){
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while(currentNode != startNode){
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Add(startNode);
		path.Reverse();

		return path;
	}

	private int GetDistance(Node nodeA, Node nodeB){
		/*if(nodeA == null){
			print("Some Error on null pointer when getting distance nodeA.");
			return 0;
		}
		if(nodeB == null){
			print("Some Error on null pointer when getting distance nodeB.");
			return 0;
		}*/
		int disX = Mathf.Abs(nodeA.xPos - nodeB.xPos);
		int disY = Mathf.Abs(nodeA.yPos - nodeB.yPos);
		if(disX > disY){
			return 14 * disY + 10 * (disX - disY);
		}
		return 14 * disX + 10 * (disY - disX);
	}
}