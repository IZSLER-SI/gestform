Public Class DelegatiInserimentoPresenzeFO
    Inherits System.Web.UI.Page
    Implements IGFNeedsEventId

    Private dbConn As SqlConnection

    'controlli
    Private noniscrittoids As New Dictionary(Of Integer, TextBox)
    Private noniscrittonomes As New Dictionary(Of Integer, TextBox)
    Private chkiscrittos As New List(Of CheckBox)
    Private ddn_ni_GIORNIDOPOFINEEVE_INSPRESFO As DropDownList
    Const maxNonIScritti = 8

    Private Sub DelegatiInserimentoPresenzeFO_Init(sender As Object, e As EventArgs) Handles Me.Init

        'apertura connessione
        dbConn = DbConnectionHandler.GetOpenDataDbConn

        'generazione controlli
        GeneraControlli()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            Dim sScript = "function clickPostBack() {" & vbCrLf
            sScript &= ClientScript.GetPostBackClientHyperlink(lnkPostBack, "").Replace("javascript:", "")
            sScript &= "}" & vbCrLf

            ltrScripts.Text = sScript
        End If

    End Sub

    Private Sub DelegatiInserimentoPresenzeFO_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        'nomi
        For i = 1 To maxNonIScritti

            If noniscrittoids(i).Text = String.Empty Then
                noniscrittonomes(i).Text = String.Empty
            Else
                noniscrittonomes(i).Text = GetNomeIscritto(CInt(noniscrittoids(i).Text))
            End If
        Next

    End Sub

    Private Function GetNomeIscritto(id_PERSONA As Integer) As String

        Dim dbCmd As SqlCommand
        Dim dbRdr As SqlDataReader

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_ext_DatiPersona"
            .Parameters.Add("@id_PERSONA", SqlDbType.Int).Value = id_PERSONA
        End With
        dbRdr = dbCmd.ExecuteReader
        dbRdr.Read()
        GetNomeIscritto = dbRdr.GetString(1)
        dbRdr.Close()
        dbCmd.Dispose()

    End Function


    Private Sub DelegatiInserimentoPresenzeFO_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If Not dbConn Is Nothing Then
            If dbConn.State = ConnectionState.Open Then dbConn.Close()
            dbConn.Dispose()
        End If
    End Sub

    Private Sub GeneraControlli()

        Dim dbCmd As SqlCommand
        Dim sAspx As String
        Dim cCreato As Control

        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_DelegatiInserimentoPresenzeFOXml"
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
        End With

        sAspx = Transformer.Transform(dbCmd, "Templates/DelegatiInserimentoPresenzeFO.xslt")
        Softailor.Global.AspxCleaner.CleanAspx(sAspx)
        cCreato = ParseControl(sAspx)

        phdControls.Controls.Clear()
        phdControls.Controls.Add(cCreato)

        noniscrittoids.Clear()
        noniscrittonomes.Clear()
        chkiscrittos.Clear()

        For i = 1 To maxNonIscritti
            noniscrittoids.Add(i, CType(cCreato.FindControl("noniscrittoid_" & i.ToString), TextBox))
            noniscrittonomes.Add(i, CType(cCreato.FindControl("noniscrittonome_" & i.ToString), TextBox))
        Next

        For Each c In cCreato.Controls
            If TypeOf c Is CheckBox Then
                If CType(c, CheckBox).ID Like "chkIscritto_*" Then
                    chkiscrittos.Add(CType(c, CheckBox))
                End If
            End If
        Next

        ddn_ni_GIORNIDOPOFINEEVE_INSPRESFO = CType(cCreato.FindControl("ddn_ni_GIORNIDOPOFINEEVE_INSPRESFO"), DropDownList)

    End Sub

    Private Sub lnkSalva_Click(sender As Object, e As EventArgs) Handles lnkSalva.Click

        Dim id_PERSONAs As New Softailor.Global.StructuredUtils.GenericIntList
        Dim dbCmd As SqlCommand

        'iscritti selezionati
        For Each chkIscritto In chkiscrittos
            If chkIscritto.Checked Then
                id_PERSONAs.Add(CInt(chkIscritto.ID.Split("_"c)(1)))
            End If
        Next

        'ID
        For Each noniscrittoid In noniscrittoids.Values
            If noniscrittoid.Text <> String.Empty Then
                id_PERSONAs.Add(CInt(noniscrittoid.Text))
            End If
        Next

        'esecuzione comando
        dbCmd = dbConn.CreateCommand
        With dbCmd
            .CommandType = CommandType.StoredProcedure
            .CommandText = "sp_eve_SetInseritoriPresenzeFO"
            .Parameters.Add("@id_utent", SqlDbType.Int).Value = ContextHandler.ID_UTENT
            .Parameters.Add("@id_evento", SqlDbType.Int).Value = GecFinalContextHandler.id_EVENTO
            .Parameters.Add("@ni_giornidopofineeve_inspresfo", SqlDbType.Int).Value = CInt(ddn_ni_GIORNIDOPOFINEEVE_INSPRESFO.SelectedValue)
            .Parameters.Add("@id_personas", SqlDbType.Structured).Value = id_PERSONAs.GetTable
        End With
        dbCmd.ExecuteNonQuery()

        'chiusura
        Response.Redirect("../EVE/HomeEvento.aspx")

    End Sub

End Class