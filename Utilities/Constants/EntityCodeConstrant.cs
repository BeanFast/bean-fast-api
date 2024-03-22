﻿
using Diacritics.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Utils;

namespace Utilities.Constants
{
    public class EntityCodeConstrant
    {
        public static readonly string Separator = "_";

        public static class GiftCodeConstrant
        {
            public static string GiftPrefix = "G";
        }

        public class FoodCodeConstrant
        {
            public static string FoodPrefix = "F";
        }

        public class CategoryCodeConstrant
        {
            public static string CategoryPrefix = "C";
        }

        public class AreaCodeConstrant
        {
            public static string AreaPrefix = "A";
        }

        public class OrderCodeConstrant
        {
            public static string OrderPrefix = "O";
        }
        public class OrderActivityCodeConstrant
        {
            public static string OrderActivityPrefix = "Oa";
        }

        public class UserCodeConstrant
        {
            public static string UserPrefix = "U";
            public static string CustomerPrefix = "Cus";
        }

        public class ComboCodeConstrant
        {
            public static string ComboPrefix = "Cb";
        }
        public class SchoolCodeConstrant
        {
            public static string SchoolPrefix = "S";
        }

        public class SessionCodeConstrant
        {
            public static string SessionPrefix = "Ss";
        }

        public class SessionDetailCodeConstrant
        {
            public static string SessionDetailPrefix = "Sd";
        }
        public class CardTypeCodeConstrant
        {
            public static string CardTypePrefix = "Ct";
        }
        public class ProfileCodeConstrant
        {
            public static string ProfilePrefix = "Pf";
        }
        public class LocationCodeConstrant
        {
            public static string LocationPrefix = "L";
        }
        public class MenuCodeConstrant
        {
            public static string MenuPrefix = "M";
        }
        public class MenuDetailCodeConstrant
        {
            public static string MenuDetailPrefix = "Md";
        }
<<<<<<< HEAD

        public class OrderActivityCodeConstrant
        {
            public static string OrderActivityPrefix = "Oa";
        }

        public class OrderDetailCodeConstrant
        {
            public static string OrderDetailPrefix = "Od";
        }

        public class TransactionCodeConstrant
        {
            public static string TransactionPrefix = "Tr";
=======
        public class ExchangeGiftCodeConstraint
        {
            public static string ExchangeGiftPrefix = "Eg";
        }
        public class TransactionCodeConstrant
        {
            public static string TransactionPrefix = "T";
            public static string ExchangeGiftTransactionPrefix = TransactionPrefix + ExchangeGiftCodeConstraint.ExchangeGiftPrefix;

>>>>>>> fc91342a0402d3445967991dcfd8a792b0fae0db
        }
    }
}
