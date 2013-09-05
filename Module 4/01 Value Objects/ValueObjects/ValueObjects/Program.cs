
namespace ValueObjects
{
    using System;
    using Shared;
    using WithoutValueObjects;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var person = new Person(new DateTime(1978, 01, 01));
                var account = person.OpenAccount(AccountTypeEnum.Investment, 50);

                account.DepositMoney(50);
                account.WithDrawMoney(90);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}

//namespace ValueObjects
//{
//    using System;
//    using WithValueObjects;
//    using Shared;
//    using ValueTypes;

//    class Program
//    {
//        static void Main(string[] args)
//        {
//            try
//            {
//                var person = new Person(new DateTime(1978, 01, 01));
//                var account = person.OpenAccount(AccountTypeEnum.Investment, new Money(50));

//                account.DepositMoney(new Money(50));
//                account.WithDrawMoney(new Money(90));
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }

//            Console.ReadKey();
//        }
//    }
//}
