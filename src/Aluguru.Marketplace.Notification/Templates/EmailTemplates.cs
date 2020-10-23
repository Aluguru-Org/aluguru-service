namespace Aluguru.Marketplace.Notification.Templates
{
    public static class EmailTemplates
    {
        public static string RegisterUser = @"
<!DOCTYPE html>
<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta charset = ""utf-8"" />
    <title> Bem vindo</title>
    <style type = ""text/css"" >
        .aluguru-body {{
            max-width: 400px;            
            margin: 0px auto;
            text-align: center;
            font-family: arial;
        }}
        .aluguru-button {{
            display: block;
            width: 100px;
            margin: 50px auto;
            text-decoration: none;
            border-radius: 5px;
            padding: 10px 20px;
            color: white !important;
            background-color: #ed2a6d;
            font-weight: bold;
        }}
    </style>
</head>
    <body>
        <div class=""aluguru-body"">
            <img src=""https://spdevblobstorage.blob.core.windows.net/mail/Logo%20Horizontal%20Colorido.png"" width=""400px""/>
            <p>
                <strong>{0}</strong>, bem vindo à Aluguru.
            </p>
            <p>
                Nossa missão é tornar sua vida muito mais simples e economica através do aluguel de produtos online!
            </p>
            <p>
                Clique logo abaixo para ativar a sua conta.        
            </p>
            <a class=""aluguru-button"" href=""{1}"">Ativar conta</a>
        </div>
    </body>
</html>
";
    }
}
