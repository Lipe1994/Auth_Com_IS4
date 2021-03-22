# Trata-se de 2 projetos, um servidor de oauth e um client para testar o IS4


# Como criei o projeto com IS4:

1 - crie o projeto dotnet mvc

    dotnet new mvc
    
2 - adicione os templates do is4

    dotnet new -i identityserver4.templates
    
3 - instale no projeto criado o template do is4

    dotnet new is4admin --force
    
4 - remova a controller inicial do projeto de sample. controllers/HomeController.cs
