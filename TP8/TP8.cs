
namespace TP8
{
    public partial class TP8 : Form
    {
        Action action { get; set; }
        Modele modele;
        public TP8()
        {
            InitializeComponent();

            modele = new Modele();
            ZoneDessin zoneDessin = new ZoneDessin(modele);
            Controls.Add(zoneDessin);


            modele.ajouterForme(new Rectangle(new Point(50, 50), 100, 50));

            modele.ajouterForme(new Disque(new Point(200, 100), 40));
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
    }
}
