using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assignment2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Assignment2.Data
{
    public class DbInitializer
    {
        public static void Initialize(LOContext context, IServiceProvider serviceProvider)
        {
            context.Database.EnsureCreated();

            if (context.Courses.Any()) return;


            var courses = new Course[]
            {

            new Course{name="Web Software Architecture", metricPercent=76, number=4540, department="Computer Science", description="Software architectures, programming models, and programming environments pertinent to developing web applications.  Topics include client-server model, multi-tier software architecture, client-side scripting (JavaScript), server-side programming (Servlets and JavaServer Pages), component reuse (JavaBeans), database connectivity (JDBC), and web servers.", professor="Jim de St Germain",professorEmail="professor_jim@cs.utah.edu", semeseter="Fall", year=2019 },
            new Course{name="Introduction to Algorithms & Data Structures", metricPercent=57, number=2420, department="Computer Science", description="This course provides an introduction to the problem of engineering computational efficiency into programs. Students will learn about classical algorithms (including sorting, searching, and graph traversal), data structures (including stacks, queues, linked lists, trees, hash tables, and graphs), and analysis of program space and time requirements. Students will complete extensive programming exercises that require the application of elementary techniques from software engineering.", professor="Jim de St Germain", professorEmail="professor_jim@cs.utah.edu", semeseter="Summer", year=2018},
            new Course{name ="Software Practice", metricPercent=88, number=3500, department="Computer Science", description="Practical exposure to the process of creating large software systems, including requirements specifications, design, implementation, testing, and maintenance. Emphasis on software process, software tools (debuggers, profilers, source code repositories, test harnesses), software engineering techniques (time management, code, and documentation standards, source code management, object-oriented analysis and design), and team development practice. Much of the work will be in groups and will involve modifying preexisting software systems", professor="Jim de St Germain", professorEmail="professor_jim@cs.utah.edu", semeseter="Spring", year=2018},
            new Course{name="Discrete Structures", metricPercent=100, number=2100, department="Computer Science", description="Introduction to propositional logic, predicate logic, formal logical arguments, finite sets, functions, relations, inductive proofs, recurrence relations, graphs, probability, and their applications to Computer Science.", professor="Mary", professorEmail="professor_mary@cs.utah.edu", semeseter="Spring", year=2018},
            new Course{name ="Computer Systems", metricPercent=65, number=4400, department="Computer Science", description="Introduction to computer systems from a programmer's point of view.  Machine level representations of programs, optimizing program performance, memory hierarchy, linking, exceptional control flow, measuring program performance, virtual memory, concurrent programming with threads, network programming.", professor="Mary", professorEmail="professor_mary@cs.utah.edu", semeseter="Fall", year=2019},
            new Course{name ="Software Practice", metricPercent=75, number=3500, department="Computer Science", description="Practical exposure to the process of creating large software systems, including requirements specifications, design, implementation, testing, and maintenance. Emphasis on software process, software tools (debuggers, profilers, source code repositories, test harnesses), software engineering techniques (time management, code, and documentation standards, source code management, object-oriented analysis and design), and team development practice. Much of the work will be in groups and will involve modifying preexisting software systems", professor="Danny", professorEmail="professor_danny@cs.utah.edu", semeseter="Spring", year=2019},

            };
            foreach (Course c in courses)
            {
                context.Courses.Add(c);
            }
            context.SaveChanges();

            var los = new LearningOutcome[]
            {
                new LearningOutcome{name="Symbolic Logic", CourseNum=2100, description="use symbolic logic to model real-world situations by converting informal language statements to propositional and predicate logic expressions, as well as apply formal methods to propositions and predicates (such as computing normal forms and calculating validity)"},
                new LearningOutcome{name="Analysis", CourseNum=2100, description="analyze problems to determine underlying recurrence relations, as well as solve such relations by rephrasing as closed formulas"},
                new LearningOutcome{name="Examples", CourseNum=2100, description="assign practical examples to the appropriate set, function, or relation model, while employing the associated terminology and operations"},
                new LearningOutcome{name="Map to Real World", CourseNum=2100, description="map real-world applications to appropriate counting formalisms, including permutations and combinations of sets, as well as exercise the rules of combinatorics (such as sums, products, and inclusion-exclusion)"},
                new LearningOutcome{name="Probabilities", CourseNum=2100, description="calculate probabilities of independent and dependent events, in addition to expectations of random variables"},
                new LearningOutcome{name="Graph Theory", CourseNum=2100, description="illustrate by example the basic terminology of graph theory, as wells as properties and special cases (such as Eulerian graphs, spanning trees, isomorphism, and planarity)"},
                new LearningOutcome{name="Proofs", CourseNum=2100, description="employ formal proof techniques (such as direct proof, proof by contradiction, induction, and the pigeonhole principle) to construct sound arguments about properties of numbers, sets, functions, relations, and graphs"},

                new LearningOutcome{name="Fundamental Data Structures", CourseNum=2420, description="implement, and analyze for efficiency, fundamental data structures (including lists, graphs, and trees) and algorithms (including searching, sorting, and hashing)"},
                new LearningOutcome{name="Big-O", CourseNum=2420, description="employ Big-O notation to describe and compare the asymptotic complexity of algorithms, as well as perform empirical studies to validate hypotheses about running time"},
                new LearningOutcome{name="Abstract Types", CourseNum=2420, description="recognize and describe common applications of abstract data types (including stacks, queues, priority queues, sets, and maps)"},
                new LearningOutcome{name="Application", CourseNum=2420, description="apply algorithmic solutions to real-world data"},
                new LearningOutcome{name="Generics", CourseNum=2420, description="use generics to abstract over functions that differ only in their types"},
                new LearningOutcome{name="Appreciation", CourseNum=2420, description="appreciate the collaborative nature of computer science by discussing the benefits of pair programming"},

                new LearningOutcome{name="Software Systems", CourseNum=3500, description="design and implement large and complex software systems (including concurrent software) through the use of process models (such as waterfall and agile), libraries (both standard and custom), and modern software development tools (such as debuggers, profilers, and revision control systems)"},
                new LearningOutcome{name="Validation and Error Handling", CourseNum=3500, description="perform input validation and error handling, as well as employ advanced testing principles and tools to systematically evaluate software"},
                new LearningOutcome{name="MVC", CourseNum=3500, description="apply the model-view-controller pattern and event handling fundamentals to create a graphical user interface"},
                new LearningOutcome{name="APIs", CourseNum=3500, description="exercise the client-server model and high-level networking APIs to build a web-based software system"},
                new LearningOutcome{name="Database", CourseNum=3500, description="operate a modern relational database to define relations, as well as store and retrieve data"},
                new LearningOutcome{name="Peer Reviews", CourseNum=3500, description="appreciate the collaborative nature of software development by discussing the benefits of peer code reviews"},

                new LearningOutcome{name="Abstraction", CourseNum=4400, description="explain the objectives and functions of abstraction layers in modern computing systems, including operating systems, programming languages, compilers, and applications"},
                new LearningOutcome{name="Cross Communication", CourseNum=4400, description="understand cross-layer communications and how each layer of abstraction is implemented in the next layer of abstraction (such as how C programs are translated into assembly code and how C library allocators are implemented in terms of operating system memory management)"},
                new LearningOutcome{name="Performance", CourseNum=4400, description="analyze how the performance characteristics of one layer of abstraction affect the layers above it (such as how caching and services of the operating system affect the performance of C programs)"},
                new LearningOutcome{name="Construct Apps", CourseNum=4400, description="construct applications using operating-system concepts (such as processes, threads, signals, virtual memory, I/O)"},
                new LearningOutcome{name="Concurrency", CourseNum=4400, description="synthesize operating-system and networking facilities to build concurrent, communicating applications"},
                new LearningOutcome{name="Parallel Programming", CourseNum=4400, description="implement reliable concurrent and parallel programs using appropriate synchronization constructs"},

                new LearningOutcome{name="HTML/CSS", CourseNum=4540, description="Construct web pages using modern HTML and CSS practices, including modern frameworks"},
                new LearningOutcome{name="Accessibility", CourseNum=4540, description="Define accessibility and utilize techniques to create accessible web pages.  "},
                new LearningOutcome{name="MVC", CourseNum=4540, description="Outline and utilize MVC technologies across the “full-stack” of web design (including front-end, back-end, and databases) to create interesting web applications. Deploy an application to a “Cloud” provider."},
                new LearningOutcome{name="Security", CourseNum=4540, description="Describe the browser security model and HTTP; utilize techniques for data validation, secure session communication, cookies, single sign-on, and separate roles.  "},
                new LearningOutcome{name="Javascript", CourseNum=4540, description="Utilize JavaScript for simple page manipulations and AJAX for more complex/complete “single-page” applications."},
                new LearningOutcome{name="Responsive", CourseNum=4540, description="Demonstrate how responsive techniques can be used to construct applications that are usable at a variety of page sizes.  Define and discuss responsive, reactive, and adaptive."},
                new LearningOutcome{name="Web Crawler", CourseNum=4540, description="Construct a simple web-crawler for validation of page functionality and data scraping."}
            };
            foreach (LearningOutcome lo in los)
            {
                context.LearningOutcomes.Add(lo);
            }
            context.SaveChanges();

            var cnotes = new CourseNote[]
            {
                new CourseNote{note="This course has the best learning outcomes", status="pending", CourseNum=4540}
            };
            foreach(CourseNote cn in cnotes)
            {
                context.CourseNote.Add(cn);
            }
            context.SaveChanges();

            var lnotes = new LearningOutcomeNote[]
            {
                new LearningOutcomeNote{note="This learning outcome is complete", lastEditChair=false, LOId=1}
            };
            foreach(LearningOutcomeNote ln in lnotes)
            {
                context.LearningOutcomeNote.Add(ln);
            }
            context.SaveChanges();
        }

        public static async Task InitializeUsers(UserRolesDB udb, IServiceProvider serviceProvider)
        {
            udb.Database.EnsureCreated();
            if (udb.Users.Any()) return;
            //seed user roles to db https://forums.asp.net/t/2151809.aspx?MVC+Core+2+1+Seeding+Users+Roles+Issue
            string[] roles = new string[] { "Instructor", "Admin", "Chair" };
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            IdentityResult roleRes;
            foreach (string r in roles)
            {
                roleRes = await RoleManager.CreateAsync(new IdentityRole(r));
            }

            var admins = new IdentityUser[]
            {
                new IdentityUser{UserName="admin_erin@cs.utah.edu", PasswordHash="123ABC!@#def", EmailConfirmed=true}
            };

            IdentityResult res;
            foreach(IdentityUser i in admins)
            {
                res = UserManager.CreateAsync(i, i.PasswordHash).Result;
                if (res.Succeeded) UserManager.AddToRoleAsync(i, "Admin").Wait();
            }

            var instructors = new IdentityUser[]
            {
                new IdentityUser{UserName="professor_jim@cs.utah.edu", PasswordHash="123ABC!@#def", EmailConfirmed=true},
                new IdentityUser{UserName="professor_mary@cs.utah.edu", PasswordHash="123ABC!@#def", EmailConfirmed=true},
                new IdentityUser{UserName="professor_danny@cs.utah.edu", PasswordHash="123ABC!@#def", EmailConfirmed=true}
            };

            IdentityResult res2;
            foreach (IdentityUser i in instructors)
            {
                res2 = UserManager.CreateAsync(i, i.PasswordHash).Result;
                if (res2.Succeeded) UserManager.AddToRoleAsync(i, "Instructor").Wait();
            }

            var chairs = new IdentityUser[]
            {
                new IdentityUser{UserName="chair_whitaker@cs.utah.edu", PasswordHash="123ABC!@#def", EmailConfirmed=true}
            };

            IdentityResult res3;
            foreach (IdentityUser i in chairs)
            {
                res3 = UserManager.CreateAsync(i, i.PasswordHash).Result;
                if (res3.Succeeded) UserManager.AddToRoleAsync(i, "Chair").Wait();
            }

            return;
        }
    }
}
