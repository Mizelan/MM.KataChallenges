using Reversi.Logics.Definition;

namespace Reversi.Logics
{
    public class MapTraverser
    {
        Map map;
        PlayerColor playerColor;

        public MapTraverser(Map map, PlayerColor playerColor)
        {
            this.map = map;
            this.playerColor = playerColor;
        }

        public Cell GetNextCell(Cell currentCell, TraversalDirection traversalDirection)
        {
            int adjX = 0, adjY = 0;
            switch (traversalDirection)
            {
                case TraversalDirection.Top:
                adjY -= 1;
                break;
                case TraversalDirection.Bottom:
                adjY += 1;
                break;
                case TraversalDirection.Left:
                adjX -= 1;
                break;
                case TraversalDirection.Right:
                adjX += 1;
                break;
            }

            if (!map.CheckValidCellPosition(currentCell.X + adjX, currentCell.Y + adjY))
                return null;

            return map[currentCell.X + adjX, currentCell.Y + adjY];
        }

        public Cell FindEnemyNextBlankCell(Cell currentCell, TraversalDirection traversalDirection)
        {
            Cell lastEnemyCell = null;
            while (true)
            {
                var targetCell = GetNextCell(currentCell, traversalDirection);
                if (targetCell == null)
                    break;

                currentCell = targetCell;

                if (lastEnemyCell != null &&
                    targetCell.OccupiedColor == PlayerColor.None)
                {
                    return targetCell;
                }

                if (targetCell.OccupiedColor == playerColor)
                    return null;

                if (targetCell.OccupiedColor != PlayerColor.None &&
                    targetCell.OccupiedColor != playerColor)
                {
                    lastEnemyCell = targetCell;
                }
            }

            return null;
        }
    }
}
