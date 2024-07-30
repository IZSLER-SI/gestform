Imports System.Web.Routing
Imports Microsoft.AspNet.FriendlyUrls

Public Module RouteConfig
    Sub RegisterRoutes(ByVal routes As RouteCollection)

        Dim basePathCompany As String
        Dim basePathPages As String

        routes.Clear()
        routes.RouteExistingFiles = False

        'path base
        basePathCompany = "~/Content/" & My.Settings.CompanyKey & "/"
        basePathPages = "~/Pages/"

        'pagine

        'Home Page
        routes.MapPageRoute("Home", "", basePathPages & "Home.aspx")

        'pagine statiche
        routes.MapPageRoute("NoteLegali", "NoteLegali", basePathPages & "GenericContent.aspx")
        routes.MapPageRoute("Privacy", "Privacy", basePathPages & "GenericContent.aspx")

        'news
        routes.MapPageRoute("News", "news", basePathPages & "News.aspx")
        routes.MapPageRoute("NewsDetail", "news/{id}", basePathPages & "News.aspx")
        'download
        routes.MapPageRoute("Download", "download", basePathPages & "Download.aspx")

        'completamento profilo
        routes.MapPageRoute("CompletamentoProfilo", "completa-profilo", basePathPages & "CompleteProfile.aspx")

        'registrazione
        routes.MapPageRoute("Registrazione", "registrazione", basePathPages & "Registration.aspx")

        'reimpostazione password (inizio)
        routes.MapPageRoute("ReimpostazionePassword", "password-smarrita", basePathPages & "PasswordReset.aspx")

        'reimpostazione password (inizio)
        routes.MapPageRoute("DoReimpostazionePassword", "reset-password", basePathPages & "DoPasswordReset.aspx")

        'cambio e-mail
        routes.MapPageRoute("CambioMail", "cambio-mail", basePathPages & "ChangeMail.aspx")

        'cambio password
        routes.MapPageRoute("CambioPassword", "cambio-password", basePathPages & "ChangePassword.aspx")

        'modifica profilo
        routes.MapPageRoute("ModificaProfilo", "modifica-profilo", basePathPages & "EditProfile.aspx")

        'pre-login
        'Validazione pre-login per check completezza profilo
        routes.MapPageRoute("ModificaProfiloPreLogin", "modifica-profilo-pre-login", basePathPages & "ControlloPreLogin.aspx")

        'lista eventi
        routes.MapPageRoute("EventList", "eventi", basePathPages & "Events.aspx")
        routes.MapPageRoute("EventListFiltred", "eventit/{filtertype}", basePathPages & "Events.aspx")

        'iscrizioni attive
        routes.MapPageRoute("IscrizioniAttive", "iscrizioni-attive", basePathPages & "IscrizioniAttive.aspx")

        'portfolio
        routes.MapPageRoute("Portfolio", "portfolio", basePathPages & "Portfolio.aspx")

        'portfolio Excel
        routes.MapPageRoute("PortfolioExcel", "portfolio-excel", basePathPages & "PortfolioExcel.aspx")

        'formazione individuale
        routes.MapPageRoute("FormazioneIndividuale", "formazione-individuale", basePathPages & "FormazioneIndividuale.aspx")

        'scheda evento
        routes.MapPageRoute("EventDetail", "eventi/{id}", basePathPages & "EventDetail.aspx")

        'registrazione presenze
        routes.MapPageRoute("RegistrazionePresenze", "inserimento-presenze/{id}", basePathPages & "InserimentoPresenze.aspx")

        'attestato ECM
        routes.MapPageRoute("AttestatoEcm", "attestato-ecm/{id}", basePathPages & "AttestatoECM.aspx")

        'attestato partecipazione
        routes.MapPageRoute("AttestatoPartecipazione", "attestato-partecipazione/{id}", basePathPages & "AttestatoPART.aspx")

        'Stampa Autocertificazione
        routes.MapPageRoute("StampaAutoCertificazione", "stampa-auto-certificazione/{id}", basePathPages & "StampaAutoCertificazione.aspx")

        'richiesta validazione eventi esterni PG
        routes.MapPageRoute("validazionePG", "validazione-pg", basePathPages & "ValidazioneEsternaPG.aspx")

        'modulo fuori sede
        routes.MapPageRoute("FormazioneEsterna", "formazione-esterna", basePathPages & "FormazioneEsterna.aspx")
    End Sub
End Module
