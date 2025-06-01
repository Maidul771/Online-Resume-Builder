
# Online Resume Builder

Online-Resume-Builder is a user-friendly web application that allows users to easily create, customize, and download professional resumes. With real-time editing, and export options (such as PDF), it streamlines the resume-building process for job seekers, students, and professionals.



##  Features


- ✅ Add/Edit Personal Information (name, contact, address)
- ✅ Add Objective/Summary
- ✅ Add Technical and Soft Skills
- ✅ Add Work Experience
- ✅ Add Projects with descriptions and technologies used
- ✅ Add Educational Qualifications
- ✅ Add Certifications
- ✅ Add References
- ✅ Resume Preview in browser
- ✅ **Download resume as PDF (QuestPDF)**
- ✅ Printable resume format
- ✅ Responsive UI with Bootstrap
- ✅ Dynamic form sections using jQuery


## Technologies Used

- **Frontend**: HTML5, Bootstrap, jQuery
- **Backend**: ASP.NET Core MVC (C#)
- **ORM**: Entity Framework Core
- **Database**: SQL Server
- **PDF Generation**: QuestPDF
- **IDE**: Visual Studio 2022 


 ## Project Structure
 
 Online Resume Builder



Controllers/         # Handles request logic for each resume section (e.g., Skills, Experience)

Models/              # C# classes representing resume data (e.g., Education, Project)

Views/               # Razor views for each section (Create, Edit, List, etc.)

Migrations/          # Entity Framework Core database migration files

Services/            # Custom services (e.g., PdfGenerator using QuestPDF)

wwwroot/             # Static content root (CSS, JS, images, Bootstrap assets)

   ├── css/          # Custom or theme CSS styles

   ├── js/           # JavaScript and jQuery scripts

   └── lib/          # External libraries (Bootstrap, jQuery via LibMan)

appsettings.json     # App configuration file (DB connection string, logging, etc.)

Program.cs           # Entry point of the application

Startup.cs           # Configures middleware, services, routing, EF, etc.

OnlineResumeBuilder.sln # Visual Studio solution file

## Screenshots
![Preview](https://github.com/Maidul771/Online-Resume-Builder/blob/ea714a05496d18b3956e4f25d03bf9303c3bb673/Preview.png)

![Add Objective](https://github.com/Maidul771/Online-Resume-Builder/blob/cfd5da912d507c9a04573a5a199aa203ccbab6a2/Add%20Objective.png)

![Skills](https://github.com/Maidul771/Online-Resume-Builder/blob/cfd5da912d507c9a04573a5a199aa203ccbab6a2/Skills.png)

![Add Skills](https://github.com/Maidul771/Online-Resume-Builder/blob/cfd5da912d507c9a04573a5a199aa203ccbab6a2/Add%20Skills.png)

![Add Experience](https://github.com/Maidul771/Online-Resume-Builder/blob/cfd5da912d507c9a04573a5a199aa203ccbab6a2/Add%20Experience.png)

![Project](https://github.com/Maidul771/Online-Resume-Builder/blob/cfd5da912d507c9a04573a5a199aa203ccbab6a2/Projects.png)

![Add Project](https://github.com/Maidul771/Online-Resume-Builder/blob/cfd5da912d507c9a04573a5a199aa203ccbab6a2/Add%20Projects.png)

![Education](https://github.com/Maidul771/Online-Resume-Builder/blob/cfd5da912d507c9a04573a5a199aa203ccbab6a2/Education.png)

![Add Education](https://github.com/Maidul771/Online-Resume-Builder/blob/cfd5da912d507c9a04573a5a199aa203ccbab6a2/Add%20Educations.png)

![Add Certification](https://github.com/Maidul771/Online-Resume-Builder/blob/cfd5da912d507c9a04573a5a199aa203ccbab6a2/Add%20Certifiactions.png)

![Add Reference](https://github.com/Maidul771/Online-Resume-Builder/blob/cfd5da912d507c9a04573a5a199aa203ccbab6a2/Add%20Reference.png)

![Database](https://github.com/Maidul771/Online-Resume-Builder/blob/cfd5da912d507c9a04573a5a199aa203ccbab6a2/Database.png)

