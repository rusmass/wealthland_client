using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CurvySpline)), CanEditMultipleObjects]
public class CurvySplineInspector : Editor {
    CurvySpline Target { get { return target as CurvySpline; } }
    SerializedProperty tInterpolation;
    SerializedProperty tClosed;
    SerializedProperty tAutoEndTangents;
    SerializedProperty tInitialUp;
    SerializedProperty tOrientation;
    SerializedProperty tSwirl;
    SerializedProperty tSwirlTurns;
    SerializedProperty tGranularity;
    SerializedProperty tShowApprox;
    SerializedProperty tShowOrientation;
    SerializedProperty tShowTangents;
    SerializedProperty tAutoRefresh;
    SerializedProperty tAutoRefreshLength;
    SerializedProperty tAutoRefreshOrientation;
    SerializedProperty tT;
    SerializedProperty tC;
    SerializedProperty tB;
    SerializedProperty tShowGizmos;
    SerializedProperty tUserValueSize;
    SerializedProperty tShowUserValues;

    GUIStyle mUserValuesLabel;

    [MenuItem("GameObject/Create Other/Curvy/Spline")]
    static void CreateCurvySpline()
    {
        CurvySpline spl=CurvySpline.Create();
        spl.Interpolation=CurvyInterpolation.CatmulRom;
        spl.AutoEndTangents=true;
        spl.Closed=true;
        spl.Add(new Vector3(-2, -1, 0), new Vector3(0, 2, 0), new Vector3(2, -1, 0));
        Selection.activeObject = spl;
    }

    void OnEnable()
    {
        tInterpolation = serializedObject.FindProperty("Interpolation");
        tClosed = serializedObject.FindProperty("Closed");
        tAutoEndTangents = serializedObject.FindProperty("AutoEndTangents");
        tInitialUp = serializedObject.FindProperty("InitialUpVector");
        tOrientation = serializedObject.FindProperty("Orientation");
        tSwirl = serializedObject.FindProperty("Swirl");
        tSwirlTurns = serializedObject.FindProperty("SwirlTurns");
        tGranularity = serializedObject.FindProperty("Granularity");
        tShowApprox = serializedObject.FindProperty("ShowApproximation");
        tShowOrientation = serializedObject.FindProperty("ShowOrientation");
        tShowTangents = serializedObject.FindProperty("ShowTangents");
        tAutoRefresh = serializedObject.FindProperty("AutoRefresh");
        tAutoRefreshLength = serializedObject.FindProperty("AutoRefreshLength");
        tAutoRefreshOrientation = serializedObject.FindProperty("AutoRefreshOrientation");
        tT = serializedObject.FindProperty("Tension");
        tC = serializedObject.FindProperty("Continuity");
        tB = serializedObject.FindProperty("Bias");
        tShowGizmos = serializedObject.FindProperty("ShowGizmos");
        tUserValueSize = serializedObject.FindProperty("UserValueSize");
        tShowUserValues = serializedObject.FindProperty("ShowUserValues");

        Target.Refresh(true, true,false);
        SceneView.RepaintAll();
    }

    void OnSceneGUI()
    {
        //Handles.Label(Target.transform.position - new Vector3(0,0.2f,0), Target.name);
        Handles.BeginGUI();
        GUILayout.Window(Target.GetInstanceID(), new Rect(10, 40, 150, 20), DoWin, Target.name);

        mUserValuesLabel= new GUIStyle(EditorStyles.boldLabel);
        mUserValuesLabel.normal.textColor = Color.green;
        
        if (Target.UserValueSize>0 && Target.ShowUserValues) 
            foreach (CurvySplineSegment cp in Target.ControlPoints)
                UserValueReadOut(cp.Transform.position,cp.UserValues);
        Handles.EndGUI();
    }

