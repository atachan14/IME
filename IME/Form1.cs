namespace IME
{
    public partial class Form1 : Form
    {
        private Dictionary<string, List<Button>> buttonGroups = new Dictionary<string, List<Button>>();

        private void InitializeButtons()
        {
            
            // •K—v‚É‰ž‚¶‚Ä’Ç‰Á
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button24_Click(object sender, EventArgs e)
        {

        }

        private void openUI(object sender, MouseEventArgs e)
        {
            b7_8.Visible = true;
            b7_6.Visible = true;
            b7_4.Visible = true;
            b7_2.Visible = true;
        }

        private void closeUI(object sender, MouseEventArgs e)
        {
            b7_8.Visible = false;
            b7_6.Visible = false;
            b7_4.Visible = false;
            b7_2.Visible = false;
        }
    }
}
