using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace Core.Economy
{
    [TestFixture]
    public class CurrencyTests
    {
        //Arrange
        Currency currency = new Currency(50);

        [Test]
        public void RecursosInsuficientes_Test()
        {
            //Act
            bool posibilidadAntes = currency.TryPurchase(60);

            //Assert
            Assert.IsFalse(posibilidadAntes);
        }

        [Test]
        public void addResources_Test()
        {
            //Act
            currency.AddCurrency(50);
            int nuevaCantidad = currency.currentCurrency;

            //Assert
            Assert.AreEqual(100, nuevaCantidad);
        }

        [Test]
        public void RecursosSuficientes_Test()
        {
            //Act
            currency.AddCurrency(50);
            int nuevaCantidad = currency.currentCurrency;
            bool posibilidadDespues = currency.TryPurchase(60);


            //Assert
            Assert.IsTrue(posibilidadDespues);
        }
    }
}


