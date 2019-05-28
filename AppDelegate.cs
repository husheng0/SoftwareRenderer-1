﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace SoftwareRenderer
{
    class AppDelegate
    {
        private Form _form = null;
        private Camera _camera = new Camera();
        private float _cameraPosX = 3;
        private float _fps = 30.0f;
        private Font _font = new Font("Courier New", 12);
        private List<Mesh> _meshes = new List<Mesh>();
        private float _meshRotationUp = 0;
        
        public AppDelegate()
        {
            _form = new Form();
            _form.Size = new Size(Screen.WIDTH, Screen.HEIGHT);
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.Text = "SoftwareRenderer by XieHeng";
            _form.KeyDown += OnFormKeyDown;

            _camera.OnPostRender += OnCameraPostRender;
        }

        public void Run()
        {
            _camera.position = new Vector(_cameraPosX, 0, 0);
            _camera.fov = 90;
            _camera.LookAt(new Vector(-1.0f, 0.0f, 1.0f), Vector.up);

            IModel model = new Pyramid();
            _meshes.Add(model.mesh);

            _form.Show();
            Stopwatch stopwatch = new Stopwatch();

            while (!_form.IsDisposed)
            {
                stopwatch.Start();

                using (var grap = _form.CreateGraphics())
                {
                    _camera.Render(grap, _meshes);
                }
                Application.DoEvents();

                stopwatch.Stop();
                _fps = 1000.0f / stopwatch.Elapsed.Milliseconds;
                stopwatch.Reset();
            }
        }

        void OnCameraPostRender(GraphicsDevice grap)
        {
            //显示帧率
            grap.DrawString(string.Format("FPS: {0}", _fps), _font, Brushes.Black);
        }

        void OnFormKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)//左转
            {
                _meshRotationUp -= 0.1f;
                foreach (Mesh mesh in _meshes)
                {
                    mesh.rotation = new Vector(0, 0, _meshRotationUp);
                }
            }
            else if (e.KeyCode == Keys.Right)//右转
            {
                _meshRotationUp += 0.1f;
                foreach (Mesh mesh in _meshes)
                {
                    mesh.rotation = new Vector(0, 0, _meshRotationUp);
                }
            }
            else if (e.KeyCode == Keys.Up)//拉近
            {
                _cameraPosX -= 0.1f;
                _camera.position = new Vector(_cameraPosX, 0, 0);
            }
            else if (e.KeyCode == Keys.Down)//推远
            {
                _cameraPosX += 0.1f;
                _camera.position = new Vector(_cameraPosX, 0, 0);
            }
        }
    }
}