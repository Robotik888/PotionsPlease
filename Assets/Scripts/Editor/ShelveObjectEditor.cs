using PotionsPlease.InGame;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(ShelveObject))]
public class ShelveObjectEditor : EditorBase<ShelveObject>
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUI.changed)
		{
			for (int i = 0; i < targets.Length; i++)
				UpdateShelve(targets[i]);
		}
	}


	private void UpdateShelve(ShelveObject target)
	{
		for (int i = 0; i < target.ItemObjects.Length; i++)
            if (target.ItemObjects[i] != null)
				DestroyImmediate(target.ItemObjects[i].gameObject);

		var positions = target.GetItemObjectPositions();
        var itemObjects = new ItemObject[target.Size];

        for (int i = 0; i < itemObjects.Length; i++)
		{
			var newItemObject = (ItemObject)PrefabUtility.InstantiatePrefab(target.ItemObjectPrefab, target.transform);

            newItemObject.Initialize(positions[i]);

			if (i != 0)
				newItemObject.gameObject.name += $" ({i})";
            
			itemObjects[i] = newItemObject;
		}

		target.ItemObjects = itemObjects;
    }
}
