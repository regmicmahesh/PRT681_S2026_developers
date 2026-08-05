/*
* FILE :            calculatorApplication.cs
* PROJECT :         C# Calculator Project
* PROGRAMMER :      Nicholas Reilly
* FIRST VERSION :   2024-12-16
* DESCRIPTION :     A complete overhaul of the past C Advanced Calcualtor. This project showcases my skills in making a basic but efficent program in C# 
*                   that utilizes my ability to program user interfaces as well as demonstrate my knowledge on object oriented programming. This file
*                   contains the code for the logic of the calculator.
*/




//Calling of the packages used to create the program. 
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

//Defining the namespace.
namespace calculatorApplication


{   /* 
    *    NAME        : MainWindow : Window
    *    PURPOSE     : The main window partial public class defines that MainWindow is a type of window that is used in the program. 
    */
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }


        /* 
        *    NAME       : Button_Click
        *    PURPOSE    : This method is called when a button in the calculator is clicked. It appends the content 
        *                 of the clicked button (either a number or operator) to the text displayed in the ResultBox 
        *                 which shows the ongoing mathematical expression entered by the user.
        *    PARAMETERS :
        *        sender  : object - The source of the event, typically the button that was clicked.
        *        e       : RoutedEventArgs - Contains event data for the button click event.
        */
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string buttonContent = button.Content.ToString();

            //Append the contents of the button.
            ResultBox.Text += buttonContent;
        }


        /* 
         *    NAME       : Clear_Click
         *    PURPOSE    : This method is called when the "Clear" button is clicked. It clears the content of the ResultBox, 
         *                 effectively resetting the calculator's display to an empty state.
         *    PARAMETERS :
         *        sender  : object - The source of the event, typically the "Clear" button that was clicked.
         *        e       : RoutedEventArgs - Contains event data for the button click event.
         */
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ResultBox.Clear();
        }


        /* 
         *    NAME       : EqualsButton_Click
         *    PURPOSE    : This method is called when the "Equals" button is clicked. It evaluates the mathematical expression 
         *                 entered in the ResultBox using `DataTable.Compute`. If the expression is valid, it displays the 
         *                 result in the ResultBox. If the expression is invalid, it catches the error and displays "Error".
         *    PARAMETERS :
         *        sender  : object - The source of the event, typically the "Equals" button that was clicked.
         *        e       : RoutedEventArgs - Contains event data for the button click event.
         */
        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string expression = ResultBox.Text;

                //Use DataTable.Compute for basic arithmetic evaluation.
                DataTable table = new DataTable();
                var result = table.Compute(expression, string.Empty);

                //Display the result in the ResultBox.
                ResultBox.Text = result.ToString();
            }
            catch (Exception ex)
            {
                //Display error message if the expression is invalid.
                ResultBox.Text = "Error";
            }
        }
    }
}

