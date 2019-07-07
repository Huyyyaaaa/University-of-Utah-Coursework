using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using SS;
using System.Windows.Forms;
using SSGui;
using Formulas;

namespace SpreadsheetGUI
{
    public class Controller
    {
        // The window that is being controlled
        // This is an interface that holds actions and information to pass to GUI
        private ISpreadsheetView window; 

        // The code comprised of the Formula, Dependency Graph, and Spreadsheet
        private Spreadsheet model;

        private bool cancelSave;



        /// <summary>
        /// Takes in interface and begins controlling the window
        /// </summary>
        public Controller(ISpreadsheetView window)
        {
            this.window = window;
            model = new Spreadsheet();
   
            window.NewEvent += HandleNew;
            window.OpenEvent += HandleOpen;
            window.SaveEvent += HandleSave;
            window.SaveAsEvent += HandleSaveAs;
            window.CloseEvent += HandleClose;
            window.SetCellContentEvent += HandleCellContents;
            window.sp.SelectionChanged += displaySelection;
            window.ArrowKeyEvent += HandleArrowKey;
            
           
        }
        private void HandleNew()
        {
            window.OpenNew();
            
        }

        private void HandleOpen(String filename)
        {
            

            //if no file selected then return and close dialog
            if (filename == "") return;

            try
            {
                foreach (string s in model.GetNamesOfAllNonemptyCells())
                {
                    int coli = GetColumnIndex(s);
                    int rowi = GetRowIndex(s);
                    window.sp.SetValue(coli, rowi, "");

                    

                }

                TextReader tr = new StreamReader(filename);

                model = new Spreadsheet(tr, new Regex(@"[a-zA-Z]{1}[0-9]{1,2}"));

                foreach (string s in model.GetNamesOfAllNonemptyCells())
                {
                    updateCell(s);
                    displaySelection(window.sp);
                }

                window.FileName = filename;

                tr.Close();
            }
            catch (Exception)
            {
              window.Message = "There was a problem reading the file. Please check your file.";
            }

            

        }

        private void HandleSave()
        {
            if (string.IsNullOrEmpty(window.FileName))
            {
                //go to save as
                window.GoSaveAs();

                if (string.IsNullOrEmpty(window.FileName))
                {
                    cancelSave = true;
                    return;
                }
            }

            if (model.Changed)
            {
                File.Delete(window.FileName);
                //save spreadsheet
                model.Save(File.CreateText(window.FileName));
            }
        }

        private void HandleSaveAs(string s)
        {
            

            // get the name of the selected file
            string savename = s;

            if (savename == "") return;

             //this.Text= savename;

          
            model.Save(File.CreateText(savename));

            
            window.FileName = savename;
            
        }

        private void HandleClose()
        {
            if (!model.Changed)
            {
                window.DoClose();
            }
           
            
        }

        /// <summary>
        /// gets the cell column index
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private int GetColumnIndex(string name)
        {
            int column;
            column = name.ToCharArray(0, 1)[0] - 'A';
            return column;
        }

        /// <summary>
        /// gets the cell row index
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private int GetRowIndex(string name)
        {
            int row;
            int.TryParse(name.Substring(1), out row);
            row--;
            return row;
        }

        /// <summary>
        /// display content and values of selection
        /// </summary>
        /// <param name="sender"></param>
        private void displaySelection(SpreadsheetPanel sender)
        {
            string name = GetCellName();
            if (model.GetCellContents(name).GetType().ToString()== "System.String" &&
                model.GetCellValue(name).GetType().ToString() == "System.Double")
            {
                window.setContentBox("="+model.GetCellContents(name).ToString());
            }
            else
            {
                window.setContentBox(model.GetCellContents(name).ToString());
            }

            window.setValueBox(model.GetCellValue(name).ToString());
            window.setCellBox(GetCellName());
            updateCell(name);

        }

        /// <summary>
        /// updates the cell value in the spreadsheet form. requires cell name.
        /// </summary>
        /// <param name="name"></param>
        private void updateCell(string name)
        {
            int column;
            int row;
            string value;

            column = GetColumnIndex(name);
            row = GetRowIndex(name);

            if (model.GetCellValue(name) is FormulaError)
            {
                FormulaError e = (FormulaError)model.GetCellValue(name);
                value = e.Reason;
            }
            else
            {
                value = model.GetCellValue(name).ToString();
                window.sp.SetValue(column, row, value);
            }


        }

        /// <summary>
        /// get cell name of selected cell
        /// </summary>
        /// <returns></returns>
        private string GetCellName()
        {
            int col, row;
            window.sp.GetSelection(out col, out row);

            int cellRow = ++row;

            char cellColumn = (char)('A' + col);
            return "" + cellColumn + cellRow;
        }

        /// <summary>
        /// Helper method to update all cells in spreadsheet to show their correct value
        /// </summary>
        public void UpdateSpreadsheet()
        {
            
            foreach (string s in model.GetNamesOfAllNonemptyCells())
            {
                updateCell(s);
                displaySelection(window.sp);
            }
        }

        public void HandleCellContents(string text)
        {
            string name = GetCellName();
            try
            {
                model.SetContentsOfCell(name, text);
                displaySelection(window.sp);
                UpdateSpreadsheet();
            }

            catch (Exception ex)
            {
                if (ex is CircularException) window.Message = "Circular dependencies in formulas are not allowed";
                if (ex is FormulaFormatException) window.Message = "There is an error in your formula";
                model.SetContentsOfCell(name, "FormulaError");
                displaySelection(window.sp);
                UpdateSpreadsheet();
            }
        }

        public void HandleArrowKey(string d)
        {
            string cell = GetCellName();
            switch (d)
            {
                case "up":
                     window.sp.SetSelection(GetColumnIndex(cell), GetRowIndex(cell) - 1);
                     displaySelection(window.sp);
                    break;
                case "down":
                    window.sp.SetSelection(GetColumnIndex(cell), GetRowIndex(cell) + 1);
                       displaySelection(window.sp);
                    break;
                case "left":
                    window.sp.SetSelection(GetColumnIndex(cell)-1, GetRowIndex(cell));
                    displaySelection(window.sp);
                    break;
                case "right":
                    window.sp.SetSelection(GetColumnIndex(cell)+1, GetRowIndex(cell));
                    displaySelection(window.sp);
                    break;
            }
        }
    }
}
