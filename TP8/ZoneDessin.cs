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
        private bool isDraggingSelection = false;
        private Rectangle? rectPreview;
        private Disque? disquePreview;

        private System.Drawing.Rectangle selectionRect; // rectangle de sélection visuel
        private bool isSelecting = false;               // on est en train de dessiner la sélection
        private List<FormeGeo> formesSelectionnees = new List<FormeGeo>();


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
                if (forme is Rectangle)
                {
                    Rectangle rect = (Rectangle)forme;
                    e.Graphics.FillRectangle(Brushes.Blue, rect.Position.X, rect.Position.Y, rect.Largeur, rect.Hauteur);
                }
                else if (forme is Disque)
                {
                    Disque disque = (Disque)forme;
                    e.Graphics.FillEllipse(Brushes.Red, disque.Position.X - disque.Rayon, disque.Position.Y - disque.Rayon, disque.Rayon * 2, disque.Rayon * 2);
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
                using var fill = new SolidBrush(Color.FromArgb(80, Color.Navy));
                using var border = new Pen(Color.FromArgb(180, Color.Navy), 2);
                var r = rectPreview;
                e.Graphics.FillRectangle(fill, r.Position.X, r.Position.Y, r.Largeur, r.Hauteur);
                e.Graphics.DrawRectangle(border, r.Position.X, r.Position.Y, r.Largeur, r.Hauteur);
            }

            if (disquePreview is not null)
            {
                using var fill = new SolidBrush(Color.FromArgb(80, Color.DarkRed));
                using var border = new Pen(Color.FromArgb(180, Color.DarkRed), 2);
                var d = disquePreview;
                e.Graphics.FillEllipse(fill, d.Position.X - d.Rayon, d.Position.Y - d.Rayon, d.Rayon * 2, d.Rayon * 2);
                e.Graphics.DrawEllipse(border, d.Position.X - d.Rayon, d.Position.Y - d.Rayon, d.Rayon * 2, d.Rayon * 2);
            }

            if (isSelecting && selectionRect.Width > 0 && selectionRect.Height > 0)
            {
                using var pen = new Pen(Color.DodgerBlue, 1);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                using var fill = new SolidBrush(Color.FromArgb(30, Color.DodgerBlue));
                e.Graphics.FillRectangle(fill, selectionRect);
                e.Graphics.DrawRectangle(pen, selectionRect);
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
            }
            else if (action == Action.creerDisque)
            {
                positionInitiale = e.Location;
                isDragging = true;
                rectPreview = null;
                disquePreview = null;
            }
            else if (action == Action.deplacer)
            {
                bool clicSurSelection = modele.getFormesSelectionnees()
                    .Any(f => f.ContientPoint(e.Location));

                if (clicSurSelection)
                {
                    lastMousePosition = e.Location;
                    formeSelection = null;
                    isDraggingSelection = true; // ← on commence vraiment à glisser
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
                    ? new Rectangle(new Point(left, top), width, height)
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
                    ? new Disque(positionInitiale, rayon)
                    : null;

                Invalidate();
                return;
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

            // calcul des dimensions (déplacement)
            if (formeSelection != null)
            {
                int deltaX = e.X - lastMousePosition.X;
                int deltaY = e.Y - lastMousePosition.Y;
                formeSelection.X += deltaX;
                formeSelection.Y += deltaY;
                lastMousePosition = e.Location;
                this.Invalidate();
            }

            if (isDraggingSelection && modele.getFormesSelectionnees().Count > 0)
            {
                int deltaX = e.X - lastMousePosition.X;
                int deltaY = e.Y - lastMousePosition.Y;
                foreach (var f in modele.getFormesSelectionnees())
                {
                    f.X += deltaX;
                    f.Y += deltaY;
                }
                lastMousePosition = e.Location;
                Invalidate();
            }
        }
    }
}