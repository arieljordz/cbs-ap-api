using Bogus;
using CbsAp.Application.FakeStoreData.FakeDTO;

namespace CbsAp.Application.FakeStoreData
{
    public class UserSearchFaker
    {
        public List<FakeSearchUser> returnFakeUser()
        {
            var rand = new Random();
            string[]? roles = new[] { "Administrator",
                "CBS System Support", "Accounts Payable",
                "Approver","Coder","Audit"};

            var userFaker = new Faker<FakeSearchUser>()
              .RuleFor(u => u.UserAccountID, f => f.IndexFaker + 1)
                    .RuleFor(u => u.UserID, f => f.Person.Email)
                    .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                    .RuleFor(u => u.LastName, f => f.Person.LastName)
              .RuleFor(u => u.UserRoles, f => GenerateRandomUserRoles());

            return userFaker.Generate(5);
        }

        private List<FakeUserRole> GenerateRandomUserRoles()
        {
            var predefinedRoles = new List<FakeUserRole> {
                  new FakeUserRole { RoleID = 1, RoleName ="Administrator"  },
                  new FakeUserRole { RoleID = 2, RoleName ="CBS System Support" },
                  new FakeUserRole { RoleID = 3, RoleName ="Accounts Payable"  },
                  new FakeUserRole { RoleID = 4, RoleName ="Approver" },
                  new FakeUserRole { RoleID = 5, RoleName ="Code"  }
                   };
            var random = new Random();
            var numberOfRoles = random.Next(1, predefinedRoles.Count() + 1);
            var randomRoles = predefinedRoles.OrderBy(x => random.Next()).Take(numberOfRoles);

            return randomRoles.ToList();
        }

        public List<FakeUserRole> GenerateRoles()
        {
            var predefinedRoles = new List<FakeUserRole> {
                  new FakeUserRole { RoleID = 1, RoleName ="Administrator"  },
                  new FakeUserRole { RoleID = 2, RoleName ="CBS System Support" },
                  new FakeUserRole { RoleID = 3, RoleName ="Accounts Payable"  },
                  new FakeUserRole { RoleID = 4, RoleName ="Approver" },
                  new FakeUserRole { RoleID = 5, RoleName ="Code"  }
                   };

            return predefinedRoles;
        }

       
    }
}