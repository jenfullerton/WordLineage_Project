using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

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

            // add children
            for(int i = 0; i < nodes.Count-1; i++)
            {
                for(int j = i+1; j < nodes.Count; j++)
                {
                    nodes[i].Children.Add(nodes[j]);
                    nodes[j].Parents.Add(nodes[i]);
                }
            }


            // data bind listbox to nodes
            FamilyDisplay.ItemsSource = nodes;
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
                NodeDefinitionDisplay.ItemsSource = selectedNode.Definition;
                NodeDescriptionDisplay.Text = selectedNode.Description;
                NodeTagDisplay.ItemsSource= selectedNode.Tags;
                NodeParentsDisplay.ItemsSource = selectedNode.Parents;
                NodeChildrenDisplay.ItemsSource = selectedNode.Children;

                // clear selected items for Definitions, Tags, Parents, and Children
                NodeDefinitionDisplay.SelectedItem = null;
                NodeTagDisplay.SelectedItem = null;
                NodeParentsDisplay.SelectedItem = null;
                NodeChildrenDisplay.SelectedItem = null;

            }
            else 
            {
                // if selection is null, clear the display
                NodeNameDisplay.Text = "[No WordNode Selected]";
                NodeDescriptionDisplay.Text = "";
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

        }

        #endregion
    }
}