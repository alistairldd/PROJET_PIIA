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

        private Color Couleur { get; set; }
        private int ZFactor { get; set; }

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
        public FormeGeo(Point p, Color couleur, int zFactor)
        {
            this.Position = p;
            this.Couleur = couleur;
            this.ZFactor = zFactor;
        }

        public abstract bool ContientPoint(Point p);

        public abstract Color getCouleur();
        public abstract void setCouleur(Color couleur);
    }
}