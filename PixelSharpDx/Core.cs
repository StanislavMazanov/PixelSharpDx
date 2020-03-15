using Engine;

namespace RandomPixelColor
{
    public class Core
    {

        public Core()
        {
            var sceneOptions = new SceneOptions
            {
                Height = 600,
                Width = 800,
                Title = "Demoscena"
            };
            var scene = new MyScene(sceneOptions);
            scene.Run();
        }
    }
}

