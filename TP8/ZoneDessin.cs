using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP8;

namespace TP8
{
    internal class ZoneDessin : Control
    {

        private Modele modele;
        Point lastMousePosition;
        FormeGeo formeSelection;
        Point positionInitiale;

        private bool isDragging;
        private Rectangle? rectPreview;
        private Disque? disquePreview;
        private Droite? droitePreview;
        private Dessin? polylignePreview;

        Color couleur = Color.Black;

        public Color Couleur
        {
            get => couleur;
            set => couleur = value;
        }

        public ZoneDessin(Modele modele)
        {
            this.Location = new Point(10, 10);
            this.Size = new Size(400, 400);
            this.modele = modele;
            this.DoubleBuffered = true;

        }



        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            for (int i = 0; i < modele.getNombreFormes(); i++)
            {
                FormeGeo forme = modele.getFormeId(i);
                Color color = forme.getCouleur();

                using var brush = new SolidBrush(color);
                using var pen = new Pen(color, 2);

                if (forme is Rectangle rect)
                {
                    e.Graphics.DrawRectangle(pen, rect.Position.X, rect.Position.Y, rect.Largeur, rect.Hauteur);
                }
                else if (forme is Disque disque)
                {
                    e.Graphics.DrawEllipse(pen, disque.Position.X - disque.getRayon(), disque.Position.Y - disque.getRayon(), disque.getRayon() * 2, disque.getRayon() * 2);
                }
                else if (forme is Droite droite)
                {
                    e.Graphics.DrawLine(pen, droite.PointDebut, droite.PointFin);
                }
                else if (forme is Dessin dessin)
                {
                    var pts = dessin.getPoints().ToArray();
                    if (pts.Length > 1)
                        e.Graphics.DrawLines(pen, pts);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (rectPreview is not null)
            {
                using var fill = new SolidBrush(Color.FromArgb(80, Color.Black));
                using var border = new Pen(Color.FromArgb(180, Color.Black), 2);
                var r = rectPreview;
                e.Graphics.FillRectangle(fill, r.Position.X, r.Position.Y, r.Largeur, r.Hauteur);
                e.Graphics.DrawRectangle(border, r.Position.X, r.Position.Y, r.Largeur, r.Hauteur);
            }

            if (disquePreview is not null)
            {
                using var fill = new SolidBrush(Color.FromArgb(80, Color.Black));
                using var border = new Pen(Color.FromArgb(180, Color.Black), 2);
                var d = disquePreview;
                e.Graphics.FillEllipse(fill, d.Position.X - d.getRayon(), d.Position.Y - d.getRayon(), d.getRayon() * 2, d.getRayon() * 2);
                e.Graphics.DrawEllipse(border, d.Position.X - d.getRayon(), d.Position.Y - d.getRayon(), d.getRayon() * 2, d.getRayon() * 2);
            }

            if (droitePreview is not null)
            {
                using var pen = new Pen(Color.FromArgb(180, Color.Black), 2);
                var dr = droitePreview;
                e.Graphics.DrawLine(pen, dr.PointDebut, dr.PointFin);
            }

            if (polylignePreview != null)
            {
                var pts = polylignePreview.getPoints().ToArray();
                if (pts.Length > 1)
                    e.Graphics.DrawLines(Pens.Black, pts);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            var action = modele.getAction();

            if (action == Action.creerRectangle)
            {
                positionInitiale = e.Location;
                isDragging = true;
                rectPreview = null;
                disquePreview = null;
                droitePreview = null;
            }
            else if (action == Action.creerDisque)
            {
                positionInitiale = e.Location;
                isDragging = true;
                rectPreview = null;
                disquePreview = null;
                droitePreview = null;
            }
            else if (action == Action.creerDroite)
            {
                positionInitiale = e.Location;
                isDragging = true;
                rectPreview = null;
                disquePreview = null;
                droitePreview = null;
            }
            else if (action == Action.dessiner)
            {
                polylignePreview = new Dessin(e.Location, couleur, 0);
                isDragging = true;
                Invalidate();
            }

            else if (action == Action.deplacer)
            {
                for (int i = 0; i < modele.getNombreFormes(); i++)
                {
                    if (modele.getFormeId(i).ContientPoint(e.Location))
                    { 
                        formeSelection = modele.getFormeId(i);
                        lastMousePosition = e.Location;
                        break;
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            // ajouter la forme dans le modele
            // faut un form temporaire
            
            Action action = modele.getAction();

            if (isDragging && action == Action.creerRectangle && rectPreview is not null)
            {
                modele.ajouterForme(rectPreview);
            }

            if (isDragging && action == Action.creerDisque && disquePreview is not null)
            {
                modele.ajouterForme(disquePreview);
            }

            if (isDragging && action == Action.creerDroite && droitePreview is not null)
            {

                modele.ajouterForme(droitePreview);
            }

            if (isDragging && action == Action.dessiner && polylignePreview is not null)
            {
                modele.ajouterForme(polylignePreview);

            }

            isDragging = false;
            rectPreview = null;
            disquePreview = null;
            droitePreview = null;
            polylignePreview = null;
            formeSelection = null;
            Invalidate();

        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            var action = modele.getAction();

            if (isDragging && action == Action.creerRectangle)
            {
                int left = Math.Min(positionInitiale.X, e.X);
                int top = Math.Min(positionInitiale.Y, e.Y);
                int width = Math.Abs(e.X - positionInitiale.X);
                int height = Math.Abs(e.Y - positionInitiale.Y);

                rectPreview = (width > 0 && height > 0)
                    ? new Rectangle(new Point(left, top), width, height, couleur, 0)
                    : null;

                Invalidate();
                return;
            }

            if (isDragging && action == Action.creerDisque)
            {
                int dx = e.X - positionInitiale.X;
                int dy = e.Y - positionInitiale.Y;
                int rayon = (int)Math.Sqrt(dx * dx + dy * dy);

                disquePreview = (rayon > 0)
                    ? new Disque(positionInitiale, rayon, couleur, 0)
                    : null;

                Invalidate();
                return;
            }

            if (isDragging && action == Action.creerDroite)
            {
                droitePreview = new Droite(positionInitiale, e.Location, couleur, 0);
                Invalidate();
                return;
            }

            if (isDragging && action == Action.dessiner)
            {
                if (polylignePreview != null)
                {
                    polylignePreview.AjouterPoint(e.Location);
                    Invalidate();
                }
            }

            if (formeSelection != null)
            {
                int deltaX = e.X - lastMousePosition.X;
                int deltaY = e.Y - lastMousePosition.Y;

                if (formeSelection is Droite droite)
                {
                    droite.PointDebut = new Point(droite.PointDebut.X + deltaX, droite.PointDebut.Y + deltaY);
                    droite.PointFin = new Point(droite.PointFin.X + deltaX, droite.PointFin.Y + deltaY);
                }

                if (formeSelection is Dessin dessin)
                {
                    for (int i = 0; i < dessin.getPoints().Count; i++)
                    {
                        dessin.getPoints()[i] = new Point(dessin.getPoints()[i].X + deltaX, dessin.getPoints()[i].Y + deltaY);
                    }
                }

                else
                {
                    formeSelection.X += deltaX;
                    formeSelection.Y += deltaY;
                }

                lastMousePosition = e.Location;
                this.Invalidate();
            }
        }
    }
}