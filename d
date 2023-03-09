warning: in the working copy of 'Assets/Scenes/SceneMain.unity', LF will be replaced by CRLF the next time Git touches it
[1mdiff --git a/Assets/Scenes/SceneMain.unity b/Assets/Scenes/SceneMain.unity[m
[1mindex 5cac184..a959a02 100644[m
[1m--- a/Assets/Scenes/SceneMain.unity[m
[1m+++ b/Assets/Scenes/SceneMain.unity[m
[36m@@ -174,7 +174,7 @@[m [mMonoBehaviour:[m
   m_Name: [m
   m_EditorClassIdentifier: [m
   m_Material: {fileID: 0}[m
[31m-  m_Color: {r: 1, g: 1, b: 1, a: 0}[m
[32m+[m[32m  m_Color: {r: 1, g: 1, b: 1, a: 0.46666667}[m
   m_RaycastTarget: 1[m
   m_RaycastPadding: {x: 0, y: 0, z: 0, w: 0}[m
   m_Maskable: 1[m
[36m@@ -649,7 +649,7 @@[m [mMonoBehaviour:[m
   m_Name: [m
   m_EditorClassIdentifier: [m
   m_Material: {fileID: 0}[m
[31m-  m_Color: {r: 1, g: 1, b: 1, a: 0}[m
[32m+[m[32m  m_Color: {r: 1, g: 1, b: 1, a: 0.46666667}[m
   m_RaycastTarget: 1[m
   m_RaycastPadding: {x: 0, y: 0, z: 0, w: 0}[m
   m_Maskable: 1[m
[36m@@ -1501,7 +1501,7 @@[m [mMonoBehaviour:[m
   m_Name: [m
   m_EditorClassIdentifier: [m
   m_Material: {fileID: 0}[m
[31m-  m_Color: {r: 1, g: 1, b: 1, a: 0}[m
[32m+[m[32m  m_Color: {r: 1, g: 1, b: 1, a: 0.46666667}[m
   m_RaycastTarget: 1[m
   m_RaycastPadding: {x: 0, y: 0, z: 0, w: 0}[m
   m_Maskable: 1[m
[36m@@ -2066,7 +2066,7 @@[m [mMonoBehaviour:[m
   m_Name: [m
   m_EditorClassIdentifier: [m
   m_Material: {fileID: 0}[m
[31m-  m_Color: {r: 1, g: 1, b: 1, a: 0}[m
[32m+[m[32m  m_Color: {r: 1, g: 1, b: 1, a: 0.46666667}[m
   m_RaycastTarget: 1[m
   m_RaycastPadding: {x: 0, y: 0, z: 0, w: 0}[m
   m_Maskable: 1[m
[1mdiff --git a/Assets/Scripts/InGame/ChatObject.cs b/Assets/Scripts/InGame/ChatObject.cs[m
[1mindex d4ef376..40a5f32 100644[m
[1m--- a/Assets/Scripts/InGame/ChatObject.cs[m
[1m+++ b/Assets/Scripts/InGame/ChatObject.cs[m
[36m@@ -1,4 +1,5 @@[m
 using Cysharp.Threading.Tasks;[m
[32m+[m[32musing PotionsPlease.Util.Managers;[m
 using System;[m
 using UnityEditor.VersionControl;[m
 using UnityEngine;[m
[1mdiff --git a/Assets/Scripts/InGame/DropAreaObject.cs b/Assets/Scripts/InGame/DropAreaObject.cs[m
[1mindex 5e3cc85..38959b4 100644[m
[1m--- a/Assets/Scripts/InGame/DropAreaObject.cs[m
[1m+++ b/Assets/Scripts/InGame/DropAreaObject.cs[m
[36m@@ -1,3 +1,4 @@[m
[32m+[m[32musing PotionsPlease.Models;[m
 using PotionsPlease.Util.Managers;[m
 using Shapes;[m
 using TMPro;[m
[36m@@ -36,6 +37,8 @@[m [mnamespace PotionsPlease.InGame[m
         private void Start()[m
         {[m
             UpdateAlphas();[m
[32m+[m
[32m+[m[32m            LevelModel[] levels = (LevelModel[])Resources.LoadAll("Levels");[m
         }[m
 [m
         private void Update()[m
[1mdiff --git a/Assets/Scripts/Models/PotionModel.cs b/Assets/Scripts/Models/PotionModel.cs[m
[1mindex 6251b58..a36a060 100644[m
[1m--- a/Assets/Scripts/Models/PotionModel.cs[m
[1m+++ b/Assets/Scripts/Models/PotionModel.cs[m
[36m@@ -13,16 +13,12 @@[m [mnamespace PotionsPlease.Models[m
         {[m
             [field: SerializeField] public ItemModel ItemModel { get; private set; }[m
             [field: SerializeField, Range(0, 1)] public float Effectivity { get; private set; } [m
[31m-[m
         }[m
 [m
         public string Name => _name;[m
 [m
         [SerializeField] private string _name;[m
         [SerializeField] private Ingredient[] _recipe;[m
[31m-        [SerializeField] private Ingredient _ingredient;[m
[31m-        [SerializeField] private Object _object;[m
[31m-[m
 [m
         public ItemModel[] GetRecipeItemModels() => _recipe.Select(e => e.ItemModel).ToArray();[m
     }[m
[1mdiff --git a/Assets/Scripts/Util/Managers/OrderManager.cs b/Assets/Scripts/Util/Managers/OrderManager.cs[m
[1mindex 49b668b..2d8bea9 100644[m
[1m--- a/Assets/Scripts/Util/Managers/OrderManager.cs[m
[1m+++ b/Assets/Scripts/Util/Managers/OrderManager.cs[m
[36m@@ -3,6 +3,7 @@[m [musing PotionsPlease.Models;[m
 using System.Collections.Generic;[m
 using System.Linq;[m
 using UnityEngine;[m
[32m+[m[32musing UnityEngine.SceneManagement;[m
 [m
 namespace PotionsPlease.Util.Managers[m
 {[m
