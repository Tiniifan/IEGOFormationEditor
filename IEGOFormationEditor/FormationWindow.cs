using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using IEGOFormationEditor.Tools;
using System.Windows.Forms.DataVisualization.Charting;
using IEGOFormationEditor.Level5.Binary;
using IEGOFormationEditor.InazumaEleven.Logic;
using IEGOFormationEditor.InazumaEleven.Common;
using IEGOFormationEditor.Level5.Binary.Logic;

namespace FormationInazumaClass
{
    public partial class FormationWindow : Form
    {
        private bool IsMousePressed = false;

        private CfgBin FormationConfig = new CfgBin();

        private List<Formation> Formations = new List<Formation>();

        public FormationWindow()
        {
            InitializeComponent();

            // Set Transparency
            selectedPlayerLabel.Parent = chart1;
            goalkeeperButton.Parent = chart1;
            defenderButton.Parent = chart1;
            midfielderButton.Parent = chart1;
            forwardButton.Parent = chart1;
            selectPlayerBox.Parent = chart1;
            locationYNumericUpDown.Parent = chart1;
            locationXNumericUpDown.Parent = chart1;
            label1.Parent = chart1;
            label2.Parent = chart1;

            // Create Background Chart
            chart1.Images.Add(new NamedImage("Team.png", Image.FromStream(new ResourceReader("Team.png").GetResourceStream())));
            chart1.Images.Add(new NamedImage("Map.png", Image.FromStream(new ResourceReader("Map.png").GetResourceStream())));
            chart1.Images.Add(new NamedImage("NoBackground.png", Image.FromStream(new ResourceReader("NoBackground.png").GetResourceStream())));

            // Create Marker
            for (int i = 0; i < 11; i++)
            {
                string markerPath = "customMarker" + i + ".png";
                chart1.Images.Add(new NamedImage(markerPath, null));
                chart1.Series["player" + i].MarkerImage = "customMarker" + i + ".png";
            }
        }

        private List<Formation> OpenFormationConfigFile(CfgBin formationConfig)
        {
            List<Formation> output = new List<Formation>();

            if (formationConfig.Entries[0].Count() > 0 && formationConfig.Entries[0].GetName() == "FORM_START")
            {
                // The file has several formations
                int formationCount = Convert.ToInt32(formationConfig.Entries[0].Variables[0].Value);

                for (int i = 0; i < formationCount; i++)
                {
                    List<IFormationInfo> formationInfo = new List<IFormationInfo>();
                    FormationHeader formationHeader = formationConfig.Entries[0].Children[i].ToClass<FormationHeader>();

                    if (formationHeader.FormationType == 3)
                    {
                        formationInfo.AddRange(formationConfig.Entries[0].Children[i].Children
                            .Select(x => x.ToClass<Battle>())
                            .ToList());
                    }
                    else if (formationHeader.FormationType == 9)
                    {
                        formationInfo.AddRange(formationConfig.Entries[0].Children[i].Children
                            .Select(x => x.ToClass<Match>())
                            .ToList());
                    }
                    else
                    {
                        continue;
                    }

                    output.Add(new Formation(formationHeader, formationInfo));
                }
            }
            else if (formationConfig.Entries[0].Count() > 0 && formationConfig.Entries[0].GetName() == "FORM_INFO_BEGIN")
            {
                // The file has one formation

                List<IFormationInfo> formationInfo = new List<IFormationInfo>();
                FormationHeader formationHeader = formationConfig.Entries[0].ToClass<FormationHeader>();

                if (formationHeader.FormationType == 3)
                {
                    formationInfo.AddRange(formationConfig.Entries[0].Children
                        .Select(x => x.ToClass<Battle>())
                        .ToList());
                }
                else if (formationHeader.FormationType == 9)
                {
                    formationInfo.AddRange(formationConfig.Entries[0].Children
                        .Select(x => x.ToClass<Match>())
                        .ToList());
                }
                else
                {
                    // Unknow
                    MessageBox.Show("Unknown formation file");
                    formationConfig = null;
                    output.Clear();
                }

                output.Add(new Formation(formationHeader, formationInfo));
            }
            else
            {
                // Unknow
                MessageBox.Show("Unknown formation file");
                formationConfig = null;
                output.Clear();
            }

            return output;
        }

        private bool SameFormationHash(int hash, string name)
        {
            int crc32 = unchecked((int)Crc32.Compute(Encoding.UTF8.GetBytes(name)));
            return hash == crc32;
        }

        private string FindFormationHash(int hash)
        {
            for (int i = 1; i < 9999; i++)
            {
                string nameHash = "btl_fm" + i.ToString().PadLeft(4, '0');

                if (SameFormationHash(hash, nameHash))
                {
                    return nameHash;
                }
            }

            // Not found
            return "Formation_" + hash.ToString("X8");
        }

