using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX;
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
    public class Scene
    {
        private readonly SceneOptions _sceneOptions;
        private readonly RenderForm _form;
        private SwapChain _swapChain;
        private Device _device;
        private Texture2D _backBuffer;
        private RenderTarget _renderTarget2D;
        private RenderTargetView _backBufferView;
        private byte[] _memory;
        private Bitmap _bitmap;
        private FormWindowState _currentFormWindowState;
        private readonly DemoTime _clock = new DemoTime();
        private float _frameAccumulator;
        private int _frameCount;

        public RenderForm Form => _form;

        /// <summary>
        ///   Gets the number of seconds passed since the last frame.
        /// </summary>
        public float FrameDelta { get; private set; }

        /// <summary>
        ///   Gets the number of seconds passed since the last frame.
        /// </summary>
        public float FramePerSecond { get; private set; }



        public SharpDX.Direct2D1.Factory Factory2D { get; private set; }

        public Scene(SceneOptions sceneOptions)
        {
            _sceneOptions = sceneOptions;
            _form = new RenderForm(sceneOptions.Title);
            _form.Width = _sceneOptions.Width;
            _form.Height = _sceneOptions.Height;
            Initialize();

        }

        /// <summary>
        /// Runs the demo.
        /// </summary>
        public void Run()
        {
            bool formIsResizing = false;
            bool isFormClosed = false;
            // bool formIsResizing = false;

            _form.MouseClick += HandleMouseClick;
            _form.KeyDown += HandleKeyDown;
            _form.KeyUp += HandleKeyUp;
            _form.Resize += (o, args) =>
            {
                if (_form.WindowState != _currentFormWindowState)
                {
                    HandleResize(o, args);
                }

                _currentFormWindowState = _form.WindowState;
            };

            _form.ResizeBegin += (o, args) => { formIsResizing = true; };
            _form.ResizeEnd += (o, args) =>
            {
                formIsResizing = false;
                HandleResize(o, args);
            };
            _form.Closed += (o, args) => { isFormClosed = true; };


            LoadContent();

            _clock.Start();



            RenderLoop.Run(_form, () =>
            {
                if (isFormClosed)
                {
                    return;
                }

                OnUpdate();
                if (!formIsResizing)
                    Render();

            });
        }

        private void OnUpdate()
        {
            FrameDelta = (float) _clock.Update();
            // Update(clock);
        }

        public void Render()
        {
            _frameAccumulator += FrameDelta;
            ++_frameCount;
            if (_frameAccumulator >= 1.0f)
            {
                FramePerSecond = _frameCount / _frameAccumulator;

                _form.Text = _sceneOptions.Title + " - FPS: " + FramePerSecond;
                _frameAccumulator = 0.0f;
                _frameCount = 0;
            }

            BeginDraw();
            DrawScene();
            Draw(_clock);
            EndDraw();
        }

        public virtual void DrawScene()
        {


        }

        public void Draw(DemoTime demoTime)
        {
            _bitmap.CopyFromMemory(_memory, _sceneOptions.Width * 4);
            _renderTarget2D.DrawBitmap(_bitmap, 1f,
                BitmapInterpolationMode.Linear);
        }

        private void HandleKeyUp(object sender, KeyEventArgs e)
        {

        }




        public void SetPixel(int x, int y, Color4 color, int alfa = 255)
        {
            int i = _sceneOptions.Width * (y << 2) + (x << 2);
            _memory[i] = (byte) color.Red;
            _memory[i + 1] = (byte) color.Green;
            _memory[i + 2] = (byte) color.Blue;
            _memory[i + 3] = (byte) color.Alpha;
        }

        public void SetPixel(int x, int y, byte red, byte green, byte blue, byte alfa = 255)
        {
            int i = _sceneOptions.Width * (y << 2) + (x << 2);
            _memory[i] = red;
            _memory[i + 1] = green;
            _memory[i + 2] = blue;
            _memory[i + 3] = alfa;
        }


        private void LoadContent()
        {
        }

        public void BeginDraw()
        {
            _renderTarget2D.BeginDraw();
        }

        public void EndDraw()
        {
            _renderTarget2D.EndDraw();
            _swapChain.Present(0, PresentFlags.None);
        }


        /// <summary>
        ///   Handles a mouse click event.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The <see cref = "System.Windows.Forms.MouseEventArgs" /> instance containing the event data.</param>
        private void HandleMouseClick(object sender, MouseEventArgs e)
        {

        }
        private void HandleKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void HandleResize(object sender, EventArgs e)
        {
            if (_form.WindowState == FormWindowState.Minimized)
            {
                return;
            }

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
                OutputHandle = _form.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            // Create Device and SwapChain
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, new[] { FeatureLevel.Level_10_0 }, desc, out _device, out _swapChain);

            // Ignore all windows events
            var factory = _swapChain.GetParent<SharpDX.DXGI.Factory>();
            factory.MakeWindowAssociation(_form.Handle, WindowAssociationFlags.IgnoreAll);

            // New RenderTargetView from the backbuffer
            _backBuffer = SharpDX.Direct3D11.Resource.FromSwapChain<Texture2D>(_swapChain, 0);
            _backBufferView = new RenderTargetView(_device, _backBuffer);

            Factory2D = new SharpDX.Direct2D1.Factory();
            using (var surface = _backBuffer.QueryInterface<Surface>())
            {
                _renderTarget2D = new RenderTarget(Factory2D, surface,
                    new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));
            }
            //  var  FactoryDWrite = new SharpDX.DirectWrite.Factory();
            // var  SceneColorBrush = new SolidColorBrush(_renderTarget2D, Color.White);


            _memory = new byte[_sceneOptions.Width * _sceneOptions.Height * 4];
            _bitmap = new Bitmap(_renderTarget2D,
                new Size2(_sceneOptions.Width, _sceneOptions.Height),
                new BitmapProperties(_renderTarget2D.PixelFormat));

        }

    }
}
