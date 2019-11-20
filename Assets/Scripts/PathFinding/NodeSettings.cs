using UnityEngine;

public class NodeSettings : MonoBehaviour {
	
	public static Color nodeDef, nodeStart, nodeEnd, nodePath, nodeChecked;
	public static bool showNodeGrid = false, showNodePath = true;
	
	public Color nodeDefCol, nodeStartCol, nodeEndCol, nodePathCol, nodeCheckedCol;

	public enum NodeType{
		Start,
		End,
		Path,
		Checked,
		Default
	}

	private void Awake(){
		nodeDef = nodeDefCol;
		nodeStart = nodeStartCol;
		nodeEnd = nodeEndCol;
		nodePath = nodePathCol;
		nodeChecked = nodeCheckedCol;
	}

	private void Update(){
		if(Input.GetKeyUp(KeyCode.N)){
			showNodeGrid = !showNodeGrid;
		}
		if(Input.GetKeyUp(KeyCode.B)){
			showNodePath = !showNodePath;
		}
	}
	
}