        public string[] GetFormationNames(List<Formation> formations)
        {
            return formations
                .Select(form =>
                {
                    string formationName = FindFormationHash(form.Header.FormationHash);

                    if (IEGOFormationEditor.InazumaEleven.Common.Formations.Galaxy.ContainsKey(formationName))
                    {
                        formationName = IEGOFormationEditor.InazumaEleven.Common.Formations.Galaxy[formationName];
                    }

                    return formationName;
                })
                .ToArray();
        }

        private (int hash, string nameHash) FindUnusedFormationHash()
        {
            for (int i = 1; i < 9999; i++)
            {
                string nameHash = "btl_fm" + i.ToString().PadLeft(4, '0');
                int crc32 = unchecked((int)Crc32.Compute(Encoding.UTF8.GetBytes(nameHash)));

                // Check if the current hash is not used by any existing formations.
                if (!Formations.Any(formation => formation.Header.FormationHash == crc32))
                {
                    return (crc32, FindFormationHash(crc32));
                }
            }

            // No unused hash found, return a generic hash.
            return (0x0, "NumberFormationRaised");
        }

        private void ShowFormationChart(bool show)
        {
            strategyComboBox.Enabled = show;
            previousButton.Enabled = show;
            nextButton.Enabled = show;
            panel1.Enabled = show;
            formationListBox.Enabled = show;

            chart1.ChartAreas[0].Visible = show;
            chart1.Legends[0].Enabled = show;
            chart1.Titles[0].Visible = show;
            selectedPlayerLabel.Visible = show;
            selectPlayerBox.Visible = show;
            locationYNumericUpDown.Visible = show;
            locationXNumericUpDown.Visible = show;
            label1.Visible = show;
            label2.Visible = show;
            goalkeeperButton.Visible = show;
            defenderButton.Visible = show;
            midfielderButton.Visible = show;
            forwardButton.Visible = show;
            label5.Visible = show;
            label4.Visible = show;
            label3.Visible = show;
            offensiveStarNumericUpDown.Visible = show;
            defensiveNumericUpDown.Visible = show;
            deletePlayerButton.Visible = show;
            addPlayerButton.Visible = show;
        }

        private void LoadFile(string filename)
        {
            selectPlayerBox.SelectedIndex = -1;

            FormationConfig.Open(File.ReadAllBytes(filename));
            Formations = OpenFormationConfigFile(FormationConfig);

            formationListBox.Items.Clear();
            formationListBox.Items.AddRange(GetFormationNames(Formations));

            bool fileOpened = Formations.Count > 0;

            ShowFormationChart(fileOpened);

            if (fileOpened)
            {
                selectPlayerBox.SelectedIndex = -1;
                formationListBox.SelectedIndex = 0;
                
                saveToolStripMenuItem.Enabled = true;
                chart1.ChartAreas[0].BackImage = "Map.png";
            }
        }

