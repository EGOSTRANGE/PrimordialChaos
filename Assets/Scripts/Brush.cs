using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
	private RaycastHit _hit;
	private Material _material;
	public Material _paintMaterial;
	public Camera _cam;
	public RenderTexture _heightMap;
	public float CircleRange;
    // Start is called before the first frame update
    void Start()
    {
        _paintMaterial = new Material(_paintMaterial);
        _paintMaterial.SetVector("_color", Color.red);
        // _material.SetTexture("_Heightmap", _heightMap);
    }

    // Update is called once per frame
    void Update()
    {
    	// if(Input.GetKey(KeyCode.Mouse0)){
    	// 	if(Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition),out _hit)){
		// 		Debug.Log("Clicking");				
    	// 		_paintMaterial.SetVector("_Coordinate", new Vector4(_hit.textureCoord.x, _hit.textureCoord.y, 0, 0));
    	// 		var temp = RenderTexture.GetTemporary(_heightMap.width, _heightMap.height, 0, RenderTextureFormat.ARGBFloat);
    	// 		Graphics.Blit(_heightMap, temp);
    	// 		Graphics.Blit(temp, _heightMap, _paintMaterial);
    	// 		RenderTexture.ReleaseTemporary(temp);
    	// 	}
    	// }
    }

	private void OnDrawGizmos() {
		if(Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition),out _hit)){
			Gizmos.DrawWireSphere(_hit.point, 1);
			// Gizmos.Color = Color.Pink;
			// Gizmos.Circle(_hit.position, 4);
    			// _paintMaterial.SetVector("_Coordinate", new Vector4(_hit.textureCoord.x, _hit.textureCoord.y, 0, 0));
    			// var temp = RenderTexture.GetTemporary(_heightMap.width, _heightMap.height, 0, RenderTextureFormat.ARGBFloat);
    			// Graphics.Blit(_heightMap, temp);
    			// Graphics.Blit(temp, _heightMap, _paintMaterial);
    			// RenderTexture.ReleaseTemporary(temp);
    		}
	}
}
