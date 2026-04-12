using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP8
{
    internal class Modele
    {
        private List<FormeGeo> formes = new List<FormeGeo>();
        private Action actionEnCours;
        public FormeGeo getFormeId(int id)
        {
            if (id < 0 || id >= formes.Count)
            {
                throw new ArgumentOutOfRangeException("id", "L'identifiant de la forme est hors de portée.");
            }
            return formes[id];
        }

        public List<FormeGeo> getFormes()
        {
            return formes;
        }

        public int getNombreFormes()
        {
            return formes.Count;
        }

        public void ajouterForme(FormeGeo forme)
        {
            formes.Add(forme);
            Console.WriteLine("Forme ajoutée : " + forme.GetType().Name);

        }

        public Action getAction()
        {
            return actionEnCours;
        }

        public void setAction(Action action)
        {
            this.actionEnCours = action;
        }
    }

    
    public class Rectangle : FormeGeo
    {
        private double longueur;
        private double hauteur;

        public int Largeur { get; set; }
        public int Hauteur { get; set; }

        public Rectangle(Point point, int largeur, int hauteur) : base(point)
        {
            this.Hauteur = hauteur;
            this.Largeur = largeur;
        }

        public Rectangle(Point p, double longueur, double hauteur) : base(p)
        {
            this.longueur = longueur;
            this.hauteur = hauteur;
        }

        public override bool ContientPoint(Point p)
        {
            return (p.X >= this.Position.X && p.X <= this.Position.X + Largeur) && (p.Y >= this.Position.Y && p.Y <= this.Position.Y + Hauteur);
        }
    }

    public class Disque : FormeGeo
    {
        public int Rayon { get; set; }

        public Disque(Point centre, int rayon) : base(centre)
        {
            Rayon = rayon;
        }

        public override bool ContientPoint(Point p)
        {
            int dx = p.X - this.Position.X;
            int dy = p.Y - this.Position.Y;
            return dx * dx + dy * dy <= Rayon * Rayon;
        }
    }
}
