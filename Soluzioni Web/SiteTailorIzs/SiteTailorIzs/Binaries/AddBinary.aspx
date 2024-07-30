<%@ Page Title="Aggiunta Immagine/Allegato" Language="vb" AutoEventWireup="false"
    MasterPageFile="~/SiteTailorPopupMP.Master" CodeBehind="AddBinary.aspx.vb"
    Inherits="Softailor.SiteTailorIzs.AddBinary" Theme="SiteTailor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phdHeadContent" runat="server">
    <script src="AddBinary.js"></script>
    <link href="AddBinary.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phdPopupTitle" runat="server">
    <div class="singlerow">Aggiunta immagine / allegato</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phdPopupButtons" runat="server">
    <div class="buttonsection">
        <a class="tbbtn" href="javascript:parent.stl_sel_done('');">
            <span class="icon close"></span>
            Annulla
        </a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="phdContent" runat="server">

    <asp:Panel ID="pnlCODCATEG" ClientIDMode="Static" runat="server" DefaultButton="btnCODCATEG">
        <div class="firstsection">1. Categoria elemento</div>
        <div class="sleft">
            <asp:DropDownList ID="ddnCODCATEG" ClientIDMode="Static" runat="server" CssClass="ddn" Width="294px">
            </asp:DropDownList>
        </div>
        <div class="scenter">
            <asp:Button ID="btnCODCATEG" ClientIDMode="Static" runat="server" Text="Avanti &gt;" CssClass="btn" />
        </div>
        <div class="sright">
            <asp:Label ID="errCODCATEG" ClientIDMode="Static" runat="server" Text="" EnableViewState="false" ForeColor="#ff0000" />
        </div>
        <div class="sclear">
            &nbsp;
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlCODFORBA" ClientIDMode="Static" runat="server" DefaultButton="btnCODFORBA">
        <div class="section">2. Formato elemento</div>
        <div class="sleft">
            <asp:DropDownList ID="ddnCODFORBA" ClientIDMode="Static" runat="server" CssClass="ddn" Width="294px">
            </asp:DropDownList>
        </div>
        <div class="scenter">
            <asp:Button ID="btnCODFORBA" ClientIDMode="Static" runat="server" Text="Avanti &gt;" CssClass="btn" />
        </div>
        <div class="sright">
            <asp:Label ID="errCODFORBA" ClientIDMode="Static" runat="server" Text="" EnableViewState="false" ForeColor="#ff0000" />
        </div>
        <div class="sclear">
            &nbsp;
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlDescription" ClientIDMode="Static" runat="server" DefaultButton="btnDESEL_TX">
        <div class="section">3. Descrizione elemento</div>
        <div class="sleft">
            <asp:TextBox ID="txtDESEL_TX" ClientIDMode="Static" runat="server" MaxLength="200" CssClass="txt" Width="290px" Font-Bold="true" />
        </div>
        <div class="scenter">
            <asp:Button ID="btnDESEL_TX" ClientIDMode="Static" runat="server" Text="Avanti &gt;" CssClass="btn" />
        </div>
        <div class="sright">
            <asp:Label ID="errDESEL_TX" ClientIDMode="Static" runat="server" Text="" EnableViewState="false" ForeColor="#ff0000" />
        </div>
        <div class="sclear">
            &nbsp;
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlData" runat="server" ClientIDMode="Static">
        <asp:Panel ID="pnlFile" runat="server" ClientIDMode="Static">
            <div class="section">4. File</div>
            <div class="sleftw">
                <asp:FileUpload ID="fupFile" ClientIDMode="Static" runat="server" CssClass="txt" Width="374px" />
                <div class="info">
                    Formati accettati: 
                        <b>
                            <asp:Label ID="lblFormatiFile" ClientIDMode="Static" runat="server" Text=""></asp:Label></b>
                    <br />
                    <asp:Panel ID="pnlDimensioniImg" ClientIDMode="Static" runat="server">
                        Dimensioni immagine:<br />
                        <b>
                            <asp:Label ID="lblDimensioniImg" ClientIDMode="Static" runat="server" Text=""></asp:Label></b>
                    </asp:Panel>
                    Dimensione massima file: <b>
                        <asp:Label ID="lblDimensionefile" ClientIDMode="Static" runat="server" Text=""></asp:Label></b>
                </div>
            </div>
            <div class="sright">
                <asp:Label ID="errFupFile" ClientIDMode="Static" runat="server" EnableViewState="False" ForeColor="Red" />
            </div>
            <div class="sclear">
                &nbsp;
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlExternalUrl" ClientIDMode="Static" runat="server">
            <div class="section">4. URL</div>
            <div class="sleftw">
                <asp:TextBox ID="txtURL_EXTE" ClientIDMode="Static" runat="server" MaxLength="256" Text="http://" CssClass="txt" Width="367px" />
                <div class="info">
                    <asp:Label ID="lblURL_EXTE" ClientIDMode="Static" runat="server" Text="" />
                </div>
            </div>
            <div class="sright">
                <asp:Label ID="errURL_EXTE" ClientIDMode="Static" runat="server" EnableViewState="False" ForeColor="Red" />
            </div>
            <div class="sclear">
                &nbsp;
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlDimensioniElemento" ClientIDMode="Static" runat="server">
            <div class="section">5. Larghezza/altezza Elemento</div>
            <div class="sleftw">
                <div style="display: block; float: left; width: 115px; padding-top: 4px;">Larghezza (pixel)</div>
                <div style="display: block; float: left;">
                    <asp:TextBox ID="txtELE_WIDT" ClientIDMode="Static" runat="server" MaxLength="4" CssClass="txt" Width="50px" />
                </div>
                <div style="display: block; float: left; padding-top: 4px; padding-left: 10px; color: #666666;">
                    <asp:Label ID="lblELE_WIDT" ClientIDMode="Static" runat="server" Text="" />
                </div>
                <div style="clear: both;"></div>
            </div>
            <div class="sright">
                <asp:Label ID="errELE_WIDT" ClientIDMode="Static" runat="server" EnableViewState="False" ForeColor="Red" />
            </div>
            <div class="sclear">
                &nbsp;
            </div>
            <div class="sleftw">
                <div style="display: block; float: left; width: 115px; padding-top: 4px;">Altezza (pixel)</div>
                <div style="display: block; float: left;">
                    <asp:TextBox ID="txtELE_HEIG" ClientIDMode="Static" runat="server" MaxLength="4" CssClass="txt" Width="50px" />
                </div>
                <div style="display: block; float: left; padding-top: 4px; padding-left: 10px; color: #666666;">
                    <asp:Label ID="lblELE_HEIG" ClientIDMode="Static" runat="server" Text="" />
                </div>
                <div style="clear: both;"></div>
            </div>
            <div class="sright">
                <asp:Label ID="errELE_HEIG" ClientIDMode="Static" runat="server" EnableViewState="False" ForeColor="Red" />
            </div>
            <div class="sclear">
                &nbsp;
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlThumbnail" ClientIDMode="Static" runat="server">
            <div class="section">
                <asp:Label ID="lblThumbnail" runat="server" Text="" />
            </div>
            <div class="sleftw">
                <asp:FileUpload ID="fupThumbnail" ClientIDMode="Static" runat="server" CssClass="txt" Width="374px" />
                <div class="info">
                    Si accettano immagini <b>gif, jpeg e png</b> di max <b>
                        <asp:Label ID="lblMaxDimensioneImgThumbnail" ClientIDMode="Static" runat="server" Text="" /></b>.<br />
                    L'inserimento dell'immagine è opzionale.
                </div>
            </div>
            <div class="sright">
                <asp:Label ID="errFupThumbnail" ClientIDMode="Static" runat="server" EnableViewState="False" ForeColor="Red" />
            </div>
            <div class="sclear">
                &nbsp;
            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="pnlUpload" runat="server" ClientIDMode="Static">
        <div class="upload">
            <asp:Button ID="btnUpload" ClientIDMode="Static" runat="server" Text="Carica" OnClientClick="ValidatePnlData();" CssClass="btnUpload" /><br />
            <asp:Label ID="errUpload" ClientIDMode="Static" runat="server" EnableViewState="False" ForeColor="Red" />
        </div>
    </asp:Panel>
    <asp:HiddenField ID="fupFile_check" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="fupFile_ext" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="URL_EXTE_check" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="ELE_WIDT_check" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="ELE_WIDT_min" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="ELE_WIDT_max" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="ELE_HEIG_check" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="ELE_HEIG_min" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="ELE_HEIG_max" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="fupThumbnail_check" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="fupThumbnail_ext" runat="server" ClientIDMode="Static" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="phdAuxilary" runat="server">
</asp:Content>
