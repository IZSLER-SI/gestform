Imports System.Configuration.ConfigurationManager
Imports Softailor.Global.XmlParser

Public Class SetupMailAccettazione
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Dim dbConn As SqlConnection

    Private Sub SetupMailAccettazione_Init(sender As Object, e As EventArgs) Handles Me.Init

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'aggancio record evento
        If Not Page.IsPostBack Then
            Me.frmEVENTI.BoundStlSqlDataSource.SelectParameters("id_EVENTO").DefaultValue = GecFinalContextHandler.id_EVENTO.ToString
        End If

        'literal
        ltrCompanyName.Text = My.Settings.CompanyName

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub SetupMailAccettazione_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        'generazione delle anteprime
        GeneraAnteprime()

    End Sub

    Private Sub SetupMailAccettazione_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            dbConn.Close()
            dbConn.Dispose()
        End If
    End Sub

    Private Sub GeneraAnteprime()

        'dati globali
        Dim tx_BASEURL As String
        Dim ragionesociale As String = ""
        Dim indirizzocompleto As String = ""
        Dim tel As String = ""
        Dim fax As String = ""
        Dim email As String = ""

        'pulizie
        phdA.Controls.Clear()
        phdB.Controls.Clear()
        phdC.Controls.Clear()
        phdD.Controls.Clear()

        'lettura dati fittizi
        Dim xDoc = ReadDatiFittizi()

        'lettura dati organizzatore
        Dim dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_iol_GetDatiSegreteriaOrganizzativaEvento"
            .Parameters.Add("@id_EVENTO", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        Dim dbrdr = dbCmd.ExecuteReader
        dbrdr.Read()
        ragionesociale = dbrdr.GetString(0)
        indirizzocompleto = dbrdr.GetString(1)
        tel = dbrdr.GetString(2)
        fax = dbrdr.GetString(3)
        email = dbrdr.GetString(4)
        dbrdr.Close()
        dbCmd.Dispose()

        'lettura dati globali
        tx_BASEURL = AppSettings("GF_FrontofficeBasePath_FromWeb")

        'generazione dei 3 subject
        Dim subjA = MailSubjectHelperGF.subjPromemoriaPartecipazione ' "Promemoria partecipazione ad evento formativo"
        Dim subjB = MailSubjectHelperGF.subjAccettazione  ' "Accettazione iscrizione ad evento formativo"
        Dim subjC = MailSubjectHelperGF.subjNonAccettazione  ' "Mancata accettazione iscrizione ad evento formativo"
        Dim subjD = MailSubjectHelperGF.subjPromemoriaDocenza

        'generazione dei 3 body
        Dim basepath = MapPath("~/GFTemplates/Mail/")
        Dim bodyA = MailSubjectHelperGF.GetBody(xDoc, basepath, "P", "I", False, tx_BASEURL)
        Dim bodyB = MailSubjectHelperGF.GetBody(xDoc, basepath, "P", "I", True, tx_BASEURL)
        Dim bodyC = MailSubjectHelperGF.GetBody(xDoc, basepath, "P", "NA", True, tx_BASEURL)
        Dim bodyD = MailSubjectHelperGF.GetBody(xDoc, basepath, "D", "I", True, tx_BASEURL)

        'generazione dei 3 messaggi completi
        Dim fullA = Transformer.Transform(New XmlDocument, "~/GFTemplates/Mail/MailContainerWeb.xslt", _
                                                "ht_testo", bodyA, _
                                                "baseurl", tx_BASEURL,
                                                "mailfrom_name", ragionesociale, _
                                                "mailfrom_address", email, _
                                                "subject", subjA, _
                                                "ragionesociale", ragionesociale, _
                                                "indirizzocompleto", indirizzocompleto, _
                                                "tel", tel, _
                                                "fax", fax, _
                                                "email", email)

        Dim fullB = Transformer.Transform(New XmlDocument, "~/GFTemplates/Mail/MailContainerWeb.xslt", _
                                                "ht_testo", bodyB, _
                                                "baseurl", tx_BASEURL,
                                                "mailfrom_name", ragionesociale, _
                                                "mailfrom_address", email, _
                                                "subject", subjB, _
                                                "ragionesociale", ragionesociale, _
                                                "indirizzocompleto", indirizzocompleto, _
                                                "tel", tel, _
                                                "fax", fax, _
                                                "email", email)

        Dim fullC = Transformer.Transform(New XmlDocument, "~/GFTemplates/Mail/MailContainerWeb.xslt", _
                                                "ht_testo", bodyC, _
                                                "baseurl", tx_BASEURL,
                                                "mailfrom_name", ragionesociale, _
                                                "mailfrom_address", email, _
                                                "subject", subjC, _
                                                "ragionesociale", ragionesociale, _
                                                "indirizzocompleto", indirizzocompleto, _
                                                "tel", tel, _
                                                "fax", fax, _
                                                "email", email)

        Dim fullD = Transformer.Transform(New XmlDocument, "~/GFTemplates/Mail/MailContainerWeb.xslt", _
                                                "ht_testo", bodyD, _
                                                "baseurl", tx_BASEURL,
                                                "mailfrom_name", ragionesociale, _
                                                "mailfrom_address", email, _
                                                "subject", subjD, _
                                                "ragionesociale", ragionesociale, _
                                                "indirizzocompleto", indirizzocompleto, _
                                                "tel", tel, _
                                                "fax", fax, _
                                                "email", email)

        phdA.Controls.Add(New LiteralControl(fullA))
        phdB.Controls.Add(New LiteralControl(fullB))
        phdC.Controls.Add(New LiteralControl(fullC))
        phdD.Controls.Add(New LiteralControl(fullD))

    End Sub

    Private Function ReadDatiFittizi() As XmlDocument

        Dim dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_iol_TemplateMailEvento"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With
        Dim xReader = dbCmd.ExecuteXmlReader
        Dim xDoc As New XmlDocument
        xDoc.Load(xReader)
        xReader.Close()
        dbCmd.Dispose()

        Return xDoc
    End Function
End Class