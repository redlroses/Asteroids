using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AsteroidsPool))]
public class AsteroidPoolEditor : Editor
{
    private enum SizeRu { Mаленький, Средний, Большой }
    
    private bool _isShowSpawnTime = true;
    private bool _isShowDufficultyLevel = true;
    private SizeRu _size;

    private SerializedProperty _maxSpawnTime;
    private SerializedProperty _minSpawnTime;
    private SerializedProperty _isActive;
    private SerializedProperty _objectsCopy;
    private SerializedProperty _container;
    private SerializedProperty _lookAtTargetsParent;
    private SerializedProperty _spawnTargetsParent;
    private SerializedProperty _difficultyLevelsAmount;
    private SerializedProperty _gameTimer;

    private GUIStyle _foldoutStyle;
    private GUIStyle _prefabs;
    private GUIStyle _fontStyle;

    public override void OnInspectorGUI()
    {
        _prefabs = EditorStyles.whiteLabel;
        _prefabs.fontStyle = FontStyle.Bold;
        _prefabs.padding.left = 15;
        
        _foldoutStyle = new GUIStyle("Box");
        _foldoutStyle.padding.left = 20;
        
        _fontStyle = EditorStyles.foldout;
        _fontStyle.fontStyle = FontStyle.Bold;

        _isActive.boolValue = EditorGUILayout.ToggleLeft("Активировать спавнер", _isActive.boolValue);

        EditorGUILayout.BeginVertical(_foldoutStyle);
            _isShowDufficultyLevel = EditorGUILayout.Foldout(_isShowDufficultyLevel, "Количество уровней сложности", true, _fontStyle);
            
            if (_isShowDufficultyLevel)
            {
                _difficultyLevelsAmount.intValue = EditorGUILayout.IntSlider(_difficultyLevelsAmount.intValue, 1,200);
                EditorGUILayout.HelpBox("Количество ступеней уменьшения времени спавна астероидов от максимального до минимального времени ",MessageType.Info);
            }
            
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space(10);

        GUILayout.BeginVertical(_foldoutStyle);
            _isShowSpawnTime = EditorGUILayout.Foldout(_isShowSpawnTime, "Настройка времени спавна астеройдов", true, _fontStyle);
            
            if (_isShowSpawnTime)
            {
                _minSpawnTime.intValue = 
                    EditorGUILayout.IntSlider("Минимум",_minSpawnTime.intValue, 50, _maxSpawnTime.intValue);
                _maxSpawnTime.intValue = 
                    EditorGUILayout.IntSlider("Максимум",_maxSpawnTime.intValue, _minSpawnTime.intValue, 5000);
            }
            
        GUILayout.EndVertical();

        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("Префабы астеройдов", _prefabs);
        
        EditorGUILayout.BeginVertical("Box");
        
        for (int i = 0; i < 3; i++) 
        {
            _size = (SizeRu) i;
            if (_objectsCopy.GetArrayElementAtIndex(i) == null)
            { 
                _objectsCopy.InsertArrayElementAtIndex(i); EditorGUILayout.ObjectField(_objectsCopy.GetArrayElementAtIndex(i), typeof(Asteroid),new GUIContent(_size.ToString()));
            }
            else
            {
                EditorGUILayout.ObjectField(_objectsCopy.GetArrayElementAtIndex(i), typeof(Asteroid),new GUIContent(_size.ToString()));
            }
        }
        
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("Родительские объекты", _prefabs);
        
        EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.ObjectField(_container, typeof(Transform), new GUIContent("Астероиды"));
            EditorGUILayout.ObjectField(_lookAtTargetsParent, typeof(Transform), new GUIContent("Цели"));
            EditorGUILayout.ObjectField(_spawnTargetsParent, typeof(Transform), new GUIContent("Спавны"));
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("Ссылки на компоненты", _prefabs);
        
        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.ObjectField(_gameTimer, typeof(GameTimer), new GUIContent("Игровой таймер"));
        EditorGUILayout.EndVertical();
        
        serializedObject.ApplyModifiedProperties();
    }

    private void OnEnable()
    {
        _maxSpawnTime = serializedObject.FindProperty("_maxSpawnTime");
        _minSpawnTime = serializedObject.FindProperty("_minSpawnTime");
        _isActive = serializedObject.FindProperty("_isSpawning");
        _objectsCopy = serializedObject.FindProperty("_objectsCopy");
        _container = serializedObject.FindProperty("_container");
        _lookAtTargetsParent = serializedObject.FindProperty("_lookAtTargetsParent");
        _spawnTargetsParent = serializedObject.FindProperty("_spawnTargetsParent");
        _difficultyLevelsAmount = serializedObject.FindProperty("_difficultyLevelsAmount");
        _gameTimer = serializedObject.FindProperty("_gameTimer");
    }
}
