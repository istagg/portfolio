using SpreadsheetGrid_Core;
using SpreadsheetUtilities;
using SS;
using System.Diagnostics;
using System.Text.RegularExpressions;


/// <summary>
/// Author:    Isaac Stagg
/// Partner:   Nate Tripp
/// Date:      02/24/2022
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Isaac Stagg and Nate Tripp - This work may not be copied for use in Academic Coursework.
///
/// I, Isaac Stagg and Nate Tripp, certify that I wrote this code from scratch and did not copy it in part or whole from
/// another source.  All references used in the completion of the assignment are cited in my README file.
///
/// File Contents
/// This file contains the GUI logic and implementation of our spreadsheet. This GUI allows typical spreadsheet usage such as
/// writing content to a cell, storing values in a cell, and computing simple arithmetic when a user enters a formula.
/// Additionally this GUI allows the user to open up more Spreadsheets load spreadsheets, and save spreadsheets to disk.
/// </summary>
namespace GUI {
    /// <summary>
    /// This class represents a single spreadsheet GUI application.
    /// </summary>
    public partial class SpreadsheetGUI : Form {

        /// <summary>
        /// Abstract spreadsheet object to hold all spreadsheet logic.
        /// </summary>
        private AbstractSpreadsheet _sheet = new Spreadsheet(_ => true, s => s.ToUpper(), "six");
        
        private bool _currency = false;

        /// <summary>
        /// SImple constructor that constructs displays a simple spreadsheet GUI.
        /// </summary>
        public SpreadsheetGUI() {
            InitializeComponent();
            SSGridWidget.SetSelection(0, 0);
        }
        
        /// <summary>
        /// Converts the SSGridWidget cell location into its corresponding cell name i.e. (0,0) -> 'A1'
        /// </summary>
        /// <returns>The corresponding cell name, i.e. "A1"</returns>
        private string GetCurrentCellName() {
            SSGridWidget.GetSelection(out var col, out var row);
            return (char)('A' + col) + (row + 1).ToString();
        }

        /// <summary>
        /// Causes the <see cref="AbstractSpreadsheet"/> to update any necessary cells and then the GUI is changed reflect any new data.
        /// </summary>
        private void UpdateSheet() {
            SSGridWidget.GetSelection(out var currentCol, out var currentRow);

            var cells = _sheet.GetNamesOfAllNonemptyCells();
            foreach (var cell in cells) {
                int col = (int) (cell[0] - 'A');
                int row = int.Parse(new string(cell.Skip(1).ToArray())) -1;

                var value = _sheet.GetCellValue(cell);
                string? strVal;

                if (value is FormulaError formulaError) {
                    strVal = formulaError.Reason;
                } else {
                    strVal = value.ToString();
                }

                if (col == currentCol && row == currentRow) {
                    TxtBoxCellValue.Text = strVal;
                }

                if(_currency && value is double) {
                    strVal = $"${value:0.00}";
                }
                SSGridWidget.SetValue(col, row, strVal);

            }
        }

        /// <summary>
        /// Causes the application to save the current state of Spreadsheet to disk.
        /// </summary>
        private bool Save() {
            var result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK) {
                string filepath = saveFileDialog1.FileName;
                _sheet.Save(filepath);
                return true;
            }
            return false;
        }
       
