using System;
using System.Collections.Generic;
using System.Drawing;
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
        private bool isDraggingSelection = false;
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

        private System.Drawing.Rectangle selectionRect; // rectangle de sélection visuel
        private bool isSelecting = false;               // on est en train de dessiner la sélection
        private List<FormeGeo> formesSelectionnees = new List<FormeGeo>();

        //pour la taille
        private const int TAILLE_POIGNEE = 8;
        private int poigneeActive = -1; // index de la poignée en cours de drag (-1 = aucune)
        private FormeGeo? formeRedimensionnee = null;


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
        
            

            // Surligner les formes sélectionnées
            foreach (var f in modele.getFormesSelectionnees())
            {
                using var pen = new Pen(Color.Yellow, 2);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                if (f is Rectangle r)
                    e.Graphics.DrawRectangle(pen, r.Position.X, r.Position.Y, r.Largeur, r.Hauteur);
                else if (f is Disque d)
                    e.Graphics.DrawEllipse(pen, d.Position.X - d.Rayon, d.Position.Y - d.Rayon, d.Rayon * 2, d.Rayon * 2);
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

            if (isSelecting && selectionRect.Width > 0 && selectionRect.Height > 0)
            {
                using var pen = new Pen(Color.DodgerBlue, 1);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                using var fill = new SolidBrush(Color.FromArgb(30, Color.DodgerBlue));
                e.Graphics.FillRectangle(fill, selectionRect);
                e.Graphics.DrawRectangle(pen, selectionRect);
            }

            // Poignées si une seule forme est sélectionnée
            var selection = modele.getFormesSelectionnees();
            if (selection.Count == 1)
            {
                var poignees = GetPoignees(selection[0]);
                foreach (var p in poignees)
                {
                    e.Graphics.FillRectangle(Brushes.White, p);
                    e.Graphics.DrawRectangle(Pens.Black, p);
                }
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
                bool clicSurSelection = modele.getFormesSelectionnees()
                    .Any(f => f.ContientPoint(e.Location));

                // Vérifier si on clique sur une poignée
                var sel = modele.getFormesSelectionnees();
                if (sel.Count == 1)
                {
                    var poignees = GetPoignees(sel[0]);
                    for (int i = 0; i < poignees.Length; i++)
                    {
                        if (poignees[i].Contains(e.Location))
                        {
                            poigneeActive = i;
                            formeRedimensionnee = sel[0];
                            lastMousePosition = e.Location;
                            return; // on gère le resize, pas le déplacement
                        }
                    }
                }

                if (clicSurSelection)
                {
                    lastMousePosition = e.Location;
                    formeSelection = null;
                    isDraggingSelection = true; //  on commence vraiment à glisser
                }
                else
                {
                    modele.clearSelection();
                    isDraggingSelection = false;
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
            else if (action == Action.selectionner)
            {
                positionInitiale = e.Location;
                isSelecting = true;
                selectionRect = System.Drawing.Rectangle.Empty;
                formesSelectionnees.Clear();
                modele.clearSelection();
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

            if (isSelecting && action == Action.selectionner)
            {
                isSelecting = false;
                // Trouver toutes les formes dans le rectangle
                var selection = new List<FormeGeo>();
                for (int i = 0; i < modele.getNombreFormes(); i++)
                {
                    var f = modele.getFormeId(i);
                    // APRÈS (vérifie toute la forme)
                    if (f is Rectangle r)
                    {
                        var boundsR = new System.Drawing.Rectangle(r.Position.X, r.Position.Y, r.Largeur, r.Hauteur);
                        if (selectionRect.IntersectsWith(boundsR))
                            selection.Add(f);
                    }
                    else if (f is Disque d)
                    {
                        var boundsD = new System.Drawing.Rectangle(d.Position.X - d.Rayon, d.Position.Y - d.Rayon, d.Rayon * 2, d.Rayon * 2);
                        if (selectionRect.IntersectsWith(boundsD))
                            selection.Add(f);
                    }
                    else if (f is Droite d2)
                    {
                        int x = Math.Min(d2.PointDebut.X, d2.PointFin.X);
                        int y = Math.Min(d2.PointDebut.Y, d2.PointFin.Y);
                        int w = Math.Abs(d2.PointFin.X - d2.PointDebut.X);
                        int h = Math.Abs(d2.PointFin.Y - d2.PointDebut.Y);
                        var boundsD2 = new System.Drawing.Rectangle(x, y, Math.Max(w, 1), Math.Max(h, 1));
                        if (selectionRect.IntersectsWith(boundsD2))
                            selection.Add(f);
                    }
                    else if (f is Dessin dessin)
                    {
                        int x = dessin.Points.Min(p => p.X);
                        int y = dessin.Points.Min(p => p.Y);
                        int w = dessin.Points.Max(p => p.X) - x;
                        int h = dessin.Points.Max(p => p.Y) - y;
                        var boundsDes = new System.Drawing.Rectangle(x, y, Math.Max(w, 1), Math.Max(h, 1));
                        if (selectionRect.IntersectsWith(boundsDes))
                            selection.Add(f);
                    }
                }
                modele.setSelection(selection);
                formesSelectionnees = selection;
                selectionRect = System.Drawing.Rectangle.Empty;
                Invalidate();
                return;
            }

            isDragging = false;
            isDraggingSelection = false;
            rectPreview = null;
            disquePreview = null;
            droitePreview = null;
            polylignePreview = null;
            formeSelection = null;
            poigneeActive = -1;
            formeRedimensionnee = null;
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

            if (isSelecting && action == Action.selectionner)
            {
                int x = Math.Min(positionInitiale.X, e.X);
                int y = Math.Min(positionInitiale.Y, e.Y);
                int w = Math.Abs(e.X - positionInitiale.X);
                int h = Math.Abs(e.Y - positionInitiale.Y);
                selectionRect = new System.Drawing.Rectangle(x, y, w, h);
                Invalidate();
                return;
            }

            if (poigneeActive >= 0 && formeRedimensionnee != null)
            {
                int deltaX = e.X - lastMousePosition.X;
                int deltaY = e.Y - lastMousePosition.Y;

                if (formeRedimensionnee is Rectangle r)
                {
                    switch (poigneeActive)
                    {
                        case 0: r.X += deltaX; r.Y += deltaY; r.Largeur -= deltaX; r.Hauteur -= deltaY; break; // haut-gauche
                        case 1: r.Y += deltaY; r.Hauteur -= deltaY; break; // haut-milieu
                        case 2: r.Y += deltaY; r.Largeur += deltaX; r.Hauteur -= deltaY; break; // haut-droite
                        case 3: r.Largeur += deltaX; break; // milieu-droite
                        case 4: r.Largeur += deltaX; r.Hauteur += deltaY; break; // bas-droite
                        case 5: r.Hauteur += deltaY; break; // bas-milieu
                        case 6: r.X += deltaX; r.Largeur -= deltaX; r.Hauteur += deltaY; break; // bas-gauche
                        case 7: r.X += deltaX; r.Largeur -= deltaX; break; // milieu-gauche
                    }
                }
                else if (formeRedimensionnee is Disque d)
                {
                    // Pour le disque on ajuste juste le rayon
                    d.Rayon = Math.Max(5, d.Rayon + Math.Max(Math.Abs(deltaX), Math.Abs(deltaY)) * (deltaX + deltaY > 0 ? 1 : -1));
                }
                else if (formeRedimensionnee is Droite dr)
                {
                    if (poigneeActive == 0)
                        dr.PointDebut = new Point(dr.PointDebut.X + deltaX, dr.PointDebut.Y + deltaY);
                    else
                        dr.PointFin = new Point(dr.PointFin.X + deltaX, dr.PointFin.Y + deltaY);
                }

                lastMousePosition = e.Location;
                Invalidate();
                return;
            }

            // calcul des dimensions (déplacement)
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

            if (isDraggingSelection && modele.getFormesSelectionnees().Count > 0)
            {
                int deltaX = e.X - lastMousePosition.X;
                int deltaY = e.Y - lastMousePosition.Y;
                foreach (var f in modele.getFormesSelectionnees())
                {
                    if (f is Droite droite)
                    {
                        droite.PointDebut = new Point(droite.PointDebut.X + deltaX, droite.PointDebut.Y + deltaY);
                        droite.PointFin = new Point(droite.PointFin.X + deltaX, droite.PointFin.Y + deltaY);
                    }
                    else if (f is Dessin dessin)
                    {
                        for (int i = 0; i < dessin.Points.Count; i++)
                            dessin.Points[i] = new Point(dessin.Points[i].X + deltaX, dessin.Points[i].Y + deltaY);
                    }
                    else
                    {
                        f.X += deltaX;
                        f.Y += deltaY;
                    }
                }
                lastMousePosition = e.Location;
                Invalidate();
            }
        }

        private System.Drawing.Rectangle[] GetPoignees(FormeGeo f)
        {
            int s = TAILLE_POIGNEE;
            if (f is Rectangle r)
            {
                int x = r.Position.X, y = r.Position.Y, w = r.Largeur, h = r.Hauteur;
                return new[]
                {
            new System.Drawing.Rectangle(x - s/2,     y - s/2,     s, s), // 0 haut-gauche
            new System.Drawing.Rectangle(x + w/2-s/2, y - s/2,     s, s), // 1 haut-milieu
            new System.Drawing.Rectangle(x + w - s/2, y - s/2,     s, s), // 2 haut-droite
            new System.Drawing.Rectangle(x + w - s/2, y + h/2-s/2, s, s), // 3 milieu-droite
            new System.Drawing.Rectangle(x + w - s/2, y + h - s/2, s, s), // 4 bas-droite
            new System.Drawing.Rectangle(x + w/2-s/2, y + h - s/2, s, s), // 5 bas-milieu
            new System.Drawing.Rectangle(x - s/2,     y + h - s/2, s, s), // 6 bas-gauche
            new System.Drawing.Rectangle(x - s/2,     y + h/2-s/2, s, s), // 7 milieu-gauche
        };
            }
            else if (f is Disque d)
            {
                int cx = d.Position.X, cy = d.Position.Y, r2 = d.Rayon;
                return new[]
                {
            new System.Drawing.Rectangle(cx - s/2,      cy - r2 - s/2, s, s), // 0 haut
            new System.Drawing.Rectangle(cx + r2 - s/2, cy - s/2,      s, s), // 1 droite
            new System.Drawing.Rectangle(cx - s/2,      cy + r2 - s/2, s, s), // 2 bas
            new System.Drawing.Rectangle(cx - r2 - s/2, cy - s/2,      s, s), // 3 gauche
        };
            }
            else if (f is Droite dr)
            {
                return new[]
                {
            new System.Drawing.Rectangle(dr.PointDebut.X - s/2, dr.PointDebut.Y - s/2, s, s), // 0 début
            new System.Drawing.Rectangle(dr.PointFin.X - s/2,   dr.PointFin.Y - s/2,   s, s), // 1 fin
        };
            }
            return Array.Empty<System.Drawing.Rectangle>();
        }

        
    }
}