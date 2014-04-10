using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;

namespace ButlerMindControlAgent
{
    public partial class Form1 : Form
    {
        int lastCommandNumber;
        XDocument xDoc;
        List<XElement> floors;
        List<XElement> currentFloorObjects;
        List<XElement> currentCommands;
        public Form1()
        {
            InitializeComponent();
            EnemySelectBox.Enabled = false;
            CommandList.Enabled = false;
            StartPosBox.Enabled = false;
            MoveCommandBox.Enabled = false;
            WaitCommandBox.Enabled = false;
            FloorSelectBox.Enabled = false;
            SaveButton.Enabled = false;
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            SuspendLayout();
            EnemySelectBox.Enabled = false;
            CommandList.Enabled = false;
            StartPosBox.Enabled = false;
            MoveCommandBox.Enabled = false;
            WaitCommandBox.Enabled = false;
            FloorSelectBox.Enabled = false;

            if (File.Exists(FileLocBox.Text))
            {
                xDoc = XDocument.Load(FileLocBox.Text);
                var objectGroups = xDoc.Element("map").Elements("objectgroup");
                if (objectGroups != null)
                {
                    FloorSelectBox.Items.Clear();
                    FloorSelectBox.ClearSelected();
                    floors = new List<XElement>();
                    foreach (var objGroup in objectGroups)
                    {
                        string groupName = ((string)(objGroup.Attribute("name")));
                        if (groupName.Contains("Entities"))
                        {
                            FloorSelectBox.Items.Add(groupName.Substring(0, 6));
                            floors.Add(objGroup);
                        }
                    }
                    FloorSelectBox.Enabled = true;
                }
                else
                    MessageBox.Show("This is not a valid Map file!");
                SaveButton.Enabled = true;
            }
            else
                MessageBox.Show("There is no map at this file location!");
            ResumeLayout();
        }

        private void FloorSelectChanged(object sender, EventArgs e)
        {
            SuspendLayout();
            EnemySelectBox.Enabled = false;
            CommandList.Enabled = false;
            StartPosBox.Enabled = false;
            MoveCommandBox.Enabled = false;
            WaitCommandBox.Enabled = false;

            EnemySelectBox.Items.Clear();
            EnemySelectBox.ClearSelected();
            currentFloorObjects = new List<XElement>();
            foreach (var obj in floors[FloorSelectBox.SelectedIndex].Elements("object"))
            {
                if (((string)obj.Attribute("type")) == "Enemy")
                {
                    string objName = ((string)obj.Attribute("name")) != null ? ((string)obj.Attribute("name")) : "Unnamed Enemy";
                    EnemySelectBox.Items.Add(objName);
                    currentFloorObjects.Add(obj);
                }
            }
            EnemySelectBox.Enabled = true;
            ResumeLayout();
        }

        private void EnemySelectChanged(object sender, EventArgs e)
        {
            SuspendLayout();
            CommandList.Enabled = false;
            StartPosBox.Enabled = false;
            MoveCommandBox.Enabled = false;
            WaitCommandBox.Enabled = false;
            currentCommands = new List<XElement>();
            CommandList.Items.Clear();
            CommandList.ClearSelected();

            var selected = currentFloorObjects[EnemySelectBox.SelectedIndex];

            XElement propertiesList;
            lastCommandNumber = 1;
            if ((propertiesList = selected.Element("properties")) != null)
            {
                foreach (var property in propertiesList.Elements("property"))
                {
                    string name = (string)property.Attribute("name");
                    string value = (string)property.Attribute("value");
                    string viewName;
                    if (name.Contains("MOVE"))
                    {
                        viewName = "MOVE";
                    }
                    else if (name.Contains("WAIT"))
                    {
                        viewName = "WAIT";
                    }
                    else if (name.Contains("END"))
                    {
                        viewName = "END";
                    }
                    else
                        //undefined
                        viewName = "UNDF";


                    CommandList.Items.Add(viewName + "|" + value);
                    currentCommands.Add(property);
                    lastCommandNumber++;
                }
            }

            StartXPosBox.Text = "" + (string)selected.Attribute("x");
            StartYPosBox.Text = "" + (string)selected.Attribute("y");

            CommandList.Enabled = true;
            StartPosBox.Enabled = true;
            MoveCommandBox.Enabled = true;
            WaitCommandBox.Enabled = true;
            ResumeLayout();
        }

        private void RemoveCommand_Click(object sender, EventArgs e)
        {
            currentCommands[CommandList.SelectedIndex].Remove();
            currentCommands.RemoveAt(CommandList.SelectedIndex);
            CommandList.Items.RemoveAt(CommandList.SelectedIndex);
        }

        private void MoveAddButton_Click(object sender, EventArgs e)
        {
            int X, Y, Z;
            if (int.TryParse(MoveXPosBox.Text, out X) && int.TryParse(MoveYPosBox.Text, out Y) && int.TryParse(MoveZPosBox.Text, out Z))
            {
                var newNode = new XElement("property", new XAttribute("name", lastCommandNumber + "MOVE"), new XAttribute("value", ""+X+","+Y+","+Z));
                lastCommandNumber++;
                currentFloorObjects[EnemySelectBox.SelectedIndex].Element("properties").Add(newNode);
                currentCommands.Add(newNode);
                CommandList.Items.Add("MOVE|" + X + "," + Y + "," + Z);
            }
        }

        private void WaitAddButton_Click(object sender, EventArgs e)
        {
            int waitTime;
            if (int.TryParse(WaitCommandTextBox.Text, out waitTime))
            {
                var newNode = new XElement("property", new XAttribute("name", lastCommandNumber + "WAIT"), new XAttribute("value", "" + waitTime));
                lastCommandNumber++;
                currentFloorObjects[EnemySelectBox.SelectedIndex].Element("properties").Add(newNode);
                currentCommands.Add(newNode);
                CommandList.Items.Add("WAIT|" + waitTime);
            }
        }

        private void StartPosChange_Click(object sender, EventArgs e)
        {
            int X, Y;
            if (int.TryParse(MoveXPosBox.Text, out X) && int.TryParse(MoveYPosBox.Text, out Y))
            {
                currentFloorObjects[EnemySelectBox.SelectedIndex].Attribute("x").SetValue(X);
                currentFloorObjects[EnemySelectBox.SelectedIndex].Attribute("y").SetValue(Y);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            xDoc.Save(FileLocBox.Text);
        }

        private void FixCommands_Click(object sender, EventArgs e)
        {
            foreach (var floor in floors)
            {
                foreach (var obj in floor.Elements("object"))
                {
                    if (((string)obj.Attribute("type")) == "Enemy")
                    {
                        List<XElement> propertyList = (List<XElement>)obj.Element("properties").Elements("property");
                        //quicksort based on command number
                    }
                }
            }
        }
        private int GetCommandNumber(XElement element)
        {
            string name = (string)element.Attribute("name");
            int ret;
            for (int i = name.Length; i > 0; i--)
            {
                if (int.TryParse(name, out ret))
                    return ret;
            }
            return int.MaxValue;
        }

        private List<XElement> QuicksortCommands(List<XElement> list)
        {
            if (list.Count < 1)
                return list;
            return list;
        }
    }
}