        private void SaveFile(string filename, int index = -1)
        {
            // Reset file
            FormationConfig.Entries.Clear();

            if (index > -1)
            {
                // Export one specific formation

                // Transform header object to a int array
                int[] headerValues = typeof(FormationHeader)
                    .GetProperties()
                    .Select(prop => (int)prop.GetValue(Formations[index].Header))
                    .ToArray();

                // Create begining entry
                Entry newFormationBegin = new Entry("FORM_INFO_BEGIN_" + 0, headerValues.Select(x => new Variable(IEGOFormationEditor.Level5.Binary.Logic.Type.Int, x)).ToList(), FormationConfig.Encoding, true);

                // Create player entry
                for (int j = 0; j < Formations[index].Header.PlayerCount; j++)
                {
                    // Transform formation info object to a object array
                    object[] formationInfoValues = typeof(IFormationInfo)
                        .GetProperties()
                        .Select(prop => prop.GetValue(Formations[index].Players[j]))
                        .ToArray();

                    // Transform formationInfoValues to Variable list
                    List<Variable> variableList = formationInfoValues
                        .Select(x =>
                        {
                            if (x is float)
                            {
                                return new Variable(IEGOFormationEditor.Level5.Binary.Logic.Type.Float, (float)x);
                            }
                            else
                            {
                                return new Variable(IEGOFormationEditor.Level5.Binary.Logic.Type.Int, Convert.ToInt32(x));
                            }
                        })
                        .ToList();

                    newFormationBegin.Children.Add(new Entry("FORM_INFO_" + j, variableList, FormationConfig.Encoding));
                }

                FormationConfig.Entries.Add(newFormationBegin);
            }
            else
            {
                // Export all  formation

                // Create form start entry
                Entry formationStart = new Entry("FORM_START_0", new List<Variable>() { new Variable(IEGOFormationEditor.Level5.Binary.Logic.Type.Int, Formations.Count) }, FormationConfig.Encoding, true);

                // Insert formation data
                for (int i = 0; i < Formations.Count; i++)
                {
                    // Transform header object to a int array
                    int[] headerValues = typeof(FormationHeader)
                        .GetProperties()
                        .Select(prop => (int)prop.GetValue(Formations[i].Header))
                        .ToArray();

                    // Create begining entry
                    Entry newFormationBegin = new Entry("FORM_INFO_BEGIN_" + i, headerValues.Select(x => new Variable(IEGOFormationEditor.Level5.Binary.Logic.Type.Int, x)).ToList(), FormationConfig.Encoding, true);

                    // Create player entry
                    for (int j = 0; j < Formations[i].Header.PlayerCount; j++)
                    {
                        // Transform formation info object to a object array
                        object[] formationInfoValues = typeof(IFormationInfo)
                            .GetProperties()
                            .Select(prop => prop.GetValue(Formations[i].Players[j]))
                            .ToArray();

                        // Transform formationInfoValues to Variable list
                        List<Variable> variableList = formationInfoValues
                            .Select(x =>
                            {
                                if (x is float)
                                {
                                    return new Variable(IEGOFormationEditor.Level5.Binary.Logic.Type.Float, (float)x);
                                }
                                else
                                {
                                    return new Variable(IEGOFormationEditor.Level5.Binary.Logic.Type.Int, Convert.ToInt32(x));
                                }
                            })
                            .ToList();

                        newFormationBegin.Children.Add(new Entry("FORM_INFO_" + j, variableList, FormationConfig.Encoding));
                    }

                    formationStart.Children.Add(newFormationBegin);
                }

                FormationConfig.Entries.Add(formationStart);
            }

            // Save
            File.WriteAllBytes(filename, FormationConfig.Save());
        }

        private void PrintCountPlayer(Formation formation)
        {
            int goalkeeper = formation.Players.Count(x => x.PlayerPosition == 1);
            int defender = formation.Players.Count(x => x.PlayerPosition >= 2 && x.PlayerPosition <= 3);
            int midfielder = formation.Players.Count(x => x.PlayerPosition >= 4 && x.PlayerPosition <= 7);
            int forward = formation.Players.Count(x => x.PlayerPosition >= 8);
            chart1.Titles[0].Text = goalkeeper + " (GK) - " + defender + " (DF) - " + midfielder + " (MF) - " + +forward + " (FW)";
        }

        private void ClearAllPoint()
        {
            for (int i = 0; i < 11; i++)
            {
                chart1.Series["player" + i].Points.Clear();
            }
        }

        private void PrintFormation(Formation formation, int schemaIndex = 0)
        {
            // Count GK/DF/MF/FW
            PrintCountPlayer(formation);

            // Show formation stat
            offensiveStarNumericUpDown.Value = formation.Header.OFStar;
            defensiveNumericUpDown.Value = formation.Header.DFStar;

            if (selectPlayerBox.Items.Count != formation.Players.Count)
            {
                selectPlayerBox.Items.Clear();
            }

            // Fill strategy depending of formation type
            if (formation.Header.FormationType == 3)
            {
                if (strategyComboBox.Items.Count != 4)
                {
                    strategyComboBox.Items.Clear();
                    strategyComboBox.Items.AddRange(Strategies.MiniMatchStrategy);
                }
            } else if (formation.Header.FormationType == 9)
            {
                if (strategyComboBox.Items.Count != 10)
                {
                    strategyComboBox.Items.Clear();
                    strategyComboBox.Items.AddRange(Strategies.BattleMatchStrategy);
                }
            }

            if (strategyComboBox.SelectedIndex == -1)
            {
                strategyComboBox.SelectedIndexChanged -= StrategyComboBox_SelectedIndexChanged;
                strategyComboBox.SelectedIndex = 0;
                schemaIndex = 0;
                strategyComboBox.SelectedIndexChanged += StrategyComboBox_SelectedIndexChanged;
            }

            // Clear point from the chart
            ClearAllPoint();

            // Draw point
            for (int i = 0; i < formation.Players.Count; i++)
            {
                int playerNumber = formation.Players[i].PlayerNumber;

                // Get coordinate
                (float, float) playerCoordinate = GetPlayerCoordinate(formation, i, schemaIndex);

                // Fil selectPlayerComboBox
                if (selectPlayerBox.Items.Count != formation.Players.Count)
                {
                    selectPlayerBox.Items.Add("Player " + (playerNumber + 1));
                }

                // Update formation schema
                chart1.Series["player" + playerNumber].MarkerColor = EnumHelper.GetColor((PlayerPositions)formation.Players[i].PlayerPosition);
                chart1.Series["player" + playerNumber].Points.AddXY(playerCoordinate.Item1, playerCoordinate.Item2);

                // Draw square with player number
                if (playerNumber < 9)
                {
                    chart1.Images["customMarker" + playerNumber + ".png"].Image = Draw.DrawString((playerNumber + 1).ToString(), 5, 3, Image.FromStream(new ResourceReader(EnumHelper.GetShortName((PlayerPositions)formation.Players[i].PlayerPosition) + ".png").GetResourceStream()));
                }
                else
                {
                    chart1.Images["customMarker" + playerNumber + ".png"].Image = Draw.DrawString((playerNumber + 1).ToString(), 2, 3, Image.FromStream(new ResourceReader(EnumHelper.GetShortName((PlayerPositions)formation.Players[i].PlayerPosition) + ".png").GetResourceStream()));
                }
            }

            // Focus player 1
            if (selectPlayerBox.Items.Count > 0)
            {
                selectPlayerBox.SelectedIndex = 0;
            }
        }

