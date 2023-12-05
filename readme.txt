##My first Full Stack Practice
##Technologies: 
* ASP.NET (Core) - .NET 7
* React - Typescript 
* SQL Server 

#APITodo
First API. It doesn't follow any structure nor architecture.

#APITodoOnion
Improved API. It follows ONION architecture plus CQRS architecture. And some more conventions.

#UITodo
React Front-ent app

##Interesting Libraries used
Backend
- **JWT Auth**: JWT Token / Refresh Token based authentication
- **.NET Identity**: User management using .NET Identity
- **Entity Framework Core**: ORM
- **Unit Of Work and Repository pattern**: Aside the included by EF Core with DbContext (Or IdentityDbContext)
- **Swagger configurations**: With Swashbuckle for documentation.
- **FluentValidator for HTTP request**: Taking advantage of CQRS architecture (With MediaTR); I've added fluentvalidator for Backend validations

Frontend
- **Axios**: For API consuming 
- **Tanstack Query**: For Mutations and cache management
- **Optimistic Updates (Query)**: For a better user UI experience
- **React Hook Forms**: For forms design and behavior
- **Zod**: For forms validations
- **Tailwind and Personalized tailwind classes**: For UI Design. Personalized theming options such as fonts and colors.
- **React Router Dom**: For routing
- **Context API**: For global states such as Auth Context
- **Universal Cookies**: For JWT Token/ Refresh Token storage
- **Auto refreshing session**: Using cookies, Context and Axios interceptors, I've coded an "auto refreshing token" system
- **Dark and Light Mode**: Depending on user preference








