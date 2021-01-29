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
        public static string OrderStarted = @"
<!DOCTYPE html>
<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta charset = ""utf-8"" />
    <title>Pedido</title>
    <style type = ""text/css"" >
        .aluguru-body {{
            max-width: 600px;            
            margin: 0px auto;
            font-family: arial;
        }}
        .aluguru-button {{
            display: block;
            width: 100px;
            margin: 0px auto;
            text-decoration: none;
            border-radius: 5px;
            padding: 10px 20px;
            color: white !important;
            background-color: #ed2a6d;
            font-weight: bold;
        }}
        .aluguru-image {{
            display: block;
            margin-left: auto;
            margin-right: auto;
            margin-top: 20px;
            margin-bottom: 20px;
        }}
        .aluguru-box {{
            display: block;
            color: white !important;
            background-color: #ed2a6d;
            text-align: center;
            padding: 5px 20px;
            margin-top: 20px;
            margin-bottom: 20px;
            border-radius: 5px;
        }}
        .aluguru-nav-item {{
            display: inline-block;
            text-align: center;
        }}
    </style>
</head>
    <body>
        <div class=aluguru-body>
            <img class=""aluguru-image"" src=""https://spdevblobstorage.blob.core.windows.net/mail/Logo%20Horizontal%20Colorido.png"" width=""200px""/>
            <span class=""aluguru-box""><h2>Recebemos seu pedido!</h2></span>
            <p style=""margin-top: 60px;"">
                Oi, <strong>{0}</strong>.
            </p>
            <p>
                Recebemos o seu pedido código <strong>{1}</strong>
            </p>
            <p>
                Assim que o pagamento for confirmado a gente te avisa.
            </p>
            <img class=""aluguru-image"" src=""https://spdevblobstorage.blob.core.windows.net/email/banner-cashback-5.png"" width=""600px"" style=""border-radius: 5px; margin-top:60px;""/>
            <p style=""margin-top: 60px;""><strong>O que eu faço agora?</strong></p>
            <ul>
                <li>Pagou cartão de crédito? Então é só aguardar a confirmação de pagamento!</li>
                <br>
                <li>Caso tenha selecionado boleto, logo em seguida você receberá um outro<br>e-mail contendo o boleto para você realizar o pagamento.</li>
            </ul>
            <p>Qualquer outra dúvida, entre em contato com a gente.</p>
            <p>WhatsApp: <strong>55 51 993253223</strong></p>
            <div style=""text-align: center; margin-top:60px;"">
                <strong>Segue a gente ;)</strong>
                <nav>
                    <a class=""aluguru-nav-item"" href=""https://www.instagram.com/aluguru.com.br""><img class=""aluguru-image"" src=""https://spdevblobstorage.blob.core.windows.net/email/instagram.png"" width=""50px""/></a>
                    <a class=""aluguru-nav-item"" href=""https://www.facebook.com/aluguru.com.br""><img class=""aluguru-image"" src=""https://spdevblobstorage.blob.core.windows.net/email/facebook.png"" width=""50px""/></a>
                    <a class=""aluguru-nav-item"" href=""https://api.whatsapp.com/send?phone=5551993253223&text=Ol%C3%A1!%20Gostaria%20de%20falar%20com%20algum%20atendente%20da%20Aluguru""><img class=""aluguru-image"" src=""https://spdevblobstorage.blob.core.windows.net/email/whatsapp.png"" width=""50px""/></a>
                    <a class=""aluguru-nav-item"" href=""https://www.linkedin.com/company/69699371/admin""><img class=""aluguru-image"" src=""https://spdevblobstorage.blob.core.windows.net/email/linkedin.png"" width=""50px""/></a>
                </nav>
            </div>
        </div>
    </body>
</html>";
        public static string PaymentConfirmed = @"
<!DOCTYPE html>
<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta charset = ""utf-8"" />
    <title>Pagamento</title>
    <style type = ""text/css"" >
        .aluguru-body {{
            max-width: 600px;            
            margin: 0px auto;
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
        .aluguru-image {{
            display: block;
            margin-left: auto;
            margin-right: auto;
            margin-top: 20px;
            margin-bottom: 20px;
        }}
        .aluguru-box {{
            display: block;
            color: white !important;
            background-color: #ed2a6d;
            text-align: center;
            padding: 5px 20px;
            margin-top: 20px;
            margin-bottom: 20px;
            border-radius: 5px;
        }}
        .aluguru-nav-item {{
            display: inline-block;
            text-align: center;
        }}
    </style>
</head>
    <body>
        <div class=aluguru-body>
            <img class=""aluguru-image"" src=https://spdevblobstorage.blob.core.windows.net/mail/Logo%20Horizontal%20Colorido.png width=""200px""/>
            <span class=""aluguru-box""><h2>Pagamento confirmado!</h2></span>
            <p style=""margin-top: 60px;"">
                Oi, <strong>{0}</strong>.
            </p>
            <p>
                Confirmamos o pagamento do seu pedido código <strong>{1}</strong>
            </p>
            <p>
                Já estamos preparando tudo para poder lhe entregar.
            </p>
            <div class=""aluguru-box"" style=""margin-top: 60px;"">
                <h2>Temos uma surpresinha pra você!</h2>
                <p>Nos próximos dias você receberá um código único no valor de 5% do seu pedido para você utilizar na sua próxima locação na Aluguru!</p>
            </div>
            <p style=""margin-top: 60px;""><strong>O que eu faço agora?</strong></p>
            <p>Basta aguardar o recebimento do seu pedido na data agendada.</p>
            <p>Qualquer outra dúvida, entre em contato com a gente.</p>
            <p>WhatsApp: <strong>55 51 993253223</strong></p>
            <div style=""text-align: center; margin-top:60px;"">
                <strong>Segue a gente ;)</strong>
                <nav>
                    <a class=""aluguru-nav-item"" href=""https://www.instagram.com/aluguru.com.br""><img class=""aluguru-image"" src=""https://spdevblobstorage.blob.core.windows.net/email/instagram.png"" width=""50px""/></a>
                    <a class=""aluguru-nav-item"" href=""https://www.facebook.com/aluguru.com.br""><img class=""aluguru-image"" src=""https://spdevblobstorage.blob.core.windows.net/email/facebook.png"" width=""50px""/></a>
                    <a class=""aluguru-nav-item"" href=""https://api.whatsapp.com/send?phone=5551993253223&text=Ol%C3%A1!%20Gostaria%20de%20falar%20com%20algum%20atendente%20da%20Aluguru""><img class=""aluguru-image"" src=""https://spdevblobstorage.blob.core.windows.net/email/whatsapp.png"" width=""50px""/></a>
                    <a class=""aluguru-nav-item"" href=""https://www.linkedin.com/company/69699371/admin""><img class=""aluguru-image"" src=""https://spdevblobstorage.blob.core.windows.net/email/linkedin.png"" width=""50px""/></a>
                </nav>
            </div>
        </div>
    </body>
</html>";
    }
}
