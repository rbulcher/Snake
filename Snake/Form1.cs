using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using sanek.Properties;

namespace sanek
{
    public partial class Snake : Form
    {
        private const int SnakeSpeed = 40;
        private Point _direction;
        private bool _fruitInGame;
        private bool _inGame;
        private readonly ArrayList _snakeBody = new ArrayList();
        private PictureBox _fruit;
        private bool _colorYellow;

        public Snake()
        {
            InitializeComponent();
            button2.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _inGame = true;
            StartGame();
        }

        private void StartGame()
        {
            _colorYellow = false;
            button1.Visible = false;
            button2.Visible = true;
            _direction = new Point(SnakeSpeed, 0);
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_inGame)
            {
                MoveSnake();
                if(!_fruitInGame)
                    AddFruit();
            }
        }

        private Image randomFruit()
        {
            var r = new Random();
            var select = r.Next(3);
            Image randomFruit;

            switch (select)
            {
                case 0:
                    randomFruit = Resources.banana;
                    break;
                case 1:
                    randomFruit = Resources.strawb;
                    break;
                case 2:
                    randomFruit = Resources.waterMelon;
                    break;
                default:
                    randomFruit = Resources.banana;
                    break;
            }
            return randomFruit;
        }

        private void AddFruit()
        {
                _fruitInGame = true;
                var r = new Random();
                var x = r.Next(Width - 80);
                var y = r.Next(Height - 80);
                _fruit = new PictureBox
                {
                    Location = new Point(x, y),
                    BackgroundImage = randomFruit(),
                    BackgroundImageLayout = BackgroundImageLayout = ImageLayout.Stretch,
                    Size = new Size(40, 40),
                    Text = @"fruit"
                };
                Controls.Add(_fruit);
        }

        private void MoveSnake()
        {
            var lastPos = snakeHead.Location;

            snakeHead.Location = new Point(snakeHead.Location.X + _direction.X, snakeHead.Location.Y + _direction.Y);
            CheckCollision();
            foreach (PictureBox bodyPiece in _snakeBody)
            {
                var curPos = bodyPiece.Location;
                bodyPiece.Location = lastPos;
                lastPos = curPos;
            }
        }

        private void CheckCollision()
        {
            CollisionFruit();
            if (_snakeBody.Cast<PictureBox>().Any(body => snakeHead.Bounds.IntersectsWith(body.Bounds)) || (snakeHead.Location.X + 40 >= Width || snakeHead.Location.X <= 0 || snakeHead.Location.Y + 40 >= Height ||
                snakeHead.Location.Y <= 0))
            {
                _inGame = false;
                timer1.Stop();
                MessageBox.Show(@"You died.");
                Reset();
                
            }
        }

        private void CollisionFruit()
        {
            if (_fruitInGame)
            {
                if (snakeHead.Bounds.IntersectsWith(_fruit.Bounds))
                {
                    _snakeBody.Add(AddBody());
                    this.Controls.Remove(_fruit);
                    
                    _fruitInGame = false;
                }
            }
        }

        private PictureBox AddBody()
        {
            _colorYellow = !_colorYellow;
            PictureBox body;
            if (_colorYellow)
            {
                body = new PictureBox { BackgroundImage = Resources.blackCircle, Text = @"body", Size = new Size(40, 40), BackgroundImageLayout = ImageLayout.Stretch, BackColor = Color.Transparent };
                this.Controls.Add(body);

            }
            else
            {
                 body = new PictureBox { BackgroundImage = Resources.yellowCircle, Text = @"body", Size = new Size(40, 40), BackgroundImageLayout = ImageLayout.Stretch, BackColor = Color.Transparent };
                this.Controls.Add(body);
            }
           
            return body;
        }

        private void Snake_KeyDown(object sender, KeyEventArgs e)
        {
            if (_inGame)
                switch (e.KeyCode)
                {
                    case Keys.W:
                        _direction = new Point(0, -SnakeSpeed);
                        break;
                    case Keys.A:
                        _direction = new Point(-SnakeSpeed, 0);
                        break;
                    case Keys.S:
                        _direction = new Point(0, SnakeSpeed);
                        break;
                    case Keys.D:
                        _direction = new Point(SnakeSpeed, 0);
                        break;
                }
        }

        private void Reset()
        {
            timer1.Stop();
            foreach (PictureBox bodyPiece in _snakeBody)
            {
                this.Controls.Remove(bodyPiece);
                bodyPiece.Dispose();
            }
            this.Controls.Remove(_fruit);
            snakeHead.Location = new Point(350, 200);
            _fruitInGame = false;
            _snakeBody.Clear();
            _inGame = false;
            button2.Visible = false;
            button1.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Reset();
        }
    }
}