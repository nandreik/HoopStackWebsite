using Microsoft.VisualStudio.TestTools.UnitTesting;
using HoopStackWebsite.Solver;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoopStackWebsite.Solver.Tests
{
    [TestClass()]
    public class MoveTests
    {
        [TestMethod()]
        public void MoveTest()
        {
            //arrange
            int step = 1;
            int from = 0;
            int to = 1;
            string color = "Pink";
            //act
            Move m = new Move(step, from, to, color);
            //assert
            Assert.AreEqual(step, m.step);
            Assert.AreEqual(from, m.from);
            Assert.AreEqual(to, m.to);
            Assert.AreEqual(color, m.color);
        }
    }
}