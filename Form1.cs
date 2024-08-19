namespace WF_Lessons_23_24
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            model = new Model();
            model.PlayerShips[0, 0] = CoordStatus.Ship;
            model.PlayerShips[5, 2] = CoordStatus.Ship;
            model.PlayerShips[5, 3] = CoordStatus.Ship;
            model.PlayerShips[5, 4] = CoordStatus.Ship;

            model.PlayerShips[7, 3] = CoordStatus.Ship;
        }

        Model model;

        private void btnTest_Click(object sender, EventArgs e)
        {

            model.LastShot = model.Shot(txtBoxCoord.Text);
            int x = int.Parse(txtBoxCoord.Text.Substring(0, 1));
            int y = int.Parse(txtBoxCoord.Text.Substring(1));
            switch (model.LastShot)
            {
                case ShotStatus.Miss:
                    model.EnemyShips[x, y] = CoordStatus.Shot; break;
                case ShotStatus.Wounded:
                    model.EnemyShips[x, y] = CoordStatus.Got; break;
                case ShotStatus.Kill:
                    model.EnemyShips[x, y] = CoordStatus.Got; break;
            }
            if (model.LastShot == ShotStatus.Wounded)
            {
                model.LastShotCoord = txtBoxCoord.Text;
                model.WoundedStatus = true;
            }
            MessageBox.Show(model.LastShot.ToString());

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s;
            int x, y;
            do
            {
                s = model.ShotGen();
                x = int.Parse(s.Substring(0, 1));
                y = int.Parse(s.Substring(1));

            }
            while (model.EnemyShips[x, y] != CoordStatus.None);


            txtBoxCoord.Text = s;
        }

        private void button102_Click(object sender, EventArgs e)
        {
            //var b = this.Controls.Find("b", true);

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    string name = "b" + x.ToString() + y.ToString();
                    var b = this.Controls.Find(name, true);
                    if (b.Count() > 0)
                    {
                        var btn = b[0];
                        switch (model.PlayerShips[x, y])
                        {
                            case CoordStatus.None:
                                btn.Text = "";
                                break;
                            case CoordStatus.Ship:
                                btn.Text = "x";
                                break;
                            case CoordStatus.Got:
                                btn.Text = "k";
                                break;
                            case CoordStatus.Shot:
                                btn.Text = "o";
                                break;
                        }

                    }
                }
            }

        }

        private void button103_Click(object sender, EventArgs e)
        {
            Direction direction;
            ShipType shipType = ShipType.x1;

            if (checkBox1.Checked)
                direction = Direction.Vertical;
            else
                direction = Direction.Horizontal;

            if (radioButton1.Checked)
                shipType = ShipType.x1;
            if (radioButton2.Checked)
                shipType = ShipType.x2;
            if (radioButton3.Checked)
                shipType = ShipType.x3;
            if (radioButton4.Checked)
                shipType = ShipType.x4;

            model.AddDelShip(txtBoxCoord.Text, shipType, direction, checkBox2.Checked);

            button102_Click(sender, e);


        }
    }
}