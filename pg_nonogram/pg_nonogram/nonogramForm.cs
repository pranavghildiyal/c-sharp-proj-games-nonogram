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

        //Variable used

        //It refers to the Main ButtonGrod
        Button[,] plates = new Button[Configuration.PLATE_MAX_ELEMENTS, Configuration.PLATE_MAX_ELEMENTS];

        //The row of Labels for showing Number values on the left of screen
        Label[] vLabels = new Label[Configuration.PLATE_MAX_ELEMENTS];

        //The row of Labels for showing Number values on the top of screen
        Label[] hLabels = new Label[Configuration.PLATE_MAX_ELEMENTS];

        //Used for populating above two rows of Labels
        string[] hVals = new string[Configuration.PLATE_MAX_ELEMENTS];
        string[] vVals = new string[Configuration.PLATE_MAX_ELEMENTS];

        // integer grid =valent to the ButtonGrid above
        int[,] blocks = new int[Configuration.PLATE_MAX_ELEMENTS, Configuration.PLATE_MAX_ELEMENTS];

        //vars for qFillMode
        Boolean qFillMode = Configuration.QUICK_MODE_INITIAL_FLAG; //qFillMode is initially OFF

        //Used to track initial button for qFill, i.e., the FROM of FROM-TO for performing quickFill.
        int qFillrVal = 0, qFillcVal = 0;

        public nonogramForm()
        {
            InitializeComponent();

            for (int ivar = 1; ivar < Configuration.PLATE_MAX_ELEMENTS; ivar++)
            {
                for (int jvar = 1; jvar < Configuration.PLATE_MAX_ELEMENTS; jvar++)
                {
                    plates[ivar, jvar] = new Button();

                    //Assign the UI buttons to the Button[,] array for easy access.
                    //Needs all buttonNames to start from "button".
                    plates[ivar, jvar] = (Button)Controls["button" + ivar +""+ jvar];

                    //Add events to the Buttons. The if condition if temporary as few events 
                    //have been already attached to button
                    //TODO remove manual event and update below if condition
                    if (ivar > 1 && jvar > 5)
                    {
                        plates[ivar, jvar].Click += new System.EventHandler(this.buttonAB_Click);
                    }
                }

                vLabels[ivar] = new Label();
                hLabels[ivar] = new Label();

                //Assign the UI labels (for Left side row of Labels) to the Label[] array for easy access.
                //Needs all labelNames to start from "labelV".
                vLabels[ivar] = (Label)Controls["labelV" + ivar];

                //Assign the UI labels (for Top side row of Labels) to the Label[] array for easy access.
                //Needs all labelNames to start from "labelH".
                hLabels[ivar] = (Label)Controls["labelH" + ivar];
            }
        }

        /**
         * Initialized new Game
         * */
        private void initializeNewGame()
        {
            //initially MARK operation should be ACTIVE
            currentSelectedAction = Constants.ACTION_MARK;
            btnActionMark.FlatAppearance.BorderColor = Constants.ACTION_HIGHLIGHT_COLOR;
            btnActionCross.FlatAppearance.BorderColor = Constants.ACTION_CROSS_COLOR;
            btnActionClear.FlatAppearance.BorderColor = Constants.ACTION_CLEAR_COLOR;

            //initialize the Top and Left Label row with Values
            for (int ivar = 1; ivar < Configuration.PLATE_MAX_ELEMENTS; ivar++)
            {
                hLabels[ivar].Text = generateValueString(hVals[ivar]);
                vLabels[ivar].Text = generateValueString(vVals[ivar]);
            }
        }

        /**
         * This method returns the string of Number Values which are displayed
         * in the various H and V valueLabels
         * */
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

        /**
         * Core Method, doing the ButtonHighlighting,
         * Verifications and Labels Highlighting
         * */
        private void performAction(int row, int col, Boolean recheckFlag)
        {
            //Highlighting of the clicked button
            performButtonHighlight(row,col);

            //Controlling qFillMode
            if (qFillMode)
            {
                if (qFillrVal == 0 && qFillcVal == 0)
                {
                    qFillrVal = row;
                    qFillcVal = col;
                }
                else
                {
                    multiModeSelection(row, col);
                    qFillrVal = 0;
                    qFillcVal = 0;
                }
            }
            int cntrVG = 0, cntrVR = 0, cntrHG = 0, cntrHR = 0;
            int cntrVGC = 0, cntrVRC = 0, cntrHGC = 0, cntrHRC = 0;
            int gVCounts = 0, gHCounts = 0, rVCounts = 0, rHCounts = 0;
            //textBox1.Text += "row" + row+ "row";
            for (int ivar = 1; ivar < Configuration.PLATE_MAX_ELEMENTS; ivar++)
            {
                if ((blocks[row,ivar] == 1))
                {
                    gVCounts +=1;
                    if((plates[row, ivar].BackColor == Constants.ACTION_MARK_COLOR))
                    {
                        cntrVG++;
                        cntrVGC++;
                    }
                    if ((plates[row, ivar].BackColor == Constants.ACTION_CROSS_COLOR))
                    {
                        cntrVRC++;
                    }

                }
                else if ((blocks[row, ivar] == 0))
                {
                    rVCounts += 1;
                    if ((plates[row, ivar].BackColor == Constants.ACTION_CROSS_COLOR))
                    {
                        cntrVR++;
                        cntrVRC++;
                    }

                    if ((plates[row, ivar].BackColor == Constants.ACTION_MARK_COLOR))
                    {
                        cntrVGC++;
                    }
                }

                if ((blocks[ivar, col] == 1))
                {
                    gHCounts += 1;
                    if ((plates[ivar, col].BackColor == Constants.ACTION_MARK_COLOR))
                    {
                        cntrHG++;
                        cntrHGC++;
                    }
                    if ((plates[ivar, col].BackColor == Constants.ACTION_CROSS_COLOR))
                    {
                        cntrHRC++;
                    }
                }
                else if ((blocks[ivar, col] == 0))
                {
                    rHCounts += 1;
                    if ((plates[ivar, col].BackColor == Constants.ACTION_CROSS_COLOR))
                    {
                        cntrHR++;
                        cntrHRC++;
                    }
                    if ((plates[ivar, col].BackColor == Constants.ACTION_MARK_COLOR))
                    {
                        cntrHGC++;
                    }
                }
            }
            //textBox1.Text += "af cntrVR-" + cntrVR + "cntrVG-" + cntrVG + ";";

            //Mark Label Green if whole Row/Column is filled and filled correctly
            //textBox1.Text += "$$$ " + cntrV;
            //if (!recheckFlag || ((cntrVG + cntrVR) == 10))
            //{
                if ((cntrVG + cntrVR) == 10)
                {
                    vLabels[row].ForeColor = Color.Lime;
                    vLabels[row].BackColor = Color.DarkGreen;
                    textBox1.Text += "G";
                }
                else
                {
                    vLabels[row].ForeColor = Color.Black;
                    vLabels[row].BackColor = Color.LightGray;
                    textBox1.Text += "R";
                }
            //}
            if ((cntrHG + cntrHR) == 10)
            {
                hLabels[col].ForeColor = Color.Lime;
                hLabels[col].BackColor = Color.DarkGreen;
                //textBox1.Text += "G";
            }
            else
            {
                hLabels[col].ForeColor = Color.Black;
                hLabels[col].BackColor = Color.LightGray;
                //textBox1.Text += "R";
            }

            //AutoFill for CROSS entries
            if (!((cntrVG + cntrVR) == 10) && (gVCounts == cntrVG) && (gVCounts == cntrVGC) && recheckFlag)
            {
                //textBox1.Text += "afR gVCounts" + gVCounts + "cntrVG" + cntrVG + ";";
                autoFillRow(row, col);
            }

            if (!((cntrHG + cntrHR) == 10) && (gHCounts == cntrHG) && (gHCounts == cntrHGC) && recheckFlag)
            {
                autoFillColumn(row, col);
            }
        }

        /**
         * This method updates the button color of the clicked Button to indicate
         * MARK - Green, Cross - Red, NoHighlight - Clear.
         * */
        private void performButtonHighlight(int row, int col)
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

        /**
         * This method calls the performButtonHightLight Method 
         * to do the quickFill
         * */
        private void multiModeSelection(int row, int col)
        {
            //Row Quick Fill
            if (qFillrVal == row)
            {
                //Fwd Dir
                if (qFillcVal <= col)
                {
                    for (int ivar = qFillcVal; ivar <= col; ivar++)
                    {
                        performButtonHighlight(row, ivar);
                    }
                }
                else //Bwd Dir
                {
                    for (int ivar = col; ivar <= qFillcVal; ivar++)
                    {
                        performButtonHighlight(row, ivar);
                    }
                }
            }
            else if (qFillcVal == col) //Column quickFill
            {
                if (qFillcVal <= row) //TopDown Fill
                {
                    for (int ivar = qFillrVal; ivar <= row; ivar++)
                    {
                        performButtonHighlight(ivar, col);
                    }
                }
                else //BottomUp Fill
                {
                    for (int ivar = row; ivar <= qFillrVal; ivar++)
                    {
                        performButtonHighlight(ivar, col);
                    }
                }
            }
        }

        /**
         * Method used to peform auto-filling of CROSS Marks if all the
         * required Green Marks have been made in the Row.
         **/
        private void autoFillRow(int row, int col)
        {
            for (int ivar = 1; ivar < Configuration.PLATE_MAX_ELEMENTS; ivar++)
            {
                if ((blocks[row, ivar] == 0))
                {
                    plates[row, ivar].BackColor = Constants.ACTION_CROSS_COLOR;
                }
            }

            //the third parameter should be always FALSE in below function call
            performAction(row,col, false);
        }

         /**
         * Method used to peform auto-filling of CROSS Marks if all the
         * required Green Marks have been made in the Column.
         **/
        private void autoFillColumn(int row, int col)
        {
            for (int ivar = 1; ivar < Configuration.PLATE_MAX_ELEMENTS; ivar++)
            {
                if ((blocks[ivar, col] == 0))
                {
                    plates[ivar, col].BackColor = Constants.ACTION_CROSS_COLOR;
                }
            }

            //the third parameter should be always FALSE in below function call
            performAction(row, col, false);
        }

        /**
         * MARK Action Button 
         **/
        private void btnActionMark_Click(object sender, EventArgs e)
        {
            currentSelectedAction = Constants.ACTION_MARK;
            btnActionMark.FlatAppearance.BorderColor = Constants.ACTION_HIGHLIGHT_COLOR;
            btnActionCross.FlatAppearance.BorderColor = Constants.ACTION_CROSS_COLOR;
            btnActionClear.FlatAppearance.BorderColor = Constants.ACTION_CLEAR_COLOR;
        }

        /**
         * CROSS Action Button 
         **/
        private void btnActionCross_Click(object sender, EventArgs e)
        {
            currentSelectedAction = Constants.ACTION_CROSS;
            btnActionMark.FlatAppearance.BorderColor = Constants.ACTION_MARK_COLOR;
            btnActionCross.FlatAppearance.BorderColor = Constants.ACTION_HIGHLIGHT_COLOR;
            btnActionClear.FlatAppearance.BorderColor = Constants.ACTION_CLEAR_COLOR;
        }

        /**
         * CLEAR Action Button 
         **/
        private void btnActionClear_Click(object sender, EventArgs e)
        {
            currentSelectedAction = Constants.ACTION_CLEAR;
            btnActionMark.FlatAppearance.BorderColor = Constants.ACTION_MARK_COLOR;
            btnActionCross.FlatAppearance.BorderColor = Constants.ACTION_CROSS_COLOR;
            btnActionClear.FlatAppearance.BorderColor = Constants.ACTION_HIGHLIGHT_COLOR;
        }

        /**
         * Method 1 - For common button Click Methods for the Grid of Buttons
         * This returns the Number value of button eg: 23 for button23
         **/
        public int getButtonNumber(object sender)
        {
            Button b = sender as Button;
            String s = b.Name;
            s = s.Replace("button", "");
            return Convert.ToInt32(s);
        }

        /**
         * Method 2 - For common button Click Methods for the Grid of Buttons
         * This is the click middleware- Click of button executes this,
         * which calls the generic Click method, providing Number value
         * of the button which was clicked by using Method 1.
         **/
        private void buttonAB_Click(object sender, EventArgs e)
        {
            button_Click(getButtonNumber(sender));
        }

        /**
         * Method 3 - For common button Click Methods for the Grid of Buttons
         * This is the actual code that shuld be executed when any of the 
         * button from the Grid of Button is clicked.
         **/
        private void button_Click(int p)
        {
            //textBox1.Text += p.ToString() + ",";true
            //the third param in perFormAction() method calls below should
            //always be true;
            if (p > 109)
            {
                performAction(p / 100, 10, true);
            }
            else if (p == 1010)
            {
                performAction(10, 10, true);
            }
            else
            {
                performAction(p / 10, p % 10, true);
            }
        }

        /**
         * ClearAll Action - Resets the Label Highlights, any Mark/Cross
         * on any of Grid of Button
         **/
        private void btnActionClearAll_Click(object sender, EventArgs e)
        {
            for (int ivar = 1; ivar < Configuration.PLATE_MAX_ELEMENTS; ivar++)
            {
                for (int jvar = 1; jvar < Configuration.PLATE_MAX_ELEMENTS; jvar++)
                {
                    plates[ivar, jvar].BackColor = Constants.ACTION_CLEAR_COLOR;
                }

                vLabels[ivar].ForeColor = Color.Black;
                vLabels[ivar].BackColor = Color.LightGray;
                hLabels[ivar].ForeColor = Color.Black;
                hLabels[ivar].BackColor = Color.LightGray;
            }
        }

        /**
         * NEW GAME Action Button
         * */
        private void btnNewGame_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            blocks = new int[Configuration.PLATE_MAX_ELEMENTS, Configuration.PLATE_MAX_ELEMENTS];
            hVals = new string[Configuration.PLATE_MAX_ELEMENTS];
            vVals = new string[Configuration.PLATE_MAX_ELEMENTS];

            //reinitialize
            for (int ivar = 1; ivar < Configuration.PLATE_MAX_ELEMENTS; ivar++)
            {
                for (int jvar = 1; jvar < Configuration.PLATE_MAX_ELEMENTS; jvar++)
                {
                    blocks[ivar, jvar] = new int();
                    blocks[ivar, jvar] = 0;
                }
                hVals[ivar] = "";
                vVals[ivar] = "";
            }

            Random randomGenerator = new Random();
            for (int ivar = 1; ivar < Configuration.PLATE_MAX_ELEMENTS; ivar++) //for each col
            {
                for (int jvar = 1; jvar < Configuration.PLATE_MAX_ELEMENTS; jvar++)
                {
                    //Creating a New Game/Grid by use formula
                    //Formula = if rnum1 is not divisible by 3 then it wil be a Green (Mark)
                    //else Red (def for each button of ButtonGrid)
                    int rnum1 = randomGenerator.Next(1, 100);
                    if (!(rnum1 % 3 == 0))
                    {
                        blocks[ivar, jvar] = 1;
                    }
                }
            }
            textBox1.Text += "\r\n";

            //Creating the label values present at top (column) and left (row)
            int cntrV = 0, cntrH = 0;
            for (int ivar = 1; ivar < Configuration.PLATE_MAX_ELEMENTS; ivar++) //for each col
            {
                for (int jvar = 1; jvar < Configuration.PLATE_MAX_ELEMENTS; jvar++)
                {
                    //For V Top-Down Labels
                    if (blocks[ivar, jvar] == 1) //inc counter till we get a zero
                    {
                        cntrV += 1;
                    }
                    else
                    {
                        if (cntrV != 0) //once we get a zero, and have a non-zero cnt put it into vVals
                                        //which will later be used to create the label which displays the numbers
                        {
                            vVals[ivar] += cntrV.ToString();
                        }

                        cntrV = 0;
                    }

                    //For H Left-Right Labels
                    if (blocks[jvar, ivar] == 1)//inc counter till we get a zero
                    {
                        cntrH += 1;
                    }
                    else
                    {
                        if (cntrH != 0)//once we get a zero, and have a non-zero cnt put it into vVals
                        //which will later be used to create the label which displays the numbers
                        {
                            hVals[ivar] += cntrH.ToString();
                        }
                        cntrH = 0;
                    }
                }

                if (cntrV != 0)
                {
                    vVals[ivar] += cntrV.ToString();
                }

                if (cntrH != 0)
                {
                    hVals[ivar] += cntrH.ToString();
                }
                cntrV = 0; cntrH = 0;
            }

            textBox1.Text += "\r\n";

            //print for debug
            for (int ivar = 1; ivar < Configuration.PLATE_MAX_ELEMENTS; ivar++)
            {
                for (int jvar = 1; jvar < Configuration.PLATE_MAX_ELEMENTS; jvar++)
                {
                    textBox1.Text += blocks[ivar, jvar].ToString() + " ";
                }
                textBox1.Text += "%%% " + vVals[ivar];
                textBox1.Text += "%%% " + hVals[ivar] + "\r\n";
            }

            //Apply to Board
            initializeNewGame();
        }

        /**
         * QFILL Action Button
         * */
        private void btnQFill_Click(object sender, EventArgs e)
        {
            if (btnQFill.BackColor == Color.DarkRed) //TURN ON
            {
                btnQFill.BackColor = Color.DarkGreen;
                btnQFill.ForeColor = Color.Lime;
                qFillMode = true;
                qFillrVal = 0;
                qFillcVal = 0;
            }
            else //TURN OFF
            {
                btnQFill.BackColor = Color.DarkRed;
                btnQFill.ForeColor = Color.Red;
                qFillMode = false;
                qFillrVal = 0;
                qFillcVal = 0;
            }
        }
    }

    /**
     * Support Class
     * Class for Constants used, accessed as static members of class
     **/
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

    /**
     * Support Class
     * Class for Configuration values, accessed as static members of class
     **/
    class Configuration
    {
        public static int ACTION_MARK = 1;
        public static Color ACTION_MARK_COLOR = Color.DarkGreen;

        public static int PLATE_MAX_ELEMENTS = 11;
        public static int VALUE_STR_MAX_ELEMENTS = 4;

        public static bool QUICK_MODE_INITIAL_FLAG = false;
    }
}
