using Unity.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems
{
    [CustomPropertyDrawer(typeof(Shape))]
    public class ShapePropertyDrawer : PropertyDrawer
    {
        private SerializedObject @object;
        private IShapeLoader loader;
        private VisualElement container;
        private VisualElement grid;
        private Shape shape;
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            container = new VisualElement();

            @object = property.serializedObject;
            loader = @object.targetObject as IShapeLoader;

            shape = loader.GetShape();
            
            if(shape == null || shape.ToString().Equals(ShapeCypher.Null))
            {
                var defaultShape = Shape.DefaultShape();
                loader.SetShape(defaultShape);
                shape = defaultShape;
            }
            
            shape.OnShapeChange += OnShapeChanged;
            
            GenerateGUI();
            
            return container;
        }

        VisualElement GenerateGUI()
        {
            var positions = Position();
            var dimensions = Dimension();
            Grid();
            var operations = ShapeOperations();
            var codeUtils = CodeUtils();
            
            container.Add(positions);
            container.Add(dimensions);
            container.Add(operations);
            container.Add(codeUtils);
            container.Add(grid);

            return container;
        }
        
        private void OnShapeChanged()
        {
            loader?.SetShape(shape);
            Grid();
        }
        
        private void OnDetailsChanged()
        {
            loader?.SetShape(shape);
            @object.ApplyModifiedProperties();
            EditorUtility.SetDirty(@object.targetObject);
        }

        VisualElement Position()
        {
            var container = new VisualElement();
            
            var x = BindableProperty<int>.Bind(() => shape.X, (value) => shape.Move(value, shape.Y, false));
            var y = BindableProperty<int>.Bind(() => shape.Y, (value) => shape.Move(shape.X, value, false));

            var X = new IntegerField("X");
            X.dataSource = x;
            X.SetBinding(nameof(IntegerField.value), new DataBinding
            {
                dataSourcePath = new PropertyPath(nameof(BindableProperty<int>.Value)),
                bindingMode = BindingMode.TwoWay,
            });
            
            var Y = new IntegerField("Y");
            Y.dataSource = y;
            Y.SetBinding(nameof(IntegerField.value), new DataBinding
            {
                dataSourcePath = new PropertyPath(nameof(BindableProperty<int>.Value)),
                bindingMode = BindingMode.TwoWay,
            });
            
            container.Add(X);
            container.Add(Y);
            
            return container;
        }
        
        VisualElement Dimension()
        {
            var container = new VisualElement();
            
            var width = BindableProperty<int>.Bind(() => shape.Width, (value) => shape.Scale(value, shape.Height));
            var height = BindableProperty<int>.Bind(() => shape.Height, (value) => shape.Scale(shape.Width, value));

            var Width = new IntegerField("Width");
            Width.dataSource = width;
            Width.SetBinding(nameof(IntegerField.value), new DataBinding
            {
                dataSourcePath = new PropertyPath(nameof(BindableProperty<int>.Value)),
                bindingMode = BindingMode.TwoWay,
            });
            
            var Height = new IntegerField("Height");
            Height.dataSource = height;
            Height.SetBinding(nameof(IntegerField.value), new DataBinding
            {
                dataSourcePath = new PropertyPath(nameof(BindableProperty<int>.Value)),
                bindingMode = BindingMode.TwoWay,
            });
            
            container.Add(Width);
            container.Add(Height);
            
            return container;
        }

        VisualElement Grid()
        {
            if (grid == null) grid = new VisualElement();
            else grid.Clear();
            
            var details = BindablePropertyArray<bool>.Bind(index => shape[index], (index, value) =>
            {
                shape[index] = value;
                OnDetailsChanged();
            });
            
            for(int y = 0; y < shape.Height; y++)
            {
                var row = new VisualElement();
                row.style.flexDirection = FlexDirection.Row;
                
                for (int x = 0; x < shape.Width; x++)
                {
                    var index = y * shape.Width + x;

                    var prop = details.GetProperty(index);
                    
                    var cell = new Toggle();
                    cell.dataSource = prop;
                    cell.SetBinding(nameof(Toggle.value), new DataBinding
                    {
                        dataSourcePath = new PropertyPath(nameof(BindableProperty<bool>.Value)),
                        bindingMode = BindingMode.TwoWay,
                    });
                    
                    row.Add(cell);
                }
                
                grid.Add(row);
            }

            return grid;
        }

        VisualElement ShapeOperations()
        {
            var container = new VisualElement();

            var rotation = new VisualElement();
            rotation.style.flexDirection = FlexDirection.Row;
            
            var leftRotation = new Button(() => shape.Rotate(1, false));
            leftRotation.text = "Rotate Left";
            rotation.Add(leftRotation);
            
            var rightRotation = new Button(() => shape.Rotate(1));
            rightRotation.text = "Rotate Right";
            rotation.Add(rightRotation);
            
            var flip = new VisualElement();
            flip.style.flexDirection = FlexDirection.Row;
            
            var horizontalFlip = new Button(() => shape.HorizontalFlip());
            horizontalFlip.text = "Horizontal Flip";
            flip.Add(horizontalFlip);
            
            var verticalFlip = new Button(() => shape.VerticalFlip());
            verticalFlip.text = "Vertical Flip";
            flip.Add(verticalFlip);
            
            container.Add(rotation);
            container.Add(flip);
            
            return container;
        }

        VisualElement CodeUtils()
        {
            var container = new VisualElement();
            container.style.flexDirection = FlexDirection.Row;
            
            var copy = new Button(() => EditorGUIUtility.systemCopyBuffer = shape.ToString());
            copy.text = "Copy Shape";

            var paste = new Button(() => PasteShape());
            paste.text = "Paste Shape";
            
            container.Add(copy);
            container.Add(paste);
            
            return container;
        }

        private void PasteShape()
        {
            var newShape = EditorGUIUtility.systemCopyBuffer.ToShape();
            
            if (newShape == null || newShape.ToString().Equals(ShapeCypher.Null)) return;
            
            shape.OnShapeChange -= OnShapeChanged;
            newShape.OnShapeChange += OnShapeChanged;
            
            shape = newShape;
            loader.SetShape(newShape);
            
            container.Clear();
            GenerateGUI();
        }
    }
}