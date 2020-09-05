using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Randomizer))]
public class RandomizerEditor : Editor
{
    private Randomizer _randomizer;
    
    private SerializedProperty _bonusSpawnChance;
    private SerializedProperty _weaponUpgradeSpawnChance;
    private SerializedProperty _weaponBonusSpawnChance;
    private SerializedProperty _shieldChargeSpawnChance;
    private SerializedProperty _rarityColors;

    private GUIStyle _chances;

    private float _maxSliderValue;

    private Bonus.RarityType _rarity;
    
    public override void OnInspectorGUI()
    {
        _chances = EditorStyles.whiteLabel;
        _chances.fontStyle = FontStyle.Bold;
        _chances.padding.left = 15;
        
        EditorGUILayout.LabelField("Настройка цветов редкости", _chances);
        
        EditorGUILayout.BeginVertical("Box");
        
        for (int i = 0; i < 3; i++) 
        {
            _rarity = (Bonus.RarityType) i;
            if (_rarityColors.GetArrayElementAtIndex(i) == null)
            { 
                _rarityColors.InsertArrayElementAtIndex(i);
                _rarityColors.GetArrayElementAtIndex(i).colorValue = EditorGUILayout.ColorField(_rarity.ToString(), _rarityColors.GetArrayElementAtIndex(i).colorValue);
            }
            else
            {
                _rarityColors.GetArrayElementAtIndex(i).colorValue = EditorGUILayout.ColorField(_rarity.ToString(), _rarityColors.GetArrayElementAtIndex(i).colorValue);
            }
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.LabelField("Настройка шансов выпадения", _chances);
        
        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.GradientField("Астеройдов %", _randomizer.AsteroidChances);
        EditorGUILayout.HelpBox("Small - Red; Medium - Green; Big - Blue", MessageType.Info, true);
        EditorGUILayout.GradientField("Редкости %", _randomizer.RarityChances);
        EditorGUILayout.HelpBox("Common - Red; Rare - Green; Legend - Blue", MessageType.Info, true);
        EditorGUILayout.GradientField("Бонусов %", _randomizer.BonusChance);
        EditorGUILayout.HelpBox("Damage - Red; Cooldown - Green; Speed - Blue", MessageType.Info, true);
        //EditorGUILayout.GradientField("Главный %", _randomizer.SpawnChances);
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("Шансы спавна бонусов", _chances);

        EditorGUILayout.BeginVertical("Box");
        _bonusSpawnChance.floatValue = 
            EditorGUILayout.Slider("Обычный бонус %", _bonusSpawnChance.floatValue, 0f,100f -  (_weaponUpgradeSpawnChance.floatValue + _weaponBonusSpawnChance.floatValue + _shieldChargeSpawnChance.floatValue));
        _weaponUpgradeSpawnChance.floatValue = 
            EditorGUILayout.Slider("Апгрейд оружия %", _weaponUpgradeSpawnChance.floatValue, 0f,100f -  (_bonusSpawnChance.floatValue + _weaponBonusSpawnChance.floatValue + _shieldChargeSpawnChance.floatValue));
        _weaponBonusSpawnChance.floatValue= 
            EditorGUILayout.Slider("Бонус оружия %", _weaponBonusSpawnChance.floatValue, 0f,100f -  (_bonusSpawnChance.floatValue + _weaponUpgradeSpawnChance.floatValue + _shieldChargeSpawnChance.floatValue));
        _shieldChargeSpawnChance.floatValue= 
            EditorGUILayout.Slider("Заряд щита %", _shieldChargeSpawnChance.floatValue, 0f,100f -  (_bonusSpawnChance.floatValue + _weaponUpgradeSpawnChance.floatValue + _weaponBonusSpawnChance.floatValue));
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }

    private void OnEnable()
    {
        _randomizer = (Randomizer) target;
        
        _rarityColors = serializedObject.FindProperty("_rarityColors");
        _bonusSpawnChance = serializedObject.FindProperty("_bonusSpawnChance");
        _weaponUpgradeSpawnChance = serializedObject.FindProperty("_weaponUpgradeSpawnChance");
        _weaponBonusSpawnChance = serializedObject.FindProperty("_weaponBonusSpawnChance");
        _shieldChargeSpawnChance = serializedObject.FindProperty("_shieldChargeSpawnChance");
    }
}
