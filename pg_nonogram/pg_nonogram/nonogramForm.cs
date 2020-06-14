using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pg_nonogram
{
    public partial class nonogramForm : Form
    {
        int currentSelectedAction;//Mark , 2 = Cross, 3 = Delete, or as configured in Constants
        int maxElements = Configuration.VALUE_STR_MAX_ELEMENTS;
        Button[,] plates = new Button[Configuration.PLATE_MAX_ELEMENTS, Configuration.PLATE_MAX_ELEMENTS];
        Label[] vLabels = new Label[Configuration.PLATE_MAX_ELEMENTS];
        Label[] hLabels = new Label[Configuration.PLATE_MAX_ELEMENTS];

        public nonogramForm()
        {
            InitializeComponent();
            for (int ivar = 1; ivar < Configuration.PLATE_MAX_ELEMENTS; ivar++)
            {
                for (int jvar = 1; jvar < Configuration.PLATE_MAX_ELEMENTS; jvar++)
                {
                    plates[ivar, jvar] = new Button();
                    plates[ivar, jvar] = (Button)Controls["button" + ivar + "" + jvar];
                }

                vLabels[ivar] = new Label();
                vLabels[ivar] = (Label)Controls["labelV" + ivar];
                hLabels[ivar] = new Label();
                hLabels[ivar] = (Label)Controls["labelH" + ivar];
            }

            initializeNewGame();
        }

        private void initializeNewGame()
        {
            currentSelectedAction = Constants.ACTION_MARK;
            btnActionMark.FlatAppearance.BorderColor = Constants.ACTION_HIGHLIGHT_COLOR;
            btnActionCross.FlatAppearance.BorderColor = Constants.ACTION_CROSS_COLOR;
            btnActionClear.FlatAppearance.BorderColor = Constants.ACTION_CLEAR_COLOR;

            hLabels[1].Text = generateValueString("2");
            hLabels[2].Text = generateValueString("5");
            hLabels[3].Text = generateValueString("4");
            hLabels[4].Text = generateValueString("2");
            hLabels[5].Text = generateValueString("4");

            vLabels[1].Text = generateValueString("2");
            vLabels[2].Text = generateValueString("31");
            vLabels[3].Text = generateValueString("5");
            vLabels[4].Text = generateValueString("4");
            vLabels[5].Text = generateValueString("11");
        }

        private string generateValueString(String srcStr)
        {
            srcStr = srcStr.Replace(" ", "");
            int numEle = srcStr.Length;
            string valueString = "";
            //textBox1.Text += numEle.ToString() +"," + (maxElements - numEle).ToString();
            for (int i = 0; i < (maxElements - numEle); i++)
            {
                valueString = "     " + valueString;
            }

            for (int i = 1; i <= numEle; i++)
            {
                valueString += "  " + srcStr[i - 1] + "  ";
            }
            return valueString;
        }

        private void performAction(int row, int col)
        {
            if (currentSelectedAction == Constants.ACTION_MARK)
            {
                plates[row, col].BackColor = Constants.ACTION_MARK_COLOR;
            }
            else if (currentSelectedAction == Constants.ACTION_CROSS)
            {
                plates[row, col].BackColor = Constants.ACTION_CROSS_COLOR;
            }
            else if (currentSelectedAction == Constants.ACTION_CLEAR)
            {
                plates[row, col].BackColor = Constants.ACTION_CLEAR_COLOR;
            }
        }


        private void btnActionMark_Click(object sender, EventArgs e)
        {
            currentSelectedAction = Constants.ACTION_MARK;
            btnActionMark.FlatAppearance.BorderColor = Constants.ACTION_HIGHLIGHT_COLOR;
            btnActionCross.FlatAppearance.BorderColor = Constants.ACTION_CROSS_COLOR;
            btnActionClear.FlatAppearance.BorderColor = Constants.ACTION_CLEAR_COLOR;
        }

        private void btnActionCross_Click(object sender, EventArgs e)
        {
            currentSelectedAction = Constants.ACTION_CROSS;
            btnActionMark.FlatAppearance.BorderColor = Constants.ACTION_MARK_COLOR;
            btnActionCross.FlatAppearance.BorderColor = Constants.ACTION_HIGHLIGHT_COLOR;
            btnActionClear.FlatAppearance.BorderColor = Constants.ACTION_CLEAR_COLOR;
        }

        private void btnActionClear_Click(object sender, EventArgs e)
        {
            currentSelectedAction = Constants.ACTION_CLEAR;
            btnActionMark.FlatAppearance.BorderColor = Constants.ACTION_MARK_COLOR;
            btnActionCross.FlatAppearance.BorderColor = Constants.ACTION_CROSS_COLOR;
            btnActionClear.FlatAppearance.BorderColor = Constants.ACTION_HIGHLIGHT_COLOR;
        }

        public int getButtonNumber(object sender)
        {
            Button b = sender as Button;
            String s = b.Name;
            s = s.Replace("button", "");
            return Convert.ToInt32(s);
        }

        private void buttonAB_Click(object sender, EventArgs e)
        {
            button_Click(getButtonNumber(sender));
        }

        private void button_Click(int p)
        {
            textBox1.Text = p.ToString();
            performAction(p / 10, p % 10);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            performAction(1, 3);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            performAction(1, 4);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            performAction(1, 5);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            performAction(2, 1);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            performAction(2, 2);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            performAction(2, 3);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            performAction(2, 4);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            performAction(2, 5);
        }

        private void button31_Click(object sender, EventArgs e)
        {
            performAction(3, 1);
        }

        private void button32_Click(object sender, EventArgs e)
        {
            performAction(3, 2);
        }

        private void button33_Click(object sender, EventArgs e)
        {
            performAction(3, 3);
        }

        private void button34_Click(object sender, EventArgs e)
        {
            performAction(3, 4);
        }

        private void button35_Click(object sender, EventArgs e)
        {
            performAction(3, 5);
        }

        private void button41_Click(object sender, EventArgs e)
        {
            performAction(4, 1);
        }

        private void button42_Click(object sender, EventArgs e)
        {
            performAction(4, 2);
        }

        private void button43_Click(object sender, EventArgs e)
        {
            performAction(4, 3);
        }

        private void button44_Click(object sender, EventArgs e)
        {
            performAction(4, 4);
        }

        private void button45_Click(object sender, EventArgs e)
        {
            performAction(4, 5);
        }

        private void button51_Click(object sender, EventArgs e)
        {
            performAction(5, 1);
        }

        private void button52_Click(object sender, EventArgs e)
        {
            performAction(5, 2);
        }

        private void button53_Click(object sender, EventArgs e)
        {
            performAction(5, 3);
        }

        private void button54_Click(object sender, EventArgs e)
        {
            performAction(5, 4);
        }

        private void button55_Click(object sender, EventArgs e)
        {
            performAction(5, 5);
        }

    }

    class Constants
    {
        public static int ACTION_MARK = 1;
        public static int ACTION_CROSS = 2;
        public static int ACTION_CLEAR = 3;

        public static Color ACTION_MARK_COLOR = Color.DarkGreen;
        public static Color ACTION_CROSS_COLOR = Color.DarkRed;
        public static Color ACTION_CLEAR_COLOR = Color.Gray;
        public static Color ACTION_HIGHLIGHT_COLOR = Color.Black;
    }

    class Configuration
    {
        public static int ACTION_MARK = 1;
        public static Color ACTION_MARK_COLOR = Color.DarkGreen;

        public static int PLATE_MAX_ELEMENTS = 6;
        public static int VALUE_STR_MAX_ELEMENTS = 3;
    }
}
