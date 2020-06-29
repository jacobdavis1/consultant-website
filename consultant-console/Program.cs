using consultant_data.Database;
using consultant_data.Models;
using consultant_data.RepositoryInterfaces;
using consultant_logic.Repositories;
using Microsoft.EntityFrameworkCore;
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

            Console.WriteLine("Time to run some live tests!");
            Console.WriteLine();
            Console.WriteLine();

            await RunUserTests(userRepository);
            await RunUserWithCasesTest(userRepository, caseRepository, caseStatusRepository);
            await RunCaseWithAppointmentsTest(userRepository, caseRepository, caseStatusRepository);
            await RunCaseWithNotesTest(userRepository, caseRepository, caseStatusRepository);
        }

        static async Task RunUserTests(IUserRepository _user)
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
            await _user.AddUserAsync(user);

            Console.WriteLine("Added user with id " + user.Id.ToString() + ". User set to null.");
            String userId = user.Id.ToString();
            user = null;

            user = await _user.GetUserByIdAsync(Guid.Parse(userId));
            Console.WriteLine("Checking get: user name: " + user.FirstName + " " + user.MiddleName + " " + user.LastName + " ");
            Console.WriteLine("Changing name...");

            user.FirstName = "TestName";
            await _user.UpdateUserAsync(user);
            Console.WriteLine("User name: " + user.FirstName + " " + user.MiddleName + " " + user.LastName + " ");

            Console.WriteLine("Deleting added user.");

            await _user.DeleteUserAsync(user);

            user = null;
            user = await _user.GetUserByIdAsync(Guid.Parse(userId));

            Console.WriteLine("Deletion: " + ((user == null) ? "Success" : "Failure"));
            Console.WriteLine("TEST END");
            Console.WriteLine();
            Console.WriteLine();
        }

        static async Task RunUserWithCasesTest(IUserRepository _user, ICaseRepository _case, ICaseStatusRepository _caseStatus)
        {
            Console.WriteLine("TEST BEGIN.");
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

            client = await _user.GetUserByIdAsync(client.Id);
            aCase = await _case.GetCaseByIdAsync(aCase.Id);
            consultant = await _user.GetUserByIdAsync(consultant.Id);

            Console.WriteLine("Deletion: " + ((client == null && aCase == null && consultant == null) ? "Success" : "Failure"));
            Console.WriteLine("TEST END.");
            Console.WriteLine();
            Console.WriteLine();
        }

        static async Task RunCaseWithAppointmentsTest(IUserRepository _user, ICaseRepository _case, ICaseStatusRepository _caseStatus)
        {
            Console.WriteLine("TEST BEGIN");
            Console.WriteLine("Beginning appointment test...");

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

            // SETUP
            Console.WriteLine("Setting up test data...");
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
            await _case.AddCaseAsync(aCase);

            // CREATE appointments for case
            Console.WriteLine("Creating appointments...");
            await _case.AddAppointmentToCaseAsync(aCase, new Appointment
            {
                Id = Guid.NewGuid(),
                CaseId = aCase.Id,
                AppointmentDateTime = DateTime.Now,
                Title = "Test Appointment 1"
            });

            await _case.AddAppointmentToCaseAsync(aCase, new Appointment
            {
                Id = Guid.NewGuid(),
                CaseId = aCase.Id,
                AppointmentDateTime = DateTime.Today,
                Title = "Test Appointment 2"
            });

            // RETRIEVE case via client
            Console.WriteLine("Retrieving data...");
            client = await _user.GetUserByIdAsync(client.Id);
            foreach (Case c in client.Cases)
            {
                foreach (Appointment a in c.UpcomingAppointments)
                {
                    Console.WriteLine("Appointment for " + c.Title + ": " + a.AppointmentDateTime);
                }
            }

            // UPDATE first appointment to be for tomorrow
            Console.WriteLine("Updating an appointment...");
            client.Cases[0].UpcomingAppointments[0].AppointmentDateTime = client.Cases[0].UpcomingAppointments[0].AppointmentDateTime.AddDays(1);
            await _case.UpdateAppointmentAsync(client.Cases[0].UpcomingAppointments[0]);

            // DELETE appointment
            Console.WriteLine("Deleting an appointment...");
            await _case.DeleteAppointmentFromCaseAsync(client.Cases[0], client.Cases[0].UpcomingAppointments[1]);

            // RETRIEVE case via client a second time to see update
            Console.WriteLine("Retrieving data again...");
            client = await _user.GetUserByIdAsync(client.Id);
            foreach (Case c in client.Cases)
            {
                foreach (Appointment a in c.UpcomingAppointments)
                {
                    Console.WriteLine("Appointment for " + c.Title + ": " + a.AppointmentDateTime);
                }
            }

            // CLEANUP
            Console.WriteLine("Deleting entries...");
            await _user.DeleteUserAsync(client);
            await _case.DeleteCaseAsync(aCase);
            await _user.DeleteUserAsync(consultant);

            client = await _user.GetUserByIdAsync(client.Id);
            aCase = await _case.GetCaseByIdAsync(aCase.Id);
            consultant = await _user.GetUserByIdAsync(consultant.Id);

            Console.WriteLine("Deletion: " + ((client == null && aCase == null && consultant == null) ? "Success" : "Failure"));
            Console.WriteLine("TEST END.");
            Console.WriteLine();
            Console.WriteLine();
        }

        static async Task RunCaseWithNotesTest(IUserRepository _user, ICaseRepository _case, ICaseStatusRepository _caseStatus)
        {
            Console.WriteLine("TEST BEGIN.");
            Console.WriteLine("Beginning note test...");

            // SET UP
            Console.WriteLine("Setting up test data...");
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
            await _case.AddCaseAsync(aCase);

            // CREATE new notes and add them to the case
            Console.WriteLine("Creating notes...");
            await _case.AddNoteToCaseAsync(aCase, new CaseNote
            {
                Id = Guid.NewGuid(),
                CaseId = aCase.Id,
                Content = "This case is a test case, and this is a test note."
            });
            await _case.AddNoteToCaseAsync(aCase, new CaseNote
            {
                Id = Guid.NewGuid(),
                CaseId = aCase.Id,
                Content = "Another test note."
            });

            // RETRIEVE the user with the case to get the client
            Console.WriteLine("Retrieving notes...");
            client = await _user.GetUserByIdAsync(client.Id);
            foreach (Case c in client.Cases)
            {
                foreach (CaseNote cn in c.Notes)
                {
                    Console.WriteLine("Note: " + cn.Content);
                }
            }

            // UPDATE a note
            Console.WriteLine("Updating a note...");
            CaseNote note = client.Cases[0].Notes[0];
            note.Content = "Updated content.";
            await _case.UpdateNoteAsync(note);

            // DELETE a note
            Console.WriteLine("Deleting  note...");
            await _case.DeleteNoteFromCaseAsync(client.Cases[0], client.Cases[0].Notes[1]);

            // RETRIEVE again to see the changes
            Console.WriteLine("Retrieving notes again...");
            client = await _user.GetUserByIdAsync(client.Id);
            foreach (Case c in client.Cases)
            {
                foreach (CaseNote cn in c.Notes)
                {
                    Console.WriteLine("Note: " + cn.Content);
                }
            }

            // CLEAN UP
            Console.WriteLine("Deleting entries...");
            await _user.DeleteUserAsync(client);
            await _case.DeleteCaseAsync(aCase);
            await _user.DeleteUserAsync(consultant);

            consultant = await _user.GetUserByIdAsync(consultant.Id);
            client = await _user.GetUserByIdAsync(client.Id);
            aCase = await _case.GetCaseByIdAsync(aCase.Id);

            Console.WriteLine("Deletion: " + ((consultant == null && client == null && aCase == null) ? "Success" : "Failure"));
            Console.WriteLine("TEST END.");
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
