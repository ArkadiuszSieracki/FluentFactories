using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
// Flyweight interface
public interface IShape
{
    void Draw();
}

public record Color(string Name);

// Concrete Flyweight class
public class Circle : IShape, IEquatable<Circle>
{
    private Color color;
    private readonly BrushFactory ctx;
    private int x;
    private int y;
    private int radius;

    public Circle(Color color, BrushFactory ctx)
    {
        this.color = color;
        this.ctx = ctx;
    }

    public void SetX(int x) { this.x = x; }
    public void SetY(int y) { this.y = y; }
    public void SetRadius(int radius) { this.radius = radius; }

    public void Draw()
    {
        Console.WriteLine($"Circle: Draw() [Color : {color}, x : {x}, y : {y}, radius : {radius}]");
    }
    public override int GetHashCode()
    {
        return color.GetHashCode();
    }
    public bool Equals(Circle? other)
    {
        return other.color == color;
    }
}
public class Rectangle : IShape,IEquatable<Rectangle>
{
    public override int GetHashCode()
    {
        return color.GetHashCode();
    }
    private Color color;
    private int x;
    private int y;
    private int a;

    public Rectangle(Color color)
    {
        this.color = color;
    }

    public void SetX(int x) { this.x = x; }
    public void SetY(int y) { this.y = y; }
    public void SetA(int radius) { this.a = radius; }

    public void Draw()
    {
        Console.WriteLine($"Circle: Draw() [Color : {color}, x : {x}, y : {y}, a : {a}]");
    }

    public bool Equals(Rectangle? other)
    {
        return color == other.color;
    }
}

public class BrushFactory {
    public static int count;
    public BrushFactory()
    {
        count++;
    }
}

// Flyweight Factory class
public class ShapeFactory
{
    private static Dictionary<Color, IShape> circleMap = new Dictionary<Color, IShape>();
    static BrushFactory ctx = new BrushFactory();
    public static IShape GetCircle(Color color)
    {
        Circle circle = null;
        if (circleMap.ContainsKey(color))
        {
            circle = (Circle)circleMap[color];
        }
        else
        {
            circle = new Circle(color,ctx );
            circleMap[color] = circle;
            Console.WriteLine($"Creating circle of color : {color}");
        }
        return circle;
    }
}

// Client code
class Program
{
    static void Main(string[] args)
    {
        for (int i = 0; i < 20; ++i)
        {
            Circle circle = (Circle)ShapeFactory.GetCircle(GetRandomColor());
            circle.SetX(GetRandomX());
            circle.SetY(GetRandomY());
            circle.SetRadius(100);
            circle.Draw();
        }
    }

    private static Color GetRandomColor()
    {
        string[] colors = { "Red", "Green", "Blue", "White", "Black" };
        Random random = new Random();
        return new Color(colors[random.Next(colors.Length)]);
    }

    private static int GetRandomX()
    {
        Random random = new Random();
        return random.Next(100);
    }

    private static int GetRandomY()
    {
        Random random = new Random();
        return random.Next(100);
    }
}