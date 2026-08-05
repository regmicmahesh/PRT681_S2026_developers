using System;
using System.Collections.Generic;

List<Shape> shapes = new List<Shape>();

shapes.Add(new Circle(4.5));
shapes.Add(new Rectangle(4, 5));

foreach (Shape shape in shapes)
{
    Console.WriteLine($"{shape.CalculateArea()}");

}
Console.ReadLine();
public abstract class Shape
{
    public Shape()
    {

    }
    public abstract double CalculateArea();
}

public class Circle : Shape
{
    public double Radius { get; set; }

    public Circle(double radius)
    {
        Radius = radius;
    }

    public override double CalculateArea()
    {
        double Area = 3.14 * Radius * Radius;
        return Area;
    }
}

public class Rectangle : Shape
{
    public double Lenght { get; set; }
    public double Breadth { get; set; }

    public Rectangle(double length, double breadth)
    {
        Lenght = length;
        Breadth = breadth;
    }

    public override double CalculateArea()
    {
        double Area = Lenght * Breadth;
        return Area;
    }
}
