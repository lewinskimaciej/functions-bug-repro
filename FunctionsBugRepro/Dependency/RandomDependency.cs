using System;

namespace FunctionsBugRepro.Dependency
{
    public class RandomDependency : IDependency
    {
        public int GetRandomNumber()
        {
            return new Random().Next();
        }
    }
}
