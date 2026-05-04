
namespace TP8
{
    public partial class TP8 : Form
    {
        Action action { get; set; }
        Modele modele;
        private ZoneDessin zoneDessin;

        private int longueur = 1200;
        private int largeur = 900;

        private Button _outilActif = null;


        public TP8()
        {
            InitializeComponent();

            sauvegarderToolStripMenuItem.Click += sauvegarderToolStripMenuItem_Click;
            ouvrirToolStripMenuItem.Click += ouvrirToolStripMenuItem_Click;

            modele = new Modele();
            zoneDessin = new ZoneDessin(modele);
            Controls.Add(zoneDessin);


            modele.ajouterForme(new Rectangle(new Point(50, 50), 100, 50, Color.Red, 0, false));

            modele.ajouterForme(new Disque(new Point(200, 100), 40, Color.Green, 0, false));

            modele.ajouterForme(new Droite(new Point(300, 50), new Point(350, 150), Color.Blue, 0, true));
            this.Size = new Size(longueur, largeur);
        }

        private void SelectionnerOutil(Button bouton)
        { // pour mettre en surbrillance le bouton de l'outil sélectionné
            if (_outilActif != null)
            {
                _outilActif.BackColor = SystemColors.Control;
                _outilActif.FlatAppearance.BorderColor = SystemColors.ControlDark;
            }

            _outilActif = bouton;
            bouton.BackColor = Color.LightSteelBlue;
            bouton.FlatAppearance.BorderColor = Color.SteelBlue;
        }

        private void SelectionnerCouleur(Button bouton)
        { // pour mettre en surbrillance le bouton de la couleur sélectionnée
            if (_outilActif != null)
            {
                _outilActif.FlatAppearance.BorderColor = Color.Black;
            }

            _outilActif = bouton;
            bouton.FlatAppearance.BorderColor = Color.LightSteelBlue;
        }


        private void button1_Click(object sender, EventArgs e)
        { // deplacer objet
            modele.setAction(Action.deplacer);
            SelectionnerOutil(button1);
            zoneDessin.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        { // rectangle
            modele.setAction(Action.creerRectangle);
            SelectionnerOutil(button2);
            zoneDessin.Invalidate();

        }

        private void button3_Click(object sender, EventArgs e)
        { // disque
            modele.setAction(Action.creerDisque);
            SelectionnerOutil(button3);
            zoneDessin.Invalidate();

        }

        private void button4_Click(object sender, EventArgs e)
        { // droite
            modele.setAction(Action.creerDroite);
            SelectionnerOutil(button4);
            zoneDessin.Invalidate();

        }

        private void button5_Click(object sender, EventArgs e)
        { // pinceau
            modele.setAction(Action.dessiner);
            SelectionnerOutil(button5);
            zoneDessin.Invalidate();

        }

        private void button6_Click(object sender, EventArgs e) // Rouge
        { 
            zoneDessin.Couleur = Color.Red;
            modele.setCouleurSelection(Color.Red);
            SelectionnerCouleur(button6);
            zoneDessin.Invalidate();
        }

        private void button7_Click(object sender, EventArgs e) // vert
        {
            zoneDessin.Couleur = Color.Green;
            modele.setCouleurSelection(Color.Green);
            SelectionnerCouleur(button7);
            zoneDessin.Invalidate();

        }

        private void button8_Click(object sender, EventArgs e) // bleu
        {
            zoneDessin.Couleur = Color.Blue;
            modele.setCouleurSelection(Color.Blue);
            SelectionnerCouleur(button8);
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
            SelectionnerCouleur(button9);
            zoneDessin.Invalidate();

        }

        private void button10_Click(object sender, EventArgs e) // gris
        {
            zoneDessin.Couleur = Color.Gray;
            modele.setCouleurSelection(Color.Gray);
            SelectionnerCouleur(button10);
            zoneDessin.Invalidate();


        }

        private void button12_Click(object sender, EventArgs e) // noir
        {
            zoneDessin.Couleur = Color.Black;
            modele.setCouleurSelection(Color.Black);
            SelectionnerCouleur(button12);
            zoneDessin.Invalidate();

        }

        private void button11_Click(object sender, EventArgs e) // white
        {
            zoneDessin.Couleur = Color.White;
            modele.setCouleurSelection(Color.White);
            SelectionnerCouleur(button11);
            zoneDessin.Invalidate();
        }

        private void button13_Click(object sender, EventArgs e)
        { // selection
            modele.setAction(Action.selectionner);
            SelectionnerOutil(button13);
            zoneDessin.Invalidate();

        }



        private void button14_Click(object sender, EventArgs e)
        {
            modele.supprimer();
            zoneDessin.Invalidate();
            SelectionnerOutil(button14);
            zoneDessin.Update(); // on met ça pour que ça affiche la suppression direct et pas une fois qu'on a cliqué autre part
        }

        private void button15_Click(object sender, EventArgs e)
        { // texte
            modele.setAction(Action.creerTexte);
            SelectionnerOutil(button15);
            zoneDessin.Invalidate();
        }

        private void button16_Click(object sender, EventArgs e)
        { // z avant
            modele.setZFactorSelectionAvant();
            SelectionnerOutil(button16);
            zoneDessin.Invalidate();

        }

        private void button17_Click(object sender, EventArgs e)
        { // z arrière
            modele.setZFactorSelectionArriere();
            SelectionnerOutil(button17);
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


        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            zoneDessin.setFill(true);
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            zoneDessin.setFill(false);
        }


        private void button19_Click(object sender, EventArgs e)
        {
            modele.dupliquerSelection();
            modele.setAction(Action.deplacer);
            SelectionnerOutil(button19);
            zoneDessin.Invalidate();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        { // 1:1
            zoneDessin.resize(450, 450);
            zoneDessin.replace(new Point((longueur - 450) / 2, (largeur - 450) / 2));
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        { // 2:1
            zoneDessin.resize(800, 400);
            zoneDessin.replace(new Point((longueur - 800) / 2, (largeur - 400) / 2));
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        { // 3:2
            zoneDessin.resize(750, 500);
            zoneDessin.replace(new Point((longueur - 750) / 2, (largeur - 500) / 2));
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        { // 4:3
            zoneDessin.resize(800, 600);
            zoneDessin.replace(new Point((longueur - 800) / 2, (largeur - 600) / 2));

        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        { // 16:9
            zoneDessin.resize(800, 450);
            zoneDessin.replace(new Point((longueur - 800) / 2, (largeur - 450) / 2));
        }

        private void exporterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var dialog = new SaveFileDialog();
            dialog.Filter = "PNG (*.png)|*.png|JPEG (*.jpg)|*.jpg|BMP (*.bmp)|*.bmp";
            dialog.DefaultExt = "png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var bitmap = new Bitmap(zoneDessin.Width, zoneDessin.Height);
                zoneDessin.DrawToBitmap(bitmap, new System.Drawing.Rectangle(0, 0, zoneDessin.Width, zoneDessin.Height));

                string ext = Path.GetExtension(dialog.FileName).ToLower();
                var format = ext switch
                {
                    ".jpg" => System.Drawing.Imaging.ImageFormat.Jpeg,
                    ".bmp" => System.Drawing.Imaging.ImageFormat.Bmp,
                    _ => System.Drawing.Imaging.ImageFormat.Png
                };

                bitmap.Save(dialog.FileName, format);
                MessageBox.Show("Export réussi !");
            }
        }
    }
}
