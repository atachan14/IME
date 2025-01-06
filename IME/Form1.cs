using System.Net.NetworkInformation;

namespace IME
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string[]> buttonDict;



        public Form1()
        {
            InitializeComponent();

            CriateButtonDict();
            SetupButtonValue();

        }

        private void CriateButtonDict()
        {
            this.buttonDict = new Dictionary<string, string[]>();
            string[] B1value = ["Ç†ÅJ", "Ç¢", "Ç§", "Ç¶", "Ç®"];
            string[] B2value = ["Ç©", "Ç´", "Ç≠", "ÇØ", "Ç±"];


            buttonDict.Add("B1", B1value);
            buttonDict.Add("B2", B2value);
        }

        private void SetupButtonValue()
        {
            B1.Text = buttonDict["B1"][0];
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

        private void B7clicked(object sender, MouseEventArgs e)
        {
            b7_8.Visible = true;
            b7_6.Visible = true;
            b7_4.Visible = true;
            b7_2.Visible = true;
        }

        private void B7awayed(object sender, MouseEventArgs e)
        {
            b7_8.Visible = false;
            b7_6.Visible = false;
            b7_4.Visible = false;
            b7_2.Visible = false;
        }
    }
}
