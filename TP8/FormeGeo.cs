using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP8
{
    public abstract class FormeGeo
    {
        private Point position;
        public Point Position
        {
            get => position;
            set
            {
                position = value;
                X = value.X;
                Y = value.Y;
            }
        }

        public int X
        {
            get => position.X;
            set
            {
                position.X = value;
            }
        }

        public int Y
        {
            get => position.Y;
            set
            {
                position.Y = value;
            }
        }
        public FormeGeo(Point p)
        {
            this.Position = p;
        }

        public abstract bool ContientPoint(Point p);
    }
}