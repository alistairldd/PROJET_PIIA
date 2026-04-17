
namespace TP8
{
    public partial class TP8 : Form
    {
        Action action { get; set; }
        Modele modele;
        private ZoneDessin zoneDessin;
        public TP8()
        {
            InitializeComponent();

            modele = new Modele();
            zoneDessin = new ZoneDessin(modele);
            Controls.Add(zoneDessin);


            modele.ajouterForme(new Rectangle(new Point(50, 50), 100, 50, Color.Red, 0));

            modele.ajouterForme(new Disque(new Point(200, 100), 40, Color.Green, 0));

            modele.ajouterForme(new Droite(new Point(300, 50), new Point(350, 150), Color.Honeydew, 0));
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
        }

        private void button7_Click(object sender, EventArgs e) // vert
        {
            zoneDessin.Couleur = Color.Green;

        }

        private void button8_Click(object sender, EventArgs e) // bleu
        {
            zoneDessin.Couleur = Color.Blue;

        }

        private void button9_Click(object sender, EventArgs e) // jaune truc
        {
            zoneDessin.Couleur = Color.FromArgb(192, 192, 0);

        }

        private void button10_Click(object sender, EventArgs e) // canard
        {
            zoneDessin.Couleur = Color.Teal;

        }

        private void button12_Click(object sender, EventArgs e) // noir
        {
            zoneDessin.Couleur = Color.Black;

        }

        private void button11_Click(object sender, EventArgs e) // white
        {
            zoneDessin.Couleur = Color.White;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            modele.setAction(Action.selectionner);
        }
    }
}
