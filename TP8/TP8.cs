
namespace TP8
{
    public partial class TP8 : Form
    {
        Action action { get; set; }
        Modele modele;
        private ZoneDessin zoneDessin;

        private int longueur = 1200;
        private int largeur = 900;



        public TP8()
        {
            InitializeComponent();

            sauvegarderToolStripMenuItem.Click += sauvegarderToolStripMenuItem_Click;
            ouvrirToolStripMenuItem.Click += ouvrirToolStripMenuItem_Click;

            modele = new Modele();
            zoneDessin = new ZoneDessin(modele);
            Controls.Add(zoneDessin);


            modele.ajouterForme(new Rectangle(new Point(50, 50), 100, 50, Color.Red, 0));

            modele.ajouterForme(new Disque(new Point(200, 100), 40, Color.Green, 0));

            modele.ajouterForme(new Droite(new Point(300, 50), new Point(350, 150), Color.Blue, 0));

            this.Size = new Size(longueur, largeur);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            modele.setAction(Action.deplacer);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            modele.setAction(Action.creerRectangle);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            modele.setAction(Action.creerDisque);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            modele.setAction(Action.creerDroite);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            modele.setAction(Action.dessiner);
        }

        private void button6_Click(object sender, EventArgs e) // Rouge
        {
            zoneDessin.Couleur = Color.Red;
            modele.setCouleurSelection(Color.Red);
            zoneDessin.Invalidate();
        }

        private void button7_Click(object sender, EventArgs e) // vert
        {
            zoneDessin.Couleur = Color.Green;
            modele.setCouleurSelection(Color.Green);
            zoneDessin.Invalidate();

        }

        private void button8_Click(object sender, EventArgs e) // bleu
        {
            zoneDessin.Couleur = Color.Blue;
            modele.setCouleurSelection(Color.Blue);
            zoneDessin.Invalidate();

        }

        private void button9_Click(object sender, EventArgs e) // truc de colordialog (doc microsoft)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = false;



            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                zoneDessin.Couleur = MyDialog.Color;
                modele.setCouleurSelection(MyDialog.Color);
            }

            zoneDessin.Invalidate();

        }

        private void button10_Click(object sender, EventArgs e) // gris
        {
            zoneDessin.Couleur = Color.Gray;
            modele.setCouleurSelection(Color.Gray);
            zoneDessin.Invalidate();


        }

        private void button12_Click(object sender, EventArgs e) // noir
        {
            zoneDessin.Couleur = Color.Black;
            modele.setCouleurSelection(Color.Black);
            zoneDessin.Invalidate();

        }

        private void button11_Click(object sender, EventArgs e) // white
        {
            zoneDessin.Couleur = Color.White;
            modele.setCouleurSelection(Color.White);
            zoneDessin.Invalidate();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            modele.setAction(Action.selectionner);
        }



        private void button14_Click(object sender, EventArgs e)
        {
            modele.supprimer();
            zoneDessin.Invalidate();
            zoneDessin.Update(); // on met ça pour que ça affiche la suppression direct et pas une fois qu'on a cliqué autre part
        }

        private void button15_Click(object sender, EventArgs e)
        {
            modele.setAction(Action.creerTexte);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            modele.setZFactorSelectionAvant();
            zoneDessin.Invalidate();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            modele.setZFactorSelectionArriere();
            zoneDessin.Invalidate();
        }


        // Tout ça en dessous c'est pour le toolstrip (la barre en haut)

        private void fichierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // y'a rien mais je le mets sinon ça marche pas 
        }

        private void sauvegarderToolStripMenuItem_Click(object sender, EventArgs e)
        { // pour sauvegarder
            using var dialog = new SaveFileDialog();
            dialog.Filter = "Fichier TP8 (*.tp8)|*.tp8";
            dialog.DefaultExt = "tp8";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Serialisation.Sauvegarder(modele, dialog.FileName);
                MessageBox.Show("Sauvegarde réussie !");
            }
        }

        private void ouvrirToolStripMenuItem_Click(object sender, EventArgs e)
        { // pour ouvrir
            using var dialog = new OpenFileDialog();
            dialog.Filter = "Fichier TP8 (*.tp8)|*.tp8";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Serialisation.Charger(modele, dialog.FileName);
                zoneDessin.Invalidate();
            }
        }

        private void accessibilitéToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // y'a rien mais je le mets sinon ça marche pas
        }

        // les radio button qui servent à resize la zone de dessin

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            zoneDessin.resize(450, 450);
            zoneDessin.replace(new Point((longueur-450)/2, (largeur-450)/2));

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            zoneDessin.resize(800, 450);
            zoneDessin.replace(new Point((longueur-800)/2, (largeur-450)/2));
            zoneDessin.Invalidate();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            zoneDessin.resize(800, 600);
            zoneDessin.replace(new Point((longueur - 800) / 2, (largeur - 600) / 2));

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            zoneDessin.resize(750, 500);
            zoneDessin.replace(new Point((longueur - 750) / 2, (largeur - 500) / 2));


        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            zoneDessin.resize(800, 400);
            zoneDessin.replace(new Point((longueur - 800) / 2, (largeur - 400) / 2));

        }
    }
}
