using System;
using PipServices.Commons.Refer;
using PipServices.Commons.Config;
using PipServices.Commons.Errors;
using PipServices.Commons.Log;
using PipServices.Commons.Run;
using PipServices.Commons.Build;

namespace PipServices.Container.Test
{
    public class DummyFactory : IFactory, IDescriptable
    {
        public static Descriptor Descriptor { get; } = new Descriptor("pip-services-dummies", "factory", "*", "1.0");

        public Descriptor GetDescriptor()
        {
            return Descriptor;
        }

        public bool CanCreate(object locator)
        {
            var descriptor = locator as Descriptor;

            if (descriptor == null)
                return false;

            if (descriptor.Match(DummyController.Descriptor))
                return true;

            return false;
        }

        public object Create(object locator)
        {
            var descriptor = locator as Descriptor;

            if (descriptor == null)
                return null;

            if (descriptor.Match(DummyController.Descriptor))
                return new DummyController();

            return null;
        }
    }
}
