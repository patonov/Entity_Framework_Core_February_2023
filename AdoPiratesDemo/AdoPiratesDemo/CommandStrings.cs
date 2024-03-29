﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoPiratesDemo
{
    public static class CommandStrings
    {
        public const string SelectShipNames = "SELECT Name FROM Ships";

        public const string InsertPirateAndPlunder = "INSERT INTO PiratesPlunders(PirateId, PlunderId) VALUES(50, 6)";

        public static string SearchParticipantsInPlunderFor(string location) 
        {
            return $"exec usp_SearchParticipantsInPlunder '{location}'";
        }

        public static string ReportAboutFreeBerthsOnBoardOfShip(int i)
        {
            return $"SELECT dbo.udf_FreeBerthsOnShip({i})";
        }

    }
}
