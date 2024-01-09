using System.Collections.Generic;

namespace IEGOFormationEditor.InazumaEleven.Logic
{
    public class Formation
    {
        public FormationHeader Header;
        public List<IFormationInfo> Players;

        public Formation(FormationHeader header, List<IFormationInfo> players)
        {
            Header = header;
            Players = players;
        }
    }

    public class FormationHeader
    {
        public int PlayerCount { get; set; }
        public int FormationType { get; set; }
        public int Unk1 { get; set; }
        public int Unk2 { get; set; }
        public int FormationHash { get; set; }
        public int OFStar { get; set; }
        public int DFStar { get; set; }
    }

    public class IFormationInfo
    {
        public int PlayerNumber { get; set; }
        public int PlayerPosition { get; set; }
        public int Unk1 { get; set; }
        public float StartX { get; set; }
        public float StartY { get; set; }
        public float DefenseX { get; set; }
        public float DefenseY { get; set; }
        public float AttackX { get; set; }
        public float AttackY { get; set; }
        public float Corner1X { get; set; }
        public float Corner1Y { get; set; }
        public float Corner2X { get; set; }
        public float Corner2Y { get; set; }
        public float Corner3X { get; set; }
        public float Corner3Y { get; set; }
        public float Corner4X { get; set; }
        public float Corner4Y { get; set; }
        public float Goal1X { get; set; }
        public float Goal1Y { get; set; }
        public float Goal2X { get; set; }
        public float Goal2Y { get; set; }
        public float PreparationX { get; set; }
        public float PreparationY { get; set; }
    }

    public class Match : IFormationInfo
    {
        public new int PlayerNumber { get => base.PlayerNumber; set => base.PlayerNumber = value; }
        public new int PlayerPosition { get => base.PlayerPosition; set => base.PlayerPosition = value; }
        public new int Unk1 { get => base.Unk1; set => base.Unk1 = value; }
        public new float StartX { get => base.StartX; set => base.StartX = value; }
        public new float StartY { get => base.StartY; set => base.StartY = value; }
        public new float DefenseX { get => base.DefenseX; set => base.DefenseX = value; }
        public new float DefenseY { get => base.DefenseY; set => base.DefenseY = value; }
        public new float AttackX { get => base.AttackX; set => base.AttackX = value; }
        public new float AttackY { get => base.AttackY; set => base.AttackY = value; }
        public new float Corner1X { get => base.Corner1X; set => base.Corner1X = value; }
        public new float Corner1Y { get => base.Corner1Y; set => base.Corner1Y = value; }
        public new float Corner2X { get => base.Corner2X; set => base.Corner2X = value; }
        public new float Corner2Y { get => base.Corner2Y; set => base.Corner2Y = value; }
        public new float Corner3X { get => base.Corner3X; set => base.Corner3X = value; }
        public new float Corner3Y { get => base.Corner3Y; set => base.Corner3Y = value; }
        public new float Corner4X { get => base.Corner4X; set => base.Corner4X = value; }
        public new float Corner4Y { get => base.Corner4Y; set => base.Corner4Y = value; }
        public new float Goal1X { get => base.Goal1X; set => base.Goal1X = value; }
        public new float Goal1Y { get => base.Goal1Y; set => base.Goal1Y = value; }
        public new float Goal2X { get => base.Goal2X; set => base.Goal2X = value; }
        public new float Goal2Y { get => base.Goal2Y; set => base.Goal2Y = value; }
        public new float PreparationX { get => base.PreparationX; set => base.PreparationX = value; }
        public new float PreparationY { get => base.PreparationY; set => base.PreparationY = value; }
    }

    public class Battle : IFormationInfo
    {
        public new int PlayerNumber { get => base.PlayerNumber; set => base.PlayerNumber = value; }
        public new int PlayerPosition { get => base.PlayerPosition; set => base.PlayerPosition = value; }
        public new int Unk1 { get => base.Unk1; set => base.Unk1 = value; }
        public new float StartX { get => base.StartX; set => base.StartX = value; }
        public new float StartY { get => base.StartY; set => base.StartY = value; }
        public new float DefenseX { get => base.DefenseX; set => base.DefenseX = value; }
        public new float DefenseY { get => base.DefenseY; set => base.DefenseY = value; }
        public new float AttackX { get => base.AttackX; set => base.AttackX = value; }
        public new float AttackY { get => base.AttackY; set => base.AttackY = value; }
        public new float PreparationX { get => base.PreparationX; set => base.PreparationX = value; }
        public new float PreparationY { get => base.PreparationY; set => base.PreparationY = value; }
    }
}
