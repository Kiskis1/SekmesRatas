using System;
using System.Drawing;

namespace SekmesRatas
{
    internal class GameWheel
    {
        private int _finalSector;
        public string Result { get; set; }

        private Sector[] _wheel;

        private Point _center;
        private int _radius;
        private float _sectorAngle;
        private Rectangle _boundRect;
        private Point[] _trianglePoints;

        // drawing tools
        private Graphics _surface;
        private Pen _borderPen, _numberPen;
        private SolidBrush _triangleBrush;

        // wheel animation
        private const int TEXT_INTVL_SMALL = 35;
        private const int TEXT_INTVL_LARGE = 75;

        private const float SLOWDOWN_INTVL = 0.15F;

        private const int APPROX_INITIAL_ROTATE = 15;

        private float _rotateAngle;
        private int _initialRotations;
        private int _repaintCounter;
        private bool _highlight;

        public int State { get; set; }
        public const int STATE_NOT_STARTED = 0;
        public const int STATE_INITIAL_SPIN = 1;
        public const int STATE_SLOWING_DOWN = 2;
        public const int STATE_HIGHLIGHTING = 3;

        private Random r = new Random();

        public GameWheel()
        {
        }

        public GameWheel(Point center, int radius, int minValue, int maxValue, int interval)
        {
            var numSectors = (maxValue - minValue) / interval + 1;
            CommonConstructor(center, radius, numSectors);
            for (var i = 0; i < numSectors; i++)
            {
                var value = (minValue + interval * i).ToString();
                var brushColor = Color.FromArgb(r.Next(0, 256),
                    r.Next(0, 256), r.Next(0, 256));

                _wheel[i] = new Sector(i, value, _sectorAngle, brushColor);
            }
        }

        private void CommonConstructor(Point center, int radius, int numSectors)
        {
            Result = "";
            _center = center;
            _radius = radius;
            _sectorAngle = 360F / numSectors;
            _wheel = new Sector[numSectors];

            _boundRect = new Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2);

            var triLength = 25;     // length of each side of the triangle       
            var triVertex1 = new Point(center.X, center.Y - (radius + 3) + triLength);
            var triVertex2 = Sector.Polar(triVertex1, triLength, 60);
            var triVertex3 = Sector.Polar(triVertex1, triLength, 120);
            _trianglePoints = new[] { triVertex1, triVertex2, triVertex3 };

            _borderPen = new Pen(Color.White, 2);
            _numberPen = new Pen(Color.White, 3);
            _triangleBrush = new SolidBrush(Color.White);

            State = STATE_NOT_STARTED;
        }

        #region Animation

        // public spin function - returns resulting value after spin
        public string Spin(ref Random randGen)
        {
            var randomRotation = randGen.Next(360, 720); // rotate between 360 and 719 degrees before slowing down
            _initialRotations = randomRotation / APPROX_INITIAL_ROTATE;
            _rotateAngle = (float)randomRotation / _initialRotations;
            State = STATE_INITIAL_SPIN;

            return Simulate();
        }

        // calculate the answer by simulating the whole spin
        private string Simulate()
        {
            var simulation = new GameWheel
            {
                _sectorAngle = _sectorAngle,
                _rotateAngle = _rotateAngle,
                _initialRotations = _initialRotations,
                _repaintCounter = 0,
                _wheel = new Sector[_wheel.Length]
            };

            for (var i = 0; i < _wheel.Length; i++) simulation._wheel[i] = new Sector(i, _wheel[i].value, _sectorAngle, Color.Blue);

            simulation.State = STATE_INITIAL_SPIN;
            while (simulation.State != STATE_HIGHLIGHTING) simulation.Refresh(false);
            return simulation.Result;
        }

        public void Refresh(bool repaint)
        {
            switch (State)
            {
                case STATE_INITIAL_SPIN:
                    Rotate();
                    _repaintCounter++;
                    if (_repaintCounter == _initialRotations)
                    {
                        _repaintCounter = 0;
                        State = STATE_SLOWING_DOWN;
                    }
                    break;
                case STATE_SLOWING_DOWN:
                    _rotateAngle -= SLOWDOWN_INTVL;
                    if (_rotateAngle <= 0)
                    {
                        // stop spinning
                        State = STATE_HIGHLIGHTING;
                        _finalSector = _wheel.Length - (int)((_wheel[0].offsetAngle + 90) % 360 / _sectorAngle) - 1;
                        Result = _wheel[_finalSector].value;
                        _wheel[_finalSector].fillBrush.Color = Color.Black;
                    }
                    else
                    {
                        Rotate();
                    }
                    break;
                case STATE_HIGHLIGHTING:
                    // the game is over - highlight winning sector
                    var old = _wheel[_finalSector].fillBrush.Color;
                    if (old.R > 235 && _highlight)
                        _highlight = false;
                    else if (old.R < 20 && _highlight == false) _highlight = true;

                    if (_highlight)
                        _wheel[_finalSector].fillBrush.Color = Color.FromArgb(old.R + 15, old.R + 15, 0);
                    else
                        _wheel[_finalSector].fillBrush.Color = Color.FromArgb(old.R - 15, old.R - 15, 0);
                    break;
            }
            if (repaint) Draw();
        }

