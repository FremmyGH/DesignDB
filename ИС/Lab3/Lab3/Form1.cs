using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        db_conf conf = new db_conf();
        public string[] resultAuth;
        public bool Transition;
        private void авторыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var autor = new Author();
            autor.ShowDialog();
        }

        private void жанрыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var genre = new Genre();
            genre.ShowDialog();
        }

        private void книгиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var book = new Book();
            book.ShowDialog();
        }

        private void издательстваToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var publish = new Publishment();
            publish.ShowDialog();
        }

        private void типыКнигToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var typeBook = new TypeBook();
            typeBook.ShowDialog();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Auth auth = new Auth();
            //auth.Show();и
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Owner is Auth auth)
            {
                    resultAuth = auth.result;
            }

            //MessageBox.Show(resultAuth[1]);
            conf.Get(dataGridView1, "sp_GetBook");
            dataGridView1.Columns["ID_Book"].Visible = false;
            dataGridView1.Columns["Publishment_ID"].Visible = false;
            dataGridView1.Columns["Genre_ID"].Visible = false;
            dataGridView1.Columns["TypeBook_ID"].Visible = false;


            if (Convert.ToInt32(resultAuth[4]) == 1)
            {
                пользователиToolStripMenuItem.Visible = false;
                ролиToolStripMenuItem.Visible = false;
                логиToolStripMenuItem.Visible = false;
            }
            else if (Convert.ToInt32(resultAuth[4]) == 2)
            {
                ролиToolStripMenuItem.Visible = false;
                логиToolStripMenuItem.Visible = false;
            }
        }

        private void пользователиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Transition = true;
            var client = new Client()
            {
                Owner = this
            };
            client.ShowDialog();
        }

        private void ролиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var role = new Role();
            role.ShowDialog();
        }

        private void логиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var log = new Log();
            log.ShowDialog();
        }
    }
}
