using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace miniGame3
{
    public partial class Form1 : Form
    {
        public class CAdvImgActor
        {
            public Bitmap img;
            public Rectangle rcDst;
            public Rectangle rcSrc;
        }

        public class Hero
        {
            public int X, Y, w, h;
            public Color cl;
        }

        public class Bonus
        {
            public int X, Y;
            public Bitmap img;
        }

        public class Colors
        {
            public int X, Y;
            public Bitmap img;
        }

        public class Obstcales
        {
            public int type;
            public List<Obstcale> obstcale = new List<Obstcale>();
            public List<Bonus> bonus = new List<Bonus>();
            public List<Colors> color = new List<Colors>();
        }

        public class Obstcale
        {
            public int X, Y, w, h, dirX, dirY;
            public int x1, y1, x2, y2, dir;
            public Color cl;
        }

        private CAdvImgActor Background = new CAdvImgActor();
        private CAdvImgActor score = new CAdvImgActor();
        private CAdvImgActor replay = new CAdvImgActor();

        private Timer tt = new Timer();
        private Bitmap off;
        private List<Hero> hero = new List<Hero>();
        private List<Obstcales> obstcales = new List<Obstcales>();
        private List<Obstcales> obstcalesA = new List<Obstcales>();

        private int ctGravity = 0;
        private int ctChangeColor1 = 0;
        private int animatecol = 0;

        private int createObst = 0;

        private int velocity = 0;
        private int acceleration = 2;

        public int Score = 0;
        private int flagLose;

        public Form1()
        {
            //   this.WindowState = FormWindowState.Maximized;
            this.Paint += Form1_Paint;
            this.Load += Form1_Load;
            this.MouseDown += Form1_MouseDown;
            this.MouseUp += Form1_MouseUp;
            this.KeyDown += Form1_KeyDown;
            tt.Tick += Tt_Tick;
            tt.Interval = 1;
            tt.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    hero[0].Y -= 30;
                    velocity = 1;
                    //if (hero[0].Y <= this.ClientSize.Height * 3 / 4)
                    //{
                    //    for (int i = 0; i < obstcalesA.Count; i++)
                    //    {
                    //        for (int j = 0; j < obstcalesA[i].obstcale.Count; j++)
                    //        {
                    //            obstcalesA[i].obstcale[j].Y += 20;
                    //        }
                    //    }
                    //}
                    break;
            }
        }

        private void Tt_Tick(object sender, EventArgs e)
        {
            if (createObst % 250 == 0)
            {
                cretaeObstacleRandomly();
            }
            if (ctGravity % 4 == 0)
            {
                AnimateHero();
                ctGravity = 1;
            }
            if (animatecol % 2 == 0)
            {
                MoveColor1();
            }
            if (ctChangeColor1 % 7 == 0)
            {
                changeColor1();
            }
            AnimateObstacleDown();
            isHitBonusOrColor();
            isHitEnemy();

            animatecol++;
            ctGravity++;
            ctChangeColor1++;
            createObst++;
            DrawDubb(this.CreateGraphics());
        }

        private void isHitEnemy()
        {
            if (hero[0].Y >= this.ClientSize.Height)
            {
                flagLose = 1;
                replay.img = new Bitmap("rr.png");
                // replay.img.MakeTransparent(replay.img.GetPixel(0, 0));
                replay.rcSrc = new Rectangle(0, replay.img.Height / 2, (replay.img.Width / 3) + 30, replay.img.Height / 5);
                replay.rcDst = new Rectangle(this.ClientSize.Width / 4, this.ClientSize.Height * 4 / 10, 250, 200);
                tt.Stop();
            }
            for (int i = 0; i < obstcalesA.Count; i++)
            {
                for (int j = 0; j < obstcalesA[i].obstcale.Count; j++)
                {
                    if (obstcalesA[i].obstcale[j].cl != hero[0].cl)
                    {
                        if (obstcalesA[i].type != 3)
                        {
                            if (hero[0].X >= obstcalesA[i].obstcale[j].X && hero[0].X <= obstcalesA[i].obstcale[j].X + obstcalesA[i].obstcale[j].w && hero[0].Y <= obstcalesA[i].obstcale[j].Y + obstcalesA[i].obstcale[j].h && hero[0].Y + hero[0].h >= obstcalesA[i].obstcale[j].Y)
                            {
                                flagLose = 1;
                                replay.img = new Bitmap("rr.png");
                                // replay.img.MakeTransparent(replay.img.GetPixel(0, 0));
                                replay.rcSrc = new Rectangle(0, replay.img.Height / 2, (replay.img.Width / 3) + 30, replay.img.Height / 5);
                                replay.rcDst = new Rectangle(this.ClientSize.Width / 4, this.ClientSize.Height * 4 / 10, 250, 200);
                                tt.Stop();
                            }
                            if (hero[0].X + hero[0].w >= obstcalesA[i].obstcale[j].X && hero[0].X + hero[0].w <= obstcalesA[i].obstcale[j].X + obstcalesA[i].obstcale[j].w && hero[0].Y >= obstcalesA[i].obstcale[j].Y && hero[0].Y <= obstcalesA[i].obstcale[j].Y + obstcalesA[i].obstcale[j].h)
                            {
                                flagLose = 1;
                                replay.img = new Bitmap("rr.png");
                                // replay.img.MakeTransparent(replay.img.GetPixel(0, 0));
                                replay.rcSrc = new Rectangle(0, replay.img.Height / 2, (replay.img.Width / 3) + 30, replay.img.Height / 5);
                                replay.rcDst = new Rectangle(this.ClientSize.Width / 4, this.ClientSize.Height * 4 / 10, 250, 200);
                                tt.Stop();
                            }
                        }
                        else if ((hero[0].X + hero[0].w >= obstcalesA[i].obstcale[j].x1 && hero[0].X <= obstcalesA[i].obstcale[j].x1 && hero[0].Y <= obstcalesA[i].obstcale[j].y2 && hero[0].Y + hero[0].h >= obstcalesA[i].obstcale[j].y1) ||
                        (hero[0].X <= obstcalesA[i].obstcale[j].x2 && hero[0].X + hero[0].w >= obstcalesA[i].obstcale[j].x2 && hero[0].Y <= obstcalesA[i].obstcale[j].y2 && hero[0].Y + hero[0].h >= obstcalesA[i].obstcale[j].y1) ||
                        (hero[0].Y <= obstcalesA[i].obstcale[j].y1 && hero[0].Y + hero[0].h >= obstcalesA[i].obstcale[j].y1 && hero[0].X <= obstcalesA[i].obstcale[j].x2 && hero[0].X + hero[0].w >= obstcalesA[i].obstcale[j].x1) ||
                        (hero[0].Y + hero[0].h >= obstcalesA[i].obstcale[j].y2 && hero[0].Y <= obstcalesA[i].obstcale[j].y2 && hero[0].X <= obstcalesA[i].obstcale[j].x2 && hero[0].X + hero[0].w >= obstcalesA[i].obstcale[j].x1))
                        {
                            flagLose = 1;
                            replay.img = new Bitmap("rr.png");
                            // replay.img.MakeTransparent(replay.img.GetPixel(0, 0));
                            replay.rcSrc = new Rectangle(0, replay.img.Height / 2, (replay.img.Width / 3) + 30, replay.img.Height / 5);
                            replay.rcDst = new Rectangle(this.ClientSize.Width / 4, this.ClientSize.Height * 4 / 10, 250, 200);
                            tt.Stop();
                        }
                    }
                }
            }
        }

        private void isHitBonusOrColor()
        {
            Random rr = new Random();
            for (int i = 0; i < obstcalesA.Count; i++)
            {
                if (obstcalesA[i].bonus.Count > 0)
                {
                    if (hero[0].X + hero[0].w / 2 >= obstcalesA[i].bonus[0].X && hero[0].X + hero[0].w / 2 <= obstcalesA[i].bonus[0].X + obstcalesA[i].bonus[0].img.Width

                  && hero[0].Y + hero[0].h / 2 >= obstcalesA[i].bonus[0].Y && hero[0].Y + hero[0].h / 2 <= obstcalesA[i].bonus[0].Y + obstcalesA[i].bonus[0].img.Height)
                    {
                        Score++;
                        obstcalesA[i].bonus.Clear();
                    }
                }

                ///
                if (obstcalesA[i].color.Count > 0)
                {
                    if (hero[0].X + hero[0].w / 2 >= obstcalesA[i].color[0].X && hero[0].X + hero[0].w / 2 <= obstcalesA[i].color[0].X + obstcalesA[i].color[0].img.Width

                  && hero[0].Y + hero[0].h / 2 >= obstcalesA[i].color[0].Y && hero[0].Y + hero[0].h / 2 <= obstcalesA[i].color[0].Y + obstcalesA[i].color[0].img.Height)
                    {
                        int v = rr.Next(1, 4);

                        if (hero[0].cl == Color.Yellow)
                        {
                            if (v == 1)
                            {
                                hero[0].cl = Color.White;
                            }
                            if (v == 2)
                            {
                                hero[0].cl = Color.Red;
                            }
                            if (v == 3)
                            {
                                hero[0].cl = Color.Green;
                            }
                        }
                        else if (hero[0].cl == Color.White)
                        {
                            if (v == 1)
                            {
                                hero[0].cl = Color.Yellow;
                            }
                            if (v == 2)
                            {
                                hero[0].cl = Color.Green;
                            }
                            if (v == 3)
                            {
                                hero[0].cl = Color.Red;
                            }
                        }
                        else if (hero[0].cl == Color.Red)
                        {
                            if (v == 1)
                            {
                                hero[0].cl = Color.Yellow;
                            }
                            if (v == 2)
                            {
                                hero[0].cl = Color.Green;
                            }
                            if (v == 3)
                            {
                                hero[0].cl = Color.White;
                            }
                        }
                        else if (hero[0].cl == Color.Green)
                        {
                            if (v == 1)
                            {
                                hero[0].cl = Color.Yellow;
                            }
                            if (v == 2)
                            {
                                hero[0].cl = Color.Red;
                            }
                            if (v == 3)
                            {
                                hero[0].cl = Color.White;
                            }
                        }

                        obstcalesA[i].color.Clear();
                    }
                }
            }
        }

        private void MoveColor1()
        {
            for (int j = 0; j < obstcalesA.Count; j++)
            {
                if (obstcalesA[j].type == 2)
                {
                    for (int i = 0; i < obstcalesA[j].obstcale.Count; i++)
                    {
                        if (obstcalesA[j].obstcale[i].X >= this.ClientSize.Width)
                        {
                            obstcalesA[j].obstcale[i].dirX = -1;
                        }
                        if (obstcalesA[j].obstcale[i].X <= 0)
                        {
                            obstcalesA[j].obstcale[i].dirX = 1;
                        }
                        if (i % 2 == 0)
                        {
                            if (obstcalesA[j].obstcale[i].dirX == 1)
                            {
                                obstcalesA[j].obstcale[i].X += 6;
                            }
                            else if (obstcalesA[j].obstcale[i].dirX == -1)
                            {
                                obstcalesA[j].obstcale[i].X -= 6;
                            }
                        }
                        else
                        {
                            if (obstcalesA[j].obstcale[i].dirX == 1)
                            {
                                obstcalesA[j].obstcale[i].X += 5;
                            }
                            else if (obstcalesA[j].obstcale[i].dirX == -1)
                            {
                                obstcalesA[j].obstcale[i].X -= 5;
                            }
                        }
                    }
                }
                if (obstcalesA[j].type == 3)
                {
                    for (int i = 0; i < obstcalesA[j].obstcale.Count; i++)
                    {
                        if (obstcalesA[j].obstcale[i].dir == 1)
                        {
                            obstcalesA[j].obstcale[i].x1 += 1;
                            obstcalesA[j].obstcale[i].y1 -= 1;
                            obstcalesA[j].obstcale[i].x2 -= 1;
                            obstcalesA[j].obstcale[i].y2 += 1;
                        }
                        if (obstcalesA[j].obstcale[i].dir == 2)
                        {
                            obstcalesA[j].obstcale[i].x1 += 1;
                            obstcalesA[j].obstcale[i].y1 += 1;
                            obstcalesA[j].obstcale[i].x2 -= 1;
                            obstcalesA[j].obstcale[i].y2 -= 1;
                        }
                        if (obstcalesA[j].obstcale[i].dir == 3)
                        {
                            obstcalesA[j].obstcale[i].x1 -= 1;
                            obstcalesA[j].obstcale[i].y1 += 1;
                            obstcalesA[j].obstcale[i].x2 += 1;
                            obstcalesA[j].obstcale[i].y2 -= 1;
                        }
                        if (obstcalesA[j].obstcale[i].dir == 4)
                        {
                            obstcalesA[j].obstcale[i].x1 -= 1;
                            obstcalesA[j].obstcale[i].y1 -= 1;
                            obstcalesA[j].obstcale[i].x2 += 1;
                            obstcalesA[j].obstcale[i].y2 += 1;
                        }
                        if (obstcalesA[j].obstcale[i].dir == 1)
                        {
                            if (obstcalesA[j].obstcale[i].x1 == obstcalesA[j].obstcale[i].x2)
                            {
                                obstcalesA[j].obstcale[i].dir = 2;
                            }
                        }
                        if (obstcalesA[j].obstcale[i].dir == 2)
                        {
                            if (obstcalesA[j].obstcale[i].y1 == obstcalesA[j].obstcale[i].y2)
                            {
                                obstcalesA[j].obstcale[i].dir = 3;
                            }
                        }
                        if (obstcalesA[j].obstcale[i].dir == 3)
                        {
                            if (obstcalesA[j].obstcale[i].x1 == obstcalesA[j].obstcale[i].x2)
                            {
                                obstcalesA[j].obstcale[i].dir = 4;
                            }
                        }
                        if (obstcalesA[j].obstcale[i].dir == 4)
                        {
                            if (obstcalesA[j].obstcale[i].y1 == obstcalesA[j].obstcale[i].y2)
                            {
                                obstcalesA[j].obstcale[i].dir = 1;
                            }
                        }
                    }
                }
            }
        }

        private void AnimateObstacleDown()
        {
            for (int i = 0; i < obstcalesA.Count; i++)
            {
                if (obstcalesA[i].bonus.Count > 0)
                {
                    obstcalesA[i].bonus[0].Y += 3;
                }
                if (obstcalesA[i].color.Count > 0)
                {
                    obstcalesA[i].color[0].Y += 3;
                }
                if (obstcalesA[i].type == 3)
                {
                    for (int j = 0; j < obstcalesA[i].obstcale.Count; j++)
                    {
                        obstcalesA[i].obstcale[j].y1 += 3;
                        obstcalesA[i].obstcale[j].y2 += 3;
                    }
                }
                else
                {
                    for (int j = 0; j < obstcalesA[i].obstcale.Count; j++)
                    {
                        obstcalesA[i].obstcale[j].Y += 3;
                    }
                }
            }
        }

        private void cretaeObstacleRandomly()
        {
            Random rr = new Random();
            int v = rr.Next(0, 4);
            int vv = rr.Next(100, 500);
            if (v == 0)
            {
                Obstcales pnn = new Obstcales();
                pnn.type = 0;
                Bonus pB = new Bonus();
                pB.X = obstcales[0].bonus[0].X;
                pB.Y = obstcales[0].bonus[0].Y - this.ClientSize.Height / 4; ;
                pB.img = new Bitmap("star.png");
                pB.img.MakeTransparent(pB.img.GetPixel(0, 0));
                pnn.bonus.Add(pB);
                ///
                Colors pC = new Colors();
                pC.X = obstcales[0].color[0].X;
                pC.Y = obstcales[0].color[0].Y - this.ClientSize.Height / 4; ;
                pC.img = new Bitmap("1.png");
                pC.img.MakeTransparent(pC.img.GetPixel(0, 0));
                pnn.color.Add(pC);
                for (int i = 0; i < obstcales[0].obstcale.Count; i++)
                {
                    Obstcale pp = new Obstcale();
                    pp.w = obstcales[0].obstcale[i].w;
                    pp.h = obstcales[0].obstcale[i].h;
                    pp.cl = obstcales[0].obstcale[i].cl;
                    pp.X = obstcales[0].obstcale[i].X;
                    pp.Y = obstcales[0].obstcale[i].Y - this.ClientSize.Height / 4;
                    pnn.obstcale.Add(pp);
                }
                obstcalesA.Add(pnn);
            }
            if (v == 1)
            {
                Obstcales pnn = new Obstcales();
                pnn.type = 1;
                Bonus pB = new Bonus();
                pB.X = obstcales[1].bonus[0].X;
                pB.Y = 0 - this.ClientSize.Height / 2;
                pB.img = new Bitmap("star.png");
                pB.img.MakeTransparent(pB.img.GetPixel(0, 0));
                pnn.bonus.Add(pB);
                ///
                Colors pC = new Colors();
                pC.X = obstcales[1].color[0].X;
                pC.Y = 0 - (this.ClientSize.Height / 2) - 50;
                pC.img = new Bitmap("1.png");
                pC.img.MakeTransparent(pC.img.GetPixel(0, 0));
                pnn.color.Add(pC);
                for (int i = 0; i < obstcales[1].obstcale.Count; i++)
                {
                    Obstcale pp = new Obstcale();
                    pp.w = obstcales[1].obstcale[i].w;
                    pp.h = obstcales[1].obstcale[i].h;
                    pp.cl = obstcales[1].obstcale[i].cl;
                    pp.X = obstcales[1].obstcale[i].X;
                    pp.Y = 0 - this.ClientSize.Height / 4;
                    pnn.obstcale.Add(pp);
                }
                obstcalesA.Add(pnn);
            }
            if (v == 2)
            {
                Obstcales pnn = new Obstcales();
                pnn.type = 2;
                Bonus pB = new Bonus();
                pB.X = obstcales[2].bonus[0].X;
                pB.Y = obstcales[2].bonus[0].Y - this.ClientSize.Height / 4;
                pB.img = new Bitmap("star.png");
                pB.img.MakeTransparent(pB.img.GetPixel(0, 0));
                pnn.bonus.Add(pB);
                ///
                Colors pC = new Colors();
                pC.X = obstcales[2].color[0].X - 10;
                pC.Y = obstcales[2].color[0].Y - this.ClientSize.Height / 4;
                pC.img = new Bitmap("1.png");
                pC.img.MakeTransparent(pC.img.GetPixel(0, 0));
                pnn.color.Add(pC);
                for (int i = 0; i < obstcales[2].obstcale.Count; i++)
                {
                    Obstcale pp = new Obstcale();

                    pp.w = obstcales[2].obstcale[i].w;
                    pp.h = obstcales[2].obstcale[i].h;
                    pp.cl = obstcales[2].obstcale[i].cl;
                    pp.X = obstcales[2].obstcale[i].X + vv;
                    pp.Y = 0 - this.ClientSize.Height / 4;
                    pp.dirX = obstcales[2].obstcale[i].dirX;
                    pnn.obstcale.Add(pp);
                }
                obstcalesA.Add(pnn);
            }
            if (v == 3)
            {
                Obstcales pnn = new Obstcales();
                pnn.type = 3;
                Bonus pB = new Bonus();
                pB.X = obstcales[3].bonus[0].X;
                pB.Y = obstcales[3].bonus[0].Y - this.ClientSize.Height / 2;
                pB.img = new Bitmap("star.png");
                pB.img.MakeTransparent(pB.img.GetPixel(0, 0));
                pnn.bonus.Add(pB);
                ///
                Colors pC = new Colors();
                pC.X = obstcales[3].color[0].X - 10;
                pC.Y = obstcales[3].color[0].Y - this.ClientSize.Height / 2;
                pC.img = new Bitmap("1.png");
                pC.img.MakeTransparent(pC.img.GetPixel(0, 0));
                pnn.color.Add(pC);
                for (int i = 0; i < obstcales[3].obstcale.Count; i++)
                {
                    Obstcale pp = new Obstcale();

                    pp.x1 = obstcales[3].obstcale[i].x1;
                    pp.y1 = obstcales[3].obstcale[i].y1 - this.ClientSize.Height;
                    pp.x2 = obstcales[3].obstcale[i].x2;
                    pp.y2 = obstcales[3].obstcale[i].y2 - this.ClientSize.Height;
                    pp.dir = obstcales[3].obstcale[i].dir;

                    if (i < 4)
                    {
                        pp.cl = hero[0].cl;
                    }
                    else
                    {
                        int cl = rr.Next(1, 4);

                        if (hero[0].cl == Color.Yellow)
                        {
                            if (cl == 1)
                            {
                                pp.cl = Color.White;
                            }
                            if (cl == 2)
                            {
                                pp.cl = Color.Red;
                            }
                            if (cl == 3)
                            {
                                pp.cl = Color.Green;
                            }
                        }
                        else if (hero[0].cl == Color.White)
                        {
                            if (cl == 1)
                            {
                                pp.cl = Color.Yellow;
                            }
                            if (cl == 2)
                            {
                                pp.cl = Color.Green;
                            }
                            if (cl == 3)
                            {
                                pp.cl = Color.Red;
                            }
                        }
                        else if (hero[0].cl == Color.Red)
                        {
                            if (cl == 1)
                            {
                                pp.cl = Color.Yellow;
                            }
                            if (cl == 2)
                            {
                                pp.cl = Color.Green;
                            }
                            if (cl == 3)
                            {
                                pp.cl = Color.White;
                            }
                        }
                        else if (hero[0].cl == Color.Green)
                        {
                            if (cl == 1)
                            {
                                pp.cl = Color.Yellow;
                            }
                            if (cl == 2)
                            {
                                pp.cl = Color.Red;
                            }
                            if (cl == 3)
                            {
                                pp.cl = Color.White;
                            }
                        }
                    }
                    pnn.obstcale.Add(pp);
                }
                obstcalesA.Add(pnn);
            }
        }

        private void changeColor1()
        {
            for (int j = 0; j < obstcalesA.Count; j++)
            {
                if (obstcalesA[j].type != 2 && obstcalesA[j].type != 3)
                {
                    for (int i = obstcalesA[j].obstcale.Count - 1; i >= 0; i--)
                    {
                        if (i == 0)
                        {
                            obstcalesA[j].obstcale[i].cl = obstcalesA[j].obstcale[obstcalesA[j].obstcale.Count - 1].cl;
                        }
                        else
                        {
                            obstcalesA[j].obstcale[i].cl = obstcalesA[j].obstcale[i - 1].cl;
                        }
                    }
                }
            }
        }

        private void AnimateHero()
        {
            // Apply acceleration to velocity
            velocity += acceleration;

            // Update position using velocity
            hero[0].Y += velocity;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:

                    break;
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (flagLose == 1)
                    {
                        if (e.X >= replay.rcDst.X && e.X <= replay.rcDst.X + replay.rcDst.Width && e.Y >= replay.rcDst.Y && e.Y <= replay.rcDst.Y + replay.rcDst.Height)
                        {
                            flagLose = 0;
                            Score = 0;
                            obstcalesA.Clear();
                            hero[0].Y = (this.ClientSize.Height * 3 / 10);
                            velocity = 0;
                            acceleration = 2;
                            tt.Start();
                        }
                    }

                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(700, Screen.PrimaryScreen.WorkingArea.Height);
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 4, 0);
            Background.img = new Bitmap("BG.jpg");
            Background.img.MakeTransparent(Background.img.GetPixel(0, 0));
            Background.rcSrc = new Rectangle(0, 0, Background.img.Width, Background.img.Height);
            Background.rcDst = new Rectangle(0, 0, this.Width, this.Height);
            score.img = new Bitmap("sc.png");
            score.img.MakeTransparent(score.img.GetPixel(0, 0));
            score.rcSrc = new Rectangle(0, 0, score.img.Width, score.img.Height);
            score.rcDst = new Rectangle(5, 5, score.img.Width, score.img.Height + 10);

            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            CreateHero();
            createObstacle();
        }

        private void createObstacle()
        {
            for (int i = 0; i < 4; i++)
            {
                Obstcales pnn = new Obstcales();
                if (i == 0)
                {
                    pnn.type = 0;

                    for (int j = 0; j < 20; j++)
                    {
                        Obstcale p = new Obstcale();

                        if (j < 5)
                        {
                            p.w = 30; p.h = 30;
                            p.X = (this.ClientSize.Width / 2) - (5 * p.w) + (j * p.w) + 80;
                            p.Y = 10;
                            p.cl = Color.Red;
                            p.dirX = 1; p.dirY = 0;
                            if (j == 4)
                            {
                                p.dirX = 0; p.dirY = 1;
                            }
                        }
                        if (j < 10 && j >= 5)
                        {
                            p.w = 30; p.h = 30;
                            p.X = pnn.obstcale[4].X;
                            p.Y = pnn.obstcale[4].Y + (j % 5 * p.h) + pnn.obstcale[4].h;
                            p.cl = Color.Green;
                            p.dirX = 0; p.dirY = 1;
                            if (j == 9)
                            {
                                p.dirX = -1; p.dirY = 0;
                            }
                        }
                        if (j < 15 && j >= 10)
                        {
                            p.w = 30; p.h = 30;
                            p.X = pnn.obstcale[9].X - p.w - (p.w) * (j % 5);
                            p.Y = pnn.obstcale[9].Y;
                            p.cl = Color.White;
                            p.dirX = -1; p.dirY = 0;
                            if (j == 14)
                            {
                                p.dirX = 0; p.dirY = -1;
                            }
                        }
                        if (j < 20 && j >= 15)
                        {
                            p.w = 30; p.h = 30;
                            p.X = pnn.obstcale[14].X;
                            p.Y = pnn.obstcale[14].Y - (j % 5 * p.h) - p.h;
                            p.cl = Color.Yellow;
                            p.dirX = 0; p.dirY = -1;
                            if (j == 19)
                            {
                                p.dirX = 1; p.dirY = 0;
                            }
                        }
                        pnn.obstcale.Add(p);
                    }
                    Bonus pB = new Bonus();
                    pB.X = hero[0].X - 15;
                    pB.Y = pnn.obstcale[12].Y - 70;
                    pB.img = new Bitmap("2.bmp");
                    pB.img.MakeTransparent(pB.img.GetPixel(0, 0));
                    pnn.bonus.Add(pB);
                    ///
                    Colors pC = new Colors();
                    pC.X = hero[0].X - 15;
                    pC.Y = pnn.obstcale[2].Y - 70;
                    pC.img = new Bitmap("1.png");
                    pC.img.MakeTransparent(pC.img.GetPixel(0, 0));
                    pnn.color.Add(pC);
                }
                if (i == 1)
                {
                    pnn.type = 1;

                    for (int j = 0; j < 20; j++)
                    {
                        Obstcale p = new Obstcale();
                        if (j < 5)
                        {
                            p.w = 30;
                            p.h = 20;
                            p.X = (this.ClientSize.Width / 3) + (j * p.w) - 190;
                            p.Y = this.ClientSize.Height / 2;
                            p.cl = Color.Red;
                        }
                        if (j < 10 && j >= 5)
                        {
                            p.w = 30;
                            p.h = 20;
                            p.X = (this.ClientSize.Width / 3) + (j * p.w) - 190;
                            p.Y = this.ClientSize.Height / 2;
                            p.cl = Color.White;
                        }
                        if (j < 15 && j >= 10)
                        {
                            p.w = 30;
                            p.h = 20;
                            p.X = (this.ClientSize.Width / 3) + (j * p.w) - 190;
                            p.Y = this.ClientSize.Height / 2;
                            p.cl = Color.Yellow;
                        }
                        if (j < 20 && j >= 15)
                        {
                            p.w = 30;
                            p.h = 20;
                            p.X = (this.ClientSize.Width / 3) + (j * p.w) - 190;
                            p.Y = this.ClientSize.Height / 2;
                            p.cl = Color.Green;
                        }
                        pnn.obstcale.Add(p);
                    }
                    Bonus pB = new Bonus();
                    pB.X = hero[0].X - 15;
                    pB.Y = pnn.obstcale[12].Y - 70;
                    pB.img = new Bitmap("2.bmp");
                    pB.img.MakeTransparent(pB.img.GetPixel(0, 0));
                    pnn.bonus.Add(pB);
                    ///
                    Colors pC = new Colors();
                    pC.X = hero[0].X - 15;
                    pC.Y = pnn.obstcale[12].Y - 200;
                    pC.img = new Bitmap("1.png");
                    pC.img.MakeTransparent(pC.img.GetPixel(0, 0));
                    pnn.color.Add(pC);
                }
                if (i == 2)
                {
                    pnn.type = 2;

                    for (int j = 0; j < 8; j++)
                    {
                        Obstcale p = new Obstcale();

                        if (j == 0)
                        {
                            p.w = 15; p.h = 150;
                            p.X = 0;
                            p.Y = 10;
                            p.cl = Color.Red;
                            p.dirX = 1; p.dirY = 0;
                        }
                        if (j == 1)
                        {
                            p.w = 15; p.h = 100;
                            p.X = 100;
                            p.Y = 10;
                            p.cl = Color.Red;
                            p.dirX = 1; p.dirY = 0;
                        }
                        if (j == 2)
                        {
                            p.w = 15; p.h = 150;
                            p.X = 200;
                            p.Y = 10;
                            p.cl = Color.Green;
                            p.dirX = 1; p.dirY = 0;
                        }
                        if (j == 3)
                        {
                            p.w = 15; p.h = 100;
                            p.X = 300;
                            p.Y = 10;
                            p.cl = Color.Green;
                            p.dirX = 1; p.dirY = 0;
                        }
                        if (j == 4)
                        {
                            p.w = 15; p.h = 150;
                            p.X = 400;
                            p.Y = 10;
                            p.cl = Color.Yellow;
                            p.dirX = 1; p.dirY = 0;
                        }
                        if (j == 5)
                        {
                            p.w = 15; p.h = 100;
                            p.X = 500;
                            p.Y = 10;
                            p.cl = Color.Yellow;
                            p.dirX = 1; p.dirY = 0;
                        }
                        if (j == 6)
                        {
                            p.w = 15; p.h = 150;
                            p.X = 600;
                            p.Y = 10;
                            p.cl = Color.White;
                            p.dirX = 1; p.dirY = 0;
                        }
                        if (j == 7)
                        {
                            p.w = 15; p.h = 100;
                            p.X = 700;
                            p.Y = 10;
                            p.cl = Color.White;
                            p.dirX = 1; p.dirY = 0;
                        }
                        pnn.obstcale.Add(p);
                    }
                    Bonus pB = new Bonus();
                    pB.X = hero[0].X;
                    pB.Y = pnn.obstcale[0].Y - 100;
                    pB.img = new Bitmap("2.bmp");
                    pB.img.MakeTransparent(pB.img.GetPixel(0, 0));
                    pnn.bonus.Add(pB);

                    Colors pC = new Colors();
                    pC.X = hero[0].X;
                    pC.Y = pnn.obstcale[0].Y - 200;
                    pC.img = new Bitmap("1.png");
                    pC.img.MakeTransparent(pC.img.GetPixel(0, 0));
                    pnn.color.Add(pC);
                }
                if (i == 3)
                {
                    //مروحه
                    pnn.type = 3;
                    int margen = 0;

                    for (int j = 0; j < 8; j++)
                    {
                        Obstcale p = new Obstcale();
                        if (j < 4)
                        {
                            if (j % 2 == 0)
                            {
                                p.x1 = hero[0].X - 85;
                                p.y1 = (margen * 100);
                                p.x2 = hero[0].X - 85;
                                p.y2 = (margen * 100) + 200;
                                p.dir = 2;
                            }
                            else
                            {
                                p.x1 = hero[0].X + 85 + hero[0].w;
                                p.y1 = (margen * 100);
                                p.x2 = hero[0].X + 85 + hero[0].w;
                                p.y2 = (margen * 100) + 200;
                                p.dir = 2;
                            }
                        }
                        if (j < 8 && j >= 4)
                        {
                            if (j % 2 == 0)
                            {
                                p.x1 = hero[0].X - 85;
                                p.y1 = (margen * 100);
                                p.x2 = hero[0].X - 85;
                                p.y2 = (margen * 100) + 200;
                                p.dir = 2;
                            }
                            else
                            {
                                p.x1 = hero[0].X + 85 + hero[0].w;
                                p.y1 = (margen * 100);
                                p.x2 = hero[0].X + 85 + hero[0].w;
                                p.y2 = (margen * 100) + 200;
                                p.dir = 2;
                            }
                        }
                        if (j % 2 != 0)
                        {
                            margen += 2;
                        }
                        pnn.obstcale.Add(p);
                    }
                    Bonus pB = new Bonus();
                    pB.X = hero[0].X;
                    pB.Y = pnn.obstcale[0].y1 - 100;
                    pB.img = new Bitmap("2.bmp");
                    pB.img.MakeTransparent(pB.img.GetPixel(0, 0));
                    pnn.bonus.Add(pB);

                    Colors pC = new Colors();
                    pC.X = hero[0].X;
                    pC.Y = pnn.obstcale[0].y1 - 200;
                    pC.img = new Bitmap("1.png");
                    pC.img.MakeTransparent(pC.img.GetPixel(0, 0));
                    pnn.color.Add(pC);
                }

                obstcales.Add(pnn);
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
        }

        private void CreateHero()
        {
            Hero pnn = new Hero();
            pnn.w = 30;
            pnn.h = 30;
            pnn.X = (this.ClientSize.Width / 2) - pnn.w / 2;
            pnn.Y = (this.ClientSize.Height * 3 / 10);
            pnn.cl = Color.Red;
            hero.Add(pnn);
        }

        private void DrawScene(Graphics g)
        {
            g.Clear(Color.Black);

            Brush b;
            Pen p;
            g.DrawImage(Background.img, Background.rcDst, Background.rcSrc, GraphicsUnit.Pixel);
            g.DrawImage(score.img, score.rcDst, score.rcSrc, GraphicsUnit.Pixel);

            for (int i = 0; i < obstcalesA.Count; i++)
            {
                if (obstcalesA[i].type == 0)
                {
                    for (int j = 0; j < obstcalesA[i].obstcale.Count; j++)
                    {
                        b = new SolidBrush(obstcalesA[i].obstcale[j].cl);
                        int x = obstcalesA[i].obstcale[j].X;
                        int y = obstcalesA[i].obstcale[j].Y;
                        int w = obstcalesA[i].obstcale[j].w;
                        int h = obstcalesA[i].obstcale[j].h;
                        g.FillEllipse(b, x, y, w, h);
                        if (obstcalesA[i].bonus.Count > 0)
                        {
                            g.DrawImage(obstcalesA[i].bonus[0].img, obstcalesA[i].bonus[0].X, obstcalesA[i].bonus[0].Y, obstcalesA[i].bonus[0].img.Width, obstcalesA[i].bonus[0].img.Height);
                        }
                        if (obstcalesA[i].color.Count > 0)
                        {
                            g.DrawImage(obstcalesA[i].color[0].img, obstcalesA[i].color[0].X, obstcalesA[i].color[0].Y, obstcalesA[i].color[0].img.Width, obstcalesA[i].color[0].img.Height);
                        }
                    }
                }
                if (obstcalesA[i].type == 1 || obstcalesA[i].type == 2)
                {
                    for (int j = 0; j < obstcalesA[i].obstcale.Count; j++)
                    {
                        b = new SolidBrush(obstcalesA[i].obstcale[j].cl);
                        int x = obstcalesA[i].obstcale[j].X;
                        int y = obstcalesA[i].obstcale[j].Y;
                        int w = obstcalesA[i].obstcale[j].w;
                        int h = obstcalesA[i].obstcale[j].h;
                        g.FillRectangle(b, x, y, w, h);
                        if (obstcalesA[i].bonus.Count > 0)
                        {
                            g.DrawImage(obstcalesA[i].bonus[0].img, obstcalesA[i].bonus[0].X, obstcalesA[i].bonus[0].Y, obstcalesA[i].bonus[0].img.Width, obstcalesA[i].bonus[0].img.Height);
                        }
                        if (obstcalesA[i].color.Count > 0)
                        {
                            g.DrawImage(obstcalesA[i].color[0].img, obstcalesA[i].color[0].X, obstcalesA[i].color[0].Y, obstcalesA[i].color[0].img.Width, obstcalesA[i].color[0].img.Height);
                        }
                    }
                }
                if (obstcalesA[i].type == 3)
                {
                    for (int j = 0; j < obstcalesA[i].obstcale.Count; j++)
                    {
                        p = new Pen(obstcalesA[i].obstcale[j].cl, 7);
                        g.DrawLine(p, obstcalesA[i].obstcale[j].x1, obstcalesA[i].obstcale[j].y1, obstcalesA[i].obstcale[j].x2, obstcalesA[i].obstcale[j].y2);
                    }
                    if (obstcalesA[i].bonus.Count > 0)
                    {
                        g.DrawImage(obstcalesA[i].bonus[0].img, obstcalesA[i].bonus[0].X, obstcalesA[i].bonus[0].Y, obstcalesA[i].bonus[0].img.Width, obstcalesA[i].bonus[0].img.Height);
                    }
                    if (obstcalesA[i].color.Count > 7)
                    {
                        g.DrawImage(obstcalesA[i].color[0].img, obstcalesA[i].color[0].X, obstcalesA[i].color[0].Y, obstcalesA[i].color[0].img.Width, obstcalesA[i].color[0].img.Height);
                    }
                }
            }
            b = new SolidBrush(hero[0].cl);
            for (int i = 0; i < hero.Count; i++)
            {
                g.FillEllipse(b, hero[0].X, hero[0].Y, hero[0].w, hero[0].h);
            }
            if (flagLose == 1)
            {
                g.DrawImage(replay.img, replay.rcDst, replay.rcSrc, GraphicsUnit.Pixel);
            }
            string text;

            text = $"{Score}";

            Font font = new Font("Arial", 20); // Define your font (you can change the font and size)
            Brush brush = Brushes.Black; // Define your brush color (you can change the color)
            int xx = 70; // X coordinate of the text
            int yy = 5; // Y coordinate of the text
            g.DrawString(text, font, brush, xx, yy);
        }

        private void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }
    }
}
