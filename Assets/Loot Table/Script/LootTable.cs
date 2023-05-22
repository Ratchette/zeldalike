using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TinyScript;

#if UNITY_EDITOR
using UnityEditorInternal;
#endif

namespace TinyScript {

    [CreateAssetMenu]
    public class LootTable : ScriptableObject {
        public DropItem[] GuaranteedLootTable = new DropItem[0];
        public DropItem[] OneItemFromList = new DropItem[1];
        public float WeightToNoDrop = 100;

        // Return List of Guaranteed Drop 
        public List<GameObject> GetGuaranteeedLoot() {
            List<GameObject> lootList = new List<GameObject>();

            for (int i = 0; i < GuaranteedLootTable.Length; i++) {
                // Adds the drawn number of items to drop
                int count = Random.Range(GuaranteedLootTable[i].MinCountItem, GuaranteedLootTable[i].MaxCountItem);
                for (int j = 0; j < count; j++) {
                    lootList.Add(GuaranteedLootTable[i].Drop);
                }
            }

            return lootList;
        }

        // Return List of Optional Drops 
        public List<GameObject> GetRandomLoot(int ChanceCount) {
            List<GameObject> lootList = new List<GameObject>();
            float totalWeight = WeightToNoDrop;

            // Executes a function a specified number of times
            for (int j = 0; j < ChanceCount; j++) {
                // They add up the entire weight of the items
                for (int i = 0; i < OneItemFromList.Length; i++) {
                    totalWeight += OneItemFromList[i].Weight;
                }

                float value = Random.Range(0, totalWeight);
                float timed_value = 0;

                for (int i = 0; i < OneItemFromList.Length; i++) {
                    // If timed_value is greater than value, it means this item has been drawn
                    timed_value += OneItemFromList[i].Weight;
                    if (timed_value > value) {
                        int count = Random.Range(OneItemFromList[i].MinCountItem, OneItemFromList[i].MaxCountItem + 1);
                        for (int c = 0; c < count; c++) {
                            lootList.Add(OneItemFromList[i].Drop);
                        }
                        break;
                    }
                }
            }

            return lootList;
        }


        public void SpawnDrop(Transform _position, int _count, float _range) {
            List<GameObject> guaranteed = GetGuaranteeedLoot();
            List<GameObject> randomLoot = GetRandomLoot(_count);

            for (int i = 0; i < guaranteed.Count; i++) {
                Instantiate(guaranteed[i], new Vector3(_position.position.x + Random.Range(-_range, _range), _position.position.y, _position.position.z + Random.Range(-_range, _range)), Quaternion.identity);
            }

            for (int i = 0; i < randomLoot.Count; i++) {
                Instantiate(randomLoot[i], new Vector3(_position.position.x + Random.Range(-_range, _range), _position.position.y, _position.position.z + Random.Range(-_range, _range)), Quaternion.identity);
            }
        }
    }

    /* --------------------- */
    // Drop Item Class
    /* --------------------- */

    [System.Serializable]
    public class DropItem {
        public float Weight;
        public GameObject Drop;
        public int MinCountItem;
        public int MaxCountItem;
    }
}


/* --------------------- */
// Custom Editor & Property Draw (look)
/* --------------------- */


#if UNITY_EDITOR

/* --------------------- */
// Custom Editor
/* --------------------- */

[CustomEditor(typeof(LootTable))]
public class LootTableEditor : Editor {
    // Guaranteed
    SerializedProperty guaranteedList;
    ReorderableList reorderableGuaranteed;
    // Chance
    SerializedProperty chanceToGetList;
    ReorderableList reorderableChance;

    LootTable ld;

    private void OnEnable() {
        /* GUARANTEED */
        guaranteedList = serializedObject.FindProperty("GuaranteedLootTable");
        reorderableGuaranteed = new ReorderableList(serializedObject, guaranteedList, true, true, true, true);
        // Functions
        reorderableGuaranteed.drawElementCallback = DrawGuaranteedListItems;
        reorderableGuaranteed.drawHeaderCallback = DrawHeaderGuaranteed;

        /* Chance */
        chanceToGetList = serializedObject.FindProperty("OneItemFromList");
        reorderableChance = new ReorderableList(serializedObject, chanceToGetList, true, true, true, true);
        // Functions
        reorderableChance.drawElementCallback += DrawChanceListItems;
        reorderableChance.drawHeaderCallback += DrawHeaderChance;

        ld = target as LootTable;
    }



