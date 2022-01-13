using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIConsume.Helpers
{
    public static class SD
    {
        public static string APIBaseURL = "https://localhost:44318/";
        public static string APIPathEmployee = APIBaseURL+ "api/employee/";
        public static string APIPathPosition = APIBaseURL+ "api/position/";
    }
}
