using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LimespotAssignment.Models.Input;

namespace LimespotAssignment.Tests.Class
{
    public class InvalidTransformerAddData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[]{"Grimlock",null},//No Rank
            new object[]{"Grimlock",12},//Rank outside the range
            new object[]{"",5},//No Name
            new object[]{ "GrimlockGrimlockGrimlockGrimlockGrimlock", 12}//Name too long
        };
        
        public IEnumerator<object[]> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
