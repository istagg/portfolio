/// <summary>
/// Author:    Isaac Stagg
/// Partner:   None
/// Date:      2/6/2022
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Isaac Stagg - This work may not be copied for use in Academic Coursework.
///
/// I, Isaac Stagg, certify that I wrote this code from scratch and did not copy it in part or whole from
/// another source.  All references used in the completion of the assignment are cited in my README file.
///
/// File Contents
/// This file creates a spreadsheet object according to the API provided by AbstractSpreadsheet. This serves
/// as the back-end of the spreadsheet. Cells are stored and their contents can be changed between a string,
/// double, and Formula. Cells also hold the calculated value of a formula provided. Dependencies are also
/// created between the cells and stored for future formula evaluation. Spreadsheets can be saved as XML
/// files and new spreadsheet objects can be created from that file. Beware the versions must match.
/// </summary>

using SpreadsheetUtilities;
using System.Text.RegularExpressions;
using System.Xml;

namespace SS {

    /// <summary>
    /// Creates a Cell object that is used by a Spreadsheet. A cell has a name, contents and type. In the
    /// future this class will also contain a value parameter which is the calculated value of the formula
    /// or a formula error. The type can either be a string, double, or Formula.
    /// </summary>
    public class Cell {
        public Type Type { get; set; }
        public string Name { get; }
        public object Contents { get; set; }
        public object Value { get; private set; }

        public Cell(string name, string contents) {
            Name = name;
            Contents = contents;
            Value = contents;
            Type = typeof(string);
        }

        public Cell(string name, double contents) {
            Name = name;
            Contents = contents;
            Value = contents;
            Type = typeof(double);
        }

        public Cell(string name, Formula contents, Func<string, double> lookup) {
            Name = name;
            Contents = contents;
            Value = contents.Evaluate(lookup);
            Type = typeof(Formula);
        }

        /// <summary>
        /// Helper method that is used to recalculate a cell's value given a change in dependencies.
        /// </summary>
        /// <param name="lookup">A function holding the values of all variables needed by the formula</param>
        public void RecalculateValue(Func<string, double> lookup) {
            if (Type.Equals(typeof(Formula))) {
                Formula formula = (Formula)Contents;
                Value = formula.Evaluate(lookup);
            } else {
                Value = Contents;
            }
        }
    }

    /// <inheritdoc/>
    public class Spreadsheet : AbstractSpreadsheet {
        private Dictionary<string, Cell> cells;
        private DependencyGraph spreadsheetVariables;

        public override bool Changed { get; protected set; }

        /// <summary>
        /// Constructor for a new, empty spreadsheet.
        /// </summary>
        public Spreadsheet() : this(_ => true, sNormalize => sNormalize, "default") { }

        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version) {
            cells = new Dictionary<string, Cell>();
            spreadsheetVariables = new DependencyGraph();
            Changed = false;
        }

        /// <summary>
        /// Reads a saved spreadsheet from a file (see save method) and uses it to construct a new spreadsheet
        /// </summary>
        public Spreadsheet(string filePath, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version) {
            cells = new Dictionary<string, Cell>();
            spreadsheetVariables = new DependencyGraph();
            ReadXML(filePath, version);
            Changed = false;
        }

        /// <summary>
        /// Helper method for opening an XML spreadsheet and setting up the data structures with the saved
        /// information
        /// </summary>
        /// <param name="filePath">As name states</param>
        /// <param name="version">The version of the stored spreadsheet</param>
        /// <exception cref="SpreadsheetReadWriteException">
        ///     Thrown if:
        ///     <list type="bullet">
        ///         <item>Provided version does not match stored spreadsheet version</item>
        ///         <item>An unknown XML tag is encountered</item>
        ///         <item>The provided file path does not exist</item>
        ///         <item>An empty file path is provided</item>
        ///     </list>
        /// </exception>
        private void ReadXML(string filePath, string version) {
            string holdName = "";
            try {
                using (XmlReader reader = XmlReader.Create(filePath)) {
                    while (reader.Read()) {
                        if (reader.IsStartElement()) {
                            switch (reader.Name) {
                                case "spreadsheet":
                                    if (reader["version"] != version) throw new SpreadsheetReadWriteException("Versions do not match");
                                    break;

                                case "name":
                                    reader.Read();
                                    holdName = reader.Value;
                                    break;

                                case "contents":
                                    reader.Read();
                                    SetContentsOfCell(holdName, reader.Value);
                                    holdName = ""; // used the value. for error checking.
                                    break;

                                case "cell":
                                    break; // fall through
                                default:
                                    throw new SpreadsheetReadWriteException("");
                            }
                        } else {
                            if (reader.Name.Equals("cell") && holdName != "") throw new SpreadsheetReadWriteException("Missing contents");
                        }
                    }
                }
            } catch (Exception) {
                throw new SpreadsheetReadWriteException("Can't follow that file path");
            }
        }