    void DrawHeaderGuaranteed(Rect rect) { EditorGUI.LabelField(rect, "Guaranteed Loot Table"); }
    void DrawHeaderChance(Rect rect) { EditorGUI.LabelField(rect, "Chance To Get Loot Table"); }

    void DrawGuaranteedListItems(Rect rect, int index, bool isActive, bool isFocused) {
        LootTable loot = (LootTable)target;
        reorderableGuaranteed.elementHeight = 42;

        SerializedProperty element = reorderableGuaranteed.serializedProperty.GetArrayElementAtIndex(index);


        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, rect.height), element, GUIContent.none);

        //LootTable loot = (LootTable)target;
    }

    void DrawChanceListItems(Rect rect, int index, bool isActive, bool isFocused) {
        reorderableChance.elementHeight = 42;

        SerializedProperty element = reorderableChance.serializedProperty.GetArrayElementAtIndex(index);
        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, rect.height), element, GUIContent.none);
    }

    public override void OnInspectorGUI() {

        EditorUtility.SetDirty(target);
        LootTable loot = (LootTable)target;
        EditorGUILayout.BeginVertical("box");

        EditorGUI.indentLevel = 0;

        // Loot Name
        GUIStyle myStyle = new GUIStyle();
        myStyle.normal.textColor = GUI.color;
        myStyle.alignment = TextAnchor.UpperCenter;
        myStyle.fontStyle = FontStyle.Bold;
        int _ti = myStyle.fontSize;

        EditorGUILayout.LabelField($"Loot Table", myStyle);


        myStyle.fontStyle = FontStyle.Italic;
        myStyle.fontSize = 20;

        EditorGUILayout.LabelField($"''{loot.name}''", myStyle);

        EditorGUILayout.Space(10);

        EditorGUI.BeginChangeCheck();


        /* Fixed by D9Construct */
        serializedObject.Update();
        ValidateGuaranteedList(loot);
        reorderableGuaranteed.DoLayoutList();
        ValidateOneItemFromList(loot);
        reorderableChance.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

        if (EditorGUI.EndChangeCheck()) {
            for (int index = 0; index < loot.OneItemFromList.Length; index++) {
                SerializedProperty OIFElement = reorderableChance.serializedProperty.GetArrayElementAtIndex(index);
                loot.OneItemFromList[index].Weight = OIFElement.FindPropertyRelative("Weight").floatValue;
                loot.OneItemFromList[index].Drop = (GameObject)OIFElement.FindPropertyRelative("Drop").objectReferenceValue;
                loot.OneItemFromList[index].MinCountItem = OIFElement.FindPropertyRelative("MinCountItem").intValue;
                loot.OneItemFromList[index].MaxCountItem = OIFElement.FindPropertyRelative("MaxCountItem").intValue;
            }
            for (int index = 0; index < loot.GuaranteedLootTable.Length; index++) {
                SerializedProperty GuaranteedElement = reorderableGuaranteed.serializedProperty.GetArrayElementAtIndex(index);
                loot.GuaranteedLootTable[index].Weight = GuaranteedElement.FindPropertyRelative("Weight").floatValue;
                loot.GuaranteedLootTable[index].Drop = (GameObject)GuaranteedElement.FindPropertyRelative("Drop").objectReferenceValue;
                loot.GuaranteedLootTable[index].MinCountItem = GuaranteedElement.FindPropertyRelative("MinCountItem").intValue;
                loot.GuaranteedLootTable[index].MaxCountItem = GuaranteedElement.FindPropertyRelative("MaxCountItem").intValue;
            }
        }
        /* Fixed by D9Construct */

        // Nothing Weight
        loot.WeightToNoDrop = EditorGUILayout.FloatField("No Drop Weight", loot.WeightToNoDrop);

        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();

        Rect r = EditorGUILayout.BeginVertical("box");
        myStyle.fontStyle = FontStyle.Bold;
        myStyle.fontSize = 20;

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField($"Drop Chance", myStyle);

        float totalWeight = loot.WeightToNoDrop;
        float guaranteedHeight = 0;

        if (loot.OneItemFromList != null) {
            for (int j = 0; j < loot.OneItemFromList.Length; j++) {
                totalWeight += loot.OneItemFromList[j].Weight;
            }
        }


        var _oldColor = GUI.backgroundColor;

        if (0 < loot.GuaranteedLootTable.Length) { guaranteedHeight += 10; }

        /* Guaranteed */
        GUI.backgroundColor = Color.green;
        for (int i = 0; i < loot.GuaranteedLootTable.Length; i++) {
            string _tmpString = "";
            guaranteedHeight += 25;
            if (loot.GuaranteedLootTable[i].Drop == null) { _tmpString = " --- No Drop Object --- "; } else { _tmpString = loot.GuaranteedLootTable[i].Drop.name; }
            EditorGUI.ProgressBar(new Rect(r.x + 5, r.y + 40 + (25 * i), r.width - 10, 20), 1, $"{_tmpString} [{loot.GuaranteedLootTable[i].MinCountItem}-{loot.GuaranteedLootTable[i].MaxCountItem}]   -   Guaranteed");
        }
        GUI.backgroundColor = _oldColor;

        /* Not Guaranteed */
        for (int i = 0; i < loot.OneItemFromList.Length; i++) {
            string _tmpString = "";
            if (loot.OneItemFromList[i].Drop == null) { _tmpString = "!!! No Drop Object Attackhment !!!"; } else { _tmpString = loot.OneItemFromList[i].Drop.name; }
            if (loot.OneItemFromList[i].Weight / totalWeight < 0) {
                GUI.backgroundColor = Color.red;
                EditorGUI.ProgressBar(new Rect(r.x + 5, r.y + 40 + (25 * i) + guaranteedHeight, r.width - 10, 20), 1, "Error");
            } else {
                EditorGUI.ProgressBar(new Rect(r.x + 5, r.y + 40 + (25 * i) + guaranteedHeight, r.width - 10, 20), loot.OneItemFromList[i].Weight / totalWeight, $"{_tmpString} [{loot.OneItemFromList[i].MinCountItem}-{loot.OneItemFromList[i].MaxCountItem}]   -   {(loot.OneItemFromList[i].Weight / totalWeight * 100).ToString("f2")}%");
            }
            GUI.backgroundColor = _oldColor;
        }

        GUI.backgroundColor = Color.gray;
        EditorGUI.ProgressBar(new Rect(r.x + 5, r.y + 40 + (25 * loot.OneItemFromList.Length + 10) + guaranteedHeight, r.width - 10, 20), loot.WeightToNoDrop / totalWeight, $"Nothing Additional   -   {(loot.WeightToNoDrop / totalWeight * 100).ToString("f2")}%");
        GUI.backgroundColor = _oldColor;

        EditorGUILayout.Space(25 * loot.OneItemFromList.Length + 45 + guaranteedHeight);

        EditorGUILayout.EndVertical();
    }

    void ValidateOneItemFromList(LootTable loot) {
        bool _countError = false;
        bool _prefabError = false;
        bool _weightError = false;

        for (int index = 0; index < loot.OneItemFromList.Length; index++) {
            if (loot.OneItemFromList[index].Drop == null) { _prefabError = true; }
            if (loot.OneItemFromList[index].MinCountItem <= 0) { _countError = true; }
            if (loot.OneItemFromList[index].MinCountItem > loot.OneItemFromList[index].MaxCountItem) { _countError = true; }
            if (loot.OneItemFromList[index].Weight < 0) { _weightError = true; }
        }
        if (_prefabError == true) { EditorGUILayout.HelpBox("One of the List Items does not have ''Item To Drop'' assigned, which will cause an error if it is drawn", MessageType.Error, true); }
        if (_countError == true) { EditorGUILayout.HelpBox("One of the List Items has an incorrect number of items, which will result in items not appearing when drawn", MessageType.Warning, true); }
        if (_weightError == true) { EditorGUILayout.HelpBox("One of the List Items has an incorrect Weight, this will cause erroneous data readings or the whole system will crash", MessageType.Error, true); }
    }
    void ValidateGuaranteedList(LootTable loot) {
        bool _countError = false;
        bool _prefabError = false;
        bool _weightError = false;

        for (int index = 0; index < loot.GuaranteedLootTable.Length; index++) {
            if (loot.GuaranteedLootTable[index].Drop == null) { _prefabError = true; }
            if (loot.GuaranteedLootTable[index].MinCountItem <= 0) { _countError = true; }
            if (loot.GuaranteedLootTable[index].MinCountItem > loot.GuaranteedLootTable[index].MaxCountItem) { _countError = true; }
            if (loot.GuaranteedLootTable[index].Weight < 0) { _weightError = true; }
        }
        if (_prefabError == true) { EditorGUILayout.HelpBox("One of the List Items does not have ''Item To Drop'' assigned, which will cause an error if it is drawn", MessageType.Error, true); }
        if (_countError == true) { EditorGUILayout.HelpBox("One of the List Items has an incorrect number of items, which will result in items not appearing when drawn", MessageType.Warning, true); }
        if (_weightError == true) { EditorGUILayout.HelpBox("One of the List Items has an incorrect Weight, this will cause erroneous data readings or the whole system will crash", MessageType.Error, true); }
    }
}

