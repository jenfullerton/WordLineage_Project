using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Data;

// using System.Diagnostics; // use for debugging

namespace WordLineage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Main
        public MainWindow()
        {
            InitializeComponent();

            ObservableCollection<WordNode> nodes = new();
            nodes.Add(new WordNode("First"));
            nodes.Add(new WordNode("Second"));
            nodes.Add(new WordNode("Third"));
            nodes.Add(new WordNode("Fourth"));
            nodes.Add(new WordNode("Fifth"));
            nodes.Add(new WordNode("Sixth"));

            // add parents and children
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                for (int j = i + 1; j < nodes.Count; j++)
                {
                    nodes[i].Children.Add(nodes[j]);
                    nodes[j].Parents.Add(nodes[i]);
                }
            }


            /*
            List<WordNode> nodes = new();
            for (int i = 1; i <= 5; i++)
            {
                nodes.Add(new WordNode(
                    "WordNode "+ i,
                    "Definition " + i,
                    "Description " + i,
                    "tag" + i
                ));
            }

            foreach (WordNode node in nodes)
            {
                // List<string> l = new List<string> { "one", "two", "three" };
                node.AddTags(new List<string> { "big", "worm", "dog" });
                node.AddDefinition("just the one definition");
                node.AddDefinition("");
                node.AddDefinitions(new List<string> { "a definition", "", "and another one" });
            }
            */

            // data bind listbox to nodes
            FamilyDisplay.ItemsSource = nodes;
            ParentComboBox.ItemsSource = nodes;
            
        }
        #endregion

        #region UI_Utility_Functions
        private void FamilyDisplay_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // nullable type wrapper so intellisense stops yelling at me
            WordNode? selectedNode = (WordNode?)(FamilyDisplay.SelectedItem);

            if(selectedNode != null)
            {
                // Update display with new info
                NodeNameDisplay.Text = selectedNode.Name;
                NodeParentsDisplay.ItemsSource = selectedNode.Parents;
                NodeChildrenDisplay.ItemsSource = selectedNode.Children;
                
                /* 
                NodeDefinitionDisplay.ItemsSource = selectedNode.Definition;
                NodeDescriptionDisplay.Text = selectedNode.Description;
                NodeTagDisplay.ItemsSource= selectedNode.Tags;
                */
            }
            else 
            {
                // if selection is null, clear the display
                NodeNameDisplay.Text = "[No WordNode Selected]";
                NodeParentsDisplay.ItemsSource = null;
                NodeChildrenDisplay.ItemsSource= null;
            }
        }

        private void DeselectCurrentNode_Click(object sender, RoutedEventArgs e)
        {
            FamilyDisplay.SelectedItem = null;
        }
        #endregion

        #region Commands

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(FamilyDisplay.SelectedItem != null)
            {
                e.CanExecute = true;
            } else
            {
                e.CanExecute = false;
            }
        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            (FamilyDisplay.ItemsSource as ObservableCollection<WordNode>).RemoveAt( FamilyDisplay.SelectedIndex );
        }

        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // new nodes can be created if the new node popup is not currently open
            if (pop_AddNode.IsOpen)
            {
                e.CanExecute = false;
            } else
            {
                e.CanExecute = true;
            }
        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // opens the popup for new node
            pop_AddNode.IsOpen = true;
        }


        #endregion

        private void AddNodeButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(AddNodeName.Text))
            {
                MessageBox.Show("Node Name cannot be empty. Please enter a valid name");
                AddNodeName.Text = "";
                return;
            }
            else
            {
                WordNode newNode = new(AddNodeName.Text);

                if(ParentComboBox.SelectedItem != null)
                {
                    newNode.Parents.Add(ParentComboBox.SelectedItem as WordNode);
                }

                (FamilyDisplay.ItemsSource as ObservableCollection<WordNode>).Add(newNode);
                
                // reset textbox & combo box and close the window
                AddNodeName.Text = "";
                ParentComboBox.SelectedIndex = -1;
                pop_AddNode.IsOpen = false;
            }
        }

        private void AddParentButton_Click(object sender, RoutedEventArgs e)
        {
            if (FamilyDisplay.SelectedItem != null && ParentComboBox.SelectedItem != null)
            {
                // add selected node in parent box to list of parents for selected node
                (FamilyDisplay.SelectedItem as WordNode).Parents.Add(ParentComboBox.SelectedItem as WordNode);

                // refresh the list in parents by calling update source on the binding
                NodeParentsDisplay.Items.Refresh();

            } else
            {
                MessageBox.Show("Please select a node and a parent before adding.");
            }
        }

        private void Btn_CancelAddNode_Click(object sender, RoutedEventArgs e)
        {
            // reset new node name, selected item in parent drop down, and close popup
            AddNodeName.Text = "";
            ParentComboBox.SelectedIndex = -1;
            pop_AddNode.IsOpen = false;
        }
    }
}