    void UserValueReadOut(Vector3 p, Vector3[] values)
    {
        string uvalues = "";
        for (int i = 0; i < values.Length; i++)
            uvalues += string.Format("{0}: {1}\n", i, values[i]);
        Handles.Label(p + new Vector3(0, -0.2f, 0), uvalues, mUserValuesLabel);
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(tInterpolation,new GUIContent("Interpolation","Interpolation Method"));
        EditorGUILayout.PropertyField(tClosed,new GUIContent("Close Spline","Close spline?"));
        GUI.enabled = !tClosed.boolValue && tInterpolation.enumNames[tInterpolation.enumValueIndex]!="Linear";
        EditorGUILayout.PropertyField(tAutoEndTangents,new GUIContent("Auto End Tangents","Handle End Control Points automatically?"));
        GUI.enabled = true;
        EditorGUILayout.PropertyField(tGranularity, new GUIContent("Granularity", "Approximation resolution"));
        tGranularity.intValue = Mathf.Max(1, tGranularity.intValue);
        EditorGUILayout.LabelField("Orientation", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(tOrientation, new GUIContent("Orientation", "How the Up-Vector should be calculated"));
        if (tOrientation.enumNames[tOrientation.enumValueIndex] == "Tangent") {
            EditorGUILayout.PropertyField(tInitialUp, new GUIContent("Initial Up-Vector", "How the first Up-Vector should be determined"));
            EditorGUILayout.PropertyField(tSwirl, new GUIContent("Swirl", "Orientation swirl mode"));
            if (tSwirl.enumNames[tSwirl.enumValueIndex] != "None")
                EditorGUILayout.PropertyField(tSwirlTurns, new GUIContent("Turns", "Swirl turns"));
        }
        EditorGUILayout.LabelField("Updates", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(tAutoRefresh, new GUIContent("Auto Refresh", "Refresh when Control Point position change?"));
        EditorGUILayout.PropertyField(tAutoRefreshLength, new GUIContent("Auto Refresh Length", "Recalculate Length on Refresh?"));
        EditorGUILayout.PropertyField(tAutoRefreshOrientation, new GUIContent("Auto Refresh Orientation", "Recalculate tangent normals and Up-Vectors on Refresh?"));
        
        if (tInterpolation.enumNames[tInterpolation.enumValueIndex] == "TCB") {
            EditorGUILayout.LabelField("TCB", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(tT, new GUIContent("Tension", "Tension for TCB-Spline"));
            EditorGUILayout.PropertyField(tC, new GUIContent("Continuity", "Continuity for TCB-Spline"));
            EditorGUILayout.PropertyField(tB, new GUIContent("Bias", "Bias for TCB-Spline"));

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Set Catmul", "Set TCB to match Catmul Rom"))) {
                tT.floatValue = 0; tC.floatValue = 0; tB.floatValue = 0;
            }
            if (GUILayout.Button(new GUIContent("Set Cubic", "Set TCB to match Simple Cubic"))) {
                tT.floatValue = -1; tC.floatValue = 0; tB.floatValue = 0;
            }
            if (GUILayout.Button(new GUIContent("Set Linear", "Set TCB to match Linear"))) {
                tT.floatValue = 0; tC.floatValue = -1; tB.floatValue = 0;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.LabelField("Miscellaneous", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(tUserValueSize, new GUIContent("User Value Size", "Size of User Value array"));
        EditorGUILayout.PropertyField(tShowUserValues, new GUIContent("Show in scene", "Show values in the scene view"));
        EditorGUILayout.LabelField("Gizmos",EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(tShowGizmos, new GUIContent("Show Gizmos", "Show Spline Gizmos"));
        EditorGUILayout.PropertyField(tShowApprox, new GUIContent("Show Approximation", "Show Approximation"));
        EditorGUILayout.PropertyField(tShowTangents, new GUIContent("Show Tangents", "Show Tangents"));
        EditorGUILayout.PropertyField(tShowOrientation, new GUIContent("Show Orientation", "Show Orientation"));

        if (serializedObject.targetObject && serializedObject.ApplyModifiedProperties()) {
            Target.Refresh(true,true,false);
            SceneView.RepaintAll();
        }
        
        EditorGUILayout.LabelField("Spline Info",EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Total Length: " + Target.Length);
            EditorGUILayout.LabelField("Control Points: " + Target.ControlPointCount);
            EditorGUILayout.LabelField("Segments: " + Target.Count);

        Repaint();   
    }

    void DoWin(int id)
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add")) 
		{
            Selection.activeTransform = Target.Add().transform;
        }
        
		GUI.enabled = Target.ControlPointCount > 0;
		if (GUILayout.Button ("Clear") && EditorUtility.DisplayDialog (
			"Clear Spline", "Really remove all Control Points?", "Yes", "No")) 
		{
			Target.Clear();
		}

		if (GUILayout.Button ("First CP")) 
		{
			Selection.activeObject = Target.ControlPoints[0];
		}
        GUI.enabled = true;
        GUILayout.EndHorizontal();
    }
}


