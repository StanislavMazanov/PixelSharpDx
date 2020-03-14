using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Engine;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using SharpDX.Windows;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using D2DFactory = SharpDX.Direct2D1.Factory;
using DWriteFactory = SharpDX.DirectWrite.Factory;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;

namespace PixelSharpDx
{



    public class Core
    {

        public Texture2D BackBuffer
        {
            get
            {
                return _backBuffer;
            }
        }

        public SharpDX.Direct2D1.Factory Factory2D { get; private set; }
        private static D2DFactory _d2dFactory;
        private static DWriteFactory _dwFactory;
        public RenderTarget RenderTarget2D { get; private set; }

        private byte[] _memory;
        readonly RenderForm _window;
        private const int _width = 320;
        private const int _height = 240;
        private Bitmap _bitmap;

        Texture2D _backBuffer;
        //Various brushes for our example
        private static SolidColorBrush backgroundBrush;
        private static SolidColorBrush defaultBrush;
        private static SolidColorBrush greenBrush;
        private static SolidColorBrush redBrush;
        private SwapChain _swapChain;
        SharpDX.Direct3D11.Device _device;
        RenderTargetView _backBufferView;

        /// <summary>
        /// Returns the device
        /// </summary>
        public SharpDX.Direct3D11.Device Device
        {
            get
            {
                return _device;
            }
        }



        public Core()
        {

            var sceneOptions = new SceneOptions
            {
                Height = 600,
                Width = 800,
                Title = "Demoscena"
            };
        //    var scene = new Scene(sceneOptions);

            //var color = 12;



            ////scene.SetPoint(0, 0, color);




             _window = new RenderForm("SharpDX Tutorial 0");
        }

        public void Run()
        {


            var bgcolor = new Color4(0.1f, 0.1f, 0.1f, 1.0f);


            //InitializeDirect2D();
            int width = _window.Width;
            int height = _window.Height;
            Initialize();

            _memory = new byte[_width * _height * 4];
            _bitmap = new Bitmap(RenderTarget2D,
                   new Size2(_width, _height),
                   new BitmapProperties(RenderTarget2D.PixelFormat));


           // var color4g = new Color4(0.1f, 0.1f, 0.1f, 1.0f);
            //var color4r = new Color4(0.5f, 0.5f, 0.5f, 1.0f);
            var color4r = Color4.White;
            for (int j = 0;j < 200;j++)
            {
                for (int i = 0;i < 200;i++)
                {
                    DrawPoint(i, j, color4r);
                    //DrawPoint(101 + j, 71 + i, color4g);
                //    DrawPoint(102 + j, 100 + i, color4r);
                    //DrawPoint(103 + j, 73 + i, color4g);
                    //DrawPoint(104 + j, 74 + i, color4r);
      
                }
            }
            int stride = _width * sizeof(int);
            _bitmap.CopyFromMemory(_memory, stride);


            RenderTarget2D.BeginDraw();
            ////_renderTarget.Clear(bgcolor);
            //_renderTarget.BeginDraw();
            //_renderTarget.DrawBitmap(_bitmap, 1f,
            //    BitmapInterpolationMode.Linear);


            RenderLoop.Run(_window, () =>
                        {
                            RenderTarget2D.BeginDraw();

                            PrepareScene();
                            
                            
                            RenderTarget2D.DrawBitmap(_bitmap, 1f,
                                BitmapInterpolationMode.Linear);
                            //Thread.Sleep(10);
















                            //_renderTarget.Clear(SharpDX.Color.Black);


                            //_swapChain.Present(0, PresentFlags.None);


                            //  _renderTarget.EndDraw();
                            // swapChain.Present(0, PresentFlags.None);




                            //_renderTarget.BeginDraw();
                            ////_renderTarget.Clear(bgcolor);

                            //_renderTarget.DrawBitmap(_bitmap, 1f,
                            // BitmapInterpolationMode.Linear);
                            //_renderTarget.d
                            //_renderTarget.FillRectangle(textRegionRect, redBrush);

                            //textLayout.Draw(textRenderer, offset.X, offset.Y);

                            try
                            {
                                _swapChain.Present(0, PresentFlags.None);
                                RenderTarget2D.EndDraw();
                            } catch
                            {
                                //   CreateResources();
                            }
                        });
        }

        private void PrepareScene()
        {
         var r = new Random();


            for (int j = 0;j < 200;j++)
            {
                for (int i = 0;i < 200;i++)
                {
                   // int d = r.Next();

                    var color4r = new Color4(r.Next(), r.Next(), r.Next(), 1000000);

                    DrawPoint(i, j, color4r);
                    //DrawPoint(101 + j, 71 + i, color4g);
                    //    DrawPoint(102 + j, 100 + i, color4r);
                    //DrawPoint(103 + j, 73 + i, color4g);
                    //DrawPoint(104 + j, 74 + i, color4r);

                }
            }
            int stride = _width * sizeof(int);
            _bitmap.CopyFromMemory(_memory, stride);

        }


        private void Initialize()
        {
            // SwapChain description
            var desc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription =
                    new ModeDescription(_width, _height,
                        new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = _window.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            // Create Device and SwapChain
            SharpDX.Direct3D11.Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, new[] { SharpDX.Direct3D.FeatureLevel.Level_10_0 }, desc, out _device, out _swapChain);

            // Ignore all windows events
            var factory = _swapChain.GetParent<SharpDX.DXGI.Factory>();
            factory.MakeWindowAssociation(_window.Handle, WindowAssociationFlags.IgnoreAll);

            // New RenderTargetView from the backbuffer
            _backBuffer = Texture2D.FromSwapChain<Texture2D>(_swapChain, 0);
            _backBufferView = new RenderTargetView(_device, _backBuffer);

            Factory2D = new SharpDX.Direct2D1.Factory();
            using (var surface = BackBuffer.QueryInterface<Surface>())
            {
                RenderTarget2D = new RenderTarget(Factory2D, surface,
                    new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));
            }

            //  var  FactoryDWrite = new SharpDX.DirectWrite.Factory();

            //var  SceneColorBrush = new SolidColorBrush(_renderTarget, Color.White);


        }


        public void DrawPoint(int x, int y, Color4 color)
        {
            //int i = Width * 4 * y + x * 4;
            int i = _width * (y << 2) + (x << 2);
            _memory[i] = (byte) color.Red;
            _memory[i + 1] = (byte) color.Green;
            _memory[i + 2] = (byte) color.Blue;
            _memory[i + 3] = (byte) color.Alpha;
        }
    }
}
