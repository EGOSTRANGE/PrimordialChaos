//using UnityEditor;
//using UnityEngine;
//
//[InitializeOnLoad]
//public class TerrainEditor : Editor
//{
//    private static Vector3 _handlerPos = Vector3.zero;
//
//    private static Vector3 _prevHandlerPos = Vector3.zero;
//    private static Camera _cam;
//    private static RaycastHit _hit;
//
//    private static Material _paintMaterial;
//
//    private static float BrushSize
//    {
//        get { return EditorPrefs.GetFloat("EditorToolBrushSize", 0); }
//        set
//        {
//            if (value == BrushSize)
//                return;
//            EditorPrefs.SetFloat("EditorToolBrushSize", value);
//        }
//    }
//
//    private static int SelectedTool
//    {
//        get { return EditorPrefs.GetInt("SelectedEditorTool", 0); }
//        set
//        {
//            if (value == SelectedTool)
//                return;
//
//            EditorPrefs.SetInt("SelectedEditorTool", value);
//            Tools.hidden = value != 0;
//        }
//    }
//
//    static TerrainEditor()
//    {
//        var shader = Shader.Find("Unlit/Brush");
//        _paintMaterial = new Material(shader);
//        GridGenerator._paintMat = _paintMaterial;
//        SceneView.onSceneGUIDelegate += OnSceneGUI;
//    }
//
//    void OnDestroy()
//    {
//        Destroy(_paintMaterial);
//        SceneView.onSceneGUIDelegate -= OnSceneGUI;
//    }
//
//    private static void DrawTools(Rect pos)
//    {
//        Handles.BeginGUI();
//
//        GUILayout.BeginArea(new Rect(0, pos.height - 35, pos.width, 20), EditorStyles.toolbar);
//        {
//            string[] buttonLabels = new string[] {"Off", "Erase", "Paint"};
//            SelectedTool = GUILayout.SelectionGrid(SelectedTool, buttonLabels, 3, EditorStyles.toolbarButton,
//                GUILayout.Width(300));
//        }
//        GUILayout.EndArea();
//        if (SelectedTool != 0)
//        {
//            GUI.Box(new Rect(0, 0, 110, pos.height - 35), GUIContent.none, EditorStyles.textArea);
//
//            BrushSize = GUI.VerticalSlider(new Rect(95, 5, 5, pos.height - 50), BrushSize, 1, 0);
//        }
//        Handles.EndGUI();
//    }
//
//    private static void OnSceneGUI(SceneView view)
//    {
//        DrawTools(view.position);
//        if (SelectedTool != 0)
//        {
//            if (Event.current == null) return;
//
//            var mousePos = Event.current.mousePosition;
//            var ray = HandleUtility.GUIPointToWorldRay(mousePos);
//
//            if (Physics.Raycast(ray, out _hit))
//            {
//                _handlerPos = _hit.point;
//                Handles.DrawWireCube(_handlerPos, new Vector3(50, 50, 50));
//
//
//                if (Event.current.type == EventType.KeyDown &&
//                    Event.current.keyCode == KeyCode.Escape)
//                {
//                    SelectedTool = 0;
//                    return;
//                }
//
//                int controlID = GUIUtility.GetControlID(FocusType.Passive);
//                HandleUtility.AddDefaultControl(controlID);
////                if (Event.current.button == 0)
////                {
////                    var chunk = _hit.collider.gameObject;
////                    var w = chunk.transform.parent.gameObject;
////                    _paintMaterial.SetVector("_Coordinate",
////                        new Vector4(_hit.textureCoord.x, _hit.textureCoord.y, 0, 0));
////                    var tex = chunk.GetComponent<MeshRenderer>().material.GetTexture("_OffsetTex");
////                    Graphics.(tex, _paintMaterial);
////                }
//            }
//        }
//    }
//}