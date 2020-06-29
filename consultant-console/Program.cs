using consultant_data.Database;
using consultant_data.Models;
using consultant_data.RepositoryInterfaces;
using consultant_logic.Repositories;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleRunner
{
    class Program
    {
        static async Task Main(string[] args)
        {
            khbatlzvContext context = new khbatlzvContext();

            IUserRepository userRepository = new UserRepository(context);
            ICaseRepository caseRepository = new CaseRepository(context);
            ICaseStatusRepository caseStatusRepository = new CaseStatusRepository(context);
            IAppointmentRepository appointmentRepository = new AppointmentRepository(context);
            ICaseNoteRepository caseNoteRepository = new CaseNoteRepository(context);

            Console.WriteLine("Time to run some live tests!");

            // await RunUserTests(userRepository);
            // await RunUserWithCasesTest(userRepository, caseRepository, caseStatusRepository);
            // await RunCaseWithAppointmentsTest(userRepository, caseRepository, caseStatusRepository, appointmentRepository);
            await RunCaseWithNotesTest(userRepository, caseRepository, caseStatusRepository, caseNoteRepository);
        }

        static async Task RunUserTests(IUserRepository userRepository)
        {
            Console.WriteLine("Beginning user tests...");
            

            User user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Jacob",
                MiddleName = "Marl",
                LastName = "Javis",
                Email = "jacob@website.com"
            };

            Console.WriteLine("Adding user...");
            await userRepository.AddUserAsync(user);

            Console.WriteLine("Added user with id " + user.Id.ToString() + ". User set to null.");
            String userId = user.Id.ToString();
            user = null;

            user = await userRepository.GetUserByIdAsync(Guid.Parse(userId));
            Console.WriteLine("Checking get: user name: " + user.FirstName + " " + user.MiddleName + " " + user.LastName + " ");
            Console.WriteLine("Changing name...");

            user.FirstName = "TestName";
            await userRepository.UpdateUserAsync(user);
            Console.WriteLine("User name: " + user.FirstName + " " + user.MiddleName + " " + user.LastName + " ");

            Console.WriteLine("Deleting added user.");

            await userRepository.DeleteUserAsync(user);

            user = null;
            user = await userRepository.GetUserByIdAsync(Guid.Parse(userId));

            Console.WriteLine("User get after delete: " + ((user == null) ? "null" : "not null (error)"));
        }

        static async Task RunUserWithCasesTest(IUserRepository _user, ICaseRepository _case, ICaseStatusRepository _caseStatus)
        {
            Console.WriteLine("Beginning user with cases test...");

            // Check that the "New" status exists, and if it doesnt, create it
            CaseStatus statusNew = await _caseStatus.GetCaseStatusByTextAsync("New");
            if (statusNew == null)
            {
                statusNew = new CaseStatus
                {
                    Id = Guid.NewGuid(),
                    Text = "New"
                };
                await _caseStatus.AddCaseStatusAsync(statusNew);
            }
                
            User consultant = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Consultant",
                MiddleName = "Del",
                LastName = "Sol",
                Email = "consultant@website.com"
            };
            await _user.AddUserAsync(consultant);

            User client = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Charles",
                MiddleName = "Dingo",
                LastName = "Jones",
                Email = "charlesdingo@website.com"
            };
            await _user.AddUserAsync(client);

            Case aCase = new Case
            {
                Id = Guid.NewGuid(),
                Title = "Test Case",
                ActiveConsultant = consultant,
                Clients = new List<User> { client },
                Status = statusNew,
            };
            await _case.AddCaseAsync(aCase);

            client = await _user.GetUserByIdAsync(client.Id);
            Console.WriteLine("Cases: ");
            for (int i = 0; i < client.Cases.Count; ++i)
            {
                Console.WriteLine(client.Cases[i].Title);
            }

            await _user.DeleteUserAsync(client);
            await _case.DeleteCaseAsync(aCase);
            await _user.DeleteUserAsync(consultant);
        }

        static async Task RunCaseWithAppointmentsTest(IUserRepository _user, ICaseRepository _case, ICaseStatusRepository _caseStatus, IAppointmentRepository _appointment)
        {
            // Check that the "New" status exists, and if it doesnt, create it
            CaseStatus statusNew = await _caseStatus.GetCaseStatusByTextAsync("New");
            if (statusNew == null)
            {
                statusNew = new CaseStatus
                {
                    Id = Guid.NewGuid(),
                    Text = "New"
                };
                await _caseStatus.AddCaseStatusAsync(statusNew);
            }

            User consultant = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Consultant",
                MiddleName = "Del",
                LastName = "Sol",
                Email = "consultant@website.com"
            };
            await _user.AddUserAsync(consultant);

            User client = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Charles",
                MiddleName = "Dingo",
                LastName = "Jones",
                Email = "charlesdingo@website.com"
            };
            await _user.AddUserAsync(client);

            Case aCase = new Case
            {
                Id = Guid.NewGuid(),
                Title = "Test Case",
                ActiveConsultant = consultant,
                Clients = new List<User> { client },
                Status = statusNew
            };
            aCase.UpcomingAppointments.Add(new Appointment
            {
                Id = Guid.NewGuid(),
                CaseId = aCase.Id,
                AppointmentDateTime = DateTime.Now,
                Title = "Test Appointment 1"
            });
            aCase.UpcomingAppointments.Add(new Appointment
            {
                Id = Guid.NewGuid(),
                CaseId = aCase.Id,
                AppointmentDateTime = DateTime.Today,
                Title = "Test Appointment 2"
            });
            await _case.AddCaseAsync(aCase);

            client = await _user.GetUserByIdAsync(client.Id);
            foreach (Case c in client.Cases)
            {
                foreach (Appointment a in c.UpcomingAppointments)
                {
                    Console.WriteLine("Appointment for " + c.Title + ": " + a.AppointmentDateTime);
                }
            }

            Console.WriteLine("Deleting entries...");
            await _user.DeleteUserAsync(client);
            await _case.DeleteCaseAsync(aCase);
            await _user.DeleteUserAsync(consultant);

            client = await _user.GetUserByIdAsync(client.Id);
            aCase = await _case.GetCaseByIdAsync(aCase.Id);
            consultant = await _user.GetUserByIdAsync(consultant.Id);

            Console.WriteLine("Deletion: " + ((client == null && aCase == null && consultant == null) ? "Success" : "Failed")); 
        }

        static async Task RunCaseWithNotesTest(IUserRepository _user, ICaseRepository _case, ICaseStatusRepository _caseStatus, ICaseNoteRepository _note)
        {
            // Check that the "New" status exists, and if it doesnt, create it
            CaseStatus statusNew = await _caseStatus.GetCaseStatusByTextAsync("New");
            if (statusNew == null)
            {
                statusNew = new CaseStatus
                {
                    Id = Guid.NewGuid(),
                    Text = "New"
                };
                await _caseStatus.AddCaseStatusAsync(statusNew);
            }

            User consultant = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Consultant",
                MiddleName = "Del",
                LastName = "Sol",
                Email = "consultant@website.com"
            };
            await _user.AddUserAsync(consultant);

            User client = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Charles",
                MiddleName = "Dingo",
                LastName = "Jones",
                Email = "charlesdingo@website.com"
            };
            await _user.AddUserAsync(client);

            Case aCase = new Case
            {
                Id = Guid.NewGuid(),
                Title = "Test Case",
                ActiveConsultant = consultant,
                Clients = new List<User> { client },
                Status = statusNew
            };
            aCase.Notes.Add(new CaseNote
            {
                Id = Guid.NewGuid(),
                CaseId = aCase.Id,
                Content = "This case is a test case, and this is a test note."
            });
            aCase.Notes.Add(new CaseNote
            {
                Id = Guid.NewGuid(),
                CaseId = aCase.Id,
                Content = "Another test note."
            });
            await _case.AddCaseAsync(aCase);

            client = await _user.GetUserByIdAsync(client.Id);
            foreach (Case c in client.Cases)
            {
                foreach (CaseNote cn in c.Notes)
                {
                    Console.WriteLine("Note: " + cn.Content);
                }
            }

            Console.WriteLine("Deleting entries...");
            await _user.DeleteUserAsync(client);
            await _case.DeleteCaseAsync(aCase);
            await _user.DeleteUserAsync(consultant);

            consultant = await _user.GetUserByIdAsync(consultant.Id);
            client = await _user.GetUserByIdAsync(client.Id);
            aCase = await _case.GetCaseByIdAsync(aCase.Id);

            Console.WriteLine("Deletion: " + ((consultant == null && client == null && aCase == null) ? "Successful" : "Failed"));
        }
    }
}