        public void Draw()
        {
            // the 'using' statement takes care of the proper disposal of the objects
            using (var bufl = new Bitmap(_boundRect.Width + 60, _boundRect.Height + 60))
            {
                using (var g = Graphics.FromImage(bufl))
                {
                    foreach (var s in _wheel)
                        s.DrawSector(g, _borderPen, _boundRect, _sectorAngle);
                    foreach (var s in _wheel)
                        s.DrawValue(g, _numberPen, _center, _sectorAngle, _radius);

                    g.FillPolygon(_triangleBrush, _trianglePoints);
                    _surface.DrawImageUnscaled(bufl, 0, 0);
                }
            }
        }

        public void SetGraphics(Graphics surface)
        {
            _surface = surface;
        }

        private void Rotate()
        {
            foreach (var s in _wheel) s.offsetAngle = (s.offsetAngle + _rotateAngle) % 360;
        }

        #endregion Animation

        private class Sector
        {
            public string value;
            public SolidBrush fillBrush;
            public float offsetAngle;

            public static Point Polar(Point start, int length, float angle)
            {
                var radians = angle * Math.PI / 180.0; // counter-clockwise angle in radians
                return new Point((int)(start.X + length * Math.Cos(radians)), (int)(start.Y - length * Math.Sin(radians)));
            }

            public Sector(int index, string value, float sectorAngle, Color brushColor)
            {
                this.value = value;
                fillBrush = new SolidBrush(brushColor);
                offsetAngle = (270 + index * sectorAngle) % 360;
            }

            public void DrawSector(Graphics g, Pen borderPen, Rectangle bounds, float sectorAngle)
            {
                // draw outline
                g.DrawPie(borderPen, bounds, offsetAngle, sectorAngle);

                // draw fill
                g.FillPie(fillBrush, bounds, offsetAngle, sectorAngle);
            }

            public void DrawValue(Graphics g, Pen numberPen, Point center, float sectorAngle, int wheelRadius)
            {
                // the COUNTER_CLOCKWISE angle of the center of the sector
                var centerAngle = 360 - (offsetAngle + sectorAngle / (float)2.0);
                var digitAngle = 90 - centerAngle;

                int charIndex = 1, numSectors = (int)(360 / sectorAngle);
                foreach (var c in value)
                {
                    Point charCenter; int size;
                    if (numSectors <= 10)
                    {
                        charCenter = Polar(center, wheelRadius - TEXT_INTVL_LARGE * charIndex, centerAngle);
                        size = 2;
                    }
                    else
                    {
                        charCenter = Polar(center, wheelRadius - TEXT_INTVL_SMALL * charIndex, centerAngle);
                        size = 1;
                    }
                    DrawChar(c, g, numberPen, digitAngle, charCenter, size);
                    //Form1.form1.DrawString(c);
                    charIndex++;
                }
            }

            #region Char Drawing

            // dispatch to appropriate draw function
            private void DrawChar(char c, Graphics g, Pen p, float angle, Point center, int size)
            {
                switch (c)
                {
                    case '0':
                        Draw0(g, p, center, size);
                        break;
                    case '1':
                        Draw1(g, p, angle, center, size);
                        break;
                    case '2':
                        Draw2(g, p, angle, center, size);
                        break;
                    case '3':
                        Draw3(g, p, angle, center, size);
                        break;
                    case '4':
                        Draw4(g, p, angle, center, size);
                        break;
                    case '5':
                        Draw5(g, p, angle, center, size);
                        break;
                    case '6':
                        Draw6(g, p, angle, center, size);
                        break;
                    case '7':
                        Draw7(g, p, angle, center, size);
                        break;
                    case '8':
                        Draw8(g, p, angle, center, size);
                        break;
                    case '9':
                        Draw9(g, p, angle, center, size);
                        break;
                }
            }

            private void DrawCharArc(Graphics g, Pen p, Point center, int radius, float startAngle, float sweepAngle)
            {
                var boundRect = new Rectangle(center.X - radius, center.Y - radius, 2 * radius, 2 * radius);
                g.DrawArc(p, boundRect, startAngle, sweepAngle);
            }

            #region Digits