/* --------------------- */
// Custom Property Draw
/* --------------------- */

[CustomPropertyDrawer(typeof(DropItem))]
public class DropChanceItemDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var _oldColor = GUI.backgroundColor;
        EditorGUI.BeginProperty(position, label, property);
        //GUI.backgroundColor = Color.red;

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var weightRectLabel = new Rect(position.x, position.y, 55, 18);
        var weightRect = new Rect(position.x, position.y + 20, 55, 18);

        EditorGUI.LabelField(weightRectLabel, "Weight");
        if (property.FindPropertyRelative("Weight").floatValue < 0) { GUI.backgroundColor = Color.red; }
        EditorGUI.PropertyField(weightRect, property.FindPropertyRelative("Weight"), GUIContent.none);
        GUI.backgroundColor = _oldColor;

        var ItemRectLabel = new Rect(position.x + 60, position.y, position.width - 60, 18);
        var ItemRect = new Rect(position.x + 60, position.y + 20, position.width - 60 - 75, 18);

        EditorGUI.LabelField(ItemRectLabel, "Item To Drop");
        if (property.FindPropertyRelative("Drop").objectReferenceValue == null) { GUI.backgroundColor = Color.red; }
        EditorGUI.PropertyField(ItemRect, property.FindPropertyRelative("Drop"), GUIContent.none);
        GUI.backgroundColor = _oldColor;

        var MinMaxRectLabel = new Rect(position.x + position.width - 70, position.y, 70, 18);

        var MinRect = new Rect(position.x + position.width - 70, position.y + 20, 30, 18);
        var MinMaxRect = new Rect(position.x + position.width - 39, position.y + 20, 9, 18);
        var MaxRect = new Rect(position.x + position.width - 30, position.y + 20, 30, 18);

        if (property.FindPropertyRelative("MinCountItem").intValue < 0) { GUI.backgroundColor = Color.red; }
        if (property.FindPropertyRelative("MaxCountItem").intValue < property.FindPropertyRelative("MinCountItem").intValue) { GUI.backgroundColor = Color.red; }

        EditorGUI.LabelField(MinMaxRectLabel, "Min  -  Max");
        EditorGUI.PropertyField(MinRect, property.FindPropertyRelative("MinCountItem"), GUIContent.none);
        EditorGUI.LabelField(MinMaxRect, "-");
        EditorGUI.PropertyField(MaxRect, property.FindPropertyRelative("MaxCountItem"), GUIContent.none);
        GUI.backgroundColor = _oldColor;

        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return 40;
    }
}

#endif
