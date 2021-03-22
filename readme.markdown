# Como criei este projeto=>

1 - crie o projeto dotnet mvc

    dotnet new mvc
    
2 - adicione os templates do is4

    dotnet new -i identityserver4.templates
    
3 - instale no projeto criado o template do is4

    dotnet new is4admin --forcedotnet new -f is4admindotnet new -f is4admin
    
4 - remova a controller inicial do projeto de sample. controllers/HomeController.cs
