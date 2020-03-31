using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace FrankieGOL
{
    public partial class frmGameofLife : Form
    {
        Grid grid;

        Bitmap GameArea;
        Graphics graphics;

        public frmGameofLife()
        {
            InitializeComponent();
            grid = new Grid(50, 50);
            backgroundWorker.WorkerSupportsCancellation = true;
            GameArea = new Bitmap(pbGrid.Size.Width, pbGrid.Size.Height);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if(!backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync();
            }

        }

        private void pbGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if(!backgroundWorker.IsBusy)
            {
                this.Invoke((Action)(() =>
                {
                    var point = pbGrid.PointToClient(Cursor.Position);
                    int x = point.X / (pbGrid.Width / grid.Width + 1);
                    int y = point.Y / (pbGrid.Height / grid.Height + 1);
                    grid.flipPoint(x, y);
                    DrawGrid();
                }));
            }
        }

        private void backgroundWorker_DoWork_1(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker.CancellationPending)
            {
                grid.NextStep();
                DrawGrid();
                System.GC.Collect();
            }
        }

        private void btnStop_Click(object sender, EventArgs e) => backgroundWorker.CancelAsync();

        private void btnClear_Click(object sender, EventArgs e)
        {
            if(!backgroundWorker.IsBusy)
            {
                this.Invoke((Action)(() =>
                {
                    grid.clear();
                    DrawGrid();
                }));
            }
        }

        private void DrawGrid()
        {
            this.Invoke((Action)(() =>
            {
                pbGrid.Image = GameArea;
                graphics = Graphics.FromImage(GameArea);
            }));

            SolidBrush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(Color.Black);

            float w = (pbGrid.Width / grid.Width + 1);
            float h = (pbGrid.Height / grid.Height + 1);

            for(int i = 0; i < grid.Width; i++)
            {
                for(int j = 0; j < grid.Height; j++)
                {
                    if(grid.IsActive(i, j))
                    {
                        brush.Color = Color.Aqua;
                    }
                    else
                    {
                        brush.Color = Color.Black;
                    }
                    Rectangle rec = new Rectangle((int)(i * w), (int)(j * h), (int)w, (int)h);
                    graphics.FillRectangle(brush, rec);
                    graphics.DrawRectangle(pen, rec);
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(!backgroundWorker.IsBusy)
            {
                grid.NextStep();
                DrawGrid();
            }
        }

        private void frmGameofLife_Load(object sender, EventArgs e) => DrawGrid();
    }
}
