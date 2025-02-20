using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine.UIElements;

namespace Systems.Inventory 
{
    [CustomEditor(typeof(ItemDetails))]
    public class ItemDetailsEditor : Editor
    {
        
        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();
            
            ItemDetails itemDetails = target as ItemDetails;
            
            var name = new PropertyField(serializedObject.FindProperty("Name"));
            var maxStack = new PropertyField(serializedObject.FindProperty("MaxStack"));
            var iconI = new PropertyField(serializedObject.FindProperty("Icon"));
            var icona = new PropertyField(serializedObject.FindProperty("IconRotation"));
            var icona2 = new PropertyField(serializedObject.FindProperty("IconRotation180"));
            var icona3 = new PropertyField(serializedObject.FindProperty("IconRotation270"));
            var iconb = new PropertyField(serializedObject.FindProperty("IconFlip"));
            var iconab = new PropertyField(serializedObject.FindProperty("IconRotationFlip"));
            var icona2b = new PropertyField(serializedObject.FindProperty("IconRotation180Flip"));
            var icona3b = new PropertyField(serializedObject.FindProperty("IconRotation270Flip"));
            var description = new PropertyField(serializedObject.FindProperty("Description"));
            var shape = new PropertyField(serializedObject.FindProperty("Shape"));
            var id = new PropertyField(serializedObject.FindProperty("Id"));
            
            var button = new Button(() => itemDetails.AssignNewId());
            button.text = "Assign New Guid";
            
            container.Add(name);
            container.Add(maxStack);
            
            container.Add(iconI);
            container.Add(icona);
            container.Add(icona2);
            container.Add(icona3);
            container.Add(iconb);
            container.Add(iconab);
            container.Add(icona2b);
            container.Add(icona3b);
            
            container.Add(description);
            container.Add(shape);
            container.Add(id);
            container.Add(button);
            
            return container;
        }
    }
}