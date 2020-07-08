using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Exam
{
    public partial class Form1 : Form
    {
        private bool stud = false;
        private bool tick = false;
        Student Stud1;
        Students[] Student1;
        Tickets[] Ticket1;
        Label Greeting = new Label();
        OpenFileDialog OpenFiles = new OpenFileDialog();
        Button ParseStudent = new Button();
        Button ParseTicket = new Button();
        ListBox Students_LB = new ListBox();
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(800, 600);
            this.CenterToScreen();

            Greeting.Size = new Size(400,100);
            Greeting.Location = new System.Drawing.Point(this.Width / 2 - Greeting.Width / 2, 5);
            Greeting.Font = new System.Drawing.Font("Times New Roman", 14);
            Greeting.TextAlign = ContentAlignment.MiddleCenter;
            Greeting.Text = "Для работы программы нужны: 'Список студентов(txt)' и 'Список билетов(word)'. Нажмите на соотвествующие кнопки.";
            this.Controls.Add(Greeting);

            ParseStudent.Size = new Size(200, 70);
            ParseStudent.Location = new System.Drawing.Point(this.Width / 2 - ParseStudent.Width - 5, 110);
            ParseStudent.Font = new System.Drawing.Font("Times New Roman", 14);
            ParseStudent.Text = "Список студентов";
            ParseStudent.Click += new EventHandler(ParseStudent_Click);
            this.Controls.Add(ParseStudent);

            ParseTicket.Size = ParseStudent.Size;
            ParseTicket.Location = new System.Drawing.Point(this.Width / 2 + 5, 110);
            ParseTicket.Font = ParseStudent.Font;
            ParseTicket.Text = "Список билетов";
            ParseTicket.Click += new EventHandler(ParseTicket_Click);
            this.Controls.Add(ParseTicket);

            Students_LB.Size = new Size(270, 350);
            Students_LB.Location = new System.Drawing.Point(this.Width / 2 - Students_LB.Width / 2, 190);
            Students_LB.SelectedIndexChanged += new EventHandler(Students_SelectedIndexChanged);
            Students_LB.Font = new System.Drawing.Font("Times New Roman", 12);
        }
        private string namestud;
        private void ParseStudent_Click(object sender, EventArgs e)
        {
            OpenFiles.InitialDirectory = Directory.GetCurrentDirectory();
            if (OpenFiles.ShowDialog() == DialogResult.Cancel)
                return;
            namestud = OpenFiles.FileName;
            Student1 = new Students[File.ReadAllLines(namestud).Length];
            using(StreamReader file = new StreamReader(namestud))
            {
                for(int i = 0; i < Student1.Length; i++)
                {
                    string line = file.ReadLine();
                    if(line.IndexOf("-") == -1)
                        Student1[i] = new Students(line , 0);
                    else
                        Student1[i] = new Students(line.Split('-')[0], Convert.ToInt32(line.Split('-')[1]));
                    Students_LB.Items.Add(Student1[i].FIO);
                }
            }
            stud = true;
        }
        private string nameticket;
        private void ParseTicket_Click(object sender, EventArgs e)
        {

            OpenFiles.InitialDirectory = Directory.GetCurrentDirectory();
            if (OpenFiles.ShowDialog() == DialogResult.Cancel)
                return;
            nameticket = OpenFiles.FileName;
            try
            {
                File.Copy(nameticket, Directory.GetCurrentDirectory() + @"\sadsadsadsa.docx");
            }
            catch (IOException)
            {
                MessageBox.Show("Закройте файл с билетами.");
                return;
            }
            File.Delete(Directory.GetCurrentDirectory() + @"\sadsadsadsa.docx");
            tick = true;
        }
        private void Generate(int index)
        {
            Random r = new Random();
            int a = r.Next(1, 26);
            MessageBox.Show("Билет номер: " + a, "Генератор");
            Ticket.OpenTicket(nameticket, a);
            Student1[index] = new Students(Student1[index].FIO, a);
            Student Stud1 = new Student(ref Student1, Student1[Students_LB.SelectedIndex], a);
            Stud1.Save(namestud);
        }
        private void Students_SelectedIndexChanged(object sender, EventArgs e)
        {
            DialogResult Question = MessageBox.Show($"Вы действительно хотите сгенерировать билет для {Students_LB.SelectedItem.ToString()}?","Вопрос", MessageBoxButtons.YesNo);
            if(Question == DialogResult.Yes)
            {
                Generate(Students_LB.SelectedIndex);
            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if(stud && tick)
            {
                this.Controls.Add(Students_LB);
                Students_LB.Focus();
            }
        }
    }
}