        /// <inheritdoc/>
        /// <param name="name">Name of the cell to get the contents of</param>
        public override object GetCellContents(string name) {
            CheckValidName(name);
            name = Normalize(name);
            if (cells.ContainsKey(name)) {
                return cells[name].Contents;
            } else {
                return ""; // empty cell returns empty string
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells() {
            if (cells.Count == 0) return new HashSet<string>();
            HashSet<string> names = new();
            foreach (var cell in cells) {
                names.Add(cell.Key);
            }
            return names;
        }

        /// <summary>
        /// Checks if the given name is fits the cell naming convention
        /// </summary>
        /// <param name="name">The given cell name</param>
        /// <exception cref="InvalidNameException">Throws if the name does not meet format</exception>
        private void CheckValidName(string name) {
            if (!Regex.IsMatch(name, "^[a-zA-Z]+[0-9]+") || !IsValid(name)) throw new InvalidNameException();
        }

        /// <summary>
        /// A recursive method to get all dependents (indirect or direct). Used in SetCellContents method
        /// with formula parameter to get all old dependencies to remove.
        /// </summary>
        /// <param name="dependents">The current list of dependents</param>
        /// <param name="name">Name of cell of which dependents are wanted</param>
        /// <returns>A list of dependents of the given name</returns>
        private List<string> RecursiveDependents(List<string> dependents, string name) {
            foreach (var dependent in spreadsheetVariables.GetDependents(name)) {
                dependents.Add(dependent);
                if (spreadsheetVariables.HasDependents(dependent)) {
                    RecursiveDependents(dependents, dependent);
                }
            }
            return dependents;
        }

        /// <summary>
        /// A helper method that the Cell class can use to lookup the values of cells in a formula.
        /// </summary>
        /// <param name="name">Variable name</param>
        /// <returns>A double containing the value of the variable</returns>
        /// <exception cref="ArgumentException">Thrown if the variable does not have a defined value</exception>
        private double Lookup(string name) {
            if (cells.ContainsKey(name) && cells[name].Type != typeof(string) && !cells[name].Value.GetType().Equals(typeof(FormulaError))) {
                return (double)cells[name].Value;
            } else {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// A helper method to remove unnecessary repeated code between SetCellContents double and string.
        /// Harder to extend this to work with formula also so kept them separate.
        /// </summary>
        /// <param name="name">The name of the cell</param>
        /// <param name="number">If this was used by the double method then the double, if not, null</param>
        /// <param name="text">If this was used by the string method then the string, if not, null</param>
        /// <returns>The List required for SetCellContents methods. See that method for more detail</returns>
        /// <exception cref="InvalidNameException">Throws if the name does not meet the specifications</exception>
        private IList<string> SetCellContentsHelper(string name, double? number, string? text) {
            if (cells.ContainsKey(name)) {
                if (cells[name].Type == typeof(Formula)) // 'is' keyword not working here for some reason
                {
                    Formula formula = (Formula)cells[name].Contents;
                    foreach (string variable in formula.GetVariables()) {
                        spreadsheetVariables.RemoveDependency(name, variable);
                    }
                }
                if (number != null) {
                    cells[name].Type = typeof(double);
                    cells[name].Contents = number;
                } else if (text != null) {
                    if (!spreadsheetVariables.HasDependees(name) && !spreadsheetVariables.HasDependents(name) && text.Equals("")) {
                        cells.Remove(name);
                        return new List<string>();
                    } else {
                        cells[name].Type = typeof(string);
                        cells[name].Contents = text;
                    }
                }
            } else {
                Cell tempCell;
                if (number != null) {
                    tempCell = new Cell(name, (double)number);
                } else {
                    tempCell = new Cell(name, text!);
                    // yes this is risky but this method is only going to be used in two places:
                    // SetCellContents for string and for double so if a double wasn't passed it was a string
                }
                cells.Add(name, tempCell);
            }
            return GetCellsToRecalculate(name).ToList();
        }

        /// <inheritdoc/>
        /// <param name="name">Name of cell</param>
        /// <param name="number">Number to set cell to</param>
        protected override IList<string> SetCellContents(string name, double number) {
            return SetCellContentsHelper(name, number, null);
        }

        /// <inheritdoc/>
        /// <param name="name">Name of cell</param>
        /// <param name="text">String to set cell to</param>
        protected override IList<string> SetCellContents(string name, string text) {
            return SetCellContentsHelper(name, null, text);
        }

        /// <inheritdoc/>
        /// <param name="name">Name of cell</param>
        /// <param name="formula">Formula object to set cell to</param>
        protected override IList<string> SetCellContents(string name, Formula formula) {
            List<string> formulaVariables = formula.GetVariables().ToList();
            // check for circular dependency.
            // I know the GetCellsToRecalculate method also checks for circular
            // dependencies however it would be too complicated (add the dependency, check if it is circular,
            // if it is, remove the dependency and reset contents) vs checking before setting dependencies
            // and changing contents.
            foreach (string variable in formulaVariables) {
                if (!IsValid(variable)) throw new FormulaFormatException("Invalid variable");
                if (variable.Equals(name)) throw new CircularException();
                foreach (var dependent in RecursiveDependents(new List<string>(), variable)) {
                    if (dependent.Equals(name)) throw new CircularException();
                }
            }

            if (cells.ContainsKey(name)) {
                if (cells[name].Type == typeof(Formula)) {
                    // if it already is a formula, must remove old dependencies
                    Formula oldFormula = (Formula)GetCellContents(name);
                    foreach (var oldVariable in oldFormula.GetVariables().ToList()) {
                        spreadsheetVariables.RemoveDependency(name, oldVariable);
                    }
                } else {
                    cells[name].Type = typeof(Formula);
                }
                cells[name] = new Cell(name, formula, s => Lookup(s));
            } else {
                Cell tempCell = new(name, formula, s => Lookup(s));
                cells.Add(name, tempCell);
            }
            // add in the new dependencies
            foreach (string variable in formulaVariables) {
                spreadsheetVariables.AddDependency(name, variable);
            }
            return GetCellsToRecalculate(name).ToList();
        }

        /// <inheritdoc/>
        /// <param name="name">Name of cell</param>
        protected override IEnumerable<string> GetDirectDependents(string name) {
            return spreadsheetVariables.GetDependees(name);
        }

        /// <inheritdoc/>
        /// <param name="name">Name of cell</param>
        /// <param name="content">A string containing either text, a double, or a formula which begins with '='</param>
        public override IList<string> SetContentsOfCell(string name, string content) {
            name = Normalize(name);
            CheckValidName(name);
            if (content.Equals("") && !cells.ContainsKey(name)) {
                return new List<string>() { name };
            }
            Changed = true;
            List<string> recalculationOrder;
            if (double.TryParse(content, out double num)) { // check if contents parses as double. if so, call the set contents for double
                recalculationOrder = SetCellContents(name, num).ToList();
            } else if (!content.Equals("") && content[0].Equals('=')) { // if not, check if content begins with =. if so, remove the equal and send the rest to the set contents for formula
                recalculationOrder = SetCellContents(name, new Formula(content.TrimStart('='), Normalize, IsValid)).ToList();
            } else { // if not, send to set contents string method
                recalculationOrder = SetCellContents(name, content).ToList();
            }
            foreach (var variable in recalculationOrder) {
                cells[variable].RecalculateValue(s => Lookup(s));
            }
            return recalculationOrder;
        }

        /// <inheritdoc/>
        /// <param name="filename">As name states</param>
        /// <exception cref="SpreadsheetReadWriteException">
        ///     Thrown if:
        ///     <list type="bullet">
        ///         <item>The version attribute is missing</item>
        ///         <item>The file path could not be followed</item>
        ///         <item>The Spreadsheet XML tag is missing</item>
        ///     </list>
        /// </exception>
        public override string GetSavedVersion(string filename) {
            try {
                using (XmlReader reader = XmlReader.Create(filename)) {
                    while (reader.Read()) {
                        if (reader.IsStartElement() && reader.Name.Equals("spreadsheet")) {
                            if (reader["version"] != null) {
                                return reader["version"]!;
                            } else {
                                throw new SpreadsheetReadWriteException("Could not find version information");
                            }
                        }
                    }
                }
            } catch (DirectoryNotFoundException) {
                throw new SpreadsheetReadWriteException("Error occurred opening file");
            }
            // missing spreadsheet opening tag
            throw new SpreadsheetReadWriteException("Could not find version information");
        }

        /// <inheritdoc/>
        /// <param name="filename">As name states</param>
        public override void Save(string filename) {
            XmlWriterSettings settings = new();
            settings.Indent = true;
            settings.IndentChars = "  ";
            try {
                using (XmlWriter writer = XmlWriter.Create(filename, settings)) {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");

                    writer.WriteAttributeString("version", Version);

                    foreach (var pair in cells) {
                        Cell cell = pair.Value;
                        writer.WriteStartElement("cell");
                        writer.WriteElementString("name", cell.Name);
                        if (cell.Type == typeof(Formula)) {
                            writer.WriteElementString("contents", "=" + cell.Contents.ToString());
                        } else {
                            writer.WriteElementString("contents", cell.Contents.ToString());
                        }
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    Changed = false;
                }
            } catch {
                throw new SpreadsheetReadWriteException("Could not find path to save");
            }
        }

        /// <inheritdoc/>
        /// <param name="name">Name of cell</param>
        public override object GetCellValue(string name) {
            CheckValidName(name);
            name = Normalize(name);
            if (!cells.ContainsKey(name)) return "";
            return cells[name].Value;
        }
    }
}