            private void Draw0(Graphics g, Pen p, Point center, int size)
            {
                DrawCharArc(g, p, center, 10 * size, 0, 360);

            }

            private void Draw1(Graphics g, Pen p, float angle, Point center, int size)
            {
                var topCenter = Polar(center, 10 * size, 90 - angle);
                var btmCenter = Polar(center, 10 * size, 270 - angle);
                var tip = Polar(topCenter, 7 * size, 225 - angle);
                var btmLeft = Polar(btmCenter, 7 * size, 180 - angle);
                var btmRight = Polar(btmCenter, 7 * size, -angle);

                g.DrawLine(p, tip, topCenter);
                g.DrawLine(p, topCenter, btmCenter);
                g.DrawLine(p, btmLeft, btmRight);
            }

            private void Draw2(Graphics g, Pen p, float angle, Point center, int size)
            {


                var topRight = Polar(center, 10 * size, 45 - angle);
                var btmLeft = Polar(center, 10 * size, 225 - angle);
                var btmRight = Polar(btmLeft, 15 * size, -angle);

                var arcRadius = 7 * size;
                var arcCenter = Polar(topRight, arcRadius, 180 - angle);

                g.DrawLine(p, btmLeft, topRight);
                g.DrawLine(p, btmLeft, btmRight);
                DrawCharArc(g, p, arcCenter, arcRadius, 180 + angle, 180);
            }

            private void Draw3(Graphics g, Pen p, float angle, Point center, int size)
            {
                var radius = 7 * size;
                var topArcCenter = Polar(center, radius, 90 - angle);
                var btmArcCenter = Polar(center, radius, 270 - angle);

                DrawCharArc(g, p, topArcCenter, radius, 170 + angle, 260);
                DrawCharArc(g, p, btmArcCenter, radius, 260 + angle, 260);
            }

            private void Draw4(Graphics g, Pen p, float angle, Point center, int size)
            {
                var intersect = Polar(center, 3 * size, 315 - angle);
                var bottom = Polar(intersect, 8 * size, 270 - angle);
                var right = Polar(intersect, 8 * size, -angle);
                var top = Polar(intersect, 15 * size, 90 - angle);
                var left = Polar(intersect, 15 * size, 180 - angle);


                g.DrawLine(p, bottom, top);
                g.DrawLine(p, left, top);
                g.DrawLine(p, left, right);
            }

            private void Draw5(Graphics g, Pen p, float angle, Point center, int size)
            {
                var middleLeft = Polar(center, 3 * size, 135 - angle);
                var topLeft = Polar(middleLeft, 11 * size, 80 - angle);
                var topRight = Polar(topLeft, 12 * size, -angle);

                var arcRadius = 8 * size;
                var arcCenter = Polar(center, 5 * size, 270 - angle);

                g.DrawLine(p, topLeft, topRight);
                g.DrawLine(p, middleLeft, topLeft);
                DrawCharArc(g, p, arcCenter, arcRadius, 245 + angle, 250);
            }

            private void Draw6(Graphics g, Pen p, float angle, Point center, int size)
            {
                var left = Polar(center, 7 * size, 180 - angle);
                var top = Polar(left, 20 * size, 60 - angle);

                var arcRadius = 7 * size;
                var arcCenter = Polar(center, 2 * size, 270 - angle);

                g.DrawLine(p, left, top);
                DrawCharArc(g, p, arcCenter, arcRadius, 0, 360);
            }

            private void Draw7(Graphics g, Pen p, float angle, Point center, int size)
            {
                var topLeft = Polar(center, 15 * size, 135 - angle);
                var topRight = Polar(center, 15 * size, 45 - angle);
                var btmLeft = Polar(topRight, 28 * size, 225 - angle);

                g.DrawLine(p, topLeft, topRight);
                g.DrawLine(p, topRight, btmLeft);
            }

            private void Draw8(Graphics g, Pen p, float angle, Point center, int size)
            {
                var radius = 7 * size;
                var topArcCenter = Polar(center, radius, 90 - angle);
                var btmArcCenter = Polar(center, radius, 270 - angle);

                DrawCharArc(g, p, topArcCenter, radius, 0, 360);
                DrawCharArc(g, p, btmArcCenter, radius, 0, 360);
            }

            private void Draw9(Graphics g, Pen p, float angle, Point center, int size)
            {
                var right = Polar(center, 7 * size, -angle);
                var btm = Polar(right, 20 * size, 240 - angle);

                var arcRadius = 7 * size;
                var arcCenter = Polar(center, 2 * size, 90 - angle);

                g.DrawLine(p, right, btm);
                DrawCharArc(g, p, arcCenter, arcRadius, 0, 360);
            }

            #endregion


            #endregion
        }
    }
}
