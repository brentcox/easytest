using EasyTest.Exceptions;
using EasyTest.Interfaces;
using EasyTest.Models.TestTypes;
using System;
using System.Collections.Generic;

namespace EasyTest.Factories
{
    public static class TestRunnerFactory
    {
        private static Dictionary<Type, Func<ITestRunner<BaseTestType>>> registered = new Dictionary<Type, Func<ITestRunner<BaseTestType>>>();

        public static void RegisterTestRunner<T>(Func<ITestRunner<BaseTestType>> createFunction) where T : ITestRunner<BaseTestType>
        {
            if (registered.ContainsKey(typeof(T)))
            {
                throw new TestRunnerAlreadyRegistered($"Test Runner {typeof(T).Name} already registered");
            }
            registered.Add(typeof(T), createFunction);
        }

        public static ITestRunner<BaseTestType> GetRunner<T>() where T : ITestRunner<BaseTestType>
        {
            if (!registered.ContainsKey(typeof(T)))
            {
                throw new TestRunnerAlreadyRegistered($"Test Runner {typeof(T).Name} not registered");
            }
            return registered[typeof(T)].Invoke();
        }

        public static ITestRunner<BaseTestType> GetRunner(Type type) 
        {
            if (!registered.ContainsKey(type))
            {
                throw new TestRunnerAlreadyRegistered($"Test Runner {type.Name} not registered");
            }
            return registered[type].Invoke();
        }

    }
}
