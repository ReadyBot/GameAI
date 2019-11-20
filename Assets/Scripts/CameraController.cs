using UnityEngine;

public class CameraController : MonoBehaviour{
	public Vector2 panLimit;
	public Vector3 camOffset;
	public Transform player;
	public float panSpeed = 20f;
	public float panBorderThickness = 10f;
	public float scrollSpeed = 2f;
	public float minY = 5f;
	public float maxY = 30f;
	public bool disableScroll;

	void Update(){
		Vector3 pos = transform.position;
		if(Input.GetKey(KeyCode.W)) pos.z += panSpeed * Time.deltaTime;
		if(Input.GetKey(KeyCode.S)) pos.z -= panSpeed * Time.deltaTime;
		if(Input.GetKey(KeyCode.D)) pos.x += panSpeed * Time.deltaTime;
		if(Input.GetKey(KeyCode.A)) pos.x -= panSpeed * Time.deltaTime;
		if(Input.GetKey(KeyCode.Escape)) Application.Quit();
		if(Input.GetKey(KeyCode.Space)){
			camOffset.y = pos.y;
			pos = player.position + camOffset;
		}

		if(Input.GetKeyUp(KeyCode.P)) HelperFunctions.gamePaused = !HelperFunctions.gamePaused;

		float scroll = Input.GetAxis("Mouse ScrollWheel");
		pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;
		pos.y = Mathf.Clamp(pos.y, minY, maxY);
		transform.position = pos;
	}
}