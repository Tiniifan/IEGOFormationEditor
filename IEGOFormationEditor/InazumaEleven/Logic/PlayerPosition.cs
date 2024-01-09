using System;
using System.Drawing;

namespace IEGOFormationEditor.InazumaEleven.Logic
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ColorAttribute : Attribute
    {
        public Color Color { get; }

        public ColorAttribute(int red, int green, int blue)
        {
            Color = Color.FromArgb(red, green, blue);
        }
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class NameAttribute : Attribute
    {
        public string Name { get; }

        public NameAttribute(string secondDescription)
        {
            Name = secondDescription;
        }
    }


    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class ShortNameAttribute : Attribute
    {
        public string ShortName { get; }

        public ShortNameAttribute(string secondDescription)
        {
            ShortName = secondDescription;
        }
    }


    public enum PlayerPositions
    {
        [Color(0, 0, 0)]
        [Name("None")]
        [ShortName("None")]
        PlayerPosition0 = 0,

        [Color(255, 165, 0)]
        [Name("Goalkeeper")]
        [ShortName("GK")]
        PlayerPosition1 = 1,

        [Color(0, 255, 0)]
        [Name("Defender")]
        [ShortName("DF")]
        PlayerPosition2 = 2,

        [Color(0, 255, 0)]
        [Name("Defender")]
        [ShortName("DF")]
        PlayerPosition3 = 3,

        [Color(0, 255, 0)]
        [Name("Midfielder")]
        [ShortName("MF")]
        PlayerPosition4 = 4,

        [Color(0, 0, 255)]
        [Name("Midfielder")]
        [ShortName("MF")]
        PlayerPosition5 = 5,

        [Color(0, 0, 255)]
        [Name("Midfielder")]
        [ShortName("MF")]
        PlayerPosition6 = 6,

        [Color(0, 0, 255)]
        [Name("Midfielder")]
        [ShortName("MF")]
        PlayerPosition7 = 7,

        [Color(255, 0, 0)]
        [Name("Forward")]
        [ShortName("FW")]
        PlayerPosition8 = 8,

        [Color(255, 0, 0)]
        [Name("Forward")]
        [ShortName("FW")]
        PlayerPosition9 = 9,

        [Color(255, 0, 0)]
        [Name("Forward")]
        [ShortName("FW")]
        PlayerPosition10 = 10,
    }
}
