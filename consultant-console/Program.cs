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
            IAppointmentRepository appointmentRepository = new AppointmentRepository(context);
            INoteRepository noteRepository = new NoteRepository(context);
            ICaseRepository caseRepository = new CaseRepository(context, appointmentRepository, noteRepository);
            ICaseStatusRepository caseStatusRepository = new CaseStatusRepository(context);
            IRoleRepository roleRepository = new RoleRepository(context);

            Console.WriteLine("Time to run some live tests!");
            Console.WriteLine();
            Console.WriteLine();

            await RunUserTests(userRepository, roleRepository);
            await RunUserWithCasesTest(userRepository, caseRepository, caseStatusRepository, roleRepository);
            await RunCaseWithAppointmentsTest(userRepository, caseRepository, caseStatusRepository, appointmentRepository, roleRepository);
            await RunCaseWithNotesTest(userRepository, caseRepository, caseStatusRepository, noteRepository, roleRepository);
        }

        static async Task RunUserTests(IUserRepository _user, IRoleRepository _role)
        {
            Console.WriteLine("Beginning user tests...");

            Role roleClient = await _role.GetRoleByText("Client");

            User user = new User
            {
                UserId = "testUser",
                Role = roleClient
            };

            Console.WriteLine("Adding user...");
            user = await _user.AddUserAsync(user);

            Console.WriteLine("Added user with id " + user.Id.ToString() + ". User set to null.");
            int userRowId = user.Id;
            user = null;

            user = await _user.GetUserByRowIdAsync(userRowId);
            Console.WriteLine("Checking get: user name: " + user.UserId);
            Console.WriteLine("Changing name...");

            user.UserId = "UpdatedId";
            await _user.UpdateUserAsync(user);
            Console.WriteLine("User name: " + user.UserId);

            Console.WriteLine("Deleting added user.");

            await _user.DeleteUserAsync(user);

            user = null;
            user = await _user.GetUserByRowIdAsync(userRowId);

            Console.WriteLine("Deletion: " + ((user == null) ? "Success" : "Failure"));
            Console.WriteLine("TEST END");
            Console.WriteLine();
            Console.WriteLine();
        }

        static async Task RunUserWithCasesTest(IUserRepository _user, ICaseRepository _case, ICaseStatusRepository _caseStatus, IRoleRepository _role)
        {
            Console.WriteLine("TEST BEGIN.");
            Console.WriteLine("Beginning user with cases test...");

            Status unassigned = await _caseStatus.GetCaseStatusByTextAsync("Unassigned");
            Role roleClient = await _role.GetRoleByText("Client");
            Role roleConsultant = await _role.GetRoleByText("Consultant");
                
            User consultant = new User
            {
                UserId = "testid_consultant",
                Role = roleConsultant
            };
            consultant = await _user.AddUserAsync(consultant);

            User client = new User
            {
                UserId = "testid_client",
                Role = roleClient
            };
            client = await _user.AddUserAsync(client);

            Case aCase = new Case
            {
                Title = "Test Case",
                ActiveConsultant = consultant,
                Clients = new List<User> { client },
                Status = unassigned,
            };
            aCase = await _case.AddCaseAsync(aCase);

            client = await _user.GetUserByRowIdAsync(client.Id);
            Console.WriteLine("Cases: ");
            for (int i = 0; i < client.Cases.Count; ++i)
            {
                Console.WriteLine(client.Cases[i].Title);
            }

            await _user.DeleteUserAsync(client);
            await _case.DeleteCaseAsync(aCase);
            await _user.DeleteUserAsync(consultant);

            client = await _user.GetUserByIdAsync(client.UserId);
            aCase = await _case.GetCaseByIdAsync(aCase.Id);
            consultant = await _user.GetUserByIdAsync(consultant.UserId);

            Console.WriteLine("Deletion: " + ((client == null && aCase == null && consultant == null) ? "Success" : "Failure"));
            Console.WriteLine("TEST END.");
            Console.WriteLine();
            Console.WriteLine();
        }

        static async Task RunCaseWithAppointmentsTest(IUserRepository _user, ICaseRepository _case, 
            ICaseStatusRepository _caseStatus, IAppointmentRepository _appointment, IRoleRepository _role)
        {
            Console.WriteLine("TEST BEGIN");
            Console.WriteLine("Beginning appointment test...");

            Status unassigned = await _caseStatus.GetCaseStatusByTextAsync("Unassigned");
            Role roleClient = await _role.GetRoleByText("Client");
            Role roleConsultant = await _role.GetRoleByText("Consultant");

            // SETUP
            User consultant = new User
            {
                UserId = "testid_consultant",
                Role = roleConsultant
            };
            consultant = await _user.AddUserAsync(consultant);

            User client = new User
            {
                UserId = "testid_client",
                Role = roleClient
            };
            client = await _user.AddUserAsync(client);

            Case aCase = new Case
            {
                Title = "Test Case",
                ActiveConsultant = consultant,
                Clients = new List<User> { client },
                Status = unassigned
            };
            aCase = await _case.AddCaseAsync(aCase);

            // CREATE appointments for case
            Console.WriteLine("Creating appointments...");
            await _appointment.AddAppointmentToCaseAsync(aCase, new Appointment
            {
                CaseId = aCase.Id,
                AppointmentDateTime = DateTime.Now,
                Title = "Test Appointment 1"
            });

            await _appointment.AddAppointmentToCaseAsync(aCase, new Appointment
            {
                CaseId = aCase.Id,
                AppointmentDateTime = DateTime.Today,
                Title = "Test Appointment 2"
            });

            // RETRIEVE case via client
            Console.WriteLine("Retrieving data...");
            client = await _user.GetUserByRowIdAsync(client.Id);
            foreach (Case c in client.Cases)
            {
                foreach (Appointment a in c.Appointments)
                {
                    Console.WriteLine("Appointment for " + c.Title + ": " + a.AppointmentDateTime);
                }
            }

            // UPDATE first appointment to be for tomorrow
            Console.WriteLine("Updating an appointment...");
            client.Cases[0].Appointments[0].AppointmentDateTime = client.Cases[0].Appointments[0].AppointmentDateTime.AddDays(1);
            await _appointment.UpdateAppointmentAsync(client.Cases[0].Appointments[0]);

            // DELETE appointment
            Console.WriteLine("Deleting an appointment...");
            await _appointment.DeleteAppointmentFromCaseAsync(client.Cases[0], client.Cases[0].Appointments[1]);

            // RETRIEVE case via client a second time to see update
            Console.WriteLine("Retrieving data again...");
            client = await _user.GetUserByIdAsync(client.UserId);
            foreach (Case c in client.Cases)
            {
                foreach (Appointment a in c.Appointments)
                {
                    Console.WriteLine("Appointment for " + c.Title + ": " + a.AppointmentDateTime);
                }
            }

            // CLEANUP
            Console.WriteLine("Deleting entries...");
            await _user.DeleteUserAsync(client);
            await _case.DeleteCaseAsync(aCase);
            await _user.DeleteUserAsync(consultant);

            client = await _user.GetUserByIdAsync(client.UserId);
            aCase = await _case.GetCaseByIdAsync(aCase.Id);
            consultant = await _user.GetUserByIdAsync(consultant.UserId);

            Console.WriteLine("Deletion: " + ((client == null && aCase == null && consultant == null) ? "Success" : "Failure"));
            Console.WriteLine("TEST END.");
            Console.WriteLine();
            Console.WriteLine();
        }

        static async Task RunCaseWithNotesTest(IUserRepository _user, ICaseRepository _case, 
            ICaseStatusRepository _caseStatus, INoteRepository _note, IRoleRepository _role)
        {
            Console.WriteLine("TEST BEGIN.");
            Console.WriteLine("Beginning note test...");

            // SET UP
            Console.WriteLine("Setting up test data...");
            Status unassigned = await _caseStatus.GetCaseStatusByTextAsync("Unassigned");
            Role roleClient = await _role.GetRoleByText("Client");
            Role roleConsultant = await _role.GetRoleByText("Consultant");

            // SETUP
            User consultant = new User
            {
                UserId = "testid_consultant",
                Role = roleConsultant
            };
            consultant = await _user.AddUserAsync(consultant);

            User client = new User
            {
                UserId = "testid_client",
                Role = roleClient
            };
            client = await _user.AddUserAsync(client);

            Case aCase = new Case
            {
                Title = "Test Case",
                ActiveConsultant = consultant,
                Clients = new List<User> { client },
                Status = unassigned
            };
            aCase = await _case.AddCaseAsync(aCase);

            // CREATE new notes and add them to the case
            Console.WriteLine("Creating notes...");
            await _note.AddNoteToCaseAsync(aCase, new Note
            {
                CaseId = aCase.Id,
                Content = "This case is a test case, and this is a test note."
            });
            await _note.AddNoteToCaseAsync(aCase, new Note
            {
                CaseId = aCase.Id,
                Content = "Another test note."
            });

            // RETRIEVE the user with the case to get the client
            Console.WriteLine("Retrieving notes...");
            client = await _user.GetUserByRowIdAsync(client.Id);
            foreach (Case c in client.Cases)
            {
                foreach (Note cn in c.Notes)
                {
                    Console.WriteLine("Note: " + cn.Content);
                }
            }

            // UPDATE a note
            Console.WriteLine("Updating a note...");
            Note note = client.Cases[0].Notes[0];
            note.Content = "Updated content.";
            await _note.UpdateNoteAsync(note);

            // DELETE a note
            Console.WriteLine("Deleting  note...");
            await _note.DeleteNoteFromCaseAsync(client.Cases[0], client.Cases[0].Notes[1]);

            // RETRIEVE again to see the changes
            Console.WriteLine("Retrieving notes again...");
            client = await _user.GetUserByRowIdAsync(client.Id);
            foreach (Case c in client.Cases)
            {
                foreach (Note cn in c.Notes)
                {
                    Console.WriteLine("Note: " + cn.Content);
                }
            }

            // CLEAN UP
            Console.WriteLine("Deleting entries...");
            await _user.DeleteUserAsync(client);
            await _case.DeleteCaseAsync(aCase);
            await _user.DeleteUserAsync(consultant);

            consultant = await _user.GetUserByIdAsync(consultant.UserId);
            client = await _user.GetUserByIdAsync(client.UserId);
            aCase = await _case.GetCaseByIdAsync(aCase.Id);

            Console.WriteLine("Deletion: " + ((consultant == null && client == null && aCase == null) ? "Success" : "Failure"));
            Console.WriteLine("TEST END.");
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
