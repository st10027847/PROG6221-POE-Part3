using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace POE_Part3
{
    public partial class MainWindow : Window
    {
        private List<Recipe> recipes = new List<Recipe>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddIngredientsButton_Click(object sender, RoutedEventArgs e)
        {
            StackPanel stackPanel = new StackPanel();

            TextBox nameTextBox = new TextBox();
            nameTextBox.Margin = new Thickness(0, 5, 0, 0);
            nameTextBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            nameTextBox.Tag = "Ingredient Name";
            nameTextBox.Text = "Ingredient Name";
            stackPanel.Children.Add(nameTextBox);

            TextBox quantityTextBox = new TextBox();
            quantityTextBox.Margin = new Thickness(0, 5, 0, 0);
            quantityTextBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            quantityTextBox.Tag = "Quantity";
            quantityTextBox.Text = "Quantity";
            stackPanel.Children.Add(quantityTextBox);

            TextBox unitTextBox = new TextBox();
            unitTextBox.Margin = new Thickness(0, 5, 0, 0);
            unitTextBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            unitTextBox.Tag = "Unit of Measurement";
            unitTextBox.Text = "Unit of Measurement";
            stackPanel.Children.Add(unitTextBox);

            TextBox caloriesTextBox = new TextBox();
            caloriesTextBox.Margin = new Thickness(0, 5, 0, 0);
            caloriesTextBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            caloriesTextBox.Tag = "Calories";
            caloriesTextBox.Text = "Calories";
            stackPanel.Children.Add(caloriesTextBox);

            TextBox foodGroupTextBox = new TextBox();
            foodGroupTextBox.Margin = new Thickness(0, 5, 0, 0);
            foodGroupTextBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            foodGroupTextBox.Tag = "Food Group";
            foodGroupTextBox.Text = "Food Group";
            stackPanel.Children.Add(foodGroupTextBox);

            IngredientsStackPanel.Children.Add(stackPanel);
        }



        private void AddRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            string recipeName = RecipeNameTextBox.Text;

            List<Ingredient> ingredients = new List<Ingredient>();
            foreach (var child in IngredientsStackPanel.Children)
            {
                if (child is TextBox textBox)
                {
                    if (textBox.Name == "IngredientNameTextBox")
                    {
                        string ingredientName = textBox.Text;
                        double.TryParse(textBox.Text, out double ingredientQuantity);

                        TextBox unitTextBox = IngredientsStackPanel.Children.OfType<TextBox>().FirstOrDefault(t => t.Name == "IngredientUnitTextBox");
                        string ingredientUnit = unitTextBox?.Text;

                        TextBox caloriesTextBox = IngredientsStackPanel.Children.OfType<TextBox>().FirstOrDefault(t => t.Name == "IngredientCaloriesTextBox");
                        int.TryParse(caloriesTextBox?.Text, out int ingredientCalories);

                        TextBox foodGroupTextBox = IngredientsStackPanel.Children.OfType<TextBox>().FirstOrDefault(t => t.Name == "IngredientFoodGroupTextBox");
                        string ingredientFoodGroup = foodGroupTextBox?.Text;

                        Ingredient ingredient = new Ingredient(ingredientName, ingredientQuantity, ingredientUnit, ingredientCalories, ingredientFoodGroup);
                        ingredients.Add(ingredient);
                    }
                }
            }

            List<string> steps = RecipeStepsTextBox.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

            Recipe recipe = new Recipe(recipeName, ingredients, steps);
            recipes.Add(recipe);

            RecipeNameTextBox.Clear();
            RecipeStepsTextBox.Clear();
            IngredientsStackPanel.Children.Clear();

            MessageBox.Show("Recipe added successfully.");
        }

        private void DisplayRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            string recipeName = RecipeNameTextBox.Text;
            Recipe recipe = recipes.Find(r => r.Name.ToLower() == recipeName.ToLower());

            if (recipe != null)
            {
                string ingredientDetails = string.Join(Environment.NewLine, recipe.Ingredients.Select(i => $"{i.Name}: {i.Quantity} {i.Unit}"));
                string stepsDetails = string.Join(Environment.NewLine, recipe.Steps.Select((step, index) => $"{index + 1}. {step}"));

                MessageBox.Show($"Ingredients for {recipe.Name}:{Environment.NewLine}{ingredientDetails}{Environment.NewLine}Preparation steps for {recipe.Name}:{Environment.NewLine}{stepsDetails}");
            }
            else
            {
                MessageBox.Show("Recipe not found.");
            }
        }

        private void ListRecipesButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> recipeNames = recipes.OrderBy(r => r.Name).Select(r => r.Name).ToList();
            string recipeList = string.Join(Environment.NewLine, recipeNames);

            MessageBox.Show($"List of recipes:{Environment.NewLine}{recipeList}");
        }

        private void ClearRecipesButton_Click(object sender, RoutedEventArgs e)
        {
            recipes.Clear();
            MessageBox.Show("All recipes cleared.");
        }

        private void RecipesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RecipesListBox.SelectedItem is Recipe selectedRecipe)
            {
                string ingredientDetails = string.Join(Environment.NewLine, selectedRecipe.Ingredients.Select(i => $"{i.Name}: {i.Quantity} {i.Unit}"));
                string stepsDetails = string.Join(Environment.NewLine, selectedRecipe.Steps.Select((step, index) => $"{index + 1}. {step}"));

                MessageBox.Show($"Ingredients for {selectedRecipe.Name}:{Environment.NewLine}{ingredientDetails}{Environment.NewLine}Preparation steps for {selectedRecipe.Name}:{Environment.NewLine}{stepsDetails}");
            }
        }

        private void AddStepsButton_Click(object sender, RoutedEventArgs e)
        {
            TextBox stepsTextBox = new TextBox();
            stepsTextBox.Margin = new Thickness(0, 5, 0, 0);
            stepsTextBox.AcceptsReturn = true;  // Allows the TextBox to accept new lines
            stepsTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;  // Enables vertical scrolling
            stepsTextBox.TextChanged += StepsTextBox_TextChanged;  // Handles text changes
            StepsStackPanel.Children.Add(stepsTextBox);
        }

        private void StepsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = textBox.Text.Replace("\r", "");  // Removes carriage return characters

            // Moves the caret to the end of the text box
            textBox.CaretIndex = textBox.Text.Length;
            textBox.ScrollToEnd();
        }

    }

    public class Recipe
    {
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<string> Steps { get; set; }

        public Recipe(string name, List<Ingredient> ingredients, List<string> steps)
        {
            Name = name;
            Ingredients = ingredients;
            Steps = steps;
        }
    }

    public class Ingredient
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public int Calories { get; set; }
        public string FoodGroup { get; set; }

        public Ingredient(string name, double quantity, string unit, int calories, string foodGroup)
        {
            Name = name;
            Quantity = quantity;
            Unit = unit;
            Calories = calories;
            FoodGroup = foodGroup;
        }
    }
}
