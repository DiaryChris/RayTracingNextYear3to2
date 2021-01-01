using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Linq;
//using HutongGames.PlayMaker.Actions;
using Object = UnityEngine.Object;


/// <summary>
/// 自定义的Transform编辑器
/// 自定义OnScreenDraw内容
///     GameObject的Material与Shader查找
/// 
/// 添加了对粒子GameObject的预览功能
/// 
/// 
/// </summary>
//[CanEditMultipleObjects, CustomEditor(typeof(Transform))]
[CustomEditor(typeof(Transform))]
public class CustomTransfromEditor : Editor
{


    //void OnSceneGUI()
    //{
    //    OnScreenDraw();
    //}

    //public override void OnInspectorGUI()
    //{
    //    DrawCustomTransformInspector();
    //    //DrawDefaultInspector();

    //}

    public void OnEnable()
    {
        this._positionProperty  = this.serializedObject.FindProperty("m_LocalPosition");
        this._rotationProperty  = this.serializedObject.FindProperty("m_LocalRotation");
        this._scaleProperty     = this.serializedObject.FindProperty("m_LocalScale");

        _transform = target as Transform;
    }

    // -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- 

    private Transform _transform;

    private const float FieldWidth = 400;
    private const bool  WideMode = true;
    private const float PositionMax = 100000.0f;

    private static readonly GUIContent PositionGuiContent = new GUIContent(LocalString("Position")
                                                                 , LocalString("The local position of this Game Object relative to the parent."));
    private static readonly GUIContent RotationGuiContent = new GUIContent(LocalString("Rotation")
                                                                 , LocalString("The local rotation of this Game Object relative to the parent."));
    private static readonly GUIContent ScaleGuiContent = new GUIContent(LocalString("Scale")
                                                                 , LocalString("The local scaling of this Game Object relative to the parent."));

    private static readonly string PositionWarningText = LocalString("Due to floating-point precision limitations, it is recommended to bring the world coordinates of the GameObject within a smaller range.");


    private SerializedProperty _positionProperty;
    private SerializedProperty _rotationProperty;
    private SerializedProperty _scaleProperty;

    private static string LocalString(string text)
    {
        return text;
        //return LocalizationDatabase.GetLocalizedString(text);
    }



    // -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- 


    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private bool ValidatePosition(Vector3 position)
    {
        if (Mathf.Abs(position.x) > CustomTransfromEditor.PositionMax) return false;
        if (Mathf.Abs(position.y) > CustomTransfromEditor.PositionMax) return false;
        if (Mathf.Abs(position.z) > CustomTransfromEditor.PositionMax) return false;
        return true;
    }


    /// <summary>
    /// RotationField
    /// 
    /// </summary>
    /// <param name="rotationProperty"></param>
    /// <param name="content"></param>
    private void RotationPropertyField(SerializedProperty rotationProperty, GUIContent content)
    {
        var transform       = (Transform)this.targets[0];
        var localRotation   = transform.localRotation;
        foreach (UnityEngine.Object t in (UnityEngine.Object[])this.targets)
        {
            if (!SameRotation(localRotation, ((Transform)t).localRotation))
            {
                EditorGUI.showMixedValue = true;
                break;
            }
        }

        EditorGUI.BeginChangeCheck();

        var eulerAngles = EditorGUILayout.Vector3Field(content, localRotation.eulerAngles);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObjects(this.targets, "Rotation Changed");
            foreach (UnityEngine.Object obj in this.targets)
            {
                Transform t = (Transform)obj;
                t.localEulerAngles = eulerAngles;
            }
            rotationProperty.serializedObject.SetIsDifferentCacheDirty();
        }

        EditorGUI.showMixedValue = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rot1"></param>
    /// <param name="rot2"></param>
    /// <returns></returns>
    private bool SameRotation(Quaternion rot1, Quaternion rot2)
    {
        const float tol = 0.01f;
        if (Math.Abs(rot1.x - rot2.x) > tol) return false;
        if (Math.Abs(rot1.y - rot2.y) > tol) return false;
        if (Math.Abs(rot1.z - rot2.z) > tol) return false;
        if (Math.Abs(rot1.w - rot2.w) > tol) return false;
        return true;
    }


