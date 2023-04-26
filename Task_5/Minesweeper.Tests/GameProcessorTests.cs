using Minesweeper.Core;
using Minesweeper.Core.Enums;
using NUnit.Framework;
using System.Data.Common;

namespace Minesweeper.Tests
{
    [TestFixture]
    public class GameProcessorTests
    {
        private bool[,] boolField;

        [SetUp]
        public void SetUp()
        {
            boolField = new bool[,]
        {
            { true, false, false },
            { false, false, false },
            { false, false, false }
        };
        }
        //Positive tests
        [TestCase(1,1)]
        [Test]
        public void Open_CellWithoutMine_Success(int row, int col)
        {
            // Precondition
            
            var gameProcessor = new GameProcessor(boolField);

            // Action
            var gameState = gameProcessor.Open(row, col);

            // Assert
            Assert.AreEqual(GameState.Active, gameState);
           
        }
        [TestCase(0,0)]
        [Test]
        public void Open_CellWithMine_Lose(int row, int col)
        {
            // Precondition
            var gameProcessor = new GameProcessor(boolField);

            // Action
            var gameState = gameProcessor.Open(row, col);
            
            // Assert
            Assert.AreEqual(GameState.Lose, gameState);
        }
        [TestCase(2,2)]
        [Test]
        public void Open_AllCellWithoutMine_Win(int row, int col)
        {
            // Precondition
            var gameProcessor = new GameProcessor(boolField);
            
            // Action
            var gameState = gameProcessor.Open(row, col);
            
            // Assert
            Assert.AreEqual(GameState.Win, gameState);

        }

        [TestCase(1, 1)]
        [Test]
        public void Open_SameCellTwoTimes_SameStateActive(int row, int col)
        {
            // Precondition
            var gameProcessor = new GameProcessor(boolField);
            
            // Action
            gameProcessor.Open(row, col);
            var gameState = gameProcessor.Open(row, col);
            
            // Assert
            Assert.AreEqual(GameState.Active, gameState);

        }

        [Test]
        public void GetCurrentField_CountNumberOfMineNeighbors_NumberOfMineNeighborsFrom0To8()
        {
            // Precondition
            var gameProcessor = new GameProcessor(boolField);

            // Action
            gameProcessor.Open(1, 1);
            var currentField = gameProcessor.GetCurrentField();

            // Assert
            Assert.AreEqual(PointState.Neighbors1, currentField[1, 1]);
            Assert.AreEqual(PointState.Close, currentField[0, 0]);
            Assert.AreEqual(PointState.Close, currentField[2, 2]);

        }

        [Test]
        public void GetCurrentField_Initial_ReturnsClosedFields()
        {
            // Precondition
            var gameProcessor = new GameProcessor(boolField);

            // Action
            var currentField = gameProcessor.GetCurrentField();

            // Assert
            for(int i = 0; i < currentField.GetLength(0); i++) 
                for(int j = 0; j < currentField.GetLength(1); j++)
                    Assert.AreEqual(PointState.Close, currentField[i, j]);
            
        }

        [TestCase(true, 0, 0)]
        [Test]
        public void GetCurrentField_AfterOpenCellWithMine_ReturnsMinePointStatus(bool mine, int row, int col)
        {

            // Precondition
            boolField[row, col] = mine;
            var gameProcessor = new GameProcessor(boolField);
            
            // Action
            gameProcessor.Open(row, col);
            var currentField = gameProcessor.GetCurrentField();

            // Assert
            Assert.AreEqual(PointState.Mine, currentField[row, col]);
            
        }

        //Negative tests
        [Test]
        public void Open_InvalidCell_ShouldThrowIndexOutOfRangeException()
        {
            var gameProcessor = new GameProcessor(boolField);

            
            Assert.Throws<IndexOutOfRangeException>(() => gameProcessor.Open(10, 10));
        }

        [Test]
        public void Open_GameFinished_ShouldThrowInvalidOperationException()
        {
            var gameProcessor = new GameProcessor(boolField);

            gameProcessor.Open(2, 2);
            
            Assert.Throws<InvalidOperationException>(() => gameProcessor.Open(0, 0));
        }

    }
}