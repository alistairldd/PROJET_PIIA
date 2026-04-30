using System.Text.Json;
using System.Text.Json.Nodes;

namespace TP8
{
    internal static class Serialisation
    {
        public static void Sauvegarder(Modele modele, string chemin)
        {
            var tableau = new JsonArray();

            for (int i = 0; i < modele.getNombreFormes(); i++)
            {
                var f = modele.getFormeId(i);
                var obj = new JsonObject();

                Color c = f.getCouleur();
                obj["couleur"] = $"{c.R},{c.G},{c.B}";

                if (f is Rectangle r)
                {
                    obj["type"] = "Rectangle";
                    obj["x"] = r.Position.X;
                    obj["y"] = r.Position.Y;
                    obj["largeur"] = r.Largeur;
                    obj["hauteur"] = r.Hauteur;
                }
                else if (f is Disque d)
                {
                    obj["type"] = "Disque";
                    obj["x"] = d.Position.X;
                    obj["y"] = d.Position.Y;
                    obj["rayon"] = d.getRayon();
                }
                else if (f is Droite dr)
                {
                    obj["type"] = "Droite";
                    obj["x1"] = dr.PointDebut.X;
                    obj["y1"] = dr.PointDebut.Y;
                    obj["x2"] = dr.PointFin.X;
                    obj["y2"] = dr.PointFin.Y;
                }
                else if (f is Dessin des)
                {
                    obj["type"] = "Dessin";
                    var points = new JsonArray();
                    foreach (var p in des.getPoints())
                    {
                        var pt = new JsonObject();
                        pt["x"] = p.X;
                        pt["y"] = p.Y;
                        points.Add(pt);
                    }
                    obj["points"] = points;
                }
                else if (f is Texte t)
                {
                    obj["type"] = "Texte";
                    obj["x"] = t.Position.X;
                    obj["y"] = t.Position.Y;
                    obj["contenu"] = t.Contenu;
                    obj["taille"] = t.Police.Size;
                }

                tableau.Add(obj);
            }

            string json = tableau.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(chemin, json);
        }

        public static void Charger(Modele modele, string chemin)
        {
            string json = File.ReadAllText(chemin);
            var tableau = JsonNode.Parse(json)!.AsArray();

            // Vider le modele
            modele.clearSelection();
            while (modele.getNombreFormes() > 0)
                modele.supprimerForme(modele.getFormeId(0));

            foreach (var node in tableau)
            {
                var obj = node!.AsObject();
                string type = obj["type"]!.GetValue<string>();

                // Lire la couleur
                string[] rgb = obj["couleur"]!.GetValue<string>().Split(',');
                Color couleur = Color.FromArgb(int.Parse(rgb[0]), int.Parse(rgb[1]), int.Parse(rgb[2]));

                FormeGeo forme;

                switch (type)
                {
                    case "Rectangle":
                        int rx = obj["x"]!.GetValue<int>();
                        int ry = obj["y"]!.GetValue<int>();
                        int rl = obj["largeur"]!.GetValue<int>();
                        int rh = obj["hauteur"]!.GetValue<int>();
                        forme = new Rectangle(new Point(rx, ry), rl, rh, couleur, 0);
                        break;

                    case "Disque":
                        int dx = obj["x"]!.GetValue<int>();
                        int dy = obj["y"]!.GetValue<int>();
                        int dr = obj["rayon"]!.GetValue<int>();
                        forme = new Disque(new Point(dx, dy), dr, couleur, 0);
                        break;

                    case "Droite":
                        int x1 = obj["x1"]!.GetValue<int>();
                        int y1 = obj["y1"]!.GetValue<int>();
                        int x2 = obj["x2"]!.GetValue<int>();
                        int y2 = obj["y2"]!.GetValue<int>();
                        forme = new Droite(new Point(x1, y1), new Point(x2, y2), couleur, 0);
                        break;

                    case "Dessin":
                        var points = obj["points"]!.AsArray();
                        var premierPt = points[0]!.AsObject();
                        var dessin = new Dessin(
                            new Point(premierPt["x"]!.GetValue<int>(), premierPt["y"]!.GetValue<int>()),
                            couleur, 0);
                        for (int i = 1; i < points.Count; i++)
                        {
                            var pt = points[i]!.AsObject();
                            dessin.AjouterPoint(new Point(pt["x"]!.GetValue<int>(), pt["y"]!.GetValue<int>()));
                        }
                        forme = dessin;
                        break;

                    case "Texte":
                        int tx = obj["x"]!.GetValue<int>();
                        int ty = obj["y"]!.GetValue<int>();
                        string contenu = obj["contenu"]!.GetValue<string>();
                        float taille = obj["taille"]!.GetValue<float>();
                        var texte = new Texte(new Point(tx, ty), contenu, couleur, 0);
                        texte.Police = new Font("Arial", taille);
                        forme = texte;
                        break;

                    default:
                        continue;
                }

                modele.ajouterForme(forme);
            }
        }
    }
}