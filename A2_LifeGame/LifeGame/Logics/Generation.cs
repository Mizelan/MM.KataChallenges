using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGame.Logics
{
    public class Generation
    {
        public int SequenceId { get; private set; }
        public Map Map { get; private set; }

        public Generation(int seqId, Map map)
        {
            this.SequenceId = seqId;
            this.Map = map;
        }

        public void GoNextGeneration()
        {
            SequenceId++;

            var lazyUpdateList = new List<Action>();
            for (var x = 0; x < Map.Width; x++)
            {
                for (var y = 0; y < Map.Height; y++)
                {
                    var cell = Map[x, y];
                    LazyUpdateCellLiveStatus(cell, out var updateAction);

                    if (updateAction != null)
                        lazyUpdateList.Add(updateAction);
                }
            }

            foreach (var action in lazyUpdateList)
                action.Invoke();
        }

        void LazyUpdateCellLiveStatus(Cell cell, out Action updateAction)
        {
            var aliveNeighborCellCount = Map.GetNeighborAliveCellCount(cell);
            if (cell.IsAlive && aliveNeighborCellCount < 2)
                updateAction = () => cell.IsAlive = false;
            else if (cell.IsAlive && aliveNeighborCellCount > 3)
                updateAction = () => cell.IsAlive = false;
            else if (cell.IsAlive && aliveNeighborCellCount == 2 || aliveNeighborCellCount == 3)
                updateAction = () => cell.IsAlive = true;
            else if (!cell.IsAlive && aliveNeighborCellCount == 3)
                updateAction = () => cell.IsAlive = true;
            else
                updateAction = null;
        }
    }
}
