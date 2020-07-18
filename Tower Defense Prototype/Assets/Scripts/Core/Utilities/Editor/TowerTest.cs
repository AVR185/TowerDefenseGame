using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace TowerDefense.Towers
{
    [TestFixture]
    public class TowerTest
    {
        //Arrange
        Tower tower = new Tower();

        [Test]
        public void MaximoNivel()
        {
            //Act
            tower.levels = new TowerLevel[]{};
            tower.UpgradeTowerToLevel(3);
            bool maximoNivel = tower.isAtMaxLevel;

            //Assert
            Assert.IsFalse(maximoNivel);
        }

        [Test]
        public void TorreDestruida()
        {
            try
            {
                //Act
                tower.levels = new TowerLevel[] { };
                tower.UpgradeTowerToLevel(2);
                tower.KillTower();

                //Assert
                Assert.Fail();
            }
            catch (Exception)
            {
                
            }
        }

        [Test]
        public void Downgrade()
        {
            //Act
            tower.levels = new TowerLevel[] { };
            tower.UpgradeTowerToLevel(1);
            tower.DowngradeTower();
            int numero = tower.currentLevel;

            //Assert
            Assert.AreEqual(0, numero);
        }
    }
}
