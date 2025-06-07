using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace The_luffy_project
{
    public class CAdvImgActor
    {
        public Bitmap img;
        public Rectangle rcSrc,rcDst;
        public int x, y;

    }
    public class Moves
    {
        public List<Bitmap> LBalloonFrames = new List<Bitmap>();
        public List<Bitmap> LBlockingFrames = new List<Bitmap>();
        public List<Bitmap> LFallingFrames = new List<Bitmap>();
        public List<Bitmap> LJumpingFrames = new List<Bitmap>();
        public List<Bitmap> LPunching1Frames = new List<Bitmap>();
        public List<Bitmap> LPunching2Frames = new List<Bitmap>();
        public List<Bitmap> LPunching3Frames = new List<Bitmap>();
        public List<Bitmap> LWalkingFrames = new List<Bitmap>();
        public List<Bitmap> LStandingFrames = new List<Bitmap>();
        public List<Bitmap> LRunningFrames = new List<Bitmap>();

        
    }


    public class Hero
    {
        public int x, y, w, h;
        public string name;
        // left key -> L
        // run -> check direction(L) -> left Running list
        public char direction = 'R'; // either L or R
        public bool IsMoving = false;
        public bool IsRunning = false;

        public Moves LD = new Moves();
        public Moves RD = new Moves();

        public int iFrames ;

        // Luffy/LDirection/Running/0.png
        // Luffy/RDirection/Running/0.png

        // lists of images for the moves
        // lists left
        // lists right
        // indexes for each move
        public int IndBalloon;
        public int IndBlocking;
        public int IndFalling;
        public int IndJumping;
        public int IndPunching1;
        public int IndPunching2;
        public int IndPunching3;
        public int IndStanding;
        public int IndWalking;
        public int IndRunning;

        public int FBalloon;
        public int FBlocking;
        public int FFalling;
        public int FJumping;
        public int FPunching1;
        public int FPunching2;
        public int FPunching3;
        public int FStanding;
        public int FWalking;
        public int FRunning;
    }


    public partial class Form1 : Form
    {
        Timer tt = new Timer();

        Hero Luffy = new Hero();

        Bitmap off; 
        // background
        List<CAdvImgActor>Lbg = new List<CAdvImgActor>();
        int XA = 0,YA=0,XB=0,YB=0,W=6805,H=1285;
        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            this.Load += Form1_Load;
            this.Paint += Form1_Paint;
            tt.Interval = 250;
            tt.Start();
            tt.Tick += Tt_Tick;
        }
        private void Tt_Tick(object sender, EventArgs e)
        {
            if(Luffy.FStanding==1)
                AnimateLuffyStanding();

            DrawDubb(this.CreateGraphics());
        }
        void AnimateLuffyStanding()
        {
            Luffy.IndStanding++;
            if (Luffy.IndStanding > 1)
            {
                Luffy.IndStanding = 0;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            CreateLuffy();
            createBG();
            DrawDubb(this.CreateGraphics());
        }
        void createBG() 
        { 
            CAdvImgActor pnn = new CAdvImgActor();
            pnn.img = new Bitmap("origExtrabig.png");
            pnn.rcSrc = new Rectangle(XA,YA, this.ClientSize.Width, pnn.img.Height);
            pnn.rcDst = new Rectangle(XB, YB, this.ClientSize.Width, this.ClientSize.Height);
            Lbg.Add(pnn);
        }
        void UseMovement(int N,string name,List<Bitmap> M, List<Bitmap> M2, string Move,string extension)
        {
            for (int i = 0; i < N; i++)
            {
                Bitmap img = new Bitmap(name + "/LDirection/"+Move+"/" + i + "."+extension);
                M.Add(img);
                       img = new Bitmap(name + "/RDirection/"+Move+"/" +  i + "."+extension);
                M2.Add(img);
            }
        }

        void CreateLuffy()
        {
            Luffy.x = 50;
            Luffy.y = this.ClientSize.Height-200;
            Luffy.w = 128;
            Luffy.h = 128;
            Luffy.name = "Luffy";
            //Index of Frames = 0
            Luffy.IndStanding = 0;
            Luffy.IndWalking = 0;
            Luffy.FJumping = 0;
            //Flags
            Luffy.FStanding = 1;
            Luffy.FWalking = 0;
            Luffy.FJumping = 0;
            //Moves
            UseMovement(2, Luffy.name, Luffy.LD.LStandingFrames, Luffy.RD.LStandingFrames, "Standing", "png");
            UseMovement(4, Luffy.name, Luffy.LD.LWalkingFrames, Luffy.RD.LWalkingFrames, "Walking", "png");
            UseMovement(4, Luffy.name, Luffy.LD.LJumpingFrames, Luffy.RD.LJumpingFrames, "Jumping", "png");
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Luffy.FStanding = 1;
            Luffy.FWalking = 0;
            Luffy.FJumping = 0;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode) 
            {
                case Keys.Z:
                    for (int i = 0; i < 6; i++)
                    {
                        Luffy.IndBalloon++;
                        DrawDubb(this.CreateGraphics());
                    }
                    Luffy.IndBalloon = 0;
                    break;
                case Keys.Right:
                    Luffy.direction = 'R';
                    Luffy.FStanding = 0;
                    Luffy.FWalking = 1;
                    if (Luffy.x + this.ClientSize.Width/4 > this.ClientSize.Width
                        && Lbg[0].rcSrc.X + 15 +this.ClientSize.Width < Lbg[0].img.Width)
                        Lbg[0].rcSrc.X += 15;
                    else
                    {
                        if(Luffy.x+Luffy.w+15<this.ClientSize.Width)
                        Luffy.x += 15;
                    }
                    //Luffy.x += 15;
                    if (Luffy.IndWalking < 3)
                    {
                        Luffy.IndWalking++;
                    }
                    else
                    {
                        Luffy.IndWalking=0;
                    }

                    break;
                case Keys.Left:
                    Luffy.direction = 'L';
                    Luffy.FStanding = 0;
                    Luffy.FWalking = 1;
                    if (Luffy.x - this.ClientSize.Width / 4 < 0 
                        && Lbg[0].rcSrc.X - 15 > 0)
                        Lbg[0].rcSrc.X -= 15;
                    else
                    {
                        if (Luffy.x - 15 > 0)
                            Luffy.x -= 15;
                    }
                    if (Luffy.IndWalking < 3)
                    {
                        Luffy.IndWalking++;
                    }
                    else
                    {
                        Luffy.IndWalking = 0;
                    }
                    break;
                case Keys.W:
                    Luffy.FJumping = 1;
                    break;
            }
            DrawDubb(this.CreateGraphics());

        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //DrawDubb(e.Graphics);
        }
        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }
        void DrawScene(Graphics g2)
        {
            g2.Clear(Color.White);
            DrawLAdvImages(g2,Lbg);
            DrawLuffy(g2, 2, Luffy, Luffy.RD.LStandingFrames, Luffy.LD.LStandingFrames, Luffy.direction, Luffy.FStanding, Luffy.IndStanding);
            DrawLuffy(g2, 4, Luffy, Luffy.RD.LWalkingFrames, Luffy.LD.LWalkingFrames, Luffy.direction, Luffy.FWalking, Luffy.IndWalking);
            DrawLuffy(g2, 4, Luffy, Luffy.RD.LJumpingFrames, Luffy.LD.LJumpingFrames, Luffy.direction, Luffy.FJumping, Luffy.IndJumping);
        }
        void DrawLAdvImages(Graphics g2, List<CAdvImgActor> Limg)
        {
            for (int i = 0; i < Limg.Count; i++)
            {
                CAdvImgActor bg = Limg[i];
                g2.DrawImage(bg.img, bg.rcDst, bg.rcSrc, GraphicsUnit.Pixel);
            }
        }
        void DrawLuffy(Graphics g2, int N, Hero H,List<Bitmap> Limgs,List<Bitmap>Limgs2, Char Direction, int Flag, int IndFrame)
        {
            if (Direction == 'R')
            {
                if (Flag == 1)
                    DrawDir(Limgs, H.x, H.y, H.w, H.h, IndFrame, Flag, N, g2);

            }
            else if (Direction == 'L')
            {
                if (Flag == 1)
                    DrawDir(Limgs2, H.x, H.y, H.w, H.h, IndFrame, Flag, N, g2);

            }
        }
        void DrawDir(List<Bitmap> Limgs, int x, int y, int w, int h, int iFrame,int F,int N, Graphics g2)
        {
            if (F == 1)
            {
                if (iFrame < N)
                {
                    Bitmap ptr = Limgs[iFrame];
                    g2.DrawImage(ptr, x, y, w, h);
                }

            }
        }
    }
}
