using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;

namespace FireDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var sceneOptions = new SceneOptions
            {
                Height = 202,
                Width = 600,
                Title = "FireScene"
            };
            var scene = new FireScene(sceneOptions);
            scene.Run();
        }
    }
}
