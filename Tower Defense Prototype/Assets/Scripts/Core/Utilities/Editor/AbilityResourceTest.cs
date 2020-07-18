using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace Abilities
{
    [TestFixture]
    public class AbilityResourceTest
    {
        AbilityResourceManager ability = new AbilityResourceManager();

        [Test]
        public void AddResource()
        {
            //Act
            ability.startAbilityResource = 20;
            ability.AddAbilityResource(20);
            int resultado = ability.CurrentAbilityResource;

            //Asset
            Assert.AreEqual(30, resultado);
        }

        [Test]
        public void BuyAbility()
        {
            //Act
            ability.startAbilityResource = 20;
            ability.BuyAbility(10);
            int resultado = ability.CurrentAbilityResource;

            //Asset
            Assert.AreEqual(20, resultado);
        }
    }
}