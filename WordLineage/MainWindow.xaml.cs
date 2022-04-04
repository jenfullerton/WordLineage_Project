using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Data;

// using System.Diagnostics; // use for debugging

/* ---  ShortHand Names     ---
 * pop = popup box  cbx = combobox
 * btn = button     spl = stackpanel
 * tbx = textbox    tbk = textblock
 * lsv = listview
 */

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

            // bind Routed Commands
            RoutedCommand EditNodeCommand = new();
            CommandBinding editNodeBinding = new(EditNodeCommand,
                EditNodeCommand_Executed, EditNodeCommand_CanExecute);
            this.CommandBindings.Add(editNodeBinding);
            EditNodeMenuBtn.Command = EditNodeCommand;

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
            Cbx_Parents.ItemsSource = nodes;
            Cbx_Children.ItemsSource = nodes;
            
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
                tbk_NodeInEditing.Text = selectedNode.Name;
                lsv_NodeParentsDisplay.ItemsSource = selectedNode.Parents;
                lsv_NodeChildrenDisplay.ItemsSource = selectedNode.Children;
                
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
                lsv_NodeParentsDisplay.ItemsSource = null;
                lsv_NodeChildrenDisplay.ItemsSource= null;
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
            // cannot delete unless a valid node is selected and all pop up
            //      menus are closed.
            if(FamilyDisplay.ItemsSource != null && FamilyDisplay.SelectedIndex > -1
                && !pop_ModifyNode.IsOpen && !pop_AddNode.IsOpen)
            {
                e.CanExecute = true;
            } else
            {
                e.CanExecute = false;
            }
        }

        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {   
            if(FamilyDisplay.ItemsSource is ObservableCollection<WordNode> nodes)
            {
                // check to see if menu was called on parents or child list
                if (e.Source.Equals(lsv_NodeParentsDisplay) &&
                    FamilyDisplay.SelectedItem is WordNode nodeP)
                {
                    if(lsv_NodeParentsDisplay.SelectedIndex > -1)
                    {
                        nodeP.RemoveParent(lsv_NodeParentsDisplay.SelectedIndex);
                        lsv_NodeParentsDisplay.Items.Refresh();
                    } 
                    else
                    {
                        MessageBox.Show("Cannot delete parent unless a parent is selected.");
                    }
                } 
                else if (e.Source.Equals(lsv_NodeChildrenDisplay) &&
                    FamilyDisplay.SelectedItem is WordNode nodeC)
                {
                    if (lsv_NodeChildrenDisplay.SelectedIndex > -1)
                    {
                        nodeC.RemoveChild(lsv_NodeChildrenDisplay.SelectedIndex);
                        lsv_NodeChildrenDisplay.Items.Refresh();
                    }
                    else
                    {
                        MessageBox.Show("Cannot delete a child unless a child is selected.");
                    }
                }
                else
                {
                    nodes.RemoveAt(FamilyDisplay.SelectedIndex);
                }
            }
        }

        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // no other popups can be open
            if (pop_ModifyNode.IsOpen || pop_AddNode.IsOpen)
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
            tbx_NewNodeName.Focus();
        }

        private void EditNodeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // in order to open the edit node command
            // 1. a valid index/node must be selected
            // 2. new node popup must be closed
            // 3. edit node popup must be closed
            e.CanExecute = ( (FamilyDisplay.SelectedIndex > -1) && !pop_AddNode.IsOpen && !pop_ModifyNode.IsOpen);
        }

        private void EditNodeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            pop_ModifyNode.IsOpen = true;
        }

        #endregion

        private void Btn_NewNode_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbx_NewNodeName.Text))
            {
                MessageBox.Show("Node Name cannot be empty. Please enter a valid name");
                tbx_NewNodeName.Text = "";
                return;
            }
            else
            {
                if (FamilyDisplay.ItemsSource is ObservableCollection<WordNode> nodes)
                {
                    WordNode newNode = new(tbx_NewNodeName.Text);
                    nodes.Add(newNode);
                    // ensure new node is selected upon exit
                    FamilyDisplay.SelectedIndex = nodes.Count - 1;

                } else
                {
                    // warning appears if no word family in the display 
                    MessageBox.Show("No family selected. (FamilyDisplay.ItemsSource is null).");
                }

                // reset textbox and close the Add Node popup
                tbx_NewNodeName.Text = "";
                pop_AddNode.IsOpen = false;

                if( sender.Equals(Btn_AddEditNode) )
                {
                    // if event sent by AddEdit, open the edit node menu
                    pop_ModifyNode.IsOpen = true;
                }
            }
        }

        private void Btn_AddParent_Click(object sender, RoutedEventArgs e)
        {
            if (FamilyDisplay.SelectedItem is WordNode node && Cbx_Parents.SelectedItem is WordNode parent)
            {
                node.Parents.Add(parent);
                // refresh the list in parents by calling update source on the binding
                lsv_NodeParentsDisplay.Items.Refresh();

            } else
            {
                MessageBox.Show("Please select a node and a parent before adding.");
            }
        }

        private void Btn_AddChild_Click(object sender, RoutedEventArgs e)
        {
            if(FamilyDisplay.SelectedItem is WordNode node && Cbx_Children.SelectedItem is WordNode child)
            {
                node.Children.Add(child);
                // Refresh list of Children
                lsv_NodeChildrenDisplay.Items.Refresh();
            } else
            {
                MessageBox.Show("Please select a node and child before adding.");
            }
        }

        private void Btn_CancelAddNode_Click(object sender, RoutedEventArgs e)
        {
            // reset new node name, selected item in parent drop down, and close popup
            tbx_NewNodeName.Text = "";
            pop_AddNode.IsOpen = false;
        }

        private void Btn_CloseEdits_Click(object sender, RoutedEventArgs e)
        {
            // clear selection boxes and close popup
            Cbx_Parents.SelectedIndex = -1;
            Cbx_Children.SelectedIndex = -1;
            pop_ModifyNode.IsOpen = false;

        }
    }
}