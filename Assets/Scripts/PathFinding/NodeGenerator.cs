using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeGenerator : MonoBehaviour{
	
	public GameObject node;
	public Transform nodeParent;
	public float nodeSpacing;
	public float zHeight;
	public Rect rect;
	
	private List<List<GameObject>> nodeNetwork = new List<List<GameObject>>();
	private List<Node> nodeScripts = new List<Node>();

	[Serializable]
	public struct Rect{
		public int x, y, width, height;
	}
	
	
	
	private void GenerateNetwork(){
		int xSize = (int) (rect.width / nodeSpacing);
		int ySize = (int) (rect.height / nodeSpacing);
		float xWidth = (rect.width / nodeSpacing) / (xSize / nodeSpacing);
		float yHeight = (rect.height / nodeSpacing) / (ySize / nodeSpacing);
		
		float x = 0, y = 0;
		int q = 0;
		for(int i = 0; i <= xSize; i++){
			List<GameObject> tmpList = new List<GameObject>();
			x = rect.x + (i * xWidth);
			for(int j = 0; j <= ySize; j++){
				q++;
				y = rect.y + (j * yHeight);
				GameObject spwnd = Instantiate(node, new Vector3(x, zHeight, y), Quaternion.identity, nodeParent);
				//spwnd.transform.name = "" + q;
				tmpList.Add(spwnd);
				Node s = spwnd.GetComponent<Node>();
				s.SetPosition(i, j);
				nodeScripts.Add(s);
			}
			nodeNetwork.Add(tmpList);
		}

		print("Points: " + q);
	}

	
	private void SetNeighbours(){
		int q = 0;
		for(int i = 0; i < nodeNetwork.Count; i++){ //x
			List<GameObject> col = nodeNetwork[i];
			for(int j = 0; j < col.Count; j++){ //y
				Node n = nodeScripts[i * nodeNetwork.Count + j];
				for(int k = -1; k <= 1; k++){
					for(int l = -1; l <= 1; l++){
						bool diagonal = false;
						if(k == 0 && l == 0) continue;
						//Check if object is along an edge
						if(i == 0 && k == -1) continue;
						if(i == nodeNetwork.Count-1 && k == 1) continue;
						if(j == 0 && l == -1) continue;
						if(j == col.Count-1 && l == 1) continue;
						//Check for diagonal neighbour
						if(k == -1 && l == -1 || k == 1 && l == 1 ||
						   k == -1 && l == 1 || k == 1 && l == -1)
							diagonal = true;
						//Set neighbour (nodeNetwork[i+k][j+l])
						n.SetNeighbour(nodeScripts[(i+k) * nodeNetwork.Count + (j+l)], diagonal);
					}
				}
			}
		}
	}

	private void ActivateNodes(){
		foreach(Node nodeScript in nodeScripts){
			nodeScript.ObstacleCheck();
		}
	}
	

	private void CleanNetwork(){
		int i = 0;
		foreach(List<GameObject> gameObjects in nodeNetwork){
			foreach(GameObject obj in gameObjects){
				gameObjects.Remove(obj);
				nodeScripts.RemoveAt(i);
				Destroy(obj);
				i++;
			}
			nodeNetwork.Remove(gameObjects);
		}
	}

	private void Start(){
		GenerateNetwork();
		SetNeighbours();
		ActivateNodes();
	}
}
