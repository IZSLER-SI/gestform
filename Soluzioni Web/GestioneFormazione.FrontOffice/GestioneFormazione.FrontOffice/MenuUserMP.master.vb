Imports System.IO
Imports System.Net
Imports System.Threading.Tasks

Public Class MenuUserMP
    Inherits System.Web.UI.MasterPage

    'path di base
    Dim baseTemplates As String

    'controlli dinamici
    Protected WithEvents lnkLogout As LinkButton

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        'path di base templates (relativo)
        baseTemplates = "~/Templates/" & My.Settings.CompanyKey & "/"

        'menu a sinistra
        GeneraNavLeft()

        'menu a destra
        GeneraNavRight()

        'labels login
        lblCompanyName_1.Text = My.Settings.CompanyName_Short
        lblCompanyName_2.Text = My.Settings.CompanyName_Short



    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub




    Private Sub GeneraNavLeft()

        phdNavLeft.Controls.Clear()
        Transformer.Transform(New XmlDocument,
                                                            baseTemplates & "MainMenu.xslt",
                                                            phdNavLeft,
                                                            "region", ContextHandler.RegionString,
                                                            "dipendente", ContextHandler.fl_DIPENDENTE01,
                                                            "nodekey", CType(Me.Page, IFOPage).GetMainMenuNodeKey)

    End Sub

    Private Sub GeneraNavRight()

        Dim sAspx As String
        Dim cCreato As Control

        sAspx = Transformer.Transform(New XmlDocument,
                              baseTemplates & "UserZone.xslt",
                              "region", ContextHandler.RegionString,
                              "nodekey", CType(Me.Page, IFOPage).GetMainMenuNodeKey,
                              "titolo", ContextHandler.tx_TITOLO,
                              "cognome", ContextHandler.tx_COGNOME,
                              "nome", ContextHandler.tx_NOME,
                              "matricola", ContextHandler.ac_MATRICOLA,
                              "email", ContextHandler.tx_EMAIL)

        Softailor.Global.AspxCleaner.CleanAspx(sAspx)
        cCreato = Page.ParseControl(sAspx)

        phdNavRight.Controls.Clear()
        phdNavRight.Controls.Add(cCreato)

        'aggancio link logout
        lnkLogout = CType(cCreato.FindControl("lnkLogout"), LinkButton)

    End Sub

    Private Sub login_lnk_login_Click(sender As Object, e As EventArgs) Handles login_lnk_login.Click

        'region sbagliata > esco
        If ContextHandler.Region <> ContextHandler.Regions.LoggedOut Then Exit Sub

        Dim valid As Boolean

        'pulizie
        login_txt_username.BackColor = Drawing.Color.Empty
        login_txt_password.BackColor = Drawing.Color.Empty

        'presenza campi
        valid = True
        If login_txt_username.Text.Trim = "" Then
            login_txt_username.BackColor = Drawing.Color.Yellow
            valid = False
        End If

        If login_txt_password.Text.Trim = "" Then
            login_txt_password.BackColor = Drawing.Color.Yellow
            valid = False
        End If

        If Not valid Then
            login_lbl_error.Text = "Inserisci username e password."
            Exit Sub
        End If

        'Validazione pre-login per check completezza profilo
        Dim result = ContextHandler.ControlloPreLogin(CType(Me.Master, RootMP).GetFpd.dbConn, login_txt_username.Text, login_txt_password.Text)
        Select Case result
            Case ContextHandler.TryLoginResult.DipendenteProfiloIncompleto
                Response.Redirect("/modifica-profilo-pre-login", True)
                Exit Sub
            Case ContextHandler.TryLoginResult.EsternoProfiloIncompleto
                Response.Redirect("/modifica-profilo-pre-login", True)
                Exit Sub
        End Select

        'ok, pre-login superato
        result = ContextHandler.TryLogin(CType(Me.Master, RootMP).GetFpd.dbConn, login_txt_username.Text, login_txt_password.Text)

        Select Case result
            Case ContextHandler.TryLoginResult.DipendentePrimoLogin
                'redirect
                Response.Redirect("/completa-profilo", True)
                Exit Sub
            Case ContextHandler.TryLoginResult.NoMatch
                login_lbl_error.Text = "Username e/o password errati."
            Case ContextHandler.TryLoginResult.Ok
                'pulizia
                login_txt_username.Text = ""
                login_txt_password.Text = ""

                'risistemo il menu
                GeneraNavLeft()
                navleft.Update()

                'risistemo l'area di destra
                GeneraNavRight()
                navright.Update()

                'faccio sparire l'area login
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType, "closelogin", "displayLogin(false);", True)
                'faccio in modo che la pagina reagisca
                CType(Me.Page, IFOPage).HandleRegionChange()
        End Select

    End Sub

    Private Sub lnkLogout_Click(sender As Object, e As EventArgs) Handles lnkLogout.Click

        'pulizia sessione
        ContextHandler.NewEmptySession()

        'notifico il cambio di regione alla pagina (in questo modo, in caso di redirect, non proseguo)
        CType(Me.Page, IFOPage).HandleRegionChange()

        'rigenerazione dei vari componenti

        'risistemo il menu
        GeneraNavLeft()
        navleft.Update()

        'risistemo l'area di destra
        GeneraNavRight()
        navright.Update()

    End Sub

    Public Sub RefreshTopMenu()
        'risistemo il menu
        GeneraNavLeft()
        navleft.Update()

        'risistemo l'area di destra
        GeneraNavRight()
        navright.Update()
    End Sub
End Class