        /// <summary>
        /// Loads a spreadsheet from disk, prompting to save the current spreadsheet if a change has occurred.
        /// </summary>
        private void Load() {
            TxtBoxCellContent_Leave(null, null);
            var result = openFileDialog1.ShowDialog();
            var saveSuccess = true;
            if (result == DialogResult.OK) {
                if (_sheet.Changed) {
                    var saveResult = MessageBox.Show("Would you like to save the current spreadsheet? All unsaved changes will be lost.", "Spreadsheet is not saved", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (saveResult == DialogResult.Yes) {
                        saveSuccess = Save();
                    }
                }
                if (saveSuccess) {
                    SSGridWidget.Clear();
                    string filepath = openFileDialog1.FileName;
                    _sheet = new Spreadsheet(filepath, _ => true, s => s.ToUpper(), "six");
                    UpdateSheet();
                }
            }
        }
        
        /// <summary>
        /// Event called every time the user exits the cell content text box. Causes the GUI to update its cells.
        /// </summary>
        private void TxtBoxCellContent_Leave(object _, EventArgs __) {
            try {
                _sheet.SetContentsOfCell(GetCurrentCellName(), TxtBoxCellContent.Text);
                UpdateSheet();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Event called whenever the TxtBoxCellContent text box has changing text.
        /// </summary>
        private void TxtBoxCellContent_TextChanged(object sender, EventArgs __) {
            if (sender == TxtBoxCellContent) {
                SSGridWidget.GetSelection(out var col, out var row);
                SSGridWidget.SetValue(col, row, TxtBoxCellContent.Text);
            }
        }

        /// <summary>
        /// Event called every time the user changes their current cell selection in the GUI.
        /// </summary>
        private void SSGridWidget_SelectionChanged(SpreadsheetGridWidget _) {
            TxtBoxCellName.Text = GetCurrentCellName();
            var contents = _sheet.GetCellContents(TxtBoxCellName.Text);
            if (contents is Formula) {
                TxtBoxCellContent.Text = "=" + contents.ToString();
            } else {
                TxtBoxCellContent.Text =  contents.ToString();
            }
            var value = _sheet.GetCellValue(TxtBoxCellName.Text);
            if (value is FormulaError formulaError) {
                TxtBoxCellValue.Text = formulaError.Reason;
            } else {
                TxtBoxCellValue.Text = value.ToString();
            }

            TxtBoxCellContent.Select();
        }

        /// <summary>
        /// Called when the 'New' menu item is selected.
        /// </summary>
        private void NewToolStripMenuItem_Click(object _, EventArgs __) {
            Process.Start(Environment.ProcessPath!);
        }

        /// <summary>
        /// Called when the 'Exit' menu item is selected.
        /// </summary>
        private void ExitToolStripMenuItem_Click(object _, EventArgs __) {
            Close();
        }

        /// <summary>
        /// Called when the 'Save' menu item is selected.
        /// </summary>
        private void SaveToolStripMenuItem_Click(object _, EventArgs __) {
             Save();
        }

        /// <summary>
        /// Event triggered just before the GUI is closed. Displays a message indicating that there is unsaved data if applicable.
        /// </summary>
        /// <param name="e">Contains arguments about the form closing.</param>
        private void SpreadsheetGUI_FormClosing(object _, FormClosingEventArgs e) {
            if (_sheet.Changed) {
                var result = MessageBox.Show("Are you sure you want to close? All changes will be lost.", "Spreadsheet is not saved", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (result.HasFlag(DialogResult.No)) {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Event called when a key is pressed in the content text box.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Arguments containing the key event.</param>
        private void TxtBoxCellContent_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
               case Keys.Down:
               case Keys.Return: {
                     TxtBoxCellContent_Leave(sender, e);
                     SSGridWidget.GetSelection(out var col, out var row);
                     SSGridWidget.SetSelection(col, row + 1, true);
                  }
                  break;
               case Keys.Up: {
                     TxtBoxCellContent_Leave(sender, e);
                     SSGridWidget.GetSelection(out var col, out var row);
                     SSGridWidget.SetSelection(col, row - 1, true);
                  }
                  break;
               case Keys.Left: {
                     TxtBoxCellContent_Leave(sender, e);
                     SSGridWidget.GetSelection(out var col, out var row);
                     SSGridWidget.SetSelection(col - 1, row, true);
                  }
                  break;
               case Keys.Right: {
                     TxtBoxCellContent_Leave(sender, e);
                     SSGridWidget.GetSelection(out var col, out var row);
                     SSGridWidget.SetSelection(col + 1, row, true);
                  }
                  break;
            }
        }

        /// <summary>
        /// Event called when the load menu strip item is pressed.
        /// </summary>
        private void loadToolStripMenuItem_Click(object _, EventArgs __) {
            Load();
        }

        /// <summary>
        /// Event called when the help menu item is clicked.
        /// </summary>
        private void helpToolStripMenuItem_Click(object _, EventArgs __) {
            new HelpForm().ShowDialog();
        }

        /// <summary>
        /// Event called when the check box is toggled.
        /// </summary>
        private void ChkBxCurrency_CheckedChanged(object _, EventArgs __) {
            _currency = ChkBxCurrency.Checked;
            UpdateSheet();
        }

        /// <summary>
        /// Called whenever a key down event occurs.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">Key event arguments.</param>
        private void SpreadsheetGUI_KeyDown(object sender, KeyEventArgs e) {
            if(e.Control) {
                e.Handled = true;
                switch(e.KeyCode) {
                    case Keys.Q:
                        ChkBxCurrency.Checked = !ChkBxCurrency.Checked;
                        ChkBxCurrency.Invalidate();
                        ChkBxCurrency_CheckedChanged(sender, e);
                        break;
                    case Keys.S:
                        if(_sheet.Changed) {
                            Save();
                        }
                        break;
                    case Keys.C:
                        TxtBoxCellContent_Leave(sender, e);
                        var cellName = GetCurrentCellName();
                        var content = _sheet.GetCellContents(cellName);
                        if(content != null) {
                            if(content is Formula) {
                                Clipboard.SetText("=" + content.ToString());
                            }
                            else {
                                Clipboard.SetText(content.ToString());
                            }
                        }
                        break;
                    case Keys.V:
                        TxtBoxCellContent_Leave(sender, e);
                        break;

                }
            }
        }
   }
}