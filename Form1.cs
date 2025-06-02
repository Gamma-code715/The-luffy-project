using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_luffy_project
{
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

        // left key -> L
        // run -> check direction(L) -> left Running list
        public char direction = 'L'; // either L or R
        public bool IsMoving = false;
        public bool IsRunning = false;

        public Moves LD = new Moves();
        public Moves RD = new Moves();

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
    }


    public partial class Form1 : Form
    {
        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            this.Load += Form1_Load;
            this.Paint += Form1_Paint;
        }

        Hero Luffy = new Hero();

        Bitmap off; // background

        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            CreateHero();
        }

        void CreateHero()
        {
            Luffy.x = 50;
            Luffy.y = 50;
            Luffy.w = 64;
            Luffy.h = 64;
            
            for (int i = 0; i < 7; i++)
            {
                Bitmap img = new Bitmap("Luffy/LDirection/Balloon/" + i + ".png");

                Luffy.LD.LBalloonFrames.Add(img);
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
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

            if (Luffy.direction == 'R')
            {
                DrawDir(Luffy.RD, Luffy.x, Luffy.y, Luffy.w, Luffy.h, g2);
            }
            else if (Luffy.direction == 'L')
            {
                DrawDir(Luffy.LD, Luffy.x, Luffy.y, Luffy.w, Luffy.h, g2);
            }
        }

        void DrawDir(Moves DirObject, int x, int y, int w, int h,Graphics g2)
        {
            int gap = 0;
            Pen P = new Pen(Color.Black);
            for (int i = 0; i < DirObject.LBalloonFrames.Count; i++)
            {
                Bitmap ptr = DirObject.LBalloonFrames[i];
                g2.DrawImage(ptr, 0 + (ptr.Width * i), y, w, h);
                g2.DrawRectangle(P, 0 + (ptr.Width * i), y, w, h);
                gap += 200;
            }
        }
    }
}
