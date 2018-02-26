using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileIOAssignment
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    TxtContents.Text = File.ReadAllText(openFileDialog.FileName);
                    string fileName = openFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file. Error message: " + ex.Message);
                }
            }
        }

        private void BtnSaveFile_Click(object sender, EventArgs e)
        {
            if (!FieldHasData())
            {
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(saveFileDialog.OpenFile());
                writer.Write(TxtContents.Text);
                writer.Dispose();
            }
        }

        private bool FieldHasData()
        {
            if (String.IsNullOrWhiteSpace(TxtContents.Text))
            {
                MessageBox.Show("Must first open a text file containing data");
                return false;
            }
            return true;
        }

        private void BtnEncrypt_Click(object sender, EventArgs e)
        {
            if (!FieldHasData())
            {
                return;
            }
            if (TxtContents.Text.Contains("Encrypted"))
            {
                MessageBox.Show("This file is already encrypted.");
                return;
            }
            byte[] bytesData = Encoding.ASCII.GetBytes(TxtContents.Text);
            TxtContents.Text = "Encrypted";
            TxtContents.Text += BitConverter.ToString(bytesData);
        }

        private void BtnDecrypt_Click(object sender, EventArgs e)
        {
            if (!FieldHasData())
            {
                return;
            }
            if (!TxtContents.Text.Contains("Encrypted"))
            {
                MessageBox.Show("File is not Encrypted. Cannot Decrypt.");
                return;
            }
            TxtContents.Text = TxtContents.Text.Remove(0, 9);

            string[] hexArray = TxtContents.Text.Split('-');
            byte[] bytes = new byte[hexArray.Length];

            for (int i = 0; i < hexArray.Length; i++)
            {
                bytes[i] = byte.Parse(hexArray[i], System.Globalization.NumberStyles.HexNumber);
            }

            TxtContents.Text = Encoding.UTF8.GetString(bytes);
        }
    }
}
