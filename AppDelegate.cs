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
        private float _cameraPosForward = -3;
        private float _fps = 30.0f;
        private Font _font = new Font("Courier New", 12);
        private List<Mesh> _meshes = new List<Mesh>();
        private float _meshRotationUp = 0;
        
        public AppDelegate()
        {
            _form = new Form();
            _form.Size = new Size(Screen.WIDTH, Screen.HEIGHT);
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.KeyDown += OnFormKeyDown;

            _camera.OnPostRender += OnCameraPostRender;

            UpdateWindowTitle();
        }

        public void Run()
        {
            _camera.position = new Vector4(0, 0, _cameraPosForward);
            _camera.fov = 90;
            _camera.LookAt(new Vector4(0.0f, -0.5f, 0.2f), Vector4.up);

            Model model = new Pyramid();
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

        void OnCameraPostRender(Canvas canvas)
        {
            //显示帧率
            canvas.DrawString(string.Format("FPS: {0:F1}", _fps),
                              _font,
                              Brushes.Black);
        }

        void OnFormKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)//左转
            {
                _meshRotationUp += 0.05f;
                foreach (Mesh mesh in _meshes)
                {
                    mesh.rotation = new Vector4(0, _meshRotationUp, 0);
                }
            }
            else if (e.KeyCode == Keys.Right)//右转
            {
                _meshRotationUp -= 0.05f;
                foreach (Mesh mesh in _meshes)
                {
                    mesh.rotation = new Vector4(0, _meshRotationUp, 0);
                }
            }
            else if (e.KeyCode == Keys.Up)//拉近
            {
                _cameraPosForward += 0.1f;
                _camera.position = new Vector4(0, 0, _cameraPosForward);
            }
            else if (e.KeyCode == Keys.Down)//推远
            {
                _cameraPosForward -= 0.1f;
                _camera.position = new Vector4(0, 0, _cameraPosForward);
            }

            if (e.KeyCode == Keys.D1)//切换到线框模式
            {
                _camera.renderType = Camera.RenderType.Wireframe;
            }
            else if (e.KeyCode == Keys.D2)//切换到颜色模式
            {
                _camera.renderType = Camera.RenderType.Color;
            }

            if (e.KeyCode == Keys.D0)//不剔除
            {
                _camera.cullType = Camera.CullType.None;
            }
            else if (e.KeyCode == Keys.D8)//剔除背面
            {
                _camera.cullType = Camera.CullType.Back;
            }
            else if (e.KeyCode == Keys.D9)//剔除正面
            {
                _camera.cullType = Camera.CullType.Front;
            }

            UpdateWindowTitle();
        }

        private void UpdateWindowTitle()
        {
            string title = "SoftwareRenderer | ";

            if (_camera.cullType == Camera.CullType.None)
            {
                title += "不剔除 | ";
            }
            else if (_camera.cullType == Camera.CullType.Front)
            {
                title += "剔除正面 | ";
            }
            else if (_camera.cullType == Camera.CullType.Back)
            {
                title += "剔除背面 | ";
            }

            if (_camera.renderType == Camera.RenderType.Wireframe)
            {
                title += "线框渲染";
            }
            else if (_camera.renderType == Camera.RenderType.Color)
            {
                title += "颜色渲染";
            }

            _form.Text = title;
        }
    }
}
