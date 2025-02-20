namespace Systems 
{
    public static class ShapeCypher 
    {
        public const string Null = "0,0,0,0,";

        public static string ToCypherString(this Shape shape)
        {
            int width = shape.Width;
            int height = shape.Height;
            int X = shape.X;
            int Y = shape.Y;
            string code = string.Empty;
            
            for (int y = 0; y < height; y++)
            for(int x = 0; x < width; x++)
                code += shape[x, y] ? '1' : '0';

            return X + "," + Y + "," + width + "," + height + "," + code;
        }

        public static Shape ToShape(this string code)
        {
            string[] @params = code.Split(',');

            if (@params.Length != 5) return Shape.DefaultShape();

            int width = int.Parse(@params[2]);
            int height = int.Parse(@params[3]);

            if (@params[4].Length != width * height) return Shape.DefaultShape();
            
            int X = int.Parse(@params[0]);
            int Y = int.Parse(@params[1]);

            bool[] list = new bool[width * height];
            
            for(int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                list[y * width + x] = @params[4][y * width + x] == '1';

            var box = new Box()
            {
                X = X,
                Y = Y,
                Width = width,
                Height = height,
            };
            
            return new Shape(box, list);
        }
    }
}