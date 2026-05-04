using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public void supprimerForme(FormeGeo forme)
        {
            if (formes.Remove(forme))
            {
                Console.WriteLine("Forme supprimée : " + forme.GetType().Name);
            }
            else
            {
                Console.WriteLine("La forme n'a pas été trouvée et ne peut pas être supprimée.");
            }
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

        public void supprimer()
        {
            var formesASupprimer = getFormesSelectionnees().ToList();
            foreach (var f in formesASupprimer)
                supprimerForme(f);

            clearSelection();
        }

        public void setCouleurSelection(Color couleur)
        {
            foreach (var f in formesSelectionnees)
                f.setCouleur(couleur);
        }

        public void dupliquerSelection()
        {
            var nouvelles = new List<FormeGeo>();
            foreach (var f in formesSelectionnees)
            {
                FormeGeo copie;
                if (f is Rectangle r)
                    copie = new Rectangle(new Point(r.Position.X + 10, r.Position.Y + 10), r.Largeur, r.Hauteur, r.getCouleur(), 0, false);
                else if (f is Disque d)
                    copie = new Disque(new Point(d.Position.X + 10, d.Position.Y + 10), d.getRayon(), d.getCouleur(), 0, false);
                else if (f is Droite dr)
                    copie = new Droite(new Point(dr.PointDebut.X + 10, dr.PointDebut.Y + 10), new Point(dr.PointFin.X + 10, dr.PointFin.Y + 10), dr.getCouleur(), 0, false);
                else if (f is Texte t)
                {
                    var copieTexte = new Texte(new Point(t.Position.X + 10, t.Position.Y + 10), t.Contenu, t.getCouleur(), 0, false);
                    copieTexte.Police = new Font(t.Police.FontFamily, t.Police.Size);
                    copie = copieTexte;
                }
                else if (f is Dessin des)
                {
                    var premierPt = des.getPoints()[0];
                    var copieDessin = new Dessin(new Point(premierPt.X + 10, premierPt.Y + 10), des.getCouleur(), 0, false);
                    for (int i = 1; i < des.getPoints().Count; i++)
                        copieDessin.AjouterPoint(new Point(des.getPoints()[i].X + 10, des.getPoints()[i].Y + 10));
                    copie = copieDessin;
                }
                else continue;

                formes.Add(copie);
                nouvelles.Add(copie);
            }
            formesSelectionnees = nouvelles;
        }

        public void setZFactorSelectionAvant()
        { // on vérifie pour chaque forme de la selection que la profondeur est pas déjà maximale, puisque ça sert à rien d'avoir une forme
            // avec un zfacteur de 15 si toutes les autres sont à 0 ; du coup on regarde juste le max quoi
            foreach (var f in formesSelectionnees)
            {
                int maxProfondeur = getFormes().Max(f => f.getZFactor());
                if (f.getZFactor() < maxProfondeur+1)
                    f.setZFactor(maxProfondeur + 1);

            }
        }

        public void setZFactorSelectionArriere()
        { // on vérifie pour chaque forme de la selection que la profondeur est pas déjà maximale, puisque ça sert à rien d'avoir une forme
            // avec un zfacteur de 15 si toutes les autres sont à 0 ; du coup on regarde juste le max quoi
            foreach (var f in formesSelectionnees)
            {
                int minProfondeur = getFormes().Min(f => f.getZFactor());
                if (f.getZFactor() > minProfondeur-1)
                    f.setZFactor(minProfondeur - 1);

            }
        }
    }


    public class Rectangle : FormeGeo
    {
        private double largeur;
        private double hauteur;
        private Color Couleur;

        public int Largeur { get; set; }
        public int Hauteur { get; set; }

        public Rectangle(Point p, int largeur, int hauteur, Color couleur, int zFactor, bool fill) : base(p, couleur, zFactor, fill)
        {
            Largeur = largeur;
            Hauteur = hauteur;
            Couleur = couleur;
        }



        public override bool ContientPoint(Point p)
        {
            return (p.X >= this.Position.X && p.X <= this.Position.X + Largeur) && (p.Y >= this.Position.Y && p.Y <= this.Position.Y + Hauteur);
        }

        public override Color getCouleur()
        {
            return Couleur;
        }

        public override void setCouleur(Color newCouleur)
        {
            Couleur = newCouleur;
        }




    }

    public class Disque : FormeGeo
    {
        private int Rayon { get; set; }
        private Color Couleur;

        public Disque(Point centre, int rayon, Color couleur, int zFactor, bool fill) : base(centre, couleur, zFactor, fill)
        {
            Rayon = rayon;
            Couleur = couleur;
        }

        public int getRayon()
        {
            return Rayon;
        }

        internal void setRayon(int value)
        {
            Rayon = value;
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



        public override Color getCouleur()
        {
            return Couleur;
        }

        public override void setCouleur(Color newCouleur)
        {
            Couleur = newCouleur;
        }

        
    }

    public class Droite : FormeGeo
    {
        public Point PointDebut { get; set; }

        public Point PointFin { get; set; }

        public Color Couleur { get; set; }

        public Droite(Point pointDebut, Point pointFin, Color couleur, int zFactor, bool fill) : base(pointDebut, couleur, zFactor, fill)
        {
            PointDebut = pointDebut;
            PointFin = pointFin;
            Couleur = couleur;
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

        public override Color getCouleur()
        {
            return Couleur;
        }

        public override void setCouleur(Color newCouleur)
        {
            Couleur = newCouleur;
        }
    }

    public class Dessin : FormeGeo
    {
        private List<Point> Points { get; } = new List<Point>();
        private Color Couleur;

        public Dessin(Point start, Color couleur, int zFactor, bool fill) : base(start, couleur, zFactor, fill)
        {
            Points.Add(start);
            Couleur = couleur;
        }

        public List<Point> getPoints()
        {
            return Points;
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

        public override Color getCouleur()
        {
            return Couleur;
        }

        public override void setCouleur(Color newCouleur)
        {
            Couleur = newCouleur;
        }
    }

    public class Texte : FormeGeo
    {
        public string Contenu { get; set; }
        public Font Police { get; set; }
        private Color Couleur;

        public Texte(Point position, string contenu, Color couleur, int zFactor, bool fill) : base(position, couleur, zFactor, fill)
        {
            Contenu = contenu;
            Police = new Font("Arial", 14);
            Couleur = couleur;
        }

        public override bool ContientPoint(Point p)
        {
            int largeur = (int)(Contenu.Length * Police.Size * 0.6f);
            int hauteur = (int)(Police.Size * 1.8f);
            return p.X >= Position.X && p.X <= Position.X + largeur &&
                   p.Y >= Position.Y && p.Y <= Position.Y + hauteur;
        }

        public override Color getCouleur() => Couleur;
        public override void setCouleur(Color c) => Couleur = c;


    }





}
