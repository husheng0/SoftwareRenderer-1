﻿using System;
using System.Drawing;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    abstract class Rasterizer
    {
        private List<Fragment> _fragments = new List<Fragment>();

        public abstract void Do(Vertex a, Vertex b, Vertex c);

        public List<Fragment> fragments
        {
            get { return _fragments; }
        }

        protected float LerpZ(float s, float e, float x, float sz, float ez)
        {
            if (Mathf.Eq(e, s))
                return sz;

            return Mathf.Lerp(sz, ez, (x - s) / (e - s));
        }
    }
}