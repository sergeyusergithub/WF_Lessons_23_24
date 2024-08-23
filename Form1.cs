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

            for (int i = 0; i < 10; i++)
            {
                dGVEnemyShips.Rows.Add(row);
                //dGVEnemyShips.ColumnAdded += i.ToString();


            }

            dGVEnemyShips.ClearSelection();

        }

        Model model;
        string[] row = { "", "", "", "", "", "", "", "", "", "" };
        
        int x4 = 1;
        int x3 = 2;
        int x2 = 3;
        int x1 = 4;

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
            button102_Click(sender, e);


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
                                btn.Text = "w";
                                break;
                            case CoordStatus.Shot:
                                btn.Text = "o";
                                break;
                        }

                    }
                }
            }


            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {

                    switch (model.EnemyShips[x, y])
                    {
                        case CoordStatus.None:
                            dGVEnemyShips[x, y].Value = "";
                            break;
                        case CoordStatus.Ship:
                            dGVEnemyShips[x, y].Value = "x";
                            break;
                        case CoordStatus.Got:
                            dGVEnemyShips[x, y].Value = "w";
                            break;
                        case CoordStatus.Shot:
                            dGVEnemyShips[x, y].Value = "o";
                            break;
                    }


                }
            }

        }

        private void button103_Click(object sender, EventArgs e)
        {
            int cnt = dGVEnemyShips.SelectedCells.Count;
            if (cnt > 0)
            {
                if (checkBox2.Checked)
                {
                    int a, b;
                    a = dGVEnemyShips.SelectedCells[0].RowIndex;
                    b = dGVEnemyShips.SelectedCells[0].ColumnIndex;

                    if (dGVEnemyShips.Rows[a].Cells[b].Value.ToString() == "")
                        return;
                }
                if (cnt == 1)
                    if (!checkBox2.Checked)
                    {
                        if (x1 == 0) return;
                        x1--;
                        
                    }
                    else
                        x1++;
                if (cnt == 2)
                    if (!checkBox2.Checked)
                    {
                        if (x2 == 0) return;
                        x2--;
                        
                    }
                    else
                        x2++;
                if (cnt == 3)
                    if (!checkBox2.Checked)
                    {
                        if (x3 == 0) return;
                        x3--;
                        
                    }
                    else
                        x3++;
                if (cnt == 4)
                    if (!checkBox2.Checked)
                    {
                        if (x4 == 0) return;
                        x4--;
                        
                    }
                    else
                        x4++;


                for (int i = 0; i < dGVEnemyShips.SelectedCells.Count; i++)
                {
                    int x = dGVEnemyShips.SelectedCells[i].ColumnIndex;
                    int y = dGVEnemyShips.SelectedCells[i].RowIndex;
                    CoordStatus coordStatus;
                    if (!checkBox2.Checked)
                    {
                        coordStatus = CoordStatus.Ship;
                    }
                    else
                    {
                        coordStatus = CoordStatus.None;
                    }
                    model.PlayerShips[x, y] = coordStatus;

                    
                }
                dGVEnemyShips.ClearSelection();


            }
            else
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

            }
            button102_Click(sender, e);

        }

        private void dGVEnemyShips_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int x = dGVEnemyShips.SelectedCells[0].ColumnIndex;
            int y = dGVEnemyShips.SelectedCells[0].RowIndex;

            txtBoxCoord.Text = x.ToString() + y.ToString();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                button103.Text = "Удалить";
            }
            else
            {
                button103.Text = "Поставить";

            }

        }

        private void dGVEnemyShips_SelectionChanged(object sender, EventArgs e)
        {
            int cnt = dGVEnemyShips.SelectedCells.Count;
            txtBoxCoord.Text = cnt.ToString();
            if( cnt > 4)
            {
                MessageBox.Show("Превышено количество клеток!");
                int x = dGVEnemyShips.SelectedCells[cnt - 1].ColumnIndex;
                int y = dGVEnemyShips.SelectedCells[0].RowIndex;
                dGVEnemyShips.Rows[y].Cells[x].Selected = false;
                dGVEnemyShips.ClearSelection();

            }
        }
    }
}