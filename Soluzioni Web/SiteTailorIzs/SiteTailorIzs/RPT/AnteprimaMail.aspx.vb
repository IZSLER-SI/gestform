Imports System.Configuration.ConfigurationManager
Imports Softailor.ReportEngine

Public Class AnteprimaMail
    Inherits System.Web.UI.Page

    Dim ac_FONTE As String
    Dim tx_OGGETTO As String
    Dim ht_CORPO As String
    Dim rptDbConn As SqlConnection
    Dim valoreFiltroBase As String
    Dim fonteXDoc As XmlDocument
    Dim myFonte As Fonte
    Dim ac_KEY As String
    Dim ragionesociale As String
    Dim indirizzocompleto As String
    Dim tel As String
    Dim fax As String
    Dim email As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'lettura parametri dalla request
        With Request.Unvalidated
            ac_FONTE = .Item("sd4_ac_FONTE")
            valoreFiltroBase = .Item("sd4_tx_VALOREFILTROBASE")
            tx_OGGETTO = .Item("sd4_tx_OGGETTO")
            ht_CORPO = .Item("sd4_ht_CORPO")
            ac_KEY = .Item("sd4_ac_KEY")
            ragionesociale = .Item("sd4_ragionesociale")
            indirizzocompleto = .Item("sd4_indirizzocompleto")
            tel = .Item("sd4_tel")
            fax = .Item("sd4_fax")
            email = .Item("sd4_email")
        End With

        'apertura connessione
        rptDbConn = DbConnectionHandler.GetOpenRptDbConn

        'generazione e-mail
        GeneraCorpoMail()


        rptDbConn.Close()
        rptDbConn.Dispose()


    End Sub

    Private Sub GeneraCorpoMail()

        'carico i dati della fonte
        fonteXDoc = New XmlDocument
        fonteXDoc.Load(Server.MapPath("~/RPT/Fonti/" & ac_FONTE & ".xml"))

        'istanzio la fonte
        myFonte = ReportEngine.Fonte.FromXml(Server.MapPath("~/RPT/Fonti/" & ac_FONTE & ".xml"))

        'ottengo i dati della mail
        Dim valoriChiave As New List(Of String)
        valoriChiave.Add(ac_KEY)
        Dim datiMails = myFonte.GeneraMails(rptDbConn, valoreFiltroBase, valoriChiave, tx_OGGETTO, ht_CORPO)
        Dim datiMail = datiMails(0)

        'trasformazione
        Dim tx_BASEURL = AppSettings("GF_FrontofficeBasePath_FromWeb")

        Dim mailHtml = Transformer.Transform(New XmlDocument, "~/GFTemplates/Mail/MailContainerWeb.xslt", _
                                                "ht_testo", datiMail.body, _
                                                "baseurl", tx_BASEURL,
                                                "mailfrom_name", ragionesociale, _
                                                "mailfrom_address", email, _
                                                "mailto", datiMail.recipient, _
                                                "subject", datiMail.subject, _
                                                "ragionesociale", ragionesociale, _
                                                "indirizzocompleto", indirizzocompleto, _
                                                "tel", tel, _
                                                "fax", fax, _
                                                "email", email)

        phdMailBody.Controls.Clear()
        phdMailBody.Controls.Add(New LiteralControl(mailHtml))

    End Sub

End Class