    /// <summary>
    /// 
    /// </summary>
    public override void OnInspectorGUI()
    {

        // WideMode
        EditorGUIUtility.wideMode = CustomTransfromEditor.WideMode;
        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - CustomTransfromEditor.FieldWidth; // align field to right of inspector

        this.serializedObject.Update();

        #region Buttons

        GUILayout.Space(12);

        //
        //GUILayout.BeginHorizontal();
        //if (GUILayout.Button("Set Asset No Rip", GUILayout.ExpandWidth(true)))
        //{
        //    SetAssetNoRip();
        //}

        //GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();

        // 重置Transform
        if (GUILayout.Button("Reset All", GUILayout.ExpandWidth(true)))
        {
            _positionProperty.vector3Value = Vector3.zero;
            _rotationProperty.quaternionValue = Quaternion.identity;
            _scaleProperty.vector3Value = Vector3.one;
        }

        // 向下选择
        if (GUILayout.Button("Down", GUILayout.ExpandWidth(true)))
        {
            var childCount = _transform.childCount;
            if (childCount > 0)
            {
                var child0 = _transform.GetChild(0).gameObject;
                Selection.objects = new Object[] { child0 };
            }
        }

        // 向上选择
        if (GUILayout.Button("Up", GUILayout.ExpandWidth(true)))
        {
            var parent = _transform.parent;
            if (parent)
            {
                Selection.objects = new Object[] { parent.gameObject };
            }
        }

        // 在同一层级内切换
        if (GUILayout.Button("Prev", GUILayout.ExpandWidth(true)))
        {

            var parent = _transform.parent;
            if (parent)
            {
                var childCount = parent.childCount;
                if (childCount > 0)
                {
                    // 自己在这个层级中排第几？
                    var index = 0;
                    for (int i = 0; i < childCount; i++)
                    {
                        if (parent.GetChild(i) == _transform)
                        {
                            index = i;
                            break;
                        }
                    }

                    var prev = index - 1;
                    if (prev >= 0)
                    {
                        var prevGo = parent.GetChild(prev).gameObject;
                        Selection.objects = new Object[] { prevGo };
                    }
                }
            }
        }

        //
        if (GUILayout.Button("Next", GUILayout.ExpandWidth(true)))
        {

            var parent = _transform.parent;
            if (parent)
            {
                var childCount = parent.childCount;
                if (childCount > 0)
                {
                    // 自己在这个层级中排第几？
                    var index = 0;
                    for (int i = 0; i < childCount; i++)
                    {
                        if (parent.GetChild(i) == _transform)
                        {
                            index = i;
                            break;
                        }
                    }

                    var next = index + 1;
                    if (next < childCount)
                    {
                        var nextGo = parent.GetChild(next).gameObject;
                        Selection.objects = new Object[] { nextGo };
                    }
                }
            }

        }

        GUILayout.EndHorizontal();
        GUILayout.Space(8);

        #endregion


        #region Override Transform Panel




        // Position // // // // // // // // // // // // // // // // // // // // // // 
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("R", GUILayout.Width(20f)))
        {
            _positionProperty.vector3Value = Vector3.zero;
        }
        EditorGUILayout.PropertyField(this._positionProperty, PositionGuiContent);
        GUILayout.EndHorizontal();


