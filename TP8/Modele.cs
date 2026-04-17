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
        private List<Dessin> dessins = new List<Dessin>();

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

        private List<FormeGeo> formesSelectionnees = new List<FormeGeo>();

        public List<FormeGeo> getFormesSelectionnees()
        {
            return formesSelectionnees;
        }

        public void setSelection(List<FormeGeo> formes)
        {
            formesSelectionnees = formes;
        }

        public void clearSelection()
        {
            formesSelectionnees.Clear();
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

    private List<FormeGeo> formesSelectionnees = new List<FormeGeo>();

        public List<FormeGeo> getFormesSelectionnees() => formesSelectionnees;

        public void setSelection(List<FormeGeo> formes)
        {
            formesSelectionnees = formes;
        }

        public void clearSelection()
        {
            formesSelectionnees.Clear();
        }

    }

    public class Droite : FormeGeo
    {
        public Point PointDebut { get; set; }

        public Point PointFin { get; set; }

        public Droite(Point pointDebut, Point pointFin) : base(pointDebut)
        {
            PointDebut = pointDebut;
            PointFin = pointFin;
        }

        public override bool ContientPoint(Point p)
        {
            // Calcul de la distance du point p au segment [PointDebut, PointFin]
            double dx = PointFin.X - PointDebut.X;
            double dy = PointFin.Y - PointDebut.Y;
            double longueur = Math.Sqrt(dx * dx + dy * dy);

            if (longueur == 0) return false;
            // Projection du point p sur la droite
            double t = ((p.X - PointDebut.X) * dx + (p.Y - PointDebut.Y) * dy) / (longueur * longueur);

            // Vérifie si la projection est sur le segment
            if (t < 0.0 || t > 1.0)
                return false;

            // Coordonnées du point projeté
            double projX = PointDebut.X + t * dx;
            double projY = PointDebut.Y + t * dy;

            // Distance entre p et la projection
            double distance = Math.Sqrt((p.X - projX) * (p.X - projX) + (p.Y - projY) * (p.Y - projY));

            // Tolérance pour "contenir" le point
            return distance <= 5;
        }
    }

    public class Dessin : FormeGeo
    {
        public List<Point> Points { get; } = new List<Point>();

        public Dessin(Point start) : base(start)
        {
            Points.Add(start);
        }

        public void AjouterPoint(Point p)
        {
            Points.Add(p);
        }

        public override bool ContientPoint(Point p)
        {
            // Teste si le point est proche d'un segment de la polyligne
            for (int i = 1; i < Points.Count; i++)
            {
                var a = Points[i - 1];
                var b = Points[i];
                double dx = b.X - a.X;
                double dy = b.Y - a.Y;
                double longueur = Math.Sqrt(dx * dx + dy * dy);
                if (longueur == 0) continue;
                double t = ((p.X - a.X) * dx + (p.Y - a.Y) * dy) / (longueur * longueur);
                if (t < 0.0 || t > 1.0) continue;
                double projX = a.X + t * dx;
                double projY = a.Y + t * dy;
                double distance = Math.Sqrt((p.X - projX) * (p.X - projX) + (p.Y - projY) * (p.Y - projY));
                if (distance <= 5) return true;
            }
            return false;
        }
    }

}
