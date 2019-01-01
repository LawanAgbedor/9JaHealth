using System;
using DAL.com._9jahealth.data.dao;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WSApi.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var u = UserDao.GetByUserName("yahaya");
            Assert.IsNotNull(u);
        }
    }
}
