using System;
using Engine;

namespace RandomPixelColor
{
    public class MyScene:Scene
    {
        private readonly Random _random;
        public MyScene(SceneOptions sceneOptions) : base(sceneOptions)
        {
            _random = new Random();

        }

        public override void DrawScene()
        {
            for (int x = 0;x < 800;x++)
            {
                for (int j = 0;j < 600;j++)
                {
                    var red = (byte) _random.Next(0, 255);
                    var green = (byte) _random.Next(0, 255);
                    var blue = (byte) _random.Next(0, 255);

                    SetPixel(x, j, red, green, blue);

                }
            }
        }
    }
}
