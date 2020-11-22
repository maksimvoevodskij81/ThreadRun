using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace threadRace2
{
    public partial class Form1 : Form
    {
        static public AutoResetEvent finish = new AutoResetEvent(false);
        List<Hero> heroes = new List<Hero>();
     
        public Form1()
        {
            InitializeComponent();
            button5.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;   
            this.KeyPreview = true;
            button2.Enabled = false;
            heroes.Add(new Hero(pictureBox1, pictureBox1.Location.X, button2.Location.X,1));
            heroes.Add(new Hero(pictureBox2, pictureBox2.Location.X, button2.Location.X,2));
            heroes.Add(new Hero(pictureBox3, pictureBox3.Location.X, button2.Location.X,3));
            label7.Text = heroes[0].step.ToString();
            label6.Text = heroes[0].sleep.ToString();
            label5.Text = heroes[0].score.ToString();
            label8.Text = (heroes[0].HP / 5).ToString();
            label10.Visible = false;
            label10.Enabled = false;
            label9.Visible = false;
            label9.Enabled = false;
        }
        void resetRace()
        {
            foreach (Hero hero in heroes)
            {
                hero.thread.Abort();
            }
        }
        void FinishObserver()
        {
            finish.WaitOne();
            resetRace();
            foreach (Hero hero in heroes)
            {
                if (hero.isFinish )
                {
                    MessageBox.Show(hero.Name);
                    if (hero.Name=="Flash - 1")
                    {
                        hero.score += 10;
                    }
                    hero.isFinish = false;
                }
              
            }
            foreach (Hero hero in heroes)
            {
                hero.Reset();
            }
            label5.Text = heroes[0].score.ToString();
            button1.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            button5.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button1.Enabled = false;
       
            finish  = new AutoResetEvent(false);
            Thread threadObserver = new Thread(FinishObserver);
            threadObserver.IsBackground = true;
            threadObserver.Start();
         
            foreach (Hero hero in heroes)
            {
                hero.thread.Start();
            }
          
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
           
            if (heroes[0].HP >= 5)
            {
                if (e.KeyCode == Keys.Space)
                {
                    heroes[0].box.Invoke(new Action(()=>  heroes[0].box.Location = new Point(heroes[0].box.Location.X + heroes[0].step * heroes[0].forsage, heroes[0].box.Location.Y)));
                    heroes[0].HP -= 5;
                    if (heroes[0].HP < 20)
                    {
                        label10.Visible = false;
                       
                    }
                     if (heroes[0].HP < 15)
                    {
                        label9.Visible = false;
                        
                    }
                     if (heroes[0].HP < 10) 
                    {
                        la.Visible = false;
                        
                    }
                     if (heroes[0].HP < 5)
                    {
                        label1.Visible = false;
                      
                    }
                }
                label8.Text = (heroes[0].HP / 5).ToString();
            }


        }
        private void button4_Click(object sender, EventArgs e)
        {
            if ( heroes[0].score>=20)
            {
                heroes[0].step += 1;
                heroes[0].score -= 20;
                label7.Text = heroes[0].step.ToString();
            }
            label5.Text = heroes[0].score.ToString();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (heroes[0].sleep > 10 && heroes[0].score >= 30)
            {
                heroes[0].sleep -= 5;
                heroes[0].score -= 30;
                label6.Text = heroes[0].sleep.ToString();
            }
            label5.Text = heroes[0].score.ToString();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (heroes[0].HP <= 20 && heroes[0].score >= 30)
            {
                heroes[0].HP += 5;
                heroes[0].score -= 30;
               
            }
            if (heroes[0].HP == 10)
            {
                la.Visible = true;

            }
            if (heroes[0].HP == 5)
            {
                label1.Visible = true;

            }
            if (heroes[0].HP== 20)
            {
                label10.Visible = true;
               
            }
            if (heroes[0].HP == 15)
            {
                label9.Visible = true;
              
            }
            label5.Text = heroes[0].score.ToString();
            label8.Text = (heroes[0].HP / 5).ToString();
        }
        class Hero
        {
            static Random random = new Random();
            public string Name { get; set; }
            public bool isFinish { get; set; }
            public int step { get; set; }
            public  int sleep { get; set; }
            public int score { get; set; }
            public Thread thread { get; set; }
            public PictureBox box { get; set; }
            public int finishX { get; set; }
            public int StartX { get; set; }
            int i = 0;
            public int HP { get; set; }
            public int forsage { get; set; }
         
            
            public Hero(PictureBox _box, int _startX, int _finishX,int i)
            {
               
                Name = "Flash - " + i;
                box = _box;
                StartX = _startX;
                finishX = _finishX;
                thread = new Thread(Run);
                thread.IsBackground = true;
                sleep = random.Next(30, 200);
                step = 1;
                isFinish = false;
                forsage = 50;
                HP = 10;
                score = 0;
            }
            public  void Reset()
            {
                thread = new Thread(Run);
                isFinish = false;
               box.Invoke(new Action(()=> box.Location = new Point(StartX,box.Location.Y)));
              
            }
            void Run()
            {
                while (true)
                { 
                    box.Invoke(new Action(() => box.Location = new Point(box.Location.X + step, box.Location.Y)));
                    if (box.Location.X+box.Width/2 > finishX)
                    {
                        Form1.finish.Set();
                        isFinish = true;
                    }
                    Thread.Sleep(sleep);
                }

            }
        }

        
    }
}
