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
    public class CActor
    {
        public int x, y, w, w2, h;
        public Pen p;
        public SolidBrush sb;
        public Color cl, cl2;
    }
    public class CAdvImgActor
    {
        public Bitmap img;
        public Rectangle rcSrc, rcDst;
        public int x, y;

    }

    public class CMulImgActor
    {
        public List<Bitmap> img = new List<Bitmap>();
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
        public CActor HP;
        public int iFrames;

        // Luffy/LDirection/Running/0.png
        // Luffy/RDirection/Running/0.png

        // lists of images for the moves
        // lists left
        // lists right
        // indexes for each move
        public int IndBalloon = 0;
        public int IndBlocking = 0;
        public int IndFalling = 0;
        public int IndJumping = 0;
        public int IndPunching1 = 0;
        public int IndPunching2 = 0;
        public int IndPunching3 = 0;
        public int IndStanding = 0;
        public int IndWalking = 0;
        public int IndRunning = 0;

        public int FBalloon = 0;
        public int FBlocking = 0;
        public int FFalling = 0;
        public int FJumping = 0;
        public int FPunching1 = 0;
        public int FPunching2 = 0;
        public int FPunching3 = 0;
        public int FStanding = 0;
        public int FWalking = 0;
        public int FRunning = 0;
    }
    public partial class Form1 : Form
    {
        Timer tt = new Timer();
        //Heroes
        Hero Luffy = new Hero();
        Hero Crocodile = new Hero();
        Hero Enel = new Hero();
        CMulImgActor Laser = new CMulImgActor();

        //
        Bitmap off;
        // background
        List<CAdvImgActor> Lbg = new List<CAdvImgActor>();
        int XA = 0, YA = 0, XB = 0, YB = 0, W = 6805, H = 1285;

        int Speed = 15;
        //Jumping
        int countJumping = 0;

        int flagBalloon = 0;

        int CtTick = 0;

        // enel laser attack flag
        bool FEnelAttack = false;

        //HP Lives 
        List<CActor> LHP = new List<CActor>();

        //Crocodile Attack Time
        int ctcrocpunch = 0;
        bool IsCrocodileAlive = false;
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
            // for attack and enemy times
            CtTick++;

            if (CtTick % 15  == 0)
                FEnelAttack = true;

            if (FEnelAttack)
            {
                Enel.IndPunching1++;
                if (Enel.IndPunching1 > 5)
                {
                    Enel.IndPunching1 = 0;
                    FEnelAttack = false;
                }
            }



            //else Speed = 15;
            gravity();
            if (Luffy.FStanding == 1)
                AnimatLuffyeStanding();
            if (Luffy.FJumping == 1)
                AnimateLuffyJumping();
            //Crocodile Standing code           
            Crocodile.IndStanding++;
            if (Crocodile.IndStanding > 3)
            {
                Crocodile.IndStanding = 0;
            }

            // balloon animation
            if (Luffy.IndBalloon > 0)
            {
                Luffy.FStanding = 0;
                Luffy.IndBalloon++;
                if (Luffy.IndBalloon == 6)
                {
                    Luffy.IndBalloon = 0;
                    Luffy.FBalloon = 0;
                    Luffy.FStanding = 1;
                }
            }

            AnimateCrocodile();
            if (Crocodile.HP.w2 <= 0)
            {
                IsCrocodileAlive = false;
                Crocodile.HP.w = 0;
            }
            DrawDubb(this.CreateGraphics());
            Crocodile.IndWalking++;
            ctcrocpunch++;

        }
        void AnimateCrocodile()
        {
            if (IsCrocodileAlive)
            {
                if (IsLuffyClosetoCrocodile())
                {
                    Crocodile.FWalking = 0;
                    Crocodile.FStanding = 0;
                    Crocodile.IndPunching1 = 0;
                    if (ctcrocpunch % 2 == 0)
                    {
                        Crocodile.FPunching1 = 1;
                        for (int i = 0; i < 5; i++)
                        {
                            DrawDubb(this.CreateGraphics());
                            Crocodile.IndPunching1++;

                        }
                        //
                        Luffy.FStanding = 0;
                        Luffy.FFalling = 1;
                        Luffy.IndFalling = 0;
                        for (int i = 0; i < 2; i++)
                        {
                            DrawDubb(this.CreateGraphics());
                            Luffy.IndFalling++;
                        }
                        Luffy.FStanding = 1;
                        Luffy.FFalling = 0;
                        if (Luffy.x < Crocodile.x && Luffy.x - 50 > 0)
                            Luffy.x -= 50;
                        else
                        {
                            if (Luffy.x + 50 < this.ClientSize.Width)
                                Luffy.x += 50;
                        }
                        Luffy.HP.w2 -= 10;

                        ctcrocpunch = -1;
                    }
                    Crocodile.FStanding = 1;
                    Crocodile.FPunching1 = 0;
                }
                else
                {
                    Crocodile.FStanding = 0;
                    Crocodile.FWalking = 1;
                    if (Crocodile.IndWalking > 7)
                    {
                        Crocodile.IndWalking = 0;

                    }
                    if (Crocodile.x > Luffy.x)
                    {
                        Crocodile.direction = 'L';
                        Crocodile.x -= 10;
                    }
                    else
                    {
                        Crocodile.direction = 'R';
                        Crocodile.x += 10;
                    }

                }
            }

        }
        bool IsLuffyClosetoCrocodile()
        {
            if (
                (Luffy.x + Luffy.w >= Crocodile.x && Luffy.x + Luffy.w < Crocodile.x + Crocodile.w && Luffy.direction == 'R')
                 || (Crocodile.x + Crocodile.w >= Luffy.x && Crocodile.x + Crocodile.w < Luffy.x + Luffy.w && Luffy.direction == 'L')
                )
            {
                return true;
            }
            return false;
        }
        void gravity()
        {
            if (Luffy.y < 593)
            {
                Luffy.FStanding = 0;
                Luffy.FJumping = 1;
                Luffy.y += Speed;
            }
            else
            {
                Speed = 15;
                if (Luffy.FWalking == 0)
                    Luffy.FStanding = 1;
                Luffy.FJumping = 0;
                countJumping = 0;
            }
            Speed += 5;

        }
        void AnimatLuffyeStanding()
        {
            Luffy.IndStanding++;
            if (Luffy.IndStanding > 1)
            {
                Luffy.IndStanding = 0;
            }

        }

        void AnimateLuffyJumping()
        {
            Luffy.IndJumping++;
            if (Luffy.IndJumping > 2)
            {
                Luffy.IndJumping = 0;


            }
            if (Luffy.y >= 593) Luffy.IndJumping = 3;

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            CreateLuffy();
            CreateCrocodile();
            CreateEnel();
            createBG();
            DrawDubb(this.CreateGraphics());
        }
        void createBG()
        {
            CAdvImgActor pnn = new CAdvImgActor();
            pnn.img = new Bitmap("origExtrabig.png");
            pnn.rcSrc = new Rectangle(XA, YA, this.ClientSize.Width, pnn.img.Height);
            pnn.rcDst = new Rectangle(XB, YB, this.ClientSize.Width, this.ClientSize.Height);
            Lbg.Add(pnn);
        }
        void UseMovement(int N, string name, List<Bitmap> M, List<Bitmap> M2, string Move, string extension)
        {
            for (int i = 0; i < N; i++)
            {
                Bitmap img = new Bitmap(name + "/LDirection/" + Move + "/" + i + "." + extension);
                M.Add(img);
                img = new Bitmap(name + "/RDirection/" + Move + "/" + i + "." + extension);
                M2.Add(img);
            }
        }

        void CreateLuffy()
        {
            Luffy.x = 50;//50
            Luffy.y = this.ClientSize.Height - 200;
            Luffy.w = 128;
            Luffy.h = 128;
            Luffy.name = "Luffy";
            //Index of Frames = 0
            Luffy.IndStanding = 0;
            Luffy.IndWalking = 0;
            Luffy.FJumping = 0;
            Luffy.FPunching1 = 0;
            //Flags
            Luffy.FStanding = 1;
            Luffy.FWalking = 0;
            Luffy.FJumping = 0;
            //Luffy Moves
            UseMovement(2, Luffy.name, Luffy.LD.LStandingFrames, Luffy.RD.LStandingFrames, "Standing", "png");
            UseMovement(4, Luffy.name, Luffy.LD.LWalkingFrames, Luffy.RD.LWalkingFrames, "Walking", "png");
            UseMovement(4, Luffy.name, Luffy.LD.LJumpingFrames, Luffy.RD.LJumpingFrames, "Jumping", "png");
            UseMovement(7, Luffy.name, Luffy.LD.LBalloonFrames, Luffy.RD.LBalloonFrames, "Balloon", "png");
            UseMovement(6, Luffy.name, Luffy.LD.LPunching1Frames, Luffy.RD.LPunching1Frames, "Punching", "png");
            UseMovement(3, Luffy.name, Luffy.LD.LFallingFrames, Luffy.RD.LFallingFrames, "Falling", "png");
            //HP - Heal Power - Lives of Luffy
            CreateLuffyHP(Luffy);
        }
        void CreateCrocodile()
        {
            Crocodile.x = this.ClientSize.Width - this.ClientSize.Width / 4;
            Crocodile.y = this.ClientSize.Height - 260;
            Crocodile.w = 128;
            Crocodile.h = 181;
            Crocodile.direction = 'L';
            Crocodile.name = "Crocodile";
            //Index of Frames = 0
            Crocodile.IndStanding = 0;
            //Flags
            Crocodile.FStanding = 1;
            //Crocodile Moves
            UseMovement(4, Crocodile.name, Crocodile.LD.LStandingFrames, Crocodile.RD.LStandingFrames, "Standing", "png");
            UseMovement(8, Crocodile.name, Crocodile.LD.LFallingFrames, Crocodile.RD.LFallingFrames, "Falling", "png");
            UseMovement(5, Crocodile.name, Crocodile.LD.LPunching1Frames, Crocodile.RD.LPunching1Frames, "Punching", "png");
            UseMovement(8, Crocodile.name, Crocodile.LD.LWalkingFrames, Crocodile.RD.LWalkingFrames, "Walking", "png");
            CreateCrocodileHP(Crocodile);
        }
        void CreateEnel()
        {
            Enel.x = this.ClientSize.Width / 5;
            Enel.y = this.ClientSize.Height / 10;
            Enel.w = 130;
            Enel.h = 158;
            Enel.direction = 'L';
            Enel.name = "Enel";
            //Index of Frames = 0
            Enel.IndPunching1 = 0;
            //Flags
            Enel.FPunching1 = 1;
            //Enel Moves
            UseMovement(6, Enel.name, Enel.LD.LPunching1Frames, Enel.RD.LPunching1Frames, "Lightning", "png");

        }
        public void CreateLuffyHP(Hero H)
        {
            CActor pnn = new CActor();
            pnn.x = 150;
            pnn.y = 50;
            pnn.w = 300;
            pnn.w2 = 300;
            pnn.h = 30;
            pnn.cl = Color.Black;
            pnn.cl2 = Color.Red;
            pnn.p = new Pen(pnn.cl);
            pnn.sb = new SolidBrush(pnn.cl2);
            H.HP = pnn;
        }
        public void CreateCrocodileHP(Hero H)
        {
            CActor pnn = new CActor();
            pnn.x = Crocodile.x;
            pnn.y = Crocodile.y - 50;
            pnn.w = 200;
            pnn.w2 = 200;
            pnn.h = 20;
            pnn.cl = Color.Black;
            pnn.cl2 = Color.Gold;
            pnn.p = new Pen(pnn.cl);
            pnn.sb = new SolidBrush(pnn.cl2);
            H.HP = pnn;

        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Luffy.FStanding = 1;
            Luffy.FWalking = 0;
            Luffy.FJumping = 0;
            Luffy.FPunching1 = 0;
            Crocodile.FStanding = 1;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            gravity();
            //if (Luffy.HP.w2 <= 0)
            //{
            //    MessageBox.Show("Game Over");
            //}
            AnimateCrocodile();
            Crocodile.IndStanding++;
            if (Crocodile.IndStanding > 3)
            {
                Crocodile.IndStanding = 0;
            }
            switch (e.KeyCode)
            {
                //Balloon
                case Keys.Z:
                    Luffy.FBalloon = 1;
                    Luffy.FStanding = 0;
                    Luffy.FWalking = 0;
                    Luffy.FJumping = 0;
                    Luffy.IndBalloon = 1;
                    break;
                //Move Right
                case Keys.Right:
                    Luffy.direction = 'R';
                    Luffy.FStanding = 0;
                    if (Luffy.FJumping == 0)
                    {
                        Luffy.FWalking = 1;
                        if (Luffy.x + this.ClientSize.Width / 4 > this.ClientSize.Width
                            && Lbg[0].rcSrc.X + 15 + this.ClientSize.Width < Lbg[0].img.Width)
                        {
                            Lbg[0].rcSrc.X += 15;
                            Crocodile.x -= 15;
                        }
                        else
                        {
                            if (Luffy.x + Luffy.w + 15 < this.ClientSize.Width)
                                Luffy.x += 15;
                        }
                        if (Luffy.IndWalking < 3)
                        {
                            Luffy.IndWalking++;
                        }
                        else
                        {
                            Luffy.IndWalking = 0;
                        }
                    }
                    break;
                //Move Left
                case Keys.Left:
                    Luffy.direction = 'L';
                    Luffy.FStanding = 0;
                    if (Luffy.FJumping == 0)
                    {
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
                    }

                    break;
                //Jump Up
                case Keys.W:
                    if (countJumping < 2)
                    {
                        countJumping++;
                        Luffy.FStanding = 0;
                        Luffy.FJumping = 1;
                        Luffy.y -= 105;
                    }
                    break;
                //Jump Right
                case Keys.D:
                    if (countJumping < 2)
                    {
                        Luffy.direction = 'R';
                        countJumping++;
                        Luffy.FStanding = 0;
                        Luffy.FJumping = 1;
                        if (Luffy.x + 105 < this.ClientSize.Width * 3 / 4)
                        {
                            Luffy.y -= 105; Luffy.x += 105;
                        }

                    }
                    break;
                //Jump Left
                case Keys.A:
                    if (countJumping < 2)
                    {
                        Luffy.direction = 'L';
                        countJumping++;
                        Luffy.FStanding = 0;
                        Luffy.FJumping = 1;
                        if (Luffy.x > 105)
                        {
                            Luffy.y -= 105; Luffy.x -= 105;
                        }
                    }
                    break;
                //Punching 1
                case Keys.C:
                    Luffy.FStanding = 0;
                    Luffy.FPunching1 = 1;
                    if (Luffy.IndPunching1 < 5)
                        Luffy.IndPunching1++;
                    else Luffy.IndPunching1 = 0;
                    if (Luffy.IndPunching1 != 0 && Luffy.IndPunching1 != 3)
                    {
                        if (
                        (Luffy.x + Luffy.w >= Crocodile.x && Luffy.x + Luffy.w < Crocodile.x + Crocodile.w && Luffy.direction == 'R')
                        || (Crocodile.x + Crocodile.w >= Luffy.x && Crocodile.x + Crocodile.w < Luffy.x + Luffy.w && Luffy.direction == 'L')
                        )
                        {
                            Crocodile.FStanding = 0;
                            Crocodile.FFalling = 1;
                            Crocodile.FWalking = 0;
                            Crocodile.IndFalling = 0;
                            for (int i = 0; i < 7; i++)
                            {
                                DrawDubb(this.CreateGraphics());
                                Crocodile.IndFalling++;
                            }
                            Crocodile.HP.w2 -= 20;
                            if (Luffy.x < Crocodile.x)
                                Crocodile.x += 50;
                            else Crocodile.x -= 50;
                        }
                        Crocodile.FWalking = 1;
                        //Crocodile.FStanding = 1;
                        Crocodile.FFalling = 0;
                    }

                    break;
                case Keys.F:

                    break;

            }
            DrawDubb(this.CreateGraphics());
            Crocodile.IndWalking++;

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
            int sizeBalloon = 300;
            g2.Clear(Color.White);
            DrawLAdvImages(g2, Lbg);
            Font F = new Font("Arial", 38, FontStyle.Regular);
            SolidBrush SB = new SolidBrush(Color.Black);
            g2.DrawString(Luffy.name, F, SB, Luffy.HP.x - 140, Luffy.HP.y - 16);
            g2.DrawRectangle(Luffy.HP.p, Luffy.HP.x, Luffy.HP.y, Luffy.HP.w, Luffy.HP.h);
            g2.FillRectangle(Luffy.HP.sb, Luffy.HP.x + 1, Luffy.HP.y + 1, Luffy.HP.w2 - 1, Luffy.HP.h - 1);
            //Crocodile HP
            g2.DrawRectangle(Crocodile.HP.p, Crocodile.x, Crocodile.HP.y, Crocodile.HP.w, Crocodile.HP.h);
            g2.FillRectangle(Crocodile.HP.sb, Crocodile.x + 1, Crocodile.HP.y + 1, Crocodile.HP.w2 - 1, Crocodile.HP.h - 1);
            //
            //g2.DrawRectangle(Luffy.HP.p, Luffy.x, Luffy.y, Luffy.w, Luffy.y);
            DrawCharacter(g2, 2, Luffy.x, Luffy.y, Luffy.x, Luffy.y, 128, 128, Luffy.RD.LStandingFrames, Luffy.LD.LStandingFrames, Luffy.direction, Luffy.FStanding, Luffy.IndStanding);
            DrawCharacter(g2, 4, Luffy.x, Luffy.y, Luffy.x, Luffy.y, 128, 128, Luffy.RD.LWalkingFrames, Luffy.LD.LWalkingFrames, Luffy.direction, Luffy.FWalking, Luffy.IndWalking);
            DrawCharacter(g2, 4, Luffy.x, Luffy.y, Luffy.x, Luffy.y, 128, 128, Luffy.RD.LJumpingFrames, Luffy.LD.LJumpingFrames, Luffy.direction, Luffy.FJumping, Luffy.IndJumping);
            DrawCharacter(g2, 7, Luffy.x, Luffy.y - 100, Luffy.x - 148, Luffy.y - 100, sizeBalloon, sizeBalloon, Luffy.RD.LBalloonFrames, Luffy.LD.LBalloonFrames, Luffy.direction, Luffy.FBalloon, Luffy.IndBalloon);
            DrawCharacter(g2, 6, Luffy.x - 20, Luffy.y - 15, Luffy.x - 22, Luffy.y - 15, 190, 190, Luffy.RD.LPunching1Frames, Luffy.LD.LPunching1Frames, Luffy.direction, Luffy.FPunching1, Luffy.IndPunching1);
            DrawCharacter(g2, 3, Luffy.x - 30, Luffy.y - 30, Luffy.x - 20, Luffy.y - 30, 172, 172, Luffy.RD.LFallingFrames, Luffy.LD.LFallingFrames, Luffy.direction, Luffy.FFalling, Luffy.IndFalling);
            //Crocodile
            if (IsCrocodileAlive)
            {
                DrawCharacter(g2, 4, Crocodile.x, Crocodile.y, Crocodile.x, Crocodile.y, 200, 200, Crocodile.RD.LStandingFrames, Crocodile.LD.LStandingFrames, Crocodile.direction, Crocodile.FStanding, Crocodile.IndStanding);
                DrawCharacter(g2, 8, Crocodile.x, Crocodile.y, Crocodile.x, Crocodile.y, 200, 200, Crocodile.RD.LFallingFrames, Crocodile.LD.LFallingFrames, Crocodile.direction, Crocodile.FFalling, Crocodile.IndFalling);
                DrawCharacter(g2, 5, Crocodile.x, Crocodile.y, Crocodile.x, Crocodile.y, 200, 200, Crocodile.RD.LPunching1Frames, Crocodile.LD.LPunching1Frames, Crocodile.direction, Crocodile.FPunching1, Crocodile.IndPunching1);
                DrawCharacter(g2, 8, Crocodile.x, Crocodile.y, Crocodile.x, Crocodile.y, 200, 200, Crocodile.RD.LWalkingFrames, Crocodile.LD.LWalkingFrames, Crocodile.direction, Crocodile.FWalking, Crocodile.IndWalking);
            }
            else
            {
                if (Crocodile.direction == 'R')
                {
                    Bitmap img = Crocodile.RD.LFallingFrames[3];
                    g2.DrawImage(img, Crocodile.x, Crocodile.y);
                }
                else
                {
                    Bitmap img = Crocodile.LD.LFallingFrames[3];
                    g2.DrawImage(img, Crocodile.x, Crocodile.y);
                }
            }

            //Enel
            DrawCharacter(g2, 6, Enel.x, Enel.y, Enel.x, Enel.y, 200, 200, Enel.RD.LStandingFrames, Enel.LD.LPunching1Frames, Enel.direction, Enel.FPunching1, Enel.IndPunching1);
        }
        void DrawLAdvImages(Graphics g2, List<CAdvImgActor> Limg)
        {
            for (int i = 0; i < Limg.Count; i++)
            {
                CAdvImgActor bg = Limg[i];
                g2.DrawImage(bg.img, bg.rcDst, bg.rcSrc, GraphicsUnit.Pixel);
            }
        }
        void DrawCharacter(Graphics g2, int N, int x, int y, int x2, int y2, int Width, int Height, List<Bitmap> Limgs, List<Bitmap> Limgs2, Char Direction, int Flag, int IndFrame)
        {


            if (Direction == 'R')
            {
                if (Flag == 1)
                    DrawDir(Limgs, x, y, Width, Height, IndFrame, Flag, N, g2);

            }
            else if (Direction == 'L')
            {

                if (Flag == 1)
                    DrawDir(Limgs2, x2, y2, Width, Height, IndFrame, Flag, N, g2);

            }
        }
        void DrawDir(List<Bitmap> Limgs, int x, int y, int w, int h, int iFrame, int F, int N, Graphics g2)
        {

            if (F == 1)
            {
                if (iFrame < N)
                {
                    Pen p = new Pen(Color.Red);
                    Bitmap ptr = Limgs[iFrame];
                    g2.DrawImage(ptr, x, y, w, h);
                }

            }
        }
    }
}