        private (float, float) GetPlayerCoordinate(Formation formation, int playerIndex, int schemaIndex)
        {
            // Get coordinate
            float pointX = 0;
            float pointY = 0;

            // Get coordinate location with reflection
            if (formation.Players[playerIndex] is Battle battle)
            {
                pointX = Convert.ToSingle(typeof(Battle).GetProperties()[3 + schemaIndex * 2].GetValue(battle));
                pointY = Convert.ToSingle(typeof(Battle).GetProperties()[4 + schemaIndex * 2].GetValue(battle));
            }
            else if (formation.Players[playerIndex] is Match match)
            {
                pointX = Convert.ToSingle(typeof(Match).GetProperties()[3 + schemaIndex * 2].GetValue(match));
                pointY = Convert.ToSingle(typeof(Match).GetProperties()[4 + schemaIndex * 2].GetValue(match));
            }

            if (pointX > 1) pointX = 1;
            if (pointX < -1) pointX = -1;
            if (pointY > 1) pointY = 1;
            if (pointY < -1) pointY = -1;

            return (pointX, pointY);
        }

        private void ChangePlayerPosition(int newPlayerPosition, int playerIndex)
        {
            // Get Formation
            Formation formation = Formations[formationListBox.SelectedIndex];

            // Get Player Number
            int playerNumber = formation.Players[playerIndex].PlayerNumber;

            // Get coordinate
            (float, float) playerCoordinate = GetPlayerCoordinate(formation, playerIndex, strategyComboBox.SelectedIndex);

            // Update player position
            formation.Players[playerIndex].PlayerPosition = newPlayerPosition;

            // Change Marker Color
            if (playerNumber < 9)
            {
                chart1.Images["customMarker" + playerNumber + ".png"].Image = Draw.DrawString((playerNumber + 1).ToString(), 5, 3, Image.FromStream(new ResourceReader(EnumHelper.GetShortName((PlayerPositions)formation.Players[playerIndex].PlayerPosition) + ".png").GetResourceStream()));
            }
            else
            {
                chart1.Images["customMarker" + playerNumber + ".png"].Image = Draw.DrawString((playerNumber + 1).ToString(), 2, 3, Image.FromStream(new ResourceReader(EnumHelper.GetShortName((PlayerPositions)formation.Players[playerIndex].PlayerPosition) + ".png").GetResourceStream()));
            }
            chart1.Series["player" + playerNumber].Points.Clear();
            chart1.Series["player" + playerNumber].Points.AddXY(playerCoordinate.Item1, playerCoordinate.Item2);

            // Update player text count
            PrintCountPlayer(formation);
        }

        private void UpdatePlayerLocation(float newLocationX, float newLocationY, int playerIndex)
        {
            // Get Formation
            Formation formation = Formations[formationListBox.SelectedIndex];

            // Update coordinate with reflexion
            if (formation.Players[playerIndex] is Battle battle)
            {
                PropertyInfo propX = typeof(Battle).GetProperties()[3 + strategyComboBox.SelectedIndex * 2];
                PropertyInfo propY = typeof(Battle).GetProperties()[4 + strategyComboBox.SelectedIndex * 2];

                propX.SetValue(battle, Convert.ToSingle(newLocationX));
                propY.SetValue(battle, Convert.ToSingle(newLocationY));
            }
            else if (formation.Players[playerIndex] is Match match)
            {
                PropertyInfo propX = typeof(Match).GetProperties()[3 + strategyComboBox.SelectedIndex * 2];
                PropertyInfo propY = typeof(Match).GetProperties()[4 + strategyComboBox.SelectedIndex * 2];

                propX.SetValue(match, Convert.ToSingle(newLocationX));
                propY.SetValue(match, Convert.ToSingle(newLocationY));
            }

            // Get Player Number
            int playerNumber = formation.Players[playerIndex].PlayerNumber;

            chart1.Series["player" + playerNumber].Points.Clear();
            chart1.Series["player" + playerNumber].Points.AddXY(newLocationX, newLocationY);
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.FileName == "openFileDialog1") openFileDialog1.FileName = string.Empty;

