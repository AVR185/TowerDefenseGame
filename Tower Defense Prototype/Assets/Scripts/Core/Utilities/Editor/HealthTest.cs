using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace Core.Health
{
    [TestFixture]
    public class HealthTest
    {
        //Arrange
        Damageable damageable = new Damageable();

        [Test]
        public void EstaMuerto()
        {
            //Act
            damageable.SetHealth(0f);
            bool muerto = damageable.isDead;

            //Assert
            Assert.IsTrue(muerto);
        }

        [Test]
        public void ActualizacionVida()
        {
            //Act
            damageable.SetHealth(5f);
            damageable.IncreaseHealth(-5f);
            float vidaActual = damageable.currentHealth;

            //Assert
            Assert.AreEqual(0f, vidaActual);
        }

        [Test]
        public void EstaVidaMaxima()
        {
            //Act
            damageable.SetMaxHealth(10f);
            damageable.SetHealth(10f);
            bool vidaMaxima = damageable.isAtMaxHealth;

            //Assert
            Assert.IsTrue(vidaMaxima);
        }

        [Test]
        public void HaSidoDañado()
        {
            //Act
            damageable.SetHealth(10f);
            IAlignmentProvider aip = damageable.alignmentProvider;
            HealthChangeInfo hci;

            bool dañado = damageable.TakeDamage(5f, aip, out hci);

            //Assert
            Assert.IsTrue(dañado);
        }
    }
}
