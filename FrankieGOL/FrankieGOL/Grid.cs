using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrankieGOL
{
    public class Grid
    {
        Point[,] world;

        public int Width
        {
            get;
            private set;
        }

        public int Height
        {
            get;
            private set;
        }

        public class Point
        {
            private bool status = false;
            private bool swap = false;
            public Point(bool status) => this.status = status;
            public void flip() => this.status = !status;
            public void deactivate() => swap = false;
            public void revive() => swap = true;
            public bool IsActive => status;
            public void refresh() => status = swap;
        }

        public Grid(int x, int y) => Initialize(x, y);
        public void clear() => Initialize(Width, Height);
        public bool IsActive(int x, int y) => world[x, y].IsActive;
        public void flipPoint(int x, int y) => world[x, y].flip();

        public void Initialize(int x, int y)
        {
            Width = x;
            Height = y;
            world = new Point[x, y];

            for(int i = 0; i < Width; i++)
            {
                for(int j = 0; j < Height; j++)
                {
                    world[i, j] = new Point(false);
                }
            }
        }

        public void NextStep(int steps = 1)
        {
            for(int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    int active = getNeighbours(x, y);

                    if(world[x, y].IsActive && (active < 2 || active > 3))
                    {
                        world[x, y].deactivate();
                    }
                    else if(active == 3)
                    {
                        world[x, y].revive();
                    }
                }
            }

            for(int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    world[x, y].refresh();
                }
            }
        }

        public int getNeighbours(int x, int y)
        {
            int active = 0;

            for(int i = -1; i <= 1; i++)
            {
                for(int j = -1; j <= 1; j++)
                {
                    if(!(i == 0 && j == 0))
                    {
                        int offsetX = (x + i);
                        int offsetY = (y + j);

                        if (offsetX == -1)
                            offsetX = Width - 1;
                        else if (offsetX == Width)
                            offsetX = 0;

                        if (offsetY == -1)
                            offsetY = Height - 1;
                        else if (offsetY == Height)
                            offsetY = 0;

                        if (world[offsetX , offsetY].IsActive)
                            active++;
                    }
                }
            }
            return active;
        }
    }
}
