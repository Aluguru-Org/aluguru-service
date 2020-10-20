namespace Aluguru.Marketplace.Register.Templates
{
    public static class EmailTemplates
    {
        public static string RegisterUser = @"
<!DOCTYPE html>

<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta charset = ""utf-8"" />
    < title > Bem vindo</title>
</head>
<body>
    <p>
        Olá {0},
    </p>
    <p>
        Obrigado por se registrar na<a href=""https://aluguru.com""> Aluguru</a>!
    </p>
    <p>
        Clique<a href=""{1}"">aqui</a> para ativar a sua conta.
    </p>
</body>
</html>
";
    }
}