            openFileDialog1.Title = "Open Inazuma Eleven Formation file";
            openFileDialog1.Filter = "Inazuma Eleven Formation (*.bin)|*.bin";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                LoadFile(openFileDialog1.FileName);
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = Path.GetFileName(openFileDialog1.FileName);
            saveFileDialog1.InitialDirectory = Path.GetDirectoryName(openFileDialog1.FileName);
            saveFileDialog1.Title = "Save Inazuma Eleven Formation file";
            saveFileDialog1.Filter = "Inazuma Eleven Formation (*.cfg.bin)|*.cfg.bin";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(saveFileDialog1.FileName) == ".bin")
                {
                    int selectedFormationIndex = (Formations.Count > 1) ? -1 : formationListBox.SelectedIndex;
                    SaveFile(saveFileDialog1.FileName, selectedFormationIndex);
                    MessageBox.Show("Formation saved!");
                }
                else if (Path.GetExtension(saveFileDialog1.FileName) == ".txt")
                {
                    // To do
                    //string outputText = "";

                    //foreach (KeyValuePair<int, Formation> formation in formations)
                    //{
                    //outputText += Formation.ShortName[formation.Key] + " <- {\n	name = \"" + Formation.ShortName[formation.Key] + "\",\n	data = [\n";
                    //for (int i = formation.Value.Players.Count; i-- > 0;)
                    //{
                    //Player player = formation.Value.Players[i];
                    //outputText += "		[" + (i+1) + ", \"" + player.Position.ShortName + "\", " + String.Format("{0:0.00}", player.Coordinate.X) + ", " + String.Format("{0:0.00}", player.Coordinate.Y) + "],\n";
                    //}
                    //outputText += "	],\n};\n\n";
                    //}

                    //File.WriteAllText(saveFileDialog1.FileName, outputText);
                    //MessageBox.Show("File saved to .txt!");
                }
            }
        }

        private void AxisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axisToolStripMenuItem.Checked = !axisToolStripMenuItem.Checked;
            chart1.ChartAreas[0].AxisX.LabelStyle.Enabled = axisToolStripMenuItem.Checked;
            chart1.ChartAreas[0].AxisY.LabelStyle.Enabled = axisToolStripMenuItem.Checked;
        }

        private void GridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridToolStripMenuItem.Checked = !gridToolStripMenuItem.Checked;
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = gridToolStripMenuItem.Checked;
            chart1.ChartAreas[0].AxisX.MinorGrid.Enabled = gridToolStripMenuItem.Checked;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = gridToolStripMenuItem.Checked;
            chart1.ChartAreas[0].AxisY.MinorGrid.Enabled = gridToolStripMenuItem.Checked;
        }

        private void BackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundToolStripMenuItem.Checked = !backgroundToolStripMenuItem.Checked;

            if (backgroundToolStripMenuItem.Checked == true)
            {
                chart1.ChartAreas[0].BackImage = "Map.png";
            }
            else
            {
                chart1.ChartAreas[0].BackImage = "NoBackground.png";
            }
        }

        private void CaptureToPNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = Path.GetFileName(Path.GetFileNameWithoutExtension(openFileDialog1.FileName) + "_" + strategyComboBox.Text);
            saveFileDialog1.InitialDirectory = Path.GetDirectoryName(openFileDialog1.FileName);
            saveFileDialog1.Title = "Capture formation to PNG";
            saveFileDialog1.Filter = "PNG (*.png)|*.png";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                chart1.SaveImage(saveFileDialog1.FileName, ChartImageFormat.Png);
            }
        }

        private void FormationListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (formationListBox.SelectedIndex == -1) return;

            if (chart1.Enabled == false)
            {
                ShowFormationChart(true);
            }

            PrintFormation(Formations[formationListBox.SelectedIndex], strategyComboBox.SelectedIndex);
        }

        private void FormationListBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = formationListBox.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    formationListBox.SelectedIndex = index;
                }
            }
        }

        private void ExportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (formationListBox.SelectedIndex == -1) return;

            saveFileDialog2.FileName = Path.GetFileName(formationListBox.Text + ".cfg.bin");
            saveFileDialog2.InitialDirectory = Path.GetDirectoryName(openFileDialog1.FileName);
            saveFileDialog2.Title = "Export Inazuma Eleven Formation file";
            saveFileDialog2.Filter = "Inazuma Eleven Formation (*.cfg.bin)|*.cfg.bin";

            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                SaveFile(saveFileDialog2.FileName, formationListBox.SelectedIndex);
                MessageBox.Show(formationListBox.Text + " has been exported!");
            }
        }

        private void ReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formationListBox.SelectedIndex == -1) return;

            openFileDialog2.FileName = string.Empty;
            openFileDialog2.Title = "Import Inazuma Eleven Formation file";
            openFileDialog2.Filter = "Inazuma Eleven Formation (*.bin)|*.bin";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                CfgBin importFormation = new CfgBin();
                importFormation.Open(File.ReadAllBytes(openFileDialog2.FileName));
                List<Formation> importedFormation = OpenFormationConfigFile(importFormation);

                ImportFormationWindow importForm = new ImportFormationWindow(GetFormationNames(importedFormation));

                if (importForm.ShowDialog() == DialogResult.OK)
                {
                    Formations[formationListBox.SelectedIndex] = importedFormation[importForm.SelectedIndex];
                    MessageBox.Show(importForm.SlectedItem + " has been imported!");
                    formationListBox.Items[formationListBox.SelectedIndex] = importForm.SlectedItem;
                }

                // Update
                FormationListBox_SelectedIndexChanged(sender, e);
            }
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formationListBox.SelectedIndex == -1) return;

            DialogResult dialogResult = MessageBox.Show("Do you want to delete " + formationListBox.Text + "?", "Delete Formation", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                Formations.RemoveAt(formationListBox.SelectedIndex);
                formationListBox.Items.RemoveAt(formationListBox.SelectedIndex);
                ShowFormationChart(false);
                formationListBox.Enabled = true;
            }
        }

        private void InsertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formationListBox.SelectedIndex == -1) return;

            // Get random hash
            (int hash, string nameHash) result = FindUnusedFormationHash();

            if (result.hash != 0x0)
            {
                NewFormationWindow newFormationWindow = new NewFormationWindow();

                if (newFormationWindow.ShowDialog() == DialogResult.OK)
                {
                    FormationHeader header;
                    List<IFormationInfo> formationInfo;

                    if (newFormationWindow.IsMatchFormation)
                    {
                        header = new FormationHeader
                        {
                            PlayerCount = 11,
                            FormationType = 9,
                            Unk1 = 8,
                            Unk2 = 10,
                            FormationHash = result.hash,
                            OFStar = 0,
                            DFStar = 0,
                        };

                        formationInfo = Enumerable.Range(0, header.PlayerCount)
                            .Select(index => new Match
                            {
                                PlayerPosition = 1,
                                PlayerNumber = index,
                            })
                            .Cast<IFormationInfo>()  // Cast Match to IFormationInfo
                            .ToList();
                    }
                    else
                    {
                        header = new FormationHeader
                        {
                            PlayerCount = 5,
                            FormationType = 3,
                            Unk1 = 4,
                            Unk2 = 3,
                            FormationHash = result.hash,
                            OFStar = 0,
                            DFStar = 0,
                        };

                        formationInfo = Enumerable.Range(0, header.PlayerCount)
                            .Select(index => new Battle
                            {
                                PlayerPosition = 1,
                                PlayerNumber = index,
                            })
                            .Cast<IFormationInfo>()  // Cast Battle to IFormationInfo
                            .ToList();
                    }

                    int newIndex = formationListBox.SelectedIndex + 1;
                    Formations.Insert(newIndex, new Formation(header, formationInfo));
                    formationListBox.Items.Insert(newIndex, result.nameHash);

                    // Focus the added item
                    formationListBox.SelectedIndex = newIndex;
                }
            }
            else
            {
                MessageBox.Show("Can't add another formation because the formation limit has been reached.");
            }
        }

        private void Chart1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                IsMousePressed = true;

                if (selectPlayerBox.SelectedIndex != -1)
                {
                    try
                    {
                        // Convert screen coordinates to data coordinates
                        double xValue = chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
                        double yValue = chart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);

                        // Ensure player position stays within the ChartArea bounds
                        xValue = Math.Max(chart1.ChartAreas[0].AxisX.Minimum, Math.Min(chart1.ChartAreas[0].AxisX.Maximum, xValue));
                        yValue = Math.Max(chart1.ChartAreas[0].AxisY.Minimum, Math.Min(chart1.ChartAreas[0].AxisY.Maximum, yValue));

                        // Update player position with adjusted data coordinates
                        UpdatePlayerLocation(Convert.ToSingle(xValue), Convert.ToSingle(yValue), selectPlayerBox.SelectedIndex);

                        // Update location numeric up down;
                        locationXNumericUpDown.Value = Convert.ToDecimal(xValue);
                        locationYNumericUpDown.Value = Convert.ToDecimal(yValue);
                    }
                    catch
                    {
                        // Ignore
                    }
                }
            }
        }

        private void Chart1_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMousePressed && e.Button == MouseButtons.Right)
            {
                if (selectPlayerBox.SelectedIndex != -1)
                {
                    try
                    {
                        // Convert screen coordinates to data coordinates
                        double xValue = chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
                        double yValue = chart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);

                        // Ensure player position stays within the ChartArea bounds
                        xValue = Math.Max(chart1.ChartAreas[0].AxisX.Minimum, Math.Min(chart1.ChartAreas[0].AxisX.Maximum, xValue));
                        yValue = Math.Max(chart1.ChartAreas[0].AxisY.Minimum, Math.Min(chart1.ChartAreas[0].AxisY.Maximum, yValue));

                        // Update player position with adjusted data coordinates
                        UpdatePlayerLocation(Convert.ToSingle(xValue), Convert.ToSingle(yValue), selectPlayerBox.SelectedIndex);

                        // Update location numeric up down;
                        locationXNumericUpDown.Value = Convert.ToDecimal(xValue);
                        locationYNumericUpDown.Value = Convert.ToDecimal(yValue);
                    }
                    catch
                    {
                        // Ignore
                    }
                }
            }
        }

        private void Chart1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                IsMousePressed = false;

                if (selectPlayerBox.SelectedIndex != -1)
                {
                    try
                    {
                        // Convert screen coordinates to data coordinates
                        double xValue = chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
                        double yValue = chart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);

                        // Ensure player position stays within the ChartArea bounds
                        xValue = Math.Max(chart1.ChartAreas[0].AxisX.Minimum, Math.Min(chart1.ChartAreas[0].AxisX.Maximum, xValue));
                        yValue = Math.Max(chart1.ChartAreas[0].AxisY.Minimum, Math.Min(chart1.ChartAreas[0].AxisY.Maximum, yValue));

                        // Update player position with adjusted data coordinates
                        UpdatePlayerLocation(Convert.ToSingle(xValue), Convert.ToSingle(yValue), selectPlayerBox.SelectedIndex);

                        // Update location numeric up down;
                        locationXNumericUpDown.Value = Convert.ToDecimal(xValue);
                        locationYNumericUpDown.Value = Convert.ToDecimal(yValue);
                    }
                    catch
                    {
                        // Ignore
                    }
                }
            }
        }

        private void SelectPlayerBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectPlayerBox.SelectedIndex == -1)
            {
                label1.Enabled = false;
                label2.Enabled = false;
                locationYNumericUpDown.Enabled = false;
                locationXNumericUpDown.Enabled = false;
                goalkeeperButton.Enabled = false;
                defenderButton.Enabled = false;
                midfielderButton.Enabled = false;
                forwardButton.Enabled = false;
            }
            else
            {
                // Get Formation
                Formation formation = Formations[formationListBox.SelectedIndex];

                switch (EnumHelper.GetShortName((PlayerPositions)formation.Players[selectPlayerBox.SelectedIndex].PlayerPosition))
                {
                    case "GK":
                        goalkeeperButton.Checked = true;
                        break;
                    case "DF":
                        defenderButton.Checked = true;
                        break;
                    case "MF":
                        midfielderButton.Checked = true;
                        break;
                    case "FW":
                        forwardButton.Checked = true;
                        break;
                    default:
                        goalkeeperButton.Checked = true;
                        break;
                }

                (float, float) playerCoordinate = GetPlayerCoordinate(formation, selectPlayerBox.SelectedIndex, strategyComboBox.SelectedIndex);
                locationXNumericUpDown.Value = Convert.ToDecimal(playerCoordinate.Item1);
                locationYNumericUpDown.Value = Convert.ToDecimal(playerCoordinate.Item2);

                label1.Enabled = true;
                label2.Enabled = true;
                locationYNumericUpDown.Enabled = true;
                locationXNumericUpDown.Enabled = true;
                goalkeeperButton.Enabled = true;
                defenderButton.Enabled = true;
                midfielderButton.Enabled = true;
                forwardButton.Enabled = true;

                this.Focus();
            }
        }

        private void AddPlayerButton_Click(object sender, EventArgs e)
        {
            if (formationListBox.SelectedIndex == -1) return;

            // Get Formation
            Formation formation = Formations[formationListBox.SelectedIndex];

            // Get Maximum player
            int maximumPlayer = 0;
            if (formation.Header.FormationType == 3)
            {
                maximumPlayer = 5;
            }
            else if (formation.Header.FormationType == 9)
            {
                maximumPlayer = 11;
            }

            if (formation.Header.PlayerCount == maximumPlayer)
            {
                MessageBox.Show("Can't add another player because the player limit has been reached.");
            } else
            {
                // Get player number
                int playerNumber = Enumerable.Range(0, 16)
                    .Except(formation.Players.Select(player => player.PlayerNumber))
                    .First();

                if (formation.Header.FormationType == 3)
                {
                    Battle battle = new Battle();
                    battle.PlayerPosition = 1;
                    battle.PlayerNumber = playerNumber;
                    formation.Players.Add(battle);
                }
                else if (formation.Header.FormationType == 9)
                {
                    Match match = new Match();
                    match.PlayerPosition = 1;
                    match.PlayerNumber = playerNumber;
                    formation.Players.Add(match);
                }

                formation.Header.PlayerCount++;            

                // Update
                StrategyComboBox_SelectedIndexChanged(sender, e);
                selectPlayerBox.SelectedIndex = formation.Header.PlayerCount-1;
            }
        }

        private void DeletePlayerButton_Click(object sender, EventArgs e)
        {
            if (selectPlayerBox.SelectedIndex == -1) return;

            // Get Formation
            Formation formation = Formations[formationListBox.SelectedIndex];

            if (formation.Header.PlayerCount == 0)
            {
                MessageBox.Show("Can't remove player because the player limit has been reached.");
            }
            else
            {
                // Search player index with playerNumero
                int playerNumber = Convert.ToInt32(selectPlayerBox.SelectedItem.ToString().Replace("Player ", "")) - 1;
                int playerIndex = formation.Players.FindIndex(x => x.PlayerNumber == playerNumber);

                formation.Players.RemoveAt(playerIndex);
                formation.Header.PlayerCount--;

                locationXNumericUpDown.Enabled = false;
                locationYNumericUpDown.Enabled = false;
                goalkeeperButton.Enabled = false;
                defenderButton.Enabled = false;
                midfielderButton.Enabled = false;
                forwardButton.Enabled = false;
                selectPlayerBox.Text = "";

                // Update
                StrategyComboBox_SelectedIndexChanged(sender, e);
            }
        }

        private void LocationNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numericUpDown = sender as NumericUpDown;

            if (!numericUpDown.Focused || selectPlayerBox.SelectedIndex == -1) return;

            UpdatePlayerLocation(Convert.ToSingle(locationXNumericUpDown.Value), Convert.ToSingle(locationYNumericUpDown.Value), selectPlayerBox.SelectedIndex);
        }

        private void GoalkeeperButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!goalkeeperButton.Focused || selectPlayerBox.SelectedIndex == -1) return;

            ChangePlayerPosition(1, selectPlayerBox.SelectedIndex);
            chart1.Focus();
        }

        private void DefenderButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!defenderButton.Focused || selectPlayerBox.SelectedIndex == -1) return;

            ChangePlayerPosition(2, selectPlayerBox.SelectedIndex);
            chart1.Focus();
        }

        private void MidfielderButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!midfielderButton.Focused || selectPlayerBox.SelectedIndex == -1) return;

            ChangePlayerPosition(5, selectPlayerBox.SelectedIndex);
            chart1.Focus();
        }

        private void ForwardButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!forwardButton.Focused || selectPlayerBox.SelectedIndex == -1) return;

            ChangePlayerPosition(8, selectPlayerBox.SelectedIndex);
            chart1.Focus();
        }

        private void OffensiveStarNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!offensiveStarNumericUpDown.Focused ||selectPlayerBox.SelectedIndex == -1) return;

            // Get Formation
            Formation formation = Formations[formationListBox.SelectedIndex];

            // Update offensive star
            formation.Header.OFStar = Convert.ToInt32(offensiveStarNumericUpDown.Value);
        }

        private void DefensiveNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!defensiveNumericUpDown.Focused || selectPlayerBox.SelectedIndex != -1) return;

            // Get Formation
            Formation formation = Formations[formationListBox.SelectedIndex];

            // Update defensive star
            formation.Header.DFStar = Convert.ToInt32(defensiveNumericUpDown.Value);
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (strategyComboBox.SelectedIndex < strategyComboBox.Items.Count - 1)
            {
                strategyComboBox.SelectedIndex++;
            }
            else
            {
                strategyComboBox.SelectedIndex = 0;
            }
        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            if (strategyComboBox.SelectedIndex == 0)
            {
                strategyComboBox.SelectedIndex = strategyComboBox.Items.Count - 1;
            }
            else
            {
                strategyComboBox.SelectedIndex--;
            }
        }

        private void StrategyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strategyComboBox.SelectedIndex == -1) return;

            PrintFormation(Formations[formationListBox.SelectedIndex], strategyComboBox.SelectedIndex);
        }
    }
}
