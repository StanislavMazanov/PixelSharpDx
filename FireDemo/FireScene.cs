using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FireDemo
{
    public class FireScene:Scene
    {
        private readonly Random _random;
        private byte[] _fireBuffer;
        private int WidthFire = 600;
        private int HeightFire = 200;


        public FireScene(SceneOptions sceneOptions) : base(sceneOptions)
        {
            _random = new Random();
            _fireBuffer = new byte[WidthFire * HeightFire];

            // var col = 0;

            for (int i = 1;i <= WidthFire;i++)
            {

                _fireBuffer[i * HeightFire - 1] = 255;
            }
        }

        public override void DrawScene()
        {
            for (int x = 0;x < WidthFire;x++)
            {
                for (int y = 0;y < HeightFire - 1;y++)
                {
                    var dst = x + _random.Next(0, 3);

                    if (dst > WidthFire - 1)
                    {
                        dst = WidthFire - 1;
                    }

                    var delta = _random.Next(1, 10);

                    var colFire = HeightFire * dst;

                    int color = _fireBuffer[y + 1 + colFire] - delta;
                    if (color < 0)
                    {
                        color = 0;
                    }

                    // _fireBuffer[y + HeightFire * x+ dst] = (byte) (color);
                    _fireBuffer[y + colFire] = (byte) (color);

                    SetPixel(x, y, (byte) color, 0, 0);
                }
            }

            //for (int i = 1;i <= WidthFire;i++)
            //{
            //    _fireBuffer[i * HeightFire - 1] = (byte) _random.Next(255, 255);
            //}


           Thread.Sleep(100);
        }

        //private void SpreadFire(int src)
        //{
        //    _fireBuffer[src - FIRE_WIDTH] = _fireBuffer[src] - 1;
        //}

    }
}
