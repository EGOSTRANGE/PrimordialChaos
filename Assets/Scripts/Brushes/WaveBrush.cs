using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBrush : MonoBehaviour
{
    public Material _paintMaterial;

    public Color color;

    [Range(0, 0.15f)] public float Range = 0.025f;

    private RenderTextureDescriptor _descriptor;
    
    // Start is called before the first frame update
    void Start()
    {
        _descriptor = GameController.MapDescriptor;
    }

    // Update is called once per frame
    void Update()
    {
        var size = GameController.MapDescriptor.width;
        var pos = transform.position;
        _paintMaterial.SetColor("_Color", color);
        _paintMaterial.SetVector("_Coordinate", new Vector4(pos.x / size, pos.z / size, 0, 0));
        _paintMaterial.SetFloat("_Radius", Range);
        var temp = RenderTexture.GetTemporary(_descriptor);
        Graphics.Blit(GameController.WaveMap, temp);
        Graphics.Blit(temp, GameController.WaveMap, _paintMaterial);
        RenderTexture.ReleaseTemporary(temp);
    }
}