        // Rotation // // // // // // // // // // // // // // // // // // // // // // 
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("R", GUILayout.Width(20f)))
        {
            _rotationProperty.quaternionValue = Quaternion.identity;
        }
        this.RotationPropertyField(this._rotationProperty, RotationGuiContent);
        GUILayout.EndHorizontal();


        // Scale     // // // // // // // // // // // // // // // // // // // // // // 
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("R", GUILayout.Width(20f)))
        {
            _scaleProperty.vector3Value = Vector3.one;
        }
        EditorGUILayout.PropertyField(this._scaleProperty, ScaleGuiContent);
        GUILayout.EndHorizontal();


        // 
        if (!ValidatePosition(((Transform)this.target).position))
        {
            EditorGUILayout.HelpBox(PositionWarningText, MessageType.Warning);
        }

        this.serializedObject.ApplyModifiedProperties();


        #endregion


    }





    #region CustomScreenDraw


    public static float ScreenRectHeight = 24.0f;


    /// <summary>
    /// 绘制区域
    /// </summary>
    protected void OnScreenDraw()
    {

        //var gos = Selection.gameObjects;
        var go = _transform.gameObject;

        // 检测是否含有ParticleSystem
        var havePs = go.GetComponentsInChildren<ParticleSystem>();
        var drawParticleBtns = havePs.Any();

        Handles.BeginGUI();
        GUILayout.BeginArea(new Rect(10f, 10f, 200f, ScreenRectHeight), GUI.skin.box);

        GUILayout.Label(go.name);

        // 
        // TODO : Lock GameObject 锁定当前GameObject的按钮和功能

        GUILayout.Space(10);

        // 是否绘制Area
        if (drawParticleBtns)
        {
            ScreenRectHeight = 300.0f;

            //CustomScreenGUIParticleOps.ScreenDraw(go);
            GUILayout.Space(20);

        }
        else
        {
            ScreenRectHeight = 24.0f;
        }

        GUILayout.EndArea();
        Handles.EndGUI();


        // -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- 
        // 材质Shader辅助

        // 收集所有的材质 
        var mats = new List<Material>();
        //foreach (var g in gos)
        //{
        //    var mr = g.GetComponentsInChildren<MeshRenderer>();
        //    var smr = g.GetComponentsInChildren<SkinnedMeshRenderer>();
        //    if (mr.Any())
        //    {
        //        mats.AddRange(mr.SelectMany(m => m.sharedMaterials));
        //    }
        //    if (smr.Any())
        //    {
        //        mats.AddRange(smr.SelectMany(m => m.sharedMaterials));
        //    }
        //}

        // 一般只查找当前选择的Go
        //var mr = go.GetComponentsInChildren<MeshRenderer>();
        var mr = go.GetComponent<MeshRenderer>();
        if (mr)
        {
            var mrMats = mr.sharedMaterials;
            mats.AddRange(mrMats.Where(m => m));

        }

        //if (mr.Any())
        //{
        //    mats.AddRange(mr.SelectMany(m => m.sharedMaterials));
        //}

        // 查找所有的SkinMesh
        var smr = go.GetComponentsInChildren<SkinnedMeshRenderer>();
        if (smr.Any())
        {
            mats.AddRange(smr.SelectMany(m => m.sharedMaterials));
        }

        // 查找所有粒子的材质
        var pRenders = go.GetComponentsInChildren<ParticleSystemRenderer>();
        foreach (var pr in pRenders)
        {
            mats.AddRange(pr.sharedMaterials);
        }

        // 如果是山体系统
        var ters = go.GetComponentsInChildren<Terrain>();
        mats.AddRange(from t in ters where t.materialType == Terrain.MaterialType.Custom select t.materialTemplate);


        // 列出所有的材质
        if (mats.Any())
        {
            var count = mats.Count;
            Handles.BeginGUI();

            // ScrollView
            //_scrollMaterials = GUILayout.BeginScrollView(_scrollMaterials);

            GUILayout.BeginArea(new Rect(220f, 10f, 500f, ScreenRectHeight * count), GUI.skin.box);
            for (var i = 0; i < count; i++)
            {
                var m = mats[i];
                var mName = m != null ? m.name : "Null";
                var sName = m != null && m.shader != null ? m.shader.name : "Null";


                GUILayout.BeginHorizontal();

                // Index
                GUILayout.Label(i + " : ", GUILayout.Width(20));

                // 查找材质
                if (GUILayout.Button("M", GUILayout.Width(20)))
                {
                    Selection.objects = new Object[] { m };
                }

                // 查找Shader
                if (GUILayout.Button("S", GUILayout.Width(20)))
                {
                    if (m && m.shader != null)
                    {
                        Selection.objects = new Object[] { m.shader };
                    }
                }

                // 查找Shader
                if (GUILayout.Button(">", GUILayout.Width(20)))
                {
                    if (m && m.shader != null)
                    {
                        var id = m.shader.GetInstanceID();
                        //Debug.Log(id);

                        var path = AssetDatabase.GetAssetPath(id);
                        EditorUtility.RevealInFinder(path);



                    }
                }

                // Display on Screen
                GUILayout.Label(mName + " >> " + sName);

                GUILayout.EndHorizontal();

            }

            
            GUILayout.EndArea();
            //GUILayout.EndScrollView();
            Handles.EndGUI();
        }




        // -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- 
        // 
        // 额外的按钮
        //Handles.BeginGUI();
        //GUILayout.BeginArea(new Rect(220f, 10f, 100f, 24f), GUI.skin.box);

        //EditorGUILayout.BeginHorizontal();


        ////    TODO: 修改相关操作的 API ?


        ////    Space.Self;
        ////    Space.World;


        //var btnPivot = GUILayout.Button("Pivot");
        //if (btnPivot)
        //{

        //}

        //var btnLocal = GUILayout.Button("Local");
        //if (btnLocal)
        //{

        //}

        //EditorGUILayout.EndHorizontal();

        //GUILayout.EndArea();
        //Handles.EndGUI();


    }


    #endregion


    #region AssetProcess

    /// <summary>
    /// 
    /// </summary>
    private void SetAssetNoRip()
    {
        //Debug.Log(_transform.gameObject.name);
        //Debug.Log(_transform.gameObject.GetInstanceID());

        // TODO 获得不到Asset的路径？？！！
        //var assetPath = AssetDatabase.GetAssetPath(_transform.gameObject.GetInstanceID());
        //Debug.Log(assetPath.Length);

        // MeshFilter
        var mf = _transform.GetComponent<MeshFilter>();
        if (mf)
        {
            var mesh = mf.sharedMesh;
            var id = mesh.GetInstanceID();
            var asset = AssetDatabase.GetAssetPath(id);

            Debug.Log(asset);
            var modelImport = AssetImporter.GetAtPath(asset) as ModelImporter;
            if (modelImport != null)
            {
                //Debug.Log("Reimport");
                // 不导入动画，以及不生成绑定与骨骼
                modelImport.animationType = ModelImporterAnimationType.None;
                modelImport.importAnimation = false;
            }
            else
            {
                Debug.LogWarning("Null ModelImport");
            }

        }

        // RemoveAnimator
        var animator = _transform.GetComponent<Animator>();
        DestroyImmediate(animator);



    }





    #endregion



}

  

