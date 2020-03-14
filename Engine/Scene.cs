using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Device = SharpDX.Direct3D11.Device;
using FeatureLevel = SharpDX.Direct3D.FeatureLevel;

namespace Engine
{
  public  class Scene
    {
        private readonly SceneOptions _sceneOptions;
        private readonly RenderForm _window;
        private SwapChain _swapChain;
        private Device _device;
        private Texture2D _backBuffer;
        private RenderTarget _renderTarget2D;
        private RenderTargetView _backBufferView;

        public SharpDX.Direct2D1.Factory Factory2D { get; private set; }

        public Scene(SceneOptions sceneOptions)
        {
            _sceneOptions = sceneOptions;
            _window = new RenderForm("SharpDX Tutorial 0");
            Initialize();
            RenderLoop.Run(_window, () =>
            {
                _renderTarget2D.BeginDraw();

                // PrepareScene();


                //_renderTarget2D.DrawBitmap(_bitmap, 1f,
                //    BitmapInterpolationMode.Linear);
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
                    _renderTarget2D.EndDraw();
                } catch
                {
                    //   CreateResources();
                }
            });
        
    }

        private void Initialize()
        {
            var desc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription =
                    new ModeDescription(_sceneOptions.Width, _sceneOptions.Height,
                        new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = _window.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            // Create Device and SwapChain
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, new[] { FeatureLevel.Level_10_0 }, desc, out _device, out _swapChain);

            // Ignore all windows events
            var factory = _swapChain.GetParent<SharpDX.DXGI.Factory>();
            factory.MakeWindowAssociation(_window.Handle, WindowAssociationFlags.IgnoreAll);

            // New RenderTargetView from the backbuffer
           _backBuffer = SharpDX.Direct3D11.Resource.FromSwapChain<Texture2D>(_swapChain, 0);
           _backBufferView = new RenderTargetView(_device, _backBuffer);

            Factory2D = new SharpDX.Direct2D1.Factory();
            using (var surface = _backBuffer.QueryInterface<Surface>())
            {
                _renderTarget2D = new RenderTarget(Factory2D, surface,
                    new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));
            }
        }

    }
}
