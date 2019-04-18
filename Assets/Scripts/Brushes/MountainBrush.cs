using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainBrush : MonoBehaviour
{
    public Material _paintMaterial;

    [Range(0, 0.1f)] public float Strength;
    [Range(0, 1)] public float Softness;
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
        _paintMaterial.SetColor("_Color", new Color(Strength, Strength, Strength));
        _paintMaterial.SetFloat("_Softness", Softness);
        _paintMaterial.SetVector("_Coordinate", new Vector4(pos.x / size, pos.z / size, 0, 0));
        _paintMaterial.SetFloat("_Radius", Range);
        var temp = RenderTexture.GetTemporary(_descriptor);
        Graphics.Blit(GameController.OffsetMap, temp);
        Graphics.Blit(temp, GameController.OffsetMap, _paintMaterial);
        RenderTexture.ReleaseTemporary(temp);
    }
}