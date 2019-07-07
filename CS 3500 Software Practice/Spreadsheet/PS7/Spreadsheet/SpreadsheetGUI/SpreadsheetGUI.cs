using Formulas;
using SS;
using SSGui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public partial class SpreadsheetGUI : Form, ISpreadsheetView
    {
        //spreadsheet
        private Spreadsheet spreadsheet;


        public string FileName { get; set; }

        public SpreadsheetPanel sp { get; set; }


        /// <summary>
        /// Fired when a new action is requested.
        /// </summary>
        public event Action NewEvent;

        /// <summary>
        /// Fire when a file is chosen.
        /// Parameter is the filename.
        /// </summary>
        public event Action<string> OpenEvent;

        /// <summary>
        /// Fired when a save is requested.
        /// </summary>
        public event Action SaveEvent;

        /// <summary>
        /// Fired when a save as action is requested.
        /// </summary>
        public event Action<string> SaveAsEvent;


        /// <summary>
        /// Fired when a close action is requested
        /// </summary>
        public event Action CloseEvent;
        public event Action<string> SetCellContentEvent;
        public event Action<string> ArrowKeyEvent;


        /// <summary>
        /// Open a new spreadsheet window
        /// </summary>
        public void OpenNew()
        {
            SpreadsheetApplicationContext.GetContext().RunNew();
        }

        /// <summary>
        /// Close the window
        /// </summary>
        public void DoClose()
        {
            Close();
        }

        public string Message
        {
            set { MessageBox.Show(value); }
        }


        public SpreadsheetGUI()
        {

            InitializeComponent();

            spreadsheet = new Spreadsheet();

            sp = spreadsheetPanel1;

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(spreadsheetPanel1_KeyUp);
        }





        private void spreadsheetPanel1_Load(object sender, EventArgs e)
        {
            //makes window full screen

            this.WindowState = FormWindowState.Maximized;

        }


        public void setContentBox(string content)
        {
            contentBox.Text = content;
        }

        public void setValueBox(string value)
        {
            valueBox.Text = value;
        }

        public void setCellBox(string s)
        {
            CellBox.Text = s;
        }

        public void GoSaveAs()
        {
            saveAsToolStripMenuItem.PerformClick();
        }

        /// <summary>
        /// closes the window, asks to save if changes were made.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            CloseEvent?.Invoke();


            DialogResult result = MessageBox.Show("Your spreadsheet has unsaved changes. Save changes before closing?",
                "Save before close?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                saveAsToolStripMenuItem.PerformClick();

            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }

            DoClose();
        }



        /// <summary>
        /// Save menu item click. if filename exists, saves the item. If filename does not exist Save As is invoked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveEvent?.Invoke();
        }

        /// <summary>
        /// Save as spreadsheet menu item click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "Spreadsheet Files (*.ss)|*.ss|All Files (*.*)|*.*";
            sf.DefaultExt = ".ss";
            sf.ShowDialog();
            sf.AddExtension = true;
            string savename = sf.FileName;
            SaveAsEvent?.Invoke(savename);
        }

      
        /// <summary>
        /// change selected cell on arrow key press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spreadsheetPanel1_KeyUp(object sender, KeyEventArgs e)
        {
            string d = "";
                                   
            switch (e.KeyData)
            {
                
                case Keys.Up:
                    e.SuppressKeyPress = true;
                    d = "up";
                    ArrowKeyEvent(d);
                  
                    break;
                case Keys.Down:
                    e.SuppressKeyPress = true;
                    d = "down";
                    ArrowKeyEvent(d);
                 
                    break;
                case Keys.Left:
                    e.SuppressKeyPress = true;
                    d = "left";
                    ArrowKeyEvent(d);
                    
                    break;
                case Keys.Right:
                    e.SuppressKeyPress = true;
                    d = "right";
                    ArrowKeyEvent(d);
                    
                    break;

                default:
                    e.SuppressKeyPress = false;
                    break;
                
            }
            

            
        }

        /// <summary>
        /// set content of cell. must press enter key to set content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contentBox_KeyUp(object sender, KeyEventArgs e)
        {

            string text = contentBox.Text;
                       
            if(e.KeyCode == Keys.Enter)
            {
                SetCellContentEvent?.Invoke(text);                
            }
            
            
        }



        /// <summary>
        /// Help button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To use the spreadsheet the cell's content must be entered into the 'contents' textbox." +
              "The enter key must be pressed to set the contents of the cell. If you want to enter a formula, begin the content" +
              "with a equal sign (=). The arrow keys can be used to navigate to adjacent cells.", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// open a spreadsheet file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Spreadsheet Files (*.ss)|*.ss|All Files (*.*)|*.*";
            of.DefaultExt = "*.ss";
            of.FileName = "";
            of.Title = "Open";
            of.ShowDialog();

            string fname = of.FileName;
            OpenEvent?.Invoke(fname);            
        }

       

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewEvent?.Invoke();
        }

        
    }

   
}
