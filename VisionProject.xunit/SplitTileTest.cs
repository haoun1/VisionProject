using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using VisionProject.Domain;
using Xunit;

namespace VisionProject.xunit
{
    public class SplitTileTest
    {
        public interface IDevice
        {
            void Connect();
            void DisConnect();
            int ReadData();
        }
        [Fact]
        public void CountTest()
        {
            List<Tile> tiles = SplitTile(1147, 926, 100, 100);
            Assert.Equal(120, tiles.Count);
            Assert.Equal(1100, tiles[tiles.Count - 1].startX);
            Assert.Equal(900, tiles[tiles.Count - 1].startY);
            Assert.Equal(47, tiles[tiles.Count - 1].width);
            Assert.Equal(26, tiles[tiles.Count - 1].height);

            List<Tile> tiles2 = SplitTile(1025, 1025, 512, 512);
            Assert.Contains(tiles2, t => t.width < 600);
            Assert.Equal(9, tiles2.Count);
            var lastTile = tiles2.FirstOrDefault((t => t.startX == 1024 || t.startY == 1024));
            Assert.NotNull(lastTile);
            Assert.Equal(typeof(Tile), lastTile.GetType());
            Assert.True(lastTile.width <= 512 && lastTile.height <= 512);
        }
        List<Tile> SplitTile(int memoryW, int memoryH, int tileW, int tileH)
        {
            List<Tile> tiles = new List<Tile>();
            int currentTileW;
            int currentTileH;
            for (int i = 0; i < memoryW; i += tileW)
            {
                currentTileW = Math.Min(tileW, memoryW - i); // 남은 너비 처리
                for (int j = 0; j < memoryH; j += tileH)
                {
                    currentTileH = Math.Min(tileH, memoryH - j); // 남은 높이 처리
                    tiles.Add(new Tile(new IntPtr(), i, j, currentTileW, currentTileH,1,1,currentTileW));
                }
            }
            return tiles;
        }
    }
}
