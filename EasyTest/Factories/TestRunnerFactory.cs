using EasyTest.Exceptions;
using EasyTest.Interfaces;
using EasyTest.Models.TestTypes;
using System;
using System.Collections.Generic;

namespace EasyTest.Factories
{
    public static class TestRunnerFactory
    {
        private static Dictionary<Type, Func<string, ITestRunner<BaseTestType>>> registered = new Dictionary<Type, Func<string, ITestRunner<BaseTestType>>>();

        public static void RegisterTestRunner<T>(Func<string, ITestRunner<BaseTestType>> createFunction) where T : ITestRunner<BaseTestType>
        {
            if (registered.ContainsKey(typeof(T)))
            {
                throw new TestRunnerAlreadyRegistered($"Test Runner {typeof(T).Name} already registered");
            }
            registered.Add(typeof(T), createFunction);
        }

        public static ITestRunner<BaseTestType> GetRunner<T>(string testName) where T : ITestRunner<BaseTestType>
        {
            if (!registered.ContainsKey(typeof(T)))
            {
                throw new TestRunnerAlreadyRegistered($"Test Runner {typeof(T).Name} not registered");
            }
            return registered[typeof(T)].Invoke(testName);
        }

        public static ITestRunner<BaseTestType> GetRunner(Type type, string testName) 
        {
            if (!registered.ContainsKey(type))
            {
                throw new TestRunnerAlreadyRegistered($"Test Runner {type.Name} not registered");
            }
            return registered[type].Invoke(testName);
        }

    }
}
