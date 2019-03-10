using Reversi.Logics.Definition;

namespace Reversi.Logics
{
    public class Game
    {
        public PlayerColor CurrentTurnPlayer { get; private set; }
        public Map Map { get; private set; }

        public Game(PlayerColor playerColor, Map map)
        {
            this.CurrentTurnPlayer = playerColor;
            this.Map = map;
        }

        public Map WriteSuggestionMovePosition()
        {
            var resultMap = new Map(Map.Width, Map.Height);
            var traverser = new MapTraverser(Map, CurrentTurnPlayer);
            for (var x = 0; x < Map.Width; x++)
            {
                for (var y = 0; y < Map.Height; y++)
                {
                    var occupiedColor = Map[x, y].OccupiedColor;
                    if (occupiedColor != PlayerColor.None)
                    {
                        resultMap[x, y].OccupiedColor = occupiedColor;
                    }

                    if (occupiedColor == CurrentTurnPlayer)
                    {
                        var cell = traverser.FindEnemyNextBlankCell(Map[x, y], TraversalDirection.Left);
                        if (cell != null)
                            resultMap[cell.X, cell.Y].IsMovementSuggestion = true;

                        cell = traverser.FindEnemyNextBlankCell(Map[x, y], TraversalDirection.Right);
                        if (cell != null)
                            resultMap[cell.X, cell.Y].IsMovementSuggestion = true;

                        cell = traverser.FindEnemyNextBlankCell(Map[x, y], TraversalDirection.Top);
                        if (cell != null)
                            resultMap[cell.X, cell.Y].IsMovementSuggestion = true;

                        cell = traverser.FindEnemyNextBlankCell(Map[x, y], TraversalDirection.Bottom);
                        if (cell != null)
                            resultMap[cell.X, cell.Y].IsMovementSuggestion = true;
                    }
                }
            }
            return resultMap;
        }
    }
}
