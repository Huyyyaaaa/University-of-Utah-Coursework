using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulas;
using Dependencies;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace SS
{
    /// <summary>
    /// spreadsheet object that extends the AbstractSpreadsheet class
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        //keep track of cells
        private Dictionary<string, Cell> cells;

        //Dependency Graph to track cell dependencies
        private DependencyGraph dg;

        //changed variable
        private Boolean changed;

        //valid name 
        private Regex valid;

        //oldValid for spreadsheet files
        private Regex oldValid;

        /// <summary>
        /// Creates an empty Spreadsheet whose IsValid regular expression accepts every string.
        /// </summary>
        public Spreadsheet()
        {
            cells = new Dictionary<string, Cell>();
            dg = new DependencyGraph();
            changed = false;
            valid = new Regex(@"^[a-zA-Z]+[0-9]+$");

        }

        /// <summary>
        /// Creates an empty Spreadsheet whose IsValid regular expression is provided as the parameter
        /// </summary>
        /// <param name="isValid"></param>
        public Spreadsheet(Regex isValid)
        {
            cells = new Dictionary<string, Cell>();
            dg = new DependencyGraph();
            changed = false;
            valid = isValid;
        }

        /// <summary>
        /// creates a Spreadsheet from a saved file
        /// </summary>
        public Spreadsheet(TextReader source, Regex newIsValid)//CHANGE HERE FOR REGRADE
        {
            cells = new Dictionary<string, Cell>();
            dg = new DependencyGraph();
            changed = false;

            try
            {

                XmlSchemaSet sc = new XmlSchemaSet();
                sc.Add(null, "Spreadsheet.xsd");

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas = sc;


                //using xml reader to read file
                try
                {
                    XmlReader reader = XmlReader.Create(source, settings);//CHANGE HERE

                    //cell name
                    string cellName = "";
                    //cell value
                    string cellContent = "";

                    bool RegexMatch = true;

                    bool setContents;

                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                             setContents = false;
                            string name = reader.Name;

                            switch (name)
                            {
                                case "spreadsheet":
                                    string v =  reader.GetAttribute("IsValid");
                                    try
                                    {


                                        oldValid = new Regex(v);
                                    }
                                    catch (Exception)
                                    {
                                        throw new SpreadsheetReadException("invalid Regex");
                                    }
                                    

                                    if (string.IsNullOrEmpty(oldValid.ToString()))
                                    {
                                        RegexMatch = false;
                                    }
                                                                   
                                   
                                     else if (oldValid.ToString().Equals(newIsValid.ToString()))
                                    {
                                        RegexMatch = true;
                                        
                                    }



                                    valid = newIsValid;

                                    break;

                                //read the cell name
                                case "cell":
                                    string a = reader.GetAttribute("name");
                                    cellName = a;
                                    string b = reader.GetAttribute("contents");
                                    cellContent = b;
                                    setContents = true;

                                 

                                    if(!Regex.IsMatch(a, newIsValid.ToString()))
                                    {
                                       
                                        throw new SpreadsheetVersionException("Spreadsheet version");
                                    }

                                    if (!Regex.IsMatch(a, oldValid.ToString()))
                                    {
                                        throw new SpreadsheetReadException("invalid regex");
                                    }
                                    break;

                                    

                            }

                            if (!RegexMatch){
                                throw new SpreadsheetReadException("Regex does not match");
                            }
                            
                            
                            

                            if (setContents)
                            {
                                if (cells.ContainsKey(cellName))
                                {
                                    throw new SpreadsheetReadException("duplicate cell");
                                }
                                SetContentsOfCell(cellName, cellContent);
                            }
                        }
                    }

                    reader.Close();
                }
                catch (XmlException)
                {
                    throw new SpreadsheetReadException("Spreadsheet cannot be read");
                }
            }
            catch (XmlSchemaException)
            {
                throw new SpreadsheetReadException("Spreadsheet cannot be read");

            }

        }

        // ADDED FOR PS6
        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed
        {
            get => changed;
            protected set => changed = value;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            if (name == null || !ValidName(name))
            {
                throw new InvalidNameException();
            }

            name = Normalize(name);
            //return conents of cell with name
            try
            {
                return cells[name].contents;
            }
            catch (KeyNotFoundException)
            {
                return "";
            }



        }

        /// <summary>
        /// checks for valid names of cells
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private Boolean ValidName(string n)
        {
            n = Normalize(n);
            return Regex.IsMatch(n, valid.ToString());
        }

        private string Normalize(string n)
        {
            return n.ToUpper();
        }

        // ADDED FOR PS6
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            
            if (name == null || !ValidName(name))
            {
                throw new InvalidNameException();
            }
            // return cell value;
            try
            {
                name = Normalize(name);
                return cells[name].value;

            }
            catch (KeyNotFoundException)
            {
                return "";
            }


        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            HashSet<string> nonEmpty = new HashSet<string>();
            // get cells that are not empty
            foreach(KeyValuePair<string, Cell> kvp in cells)
            {
                if (!kvp.Value.contents.Equals(""))
                {
                    nonEmpty.Add(kvp.Key.ToString());
                }
            }
            return nonEmpty;
        }

        // ADDED FOR PS6
        /// <summary>
        /// Writes the contents of this spreadsheet to dest using an XML format.
        /// The XML elements should be structured as follows:
        ///
        /// <spreadsheet IsValid="IsValid regex goes here">
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        /// </spreadsheet>
        ///
        /// The value of the IsValid attribute should be IsValid.ToString()
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.
        /// If the cell contains a string, the string (without surrounding double quotes) should be written as the contents.
        /// If the cell contains a double d, d.ToString() should be written as the contents.
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        ///
        /// If there are any problems writing to dest, the method should throw an IOException.
        /// </summary>
        public override void Save(TextWriter dest)
        {
            try
            {
                //xm. write to save file
                using (XmlWriter writer = XmlWriter.Create(dest))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("IsValid", valid.ToString());
                    foreach (string cell in cells.Keys)
                    {
                        writer.WriteStartElement("cell");
                        writer.WriteAttributeString("name", cell);

                        string content;

                        if (cells[cell].contents is double)
                        {
                            content = cells[cell].contents.ToString();
                        }
                        else if (ContainsVariables(cells[cell].contents.ToString()))
                        {
                            content = "=" + cells[cell].contents.ToString();
                        }
                        else
                        {
                            content = cells[cell].contents.ToString();
                        }

                        writer.WriteAttributeString("contents", content);
                        writer.WriteEndElement();


                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch (XmlException)
            {
                throw new IOException();
            }

            changed = false;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, double number)
        {
            //create new cell
            Cell cell = new Cell(number);

            //SetcontentsofCell checks this, no longer needed
            //if (name == null || !ValidName(name))
            //{
            //    throw new InvalidNameException();
            //}
            if (cells.ContainsKey(name))
            {
                cells[name] = cell;
            }
            else
            {
                cells.Add(name, cell);
            }

            dg.ReplaceDependees(name, new HashSet<String>());

            HashSet<String> dependees = new HashSet<string>(GetCellsToRecalculate(name));
            return dependees;
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, string text)
        {
            //Setcontents of cell will check
            // if (name == null)
            //{
            //     throw new InvalidNameException();
            // }

            //  if (!ValidName(name))
            //  {
            //      throw new InvalidNameException();
            //  }
            //  if(text == null)
            //  {
            //      throw new ArgumentNullException();
            //  }

            Cell cell = new Cell(text);
            if (cells.ContainsKey(name))
            {

                cells[name] = cell;
            }
            else
            {
                cells.Add(name, cell);
            }

            string c = (string)cells[name].contents;

            

           
            dg.ReplaceDependees(name, new HashSet<String>());

            HashSet<String> dependees = new HashSet<string>(GetCellsToRecalculate(name));
            return dependees;

        }

        /// <summary>
        /// Requires that all of the variables in formula are valid cell names.
        /// 
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            //setcontents of cell checks now
            // if (name == null || !ValidName(name))
            //  {
            //      throw new InvalidNameException();
            // }

            IEnumerable<string> dep = dg.GetDependees(name);
            dg.ReplaceDependees(name, formula.GetVariables());

            try
            {
                
                HashSet<string> all = new HashSet<string>(GetCellsToRecalculate(name));
                Cell cell = new Cell(formula, lookupValue);

                if (cells.ContainsKey(name))
                {
                    cells[name] = cell;
                }
                else
                {
                    cells.Add(name, cell);
                }



                return all;
            } catch (CircularException)
            {
                dg.ReplaceDependees(name, dep);
                throw new CircularException();
            }




        }

        /// <summary>
        /// lookups the values in formulas if the cell contains a formula
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private double lookupValue(string s)
        {
           

            if (cells[s].value is double)
            {
                return (double)cells[s].value;
            }

            throw new ArgumentException();

        }

        // ADDED FOR PS6
        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        ///
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        ///
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor with s => s.ToUpper() as the normalizer and a validator that
        /// checks that s is a valid cell name as defined in the AbstractSpreadsheet
        /// class comment.  There are then three possibilities:
        ///
        ///   (1) If the remainder of content cannot be parsed into a Formula, a
        ///       Formulas.FormulaFormatException is thrown.
        ///
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///
        ///   (3) Otherwise, the contents of the named cell becomes f.
        ///
        /// Otherwise, the contents of the named cell becomes content.
        ///
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {

            HashSet<string> dep;

            if (content == null)
            {
                throw new ArgumentNullException("content cannot be null");
            }

            if (name == null || !ValidName(name))
            {
                throw new InvalidNameException();
            }
            name = Normalize(name);

            if (double.TryParse(content, out double d))
            {

                dep = new HashSet<string>(SetCellContents(name, d));
                changed = true;
            }
            else if (content == "")
            {
                
                dep = new HashSet<string>(SetCellContents(name, content));
                changed = true;
            }
            else if (content.Substring(0, 1).Equals("="))
            {
                string formula = content.Substring(1, content.Length - 1);
                Formula f = new Formula(formula, s => s.ToUpper(), s => ValidName(s));
                changed = true;
                dep = new HashSet<string>(SetCellContents(name, f));

            }
            else
            {
                changed = true;
                dep = new HashSet<string>(SetCellContents(name, content));

            }

          foreach(string s in dep)
            {
                if (ContainsVariables(cells[s].contents.ToString())){
                    Formula f = new Formula(cells[s].contents.ToString());
                    SetCellContents(s, f);
                }
            }

            return dep;
        }




        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }

            if (!ValidName(name))
            {
                throw new InvalidNameException();
            }

            name = Normalize(name);
            
            return dg.GetDependents(name);
        }

        //checks if string contains variables
        private bool ContainsVariables(string s)
        {
            if(Regex.IsMatch(s, @"[A-Z]+[0-9]+"))
            {
                return true;
            }

            return false;
        }
      


        /// <summary>
        /// private class Cell creates a cell object
        /// </summary>
        private class Cell 
        {
            public Object contents
            {
                get;
                set;
            }

            public Object value
            {
                get;  set;
            }
            string content_type;
            string value_type;
            
            /// <summary>
            /// constructs a Cell of type string
            /// </summary>
            /// <param name="name"></param>
            public Cell(string name)
            {
                contents = name;
                content_type = contents.GetType().ToString();
                value = contents;
                value_type = value.GetType().ToString();
               
            }

            /// <summary>
            /// constructs a cell of type double
            /// </summary>
            /// <param name="name"></param>
            public Cell(double name)
            {
                contents = name;
                value = contents;
                content_type = contents.GetType().ToString();
                value_type = value.GetType().ToString();
                
            }
            /// <summary>
            /// constructs a Cell of Formula and Lookup
            /// </summary>
            /// <param name="f"></param>
            /// <param name="l"></param>

            public Cell(Formula f, Lookup l)
            {
                contents = f.ToString();
                content_type = typeof(Formula).ToString();
                try
                {
                    value = f.Evaluate(l);
                    
                }catch(Exception)
                {
                    value = new FormulaError();
                    
                    
                }
                value_type = value.GetType().ToString();
              

                
            }

        }
    }
      

}
