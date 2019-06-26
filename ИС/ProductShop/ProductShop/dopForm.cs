using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductShop
{
    public partial class dopForm : Form
    {
        private bool anyForm;
        public bool error = true;
        public dopForm()
        {
            InitializeComponent();
        }

        private void dopForm_Load(object sender, EventArgs e)
        {
            if (Owner is Product pr)
            {
                anyForm = pr.transitionDop;
                if (anyForm)
                {
                    label3.Text = pr.nameProduct; 
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Owner is Product pr && int.TryParse(textBox1.Text, out var result))
            {
                error = false;
                anyForm = pr.transitionDop;
                if (anyForm&&pr.foolKolvo > Convert.ToInt32(result) && Convert.ToInt32(result)>0)
                {
                    //error = false;
                    pr.Kolvo = Convert.ToInt32(result);
                    Close();
                }
                else
                {
                    MessageBox.Show("Товар в таком кол-ве отсутствует на складе");
                    //error = true;
                }

            }
            else
            {
                MessageBox.Show("Проверьте корректность введенных данных.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Owner is Product pr)
            {
                pr.error = true;
                pr.errorMN = true;
                Close();
            }
        }

        private void dopForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Owner is Product pr && error)
            {
                pr.errorMN = true;
            }
        }
    }
}
