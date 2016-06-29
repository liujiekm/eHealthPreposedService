using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using eHPS.CrossCutting.Adapter;
using eHPS.CrossCutting.NetFramework.Adapter;

namespace eHPS.API.Test
{
    [TestClass]
    public class TinyMapperTest
    {

        private  ITypeAdapterFactory typeAdapterFactory;


        [TestInitialize]
        public void Initialize()
        {
            var container = new UnityContainer();

            //注册AutoMapper Adapter Factory
            container.RegisterType<ITypeAdapterFactory, AutomapperTypeAdapterFactory>(
               new ContainerControlledLifetimeManager());


            typeAdapterFactory = container.Resolve<ITypeAdapterFactory>();
            TypeAdapterFactory.SetCurrent(typeAdapterFactory);
        }


        [TestMethod]
        public void Could_Transfer_With_TinyMapper()
        {

            var person = new Person
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "John",
                LastName = "Doe",
                Address="Los Angeless"
            };

            var persionDto = TypeAdapterFactory.CreateAdapter().Adapter<PersonDto>(person);


            Assert.IsNotNull(persionDto.Name);


        }


        [TestMethod]
        public void Could_Transfer_With_AutoMapper()
        {

            var person = new Person
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "John",
                LastName = "Doe",
                Address = "Los Angeless"
            };

            var persionDto = TypeAdapterFactory.CreateAdapter().Adapter<PersonDto>(person);


            Assert.IsNotNull(persionDto.Name);


        }
    }
}
